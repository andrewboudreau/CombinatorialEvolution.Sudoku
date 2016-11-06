namespace CombinatorialEvolution.Sudoku.Loggers
{
    using Microsoft.Extensions.Logging;

    public class ResponseHeaderLoggerProvider : ILoggerProvider
    {

        public ResponseHeaderLoggerProvider()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ResponseHeaderLogger(categoryName);
        }

        public void Dispose()
        {
        }
    }
}
