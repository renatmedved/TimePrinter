using BeetleX.Redis;
using DAL.Redis.DBFactory;
using Logic.DALAbstractions;

using System;
using System.Threading.Tasks;

namespace DAL.Redis.Implementations
{
    public class MessageCleaner : IMessageCleaner
    {
        private readonly RedisDB _db;
        private readonly RedisEntitiesNames _redisEntitiesNames;

        public MessageCleaner(RedisDB db, RedisEntitiesNames redisEntitiesNames)
        {
            _db = db;
            _redisEntitiesNames = redisEntitiesNames;
        }

        /// <summary>
        /// clean message from sorted set and from key-value record
        /// </summary>
        public async Task CleanMessage(Guid guid)
        {
            string id = guid.ToString();

            Sequence seq = _db.CreateSequence(_redisEntitiesNames.MessagesThisHostSequence);

            await seq.ZRem(id);
            await _db.Del(_redisEntitiesNames.GetMessageId(id));
        }
    }
}
