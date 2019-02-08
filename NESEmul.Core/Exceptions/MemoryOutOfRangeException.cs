namespace NESEmul.Core.Exceptions
{
    public class MemoryOutOfRangeException : ApplicationException
    {
        public MemoryOutOfRangeException(int address) : base($"Memory out of range {address}")
        {
        }
    }
}