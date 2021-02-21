using BeetleX.Redis;

using Logic.Options;

namespace DAL.Redis.DBFactory
{
    public class RedisDBFactory
    {
        private readonly RedisOptions _opts;

        public RedisDBFactory(RedisOptions opts)
        {
            _opts = opts;
        }

        /// <summary>
        /// return new Disposable DB object
        /// </summary>
        public RedisDB Make()
        {
            RedisDB db = new();

            db.Host.AddWriteHost(_opts.Host, _opts.Port, _opts.SSL);

            return db;
        }
    }
}
