using BeetleX.Redis;

using DAL.Redis.DBFactory;

using Logic.DALAbstractions;
using Logic.Dtos;

using System.Threading.Tasks;

namespace DAL.Redis.Implementations
{
    public class MessageWriter : IMessageWriter
    {
        private readonly RedisDBFactory _dbFactory;
        private readonly RedisEntitiesNames _redisEntitiesNames;

        public MessageWriter(RedisDBFactory dbFactory, RedisEntitiesNames redisEntitiesNames)
        {
            _dbFactory = dbFactory;
            _redisEntitiesNames = redisEntitiesNames;
        }

        /// <summary>
        /// write meta information of message in sorted set and text in key-value 
        /// </summary>
        public async Task WriteInTimeQueue(Message message)
        {
            using RedisDB db = _dbFactory.Make();

            Sequence seq = db.CreateSequence(_redisEntitiesNames.MessagesThisHostSequence);

            long ticks = message.Meta.Time.Ticks;
            string id = message.Meta.Id.ToString();

            await seq.ZAdd((ticks, id));

            await db.Set(_redisEntitiesNames.GetMessageId(id), message.Text);
        }
    }
}
