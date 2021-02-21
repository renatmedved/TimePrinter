using System;

namespace Logic.DALAbstractions
{
    /// <summary>
    /// disposable factory that contains connection
    /// </summary>
    public interface IDALServicesMaker : IDisposable
    {
        IMessageCleaner MakeCleaner();
        IMessageReader MakeReader();
        IMessageWriter MakeWriter();
    }
}
