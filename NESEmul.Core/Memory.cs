using System;
using System.Runtime.CompilerServices;
using NESEmul.Core.Exceptions;

namespace NESEmul.Core
{
    public class Memory
    {
        private const int MaxMemorySize = 64 * 1024;
        private readonly byte[] _bytes;

        public Memory()
        {
            _bytes = new byte[MaxMemorySize];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void GuardAddress(int address)
        {
            if(address > MaxMemorySize)
                throw new MemoryOutOfRangeException(address);
        }

        public byte LoadByteFromMemory(int address)
        {
            GuardAddress(address);
            return _bytes[address];
        }
        
        public byte[] Load2BytesFromMemory(int address)
        {
            GuardAddress(address);
            byte byte1 = _bytes[address];
            byte byte2 = _bytes[address + 1];
            return  new []{byte1, byte2};
        }

        public void StoreByteInMemory(int address, byte value)
        {
            GuardAddress(address);
            _bytes[address] = value;
        }
    }
}