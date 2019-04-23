namespace NESEmul.Core
{
    public class CPU
    {
        private readonly Memory _memory;
        public ushort ProgramCounter { get; set; }
        private readonly ALUUnit _alu;
        private InterruptType? _interruptType;
        public byte Accumulator { get; set; }
        public byte IndexRegisterX { get; set; }
        public byte IndexRegisterY { get; set; }
        private Stack _stack;
        
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
            _memory = memory;
            ProgramCounter = programCounter;
            _stack = new Stack(0xFF, this, memory);
            _alu = new ALUUnit(this, memory, _stack);
            BreakCommand = false;
        }
        
        public CPU(Memory memory)
        {
            _memory = memory;
            _stack = new Stack(0xFF, this, memory);
            _alu = new ALUUnit(this, memory, _stack);
            BreakCommand = false;
            _interruptType = null;
        }

        public void Do()
        {
            _alu.Do();
        }

        public void Tick()
        {
            if(--_alu.CurrentCycles > 0)
                return;
            if (_interruptType != null)
            {
                HandleInterrupt(_interruptType.Value);
                _interruptType = null;
                return;
            }
            _alu.DoSingleOperation();
        }

        public void TriggerInterrupt(InterruptType interruptType)
        {
            _interruptType = interruptType;
        }

        private void HandleInterrupt(InterruptType interruptType)
        {
            if (interruptType == InterruptType.NMI)
            {
                byte hiByte = (byte) ((ProgramCounter & 0xFF00) >> 8);
                byte loByte = (byte) (ProgramCounter & 0xFF);
                _stack.Push(hiByte);
                _stack.Push(loByte);
                _stack.PushFlags();
            }

            var bytes = _memory.Load2BytesFromMemory(InterruptsVectorTable.GetHandlingRoutineAddress(interruptType));
            var address = ALUUnit.Build2BytesAddress(bytes[0], bytes[1]);
            ProgramCounter = (ushort) address;
            _alu.CurrentCycles += 7;
        }
    }
}