namespace Logic.MessageProcessors.QueueState
{
    public interface INewMessageReceiver
    {
        /// <summary>
        /// should be called to check that message was added to db
        /// </summary>
        bool WasNewMessage();
    }
}
