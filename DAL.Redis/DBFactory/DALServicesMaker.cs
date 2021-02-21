using BeetleX.Redis;

using DAL.Redis.Implementations;

using Logic.DALAbstractions;
using Logic.Options;

namespace DAL.Redis.DBFactory
{
    public sealed class DALServicesMaker : IDALServicesMaker
    {
        private readonly RedisDB _redisDB = new();
        private readonly RedisEntitiesNames _redisEntitiesNames;

        public DALServicesMaker(RedisOptions opts, RedisEntitiesNames redisEntitiesNames)
        {
            _redisDB.Host.AddWriteHost(opts.Host, opts.Port, opts.SSL);
            _redisEntitiesNames = redisEntitiesNames;
        }

        public void Dispose()
        {
            _redisDB.Dispose();
        }

        public IMessageCleaner MakeCleaner()
        {
            return new MessageCleaner(_redisDB, _redisEntitiesNames);
        }

        public IMessageReader MakeReader()
        {
            return new MessageReader(_redisDB, _redisEntitiesNames);
        }

        public IMessageWriter MakeWriter()
        {
            return new MessageWriter(_redisDB, _redisEntitiesNames);
        }
    }
}
