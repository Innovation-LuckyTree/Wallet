public class NameExistsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the NameExistsException class.
    /// </summary>
    public NameExistsException() : base() { }

    /// <summary>
    /// Initializes a new instance of the NameExistsException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NameExistsException(string message) : base(message) { }
}
