using System;

namespace Logic.Dtos
{
    public record Message
    {
        public MessageMetaInformation Meta { get; }
        public string Text { get; }

        public Message(DateTime time, string text)
        {
            Meta = new MessageMetaInformation(Guid.NewGuid(), time);
            Text = text;
        }
    }
}
