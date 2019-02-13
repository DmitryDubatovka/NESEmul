namespace NESEmul.Core.Exceptions
{
    public class InvalidByteCodeException : ApplicationException
    {
        public InvalidByteCodeException(byte code) : base($"Invalid byte code {code}"){}
    }
}