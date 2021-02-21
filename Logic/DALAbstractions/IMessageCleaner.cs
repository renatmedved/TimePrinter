using System;
using System.Threading.Tasks;

namespace Logic.DALAbstractions
{
    public interface IMessageCleaner
    {
        /// <summary>
        /// clean all message information by id
        /// </summary>
        Task CleanMessage(Guid id);
    }
}
