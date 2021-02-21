using Logic.Dtos;
using System;
using System.Threading.Tasks;

namespace Logic.DALAbstractions
{
    public interface IMessageReader
    {
        /// <summary>
        /// read meta information for message with minimum time
        /// </summary>
        /// <returns>meta information</returns>
        Task<MessageMetaInformation> ReadFirstMessageMeta();
        /// <summary>
        /// get message text by id
        /// </summary>
        /// <param name="id">message id</param>
        /// <returns>message text</returns>
        Task<string> ReadMessageText(Guid id);
    }
}
