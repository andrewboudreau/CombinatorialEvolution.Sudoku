using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombinatorialEvolution.Sudoku.Loggers
{
    public class ResponseHeaderLogger : ILogger
    {
        private readonly string categoryName;
        
        public ResponseHeaderLogger(string categoryName)
        {
            //_path = Path.Combine(basePath, "mylog.txt");
            this.categoryName = categoryName;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var log = $"{logLevel} :: {categoryName} :: {formatter(state, exception)}";
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new NoopDisposable();
        }

        private class NoopDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }
}
