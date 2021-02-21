using Logic.PrintAbstractions;

using System;

namespace TimePrinter.Printers
{
    public class MessagePrinter : IMessagePrinter
    {
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
