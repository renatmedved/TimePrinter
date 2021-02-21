using Logic.Dtos;

using System.Threading.Tasks;

namespace Logic.DALAbstractions
{
    public interface IMessageWriter
    {
        /// <summary>
        /// save all the information for further print
        /// </summary>
        /// <param name="message">message for saving</param>
        Task WriteInTimeQueue(Message message);
    }
}
