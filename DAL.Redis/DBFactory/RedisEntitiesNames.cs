using Logic.Options;

using System;

namespace DAL.Redis.DBFactory
{
    /// <summary>
    /// helper for making keys, sequences
    /// </summary>
    public class RedisEntitiesNames
    {
        private readonly HostOptions _hostOpts;

        public RedisEntitiesNames(HostOptions hostOpts)
        {
            _hostOpts = hostOpts;
        }

        public string MessagesThisHostSequence => $"messages:instance-{_hostOpts.InstanceId}";
        public string GetMessageId(Guid id) => GetMessageId(id.ToString());
        public string GetMessageId(string id) => $"messages:{id}";
    }
}
