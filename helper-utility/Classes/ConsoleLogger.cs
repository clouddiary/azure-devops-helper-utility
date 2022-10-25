using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace helper_utility
{
    public class ConsoleLogger
    {
        public void StatusBegin(string message, ILog logger)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(message);
            logger.Info(message);
        }

        public void StatusEndSuccess(string message, ILog logger)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            logger.Info(message);
        }
        public void StatusEndFailed(string message, ILog logger)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            logger.Error(message);
        }

        public void Error(string message, ILog logger)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            logger.Error(message);
        }

        public void Message(string message, ILog logger)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            logger.Info(message);
        }

        public void SilentError(string message)
        {
            // Doing nothing for now
        }
    }
}
