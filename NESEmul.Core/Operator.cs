namespace NESEmul.Core
{
    public class Operator
    {
        public byte[] Operands;
        public OpCodes OpCode { get; private set; }
        public byte Length { get; private set; }
        public AddressingMode AddressingMode { get; private set; }
        public int Cycles { get; set; }
        public bool HasExtraCycle { get; set; }

        public Operator(OpCodes opCode, byte[] operands, AddressingMode addressingMode, int cycles, bool hasExtraCycle)
        {
            OpCode = opCode;
            Operands = operands;
            AddressingMode = addressingMode;
            Cycles = cycles;
            HasExtraCycle = hasExtraCycle;
            Length = (byte) (1 + Operands.Length);
        }
    }
}