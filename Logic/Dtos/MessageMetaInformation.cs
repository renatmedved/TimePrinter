using System;

namespace Logic.Dtos
{
    public record MessageMetaInformation
    {
        public DateTime Time { get; }
        public Guid Id { get; }

        public MessageMetaInformation(Guid id, DateTime time) =>
            (Id, Time) = (id, time);
    }
}
