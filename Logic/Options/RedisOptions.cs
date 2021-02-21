namespace Logic.Options
{
    public record RedisOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
    }
}
