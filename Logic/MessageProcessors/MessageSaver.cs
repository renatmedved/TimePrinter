using Logic.DALAbstractions;
using Logic.Dtos;

using System.Threading.Tasks;

namespace Logic.MessageProcessors
{
    public class MessageSaver
    {
        private readonly IMessageWriter _writer;

        public MessageSaver(IMessageWriter writer)
        {
            _writer = writer;
        }

        public async Task Write(Message message)
        {
            await _writer.WriteInTimeQueue(message);
        }

    }
}
