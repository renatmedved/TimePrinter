using Logic.DALAbstractions;
using Logic.Options;

namespace DAL.Redis.DBFactory
{
    public class DBFactory : IDBFactory
    {
        private readonly RedisOptions _redisOptions;
        private readonly RedisEntitiesNames _redisEntitiesNames;

        public DBFactory(RedisOptions opts, RedisEntitiesNames redisEntitiesNames)
        {
            _redisOptions = opts;
            _redisEntitiesNames = redisEntitiesNames;
        }

        public IDALServicesMaker Create()
        {
            return new DALServicesMaker(_redisOptions, _redisEntitiesNames);
        }
    }
}
