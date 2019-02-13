namespace NESEmul.Core
{
    public class Operator
    {
        public byte[] Operands;
        public OpCodes OpCode { get; private set; }
        public byte Length { get; private set; }
        public AddressingMode AddressingMode { get; private set; }

        public Operator(OpCodes opCode, byte[] operands, AddressingMode addressingMode)
        {
            OpCode = opCode;
            Operands = operands;
            AddressingMode = addressingMode;
            Length = (byte) (1 + Operands.Length);
        }
    }
}