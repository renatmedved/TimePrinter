using Logic.DALAbstractions;
using Logic.Dtos;
using Logic.MessageProcessors.QueueState;
using Logic.Options;
using Logic.PrintAbstractions;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.MessageProcessors
{
    public class PrintJob
    {
        private readonly PrintJobOptions _opts;

        private readonly IMessageReader _messageReader;
        private readonly IMessageCleaner _messageCleaner;
        private readonly IMessagePrinter _printer;
        private readonly INewMessageReceiver _newMessageReceiver;

        private DateTime _minDateTime;

        public PrintJob(PrintJobOptions opts, 
            IMessageReader messageReader, 
            IMessageCleaner messageCleaner,
            IMessagePrinter printer,
            INewMessageReceiver newMessageReceiver)
        {
            _opts = opts;

            _messageReader = messageReader;
            _messageCleaner = messageCleaner;
            _printer = printer;
            _newMessageReceiver = newMessageReceiver;
        }

        public async Task PerformLoop(CancellationToken token)
        {
            do
            {
                _minDateTime = await ProcessCurrentMessages();//now we know when we should extract message

                while(!_newMessageReceiver.WasNewMessage() //while wasn't new message
                    && _minDateTime >= DateTime.Now.AddMilliseconds(_opts.WaitMilliseconds)) //add we can just sleep
                {
                    await Task.Delay(_opts.WaitMilliseconds);//we can just wait
                }

                await ProcessCurrentMessages();//Now we should process all the new messages

                await Task.Delay(_opts.WaitMilliseconds);

            } while (!token.IsCancellationRequested);
            
        }

        /// <summary>
        /// print current messages 
        /// </summary>
        /// <returns>minimum time the next message or DateTime.MaxValue (if queue is empty)</returns>
        private async Task<DateTime> ProcessCurrentMessages()
        {
            MessageMetaInformation meta = await _messageReader.ReadFirstMessageMeta();

            while (meta != null && meta.Time <= DateTime.Now)
            {
                string messageText = await _messageReader.ReadMessageText(meta.Id);

                _printer.Print(messageText);

                await _messageCleaner.CleanMessage(meta.Id);

                meta = await _messageReader.ReadFirstMessageMeta();
            }

            return meta?.Time ?? DateTime.MaxValue;

        }
    }
}
