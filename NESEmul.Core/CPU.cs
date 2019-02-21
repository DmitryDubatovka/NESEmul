namespace NESEmul.Core
{
    public class CPU
    {
        public ushort ProgramCounter { get; set; }
        private readonly ALUUnit _alu;
        public byte Accumulator { get; set; }
        public byte IndexRegisterX { get; set; }
        public byte IndexRegisterY { get; set; }
        
        /// <summary>
        /// 0th bit
        /// </summary>
        public bool CarryFlag { get; set; }
        /// <summary>
        /// 1st bit
        /// </summary>
        public bool ZeroFlag { get; set; }
        /// <summary>
        /// 2nd bit
        /// </summary>
        public bool InterruptDisable { get; set; }
        /// <summary>
        /// 3rd bit
        /// </summary>
        public bool DecimalMode { get; set; }
        /// <summary>
        /// 4th bit
        /// </summary>
        public bool BreakCommand { get; set; }
        /// <summary>
        /// 6th bit
        /// </summary>
        public bool OverflowFlag { get; set; }
        /// <summary>
        /// 7th bit
        /// </summary>
        public bool NegativeFlag { get; set; }
        

        public CPU(ushort programCounter, Memory memory)
        {
            ProgramCounter = programCounter;
            _alu = new ALUUnit(this, memory, new Stack(0xFF, this, memory));
            BreakCommand = false;
        }

        public void Do()
        {
            _alu.Do();
        }
    }
}