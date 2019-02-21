namespace NESEmul.Core
{
    public class Stack
    {
        private readonly Memory _memory;
        public byte StackPointer;
        private readonly CPU _cpu;
        private const short StackMemoryOffset = 0x100;

        public Stack(byte initialStackPointer, CPU cpu, Memory memory)
        {
            _memory = memory;
            StackPointer = initialStackPointer;
            _cpu = cpu;
        }

        public void Push(byte value)
        {
            var address = StackMemoryOffset + StackPointer;
            _memory.StoreByteInMemory(address, value);
            StackPointer--;
        }

        public void PushFlags()
        {
            byte value = (byte) (_cpu.NegativeFlag ? 1 : 0);
            value <<= 1;
            value += (byte) (_cpu.OverflowFlag ? 1 : 0);
            value <<= 1;
            value += 1; // unused bit
            value <<= 1;
            value += (byte) (_cpu.BreakCommand ? 1 : 0);
            value <<= 1;
            value += (byte) (_cpu.DecimalMode ? 1 : 0);
            value <<= 1;
            value += (byte) (_cpu.InterruptDisable ? 1 : 0);
            value <<= 1;
            value += (byte) (_cpu.ZeroFlag ? 1 : 0);
            value <<= 1;
            value += (byte) (_cpu.CarryFlag ? 1 : 0);
            Push(value);
        }

        public void PopFlags()
        {
            byte value = Pop();
            _cpu.NegativeFlag = (value & 0x80) != 0;
            _cpu.OverflowFlag = (value & 0x40) != 0;
            _cpu.BreakCommand = (value & 0x10) != 0;
            _cpu.DecimalMode = (value & 0x08) != 0;
            _cpu.InterruptDisable = (value & 0x04) != 0;
            _cpu.ZeroFlag = (value & 0x02) != 0;
            _cpu.CarryFlag = (value & 0x01) != 0;
        }

        public byte Pop()
        {
            var address = StackMemoryOffset + ++StackPointer;
            return _memory.LoadByteFromMemory(address);
        }
    }
}