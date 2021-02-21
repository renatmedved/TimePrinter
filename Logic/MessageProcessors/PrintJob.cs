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

        private readonly IDBFactory _dbFactory;
        private readonly IMessagePrinter _printer;
        private readonly INewMessageReceiver _newMessageReceiver;

        private DateTime _minDateTime;

        public PrintJob(PrintJobOptions opts, 
            IDBFactory dbFactory,
            IMessagePrinter printer,
            INewMessageReceiver newMessageReceiver)
        {
            _opts = opts;
            _dbFactory = dbFactory;
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
            using IDALServicesMaker servicesMaker = _dbFactory.Create();

            IMessageReader messageReader = servicesMaker.MakeReader();

            MessageMetaInformation meta = await messageReader.ReadFirstMessageMeta();

            while (meta != null && meta.Time <= DateTime.Now)
            {
                string messageText = await messageReader.ReadMessageText(meta.Id);

                _printer.Print(messageText);

                IMessageCleaner messageCleaner = servicesMaker.MakeCleaner();
                await messageCleaner.CleanMessage(meta.Id);

                meta = await messageReader.ReadFirstMessageMeta();
            }

            return meta?.Time ?? DateTime.MaxValue;

        }
    }
}
