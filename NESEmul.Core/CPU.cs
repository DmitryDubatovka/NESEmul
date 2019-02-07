namespace NESEmul.Core
{
    public class CPU
    {
        private ushort _programCounter;
        private byte _stackPointer;
        
        public byte Accumulator { get; set; }
        public byte IndexRegisterX { get; set; }
        public byte IndexRegisterY { get; set; }
        
        public bool CarryFlag { get; set; }
        public bool ZeroFlag { get; set; }
        public bool DecimalMode { get; set; }
        public bool BreakCommand { get; set; }
        public bool OverflowFlag { get; set; }
        public bool NegativeFlag { get; set; }

        public CPU(ushort programCounter, byte stackPointer)
        {
            _programCounter = programCounter;
            _stackPointer = stackPointer;
        }
    }
}