namespace Logic.DALAbstractions
{
    /// <summary>
    /// factory for making disposable service container
    /// </summary>
    public interface IDBFactory
    {
        IDALServicesMaker Create();
    }
}
