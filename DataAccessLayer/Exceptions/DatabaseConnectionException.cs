namespace DataAccessLayer.Exceptions;

public class DatabaseConnectionException : Exception
{
    public DatabaseConnectionException(string message) : base(message)
    {
    }
}
