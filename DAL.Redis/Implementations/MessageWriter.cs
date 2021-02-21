using BeetleX.Redis;

using DAL.Redis.DBFactory;

using Logic.DALAbstractions;
using Logic.Dtos;

using System.Threading.Tasks;

namespace DAL.Redis.Implementations
{
    public class MessageWriter : IMessageWriter
    {
        private readonly RedisDB _db;
        private readonly RedisEntitiesNames _redisEntitiesNames;

        public MessageWriter(RedisDB db, RedisEntitiesNames redisEntitiesNames)
        {
            _db = db;
            _redisEntitiesNames = redisEntitiesNames;
        }

        /// <summary>
        /// write meta information of message in sorted set and text in key-value 
        /// </summary>
        public async Task WriteInTimeQueue(Message message)
        {
            Sequence seq = _db.CreateSequence(_redisEntitiesNames.MessagesThisHostSequence);

            long ticks = message.Meta.Time.Ticks;
            string id = message.Meta.Id.ToString();

            await seq.ZAdd((ticks, id));

            await _db.Set(_redisEntitiesNames.GetMessageId(id), message.Text);
        }
    }
}
