using Logic.DALAbstractions;
using Logic.Dtos;
using Logic.MessageProcessors.QueueState;

using System.Threading.Tasks;

namespace Logic.MessageProcessors
{
    public class MessageSaver
    {
        private readonly IDALServicesMaker _dalServicesMaker;
        private readonly INewMessageInformer _newMessageInformer;

        public MessageSaver(IDALServicesMaker dalServicesMaker, INewMessageInformer newMessageInformer)
        {
            _dalServicesMaker = dalServicesMaker;
            _newMessageInformer = newMessageInformer;
        }

        public async Task Write(Message message)
        {
            IMessageWriter writer = _dalServicesMaker.MakeWriter();

            await writer.WriteInTimeQueue(message);

            _newMessageInformer.MessageWasAdded();
        }

    }
}
