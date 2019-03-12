namespace NESEmul.Core.Exceptions
{
    public class InvalidROMFile : ApplicationException
    {
        public InvalidROMFile(string message) : base(message)
        {
        }

        public InvalidROMFile()
        {
        }
    }
}