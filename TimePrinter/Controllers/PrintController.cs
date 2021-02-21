using Logic.Dtos;
using Logic.MessageProcessors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace TimePrinter.Controllers
{
    [Route("")]
    public class PrintController : Controller
    {
        private readonly MessageSaver _messageSaver;

        public PrintController(MessageSaver messageSaver)
        {
            _messageSaver = messageSaver;
        }

        /// <summary>
        /// Save message in queue
        /// </summary>
        /// <param name="text">text of your message</param>
        /// <param name="time">time of your message (format is yyyy-MM-ddTHH:mm:ss)</param>
        /// <returns></returns>
        [HttpGet("printMeAt")]
        public async Task<StatusCodeResult> PrintMeAt(string text, DateTime time)
        {
            Message message = new (time, text);

            await _messageSaver.Write(message);

            return new StatusCodeResult(200);
        }
    }
}
