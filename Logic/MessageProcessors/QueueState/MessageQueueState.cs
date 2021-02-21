using System.Threading;

namespace Logic.MessageProcessors.QueueState
{
    /// <summary>
    /// Information about latest messages 
    /// </summary>
    public class MessageQueueState : INewMessageInformer, INewMessageReceiver
    {
        private const int NO_MESSAGES = 0;
        private const int MESSAGE_WAS_ADDED = 1;

        /// <summary>
        /// <see cref="NO_MESSAGES"/> is no messages after <see cref="WasNewMessage"/> 
        /// <see cref="MESSAGE_WAS_ADDED"/> is was message (by <see cref="MessageWasAdded"/>)
        /// </summary>
        private int _wasAdded = NO_MESSAGES;

        public void MessageWasAdded()
        {
            Interlocked.Exchange(ref _wasAdded, MESSAGE_WAS_ADDED);
        }

        public bool WasNewMessage()
        {
            //set 0 if was 1
            if (Interlocked.CompareExchange(ref _wasAdded, NO_MESSAGES, MESSAGE_WAS_ADDED) 
                == MESSAGE_WAS_ADDED)
            {
                //was new messages
                return true;
            }

            //wasn't new messages
            return false;
        }
    }
}
