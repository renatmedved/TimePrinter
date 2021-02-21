using Logic.Dtos;

namespace Logic.MessageProcessors.QueueState
{
    public interface INewMessageInformer
    {
        /// <summary>
        /// should be called after message was added to db
        /// </summary>
        void MessageWasAdded();
    }
}
