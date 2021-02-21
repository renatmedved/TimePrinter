using Logic.DALAbstractions;
using Logic.Dtos;
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

        public PrintJob(PrintJobOptions opts, 
            IMessageReader messageReader, 
            IMessageCleaner messageCleaner,
            IMessagePrinter printer)
        {
            _opts = opts;

            _messageReader = messageReader;
            _messageCleaner = messageCleaner;
            _printer = printer;
        }

        public async Task PerformLoop(CancellationToken token)
        {
            do
            {
                await ProcessCurrentMessages();
            } while (!token.IsCancellationRequested);
            
        }

        private async Task ProcessCurrentMessages()
        {
            MessageMetaInformation meta = await _messageReader.ReadFirstMessageMeta();

            while (meta != null && meta.Time <= DateTime.Now)
            {
                string messageText = await _messageReader.ReadMessageText(meta.Id);

                _printer.Print(messageText);

                await _messageCleaner.CleanMessage(meta.Id);

                meta = await _messageReader.ReadFirstMessageMeta();
            }

            await Task.Delay(_opts.WaitMilliseconds);
        }
    }
}
