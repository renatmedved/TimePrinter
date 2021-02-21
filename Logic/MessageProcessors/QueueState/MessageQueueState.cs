using System.Collections.Concurrent;
using System.Threading;

namespace Logic.MessageProcessors.QueueState
{
    /// <summary>
    /// Information about latest messages 
    /// </summary>
    public class MessageQueueState : INewMessageInformer, INewMessageReceiver
    {
        private int _wasAdded = 0;

        public void MessageWasAdded()
        {
            Interlocked.Exchange(ref _wasAdded, 1);
        }

        public bool WasNewMessage()
        {
            if (Interlocked.CompareExchange(ref _wasAdded, 0, 1)==1)
            {
                return true;
            }

            return false;
        }
    }
}
