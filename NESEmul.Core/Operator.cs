namespace NESEmul.Core
{
    public class Operator
    {
        public byte[] Operands;
        public OpCodes OpCode { get; private set; }
        public byte Length { get; private set; }

        public Operator(OpCodes opCode, byte[] operands)
        {
            OpCode = opCode;
            Operands = operands;
            Length = (byte) (1 + Operands.Length);
        }
    }
}