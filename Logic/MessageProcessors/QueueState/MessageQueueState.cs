using System.Collections.Concurrent;

namespace Logic.MessageProcessors.QueueState
{
    /// <summary>
    /// Information about latest messages 
    /// </summary>
    public class MessageQueueState : INewMessageInformer, INewMessageReceiver
    {
        private readonly ConcurrentQueue<bool> _queue = new();

        public void MessageWasAdded()
        {
            _queue.Enqueue(true);
        }

        public bool WasNewMessage()
        {
            if (_queue.TryDequeue(out bool _))
            {
                _queue.Clear();

                return true;
            }

            return false;
        }
    }
}
