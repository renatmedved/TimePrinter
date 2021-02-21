using BeetleX.Redis;
using DAL.Redis.DBFactory;
using Logic.DALAbstractions;
using Logic.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Redis.Implementations
{
    public class MessageReader : IMessageReader
    {
        private readonly RedisDB _db;
        private readonly RedisEntitiesNames _redisSequencesNames;

        public MessageReader(RedisDB db, RedisEntitiesNames redisSequencesNames)
        {
            _db = db;
            _redisSequencesNames = redisSequencesNames;
        }

        /// <summary>
        /// read meta information for message with min time
        /// </summary>
        public async Task<MessageMetaInformation> ReadFirstMessageMeta()
        {
            Sequence seq = _db.CreateSequence(_redisSequencesNames.MessagesThisHostSequence);

            List<(double Score, string Member)> list = await seq.ZRange(0, 1, true);

            if (list.Count == 0)
            {
                return null;
            }

            (double Score, string Member) = list.First();

            Guid id = Guid.Parse(Member);
            DateTime time = new DateTime((long)Score);

            return new MessageMetaInformation(id, time);
        }

        public async Task<string> ReadMessageText(Guid id)
        {
            string text = await _db.Get<string>(_redisSequencesNames.GetMessageId(id));

            return text;
        }
    }
}
