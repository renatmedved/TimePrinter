using BeetleX.Redis;
using DAL.Redis.DBFactory;
using Logic.DALAbstractions;

using System;
using System.Threading.Tasks;

namespace DAL.Redis.Implementations
{
    public class MessageCleaner : IMessageCleaner
    {
        private readonly RedisDBFactory _dbFactory;
        private readonly RedisEntitiesNames _redisEntitiesNames;

        public MessageCleaner(RedisDBFactory dbFactory, RedisEntitiesNames redisEntitiesNames)
        {
            _dbFactory = dbFactory;
            _redisEntitiesNames = redisEntitiesNames;
        }

        /// <summary>
        /// clean message from sorted set and from key-value record
        /// </summary>
        public async Task CleanMessage(Guid guid)
        {
            using RedisDB db = _dbFactory.Make();

            string id = guid.ToString();

            Sequence seq = db.CreateSequence(_redisEntitiesNames.MessagesThisHostSequence);

            await seq.ZRem(id);
            await db.Del(_redisEntitiesNames.GetMessageId(id));
        }
    }
}
