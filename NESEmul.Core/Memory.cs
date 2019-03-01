using System;
using System.Runtime.CompilerServices;
using NESEmul.Core.Exceptions;

namespace NESEmul.Core
{
    public class Memory
    {
        private const int MaxMemorySize = 64 * 1024;
        private readonly byte[] _bytes;
        private const int LowRamAddress = 0;
        private const int HiRamAddress = 0x1FFF;

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
            return new[] {byte1, byte2};
        }

        public void StoreByteInMemory(int address, byte value)
        {
            GuardAddress(address);
            if (AddressInRamRange(address))
                StoreByteInRam(address, value);
            _bytes[address] = value;
        }

        public void WriteBytes(int address, byte[] data)
        {
            if(address + data.Length > MaxMemorySize)
                throw new MemoryOutOfRangeException(address + data.Length);
            //TODO: check for RAM mirrors
            Array.Copy(data, 0, _bytes, address, data.Length);
        }

        private void StoreByteInRam(int address, byte value)
        {
            var mirroredAddresses = GetRamMirroredAddresses(address);
            _bytes[mirroredAddresses.Item1] = value;
            _bytes[mirroredAddresses.Item2] = value;
            _bytes[mirroredAddresses.Item3] = value;
            _bytes[mirroredAddresses.Item4] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AddressInRamRange(int address)
        {
            return address >= LowRamAddress && address <= HiRamAddress;
        }

        private static Tuple<int, int, int, int> GetRamMirroredAddresses(int address)
        {
            const int mirrorSize = 0x800;
            var baseAddress = address;
            while (baseAddress - mirrorSize >= 0)
                baseAddress = baseAddress - mirrorSize;
            return new Tuple<int, int, int, int>(baseAddress, baseAddress + mirrorSize, baseAddress + 2 * mirrorSize, baseAddress + 3 * mirrorSize);
        }
    }
}