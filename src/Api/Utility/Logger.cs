using System;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Api.Utility
{
    public interface ILogger
    {
        void Info(string message);
        void Debug(string message);
        void Warn(string message);
        void Warn(string message, Exception ex);
        void Error(string message);
        void Error(string message, Exception ex);
    }

    public class Logger : ILogger
    {
        private readonly Type _sourceType;
        private readonly Lazy<ILog> _lazy;
        private static PatternLayout layout;
        private static bool verbose;
        private ILog Log => _lazy.Value;

        public static void Initialize(bool verbose = false)
        {
            Logger.verbose = verbose;
            var repository = (Hierarchy)LogManager.GetRepository(typeof(Logger).Assembly);
            log4net.Config.BasicConfigurator.Configure(repository);
            repository.Root.RemoveAllAppenders();
            layout = new PatternLayout("[%d{yyyyMMdd HH:mm:ss}][%level][%logger][%thread] %message%newline");
            repository.Root.AddAppender(_ColoredConsoleAppender());
            repository.Root.Level = Level.All;
        }

        private static IAppender _ColoredConsoleAppender()
        {
            var appender = new ManagedColoredConsoleAppender();
            appender.Name = "Console Appender";
            appender.Layout = layout;
            appender.Threshold = verbose ? Level.Verbose : Level.Info;
            appender.AddMapping(new ManagedColoredConsoleAppender.LevelColors { Level = Level.Info, ForeColor = ConsoleColor.Cyan });
            appender.AddMapping(new ManagedColoredConsoleAppender.LevelColors { Level = Level.Verbose, ForeColor = ConsoleColor.Gray });
            appender.AddMapping(new ManagedColoredConsoleAppender.LevelColors { Level = Level.Debug, ForeColor = ConsoleColor.White });
            appender.AddMapping(new ManagedColoredConsoleAppender.LevelColors { Level = Level.Warn, ForeColor = ConsoleColor.Yellow });
            appender.AddMapping(new ManagedColoredConsoleAppender.LevelColors { Level = Level.Error, ForeColor = ConsoleColor.Red });
            appender.AddMapping(new ManagedColoredConsoleAppender.LevelColors { Level = Level.Critical, ForeColor = ConsoleColor.Red });
            appender.ActivateOptions();
            return appender;
        }

        public Logger(Type sourceType)
        {
            _sourceType = sourceType;
            _lazy = new Lazy<ILog>(()=>LogManager.GetLogger(sourceType));
        }

        public void Info(string message)
        {
            Log.Info(message);
        }

        public void Debug(string message)
        {
            Log.Debug(message);
        }

        public void Verbose(string message)
        {
            Log.Logger.Log(_sourceType, Level.Verbose, message, null);
        }

        public void Warn(string message)
        {
            Log.Warn(message);
        }

        public void Warn(string message, Exception ex)
        {
            Log.Warn(message, ex);
        }

        public void Error(string message)
        {
            Log.Error(message);
        }

        public void Error(string message, Exception ex)
        {
            Log.Error(message, ex);
        }
    }
}