using System;

namespace Logic.Options
{
    public record HostOptions
    {
        /// <summary>
        /// each instance has id
        /// </summary>
        public Guid InstanceId { get; set; }
    }
}
