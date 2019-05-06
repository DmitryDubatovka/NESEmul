using System;
using System.Runtime.CompilerServices;
using NESEmul.Core.Exceptions;

namespace NESEmul.Core
{
    public abstract class MirroredMemory
    {
        private readonly int _maxMemorySize;
        protected readonly byte[] Bytes;

        protected MirroredMemory(int maxMemorySize)
        {
            _maxMemorySize = maxMemorySize;
            Bytes = new byte[maxMemorySize];
        }

        protected static Tuple<int, int, int, int> GetRamMirroredAddresses(int address, int mirrorSize)
        {
            var baseAddress = address;
            while (baseAddress - mirrorSize >= 0)
                baseAddress = baseAddress - mirrorSize;
            return new Tuple<int, int, int, int>(baseAddress, baseAddress + mirrorSize, baseAddress + 2 * mirrorSize, baseAddress + 3 * mirrorSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void GuardAddress(int address)
        {
            if(address > _maxMemorySize || address < 0)
                throw new MemoryOutOfRangeException(address);
        }

        public virtual byte LoadByteFromMemory(int address)
        {
            GuardAddress(address);
            return Bytes[address];
        }
        
        public virtual byte[] Load2BytesFromMemory(int address)
        {
            GuardAddress(address);
            byte byte1 = Bytes[address];
            byte byte2 = Bytes[address + 1];
            return new[] {byte1, byte2};
        }

        public virtual void StoreByteInMemory(int address, byte value)
        {
            GuardAddress(address);
            Bytes[address] = value;
            
        }
    }

    public class Memory : MirroredMemory
    {
        private const int MaxMemorySize = 64 * 1024;
        private const int LowRamAddress = 0;
        private const int HiRamAddress = 0x1FFF;
        private PPU _ppu;

        public Memory() : base(MaxMemorySize)
        {
            _ppu = PPU.Instance;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //private static void GuardAddress(int address)
        //{
        //    if(address > MaxMemorySize)
        //        throw new MemoryOutOfRangeException(address);
        //}

        public override byte LoadByteFromMemory(int address)
        {
            if(!PPU.InPPURegistersAddress(address))
                return base.LoadByteFromMemory(address);
            return _ppu.ReadFromRegister(address);
        }

        //public byte[] Load2BytesFromMemory(int address)
        //{
        //    GuardAddress(address);
        //    byte byte1 = Bytes[address];
        //    byte byte2 = Bytes[address + 1];
        //    return new[] {byte1, byte2};
        //}

        public override void StoreByteInMemory(int address, byte value)
        {
            GuardAddress(address);
            if (AddressInRamRange(address))
                StoreByteInRam(address, value);
            else if (PPU.InPPURegistersAddress(address))
            {
                _ppu.WriteToRegister(address, value);
            }
            else
            {
                Bytes[address] = value;
            }
            
        }

        public void WriteBytes(int address, byte[] data)
        {
            if(address + data.Length > MaxMemorySize)
                throw new MemoryOutOfRangeException(address + data.Length);
            //TODO: check for RAM mirrors
            Array.Copy(data, 0, Bytes, address, data.Length);
        }

        private void StoreByteInRam(int address, byte value)
        {
            const int mirrorSize = 0x800;
            var mirroredAddresses = GetRamMirroredAddresses(address, mirrorSize);
            Bytes[mirroredAddresses.Item1] = value;
            Bytes[mirroredAddresses.Item2] = value;
            Bytes[mirroredAddresses.Item3] = value;
            Bytes[mirroredAddresses.Item4] = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool AddressInRamRange(int address)
        {
            return address >= LowRamAddress && address <= HiRamAddress;
        }
    }

    public class PPUMemory : MirroredMemory
    {
        public static PPUMemory Instance { get; }
        private const int MemorySize = 1024 * 16;
        private PPUMemory() : base(MemorySize)
        {
        }

        static PPUMemory()
        {
            Instance = new PPUMemory();
        }
    }

    public class PPUSpriteMemory : MirroredMemory
    {
        private const int SpriteMemorySize = 256;
        public static PPUSpriteMemory Instance { get; }

        private PPUSpriteMemory() : base(SpriteMemorySize)
        {
        }

        static PPUSpriteMemory()
        {
            Instance = new PPUSpriteMemory();
        }

    }
}