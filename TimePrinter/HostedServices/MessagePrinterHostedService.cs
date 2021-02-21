using Logic.MessageProcessors;
using Microsoft.Extensions.Hosting;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TimePrinter.HostedServices
{
    /// <summary>
    /// Print all the messages in infinite loop
    /// </summary>
    public class MessagePrinterHostedService : IHostedService
    {
        private readonly CancellationTokenSource _stopper = new();
        private readonly PrintJob _job;
        private Task _executingTask;

        public MessagePrinterHostedService(PrintJob job)
        {
            _job = job;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync();

            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            return Task.CompletedTask; 
        }

        private async Task ExecuteAsync()
        {
            do
            {
                try
                {
                    await _job.PerformLoop(_stopper.Token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"print job ex: {ex}");
                }
            }
            while (true);

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                _stopper.Cancel();
            }
            finally
            {
                Task waitOuterToken = Task.Delay(Timeout.Infinite, cancellationToken);
                
                await Task.WhenAny(_executingTask, waitOuterToken);
            }
        }
    }
}
