using Logic.DALAbstractions;
using Logic.Dtos;
using Logic.MessageProcessors.QueueState;

using System.Threading.Tasks;

namespace Logic.MessageProcessors
{
    public class MessageSaver
    {
        private readonly IMessageWriter _writer;
        private readonly INewMessageInformer _newMessageInformer;

        public MessageSaver(IMessageWriter writer, INewMessageInformer newMessageInformer)
        {
            _writer = writer;
            _newMessageInformer = newMessageInformer;
        }

        public async Task Write(Message message)
        {
            await _writer.WriteInTimeQueue(message);

            _newMessageInformer.MessageWasAdded();
        }

    }
}
