using System;
using System.Runtime.Serialization;

namespace lab18v8
{
    [Serializable]
    public class DatabaseConnectionException : Exception
    {
        public string ConnectionString { get; } = string.Empty;

        public DatabaseConnectionException()
            : base("Не вдалося встановити з'єднання з базою даних.") { }

        public DatabaseConnectionException(string message)
            : base(message) { }

        public DatabaseConnectionException(string message, Exception innerException)
            : base(message, innerException) { }

        public DatabaseConnectionException(string connectionString, string message)
            : base(message)
        {
            ConnectionString = connectionString;
        }

        public DatabaseConnectionException(string connectionString, string message, Exception innerException)
            : base(message, innerException)
        {
            ConnectionString = connectionString;
        }

#pragma warning disable SYSLIB0051
        protected DatabaseConnectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ConnectionString = info.GetString(nameof(ConnectionString)) ?? string.Empty;
        }
#pragma warning restore SYSLIB0051
    }
}
