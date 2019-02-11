using System.Runtime.CompilerServices;
using NESEmul.Core.Exceptions;

namespace NESEmul.Core
{
    public class CPU
    {
        public ushort ProgramCounter { get; set; }
        private byte _stackPointer;
        private readonly Memory _memory;
        private readonly OpCodesDecoder _decoder;

        public byte Accumulator { get; set; }
        public byte IndexRegisterX { get; set; }
        public byte IndexRegisterY { get; set; }
        
        public bool CarryFlag { get; set; }
        public bool ZeroFlag { get; set; }
        public bool DecimalMode { get; set; }
        public bool BreakCommand { get; set; }
        public bool OverflowFlag { get; set; }
        public bool NegativeFlag { get; set; }

        public CPU(ushort programCounter, byte stackPointer, Memory memory)
        {
            ProgramCounter = programCounter;
            _stackPointer = stackPointer;
            _memory = memory;
            _decoder = new OpCodesDecoder(this, memory);
        }

        public void Do()
        {
            while (true)
            {

                var @operator = _decoder.Decode(_memory.LoadByteFromMemory(ProgramCounter));
                DoOperator(@operator);
                ProgramCounter += @operator.Length;
            }
        }

        private void DoOperator(Operator @operator)
        {
            switch (@operator.OpCode)
            {
                case OpCodes.ADCIm:
                case OpCodes.ADCAbs:
                case OpCodes.ADCZP:
                case OpCodes.ADCAbsX:
                case OpCodes.ADCAbsY:
                case OpCodes.ADCIndX:
                case OpCodes.ADCIndY:
                case OpCodes.ADCZPX:
                    DoADCOperation(@operator);
                    break;
            }
        }

        private void DoADCOperation(Operator op)
        {
            byte accum = Accumulator;
            byte operandValue = 0;
            switch (op.OpCode)
            {
                case OpCodes.ADCIm:
                    operandValue = op.Operands[0]; 
                    break;
                case OpCodes.ADCZP:
                    byte address = op.Operands[0];
                    operandValue = _memory.LoadByteFromMemory(address);
                    break;
                case OpCodes.ADCZPX:
                    address = (byte) (op.Operands[0] + IndexRegisterX);
                    operandValue = _memory.LoadByteFromMemory(address);
                    break;
                case OpCodes.ADCAbs:
                    int twoBytesAddress = Build2BytesAddress(op);
                    operandValue = _memory.LoadByteFromMemory(twoBytesAddress);
                    break;
                case OpCodes.ADCAbsX:
                    twoBytesAddress = Build2BytesAddress(op, IndexRegisterX);
                    operandValue = _memory.LoadByteFromMemory(twoBytesAddress);
                    break;
                case OpCodes.ADCAbsY:
                    twoBytesAddress = Build2BytesAddress(op, IndexRegisterY);
                    operandValue = _memory.LoadByteFromMemory(twoBytesAddress);
                    break;
                case OpCodes.ADCIndX:
                    address = (byte) (op.Operands[0] + IndexRegisterX);
                    var bytes = _memory.Load2BytesFromMemory(address);
                    twoBytesAddress = Build2BytesAddress(bytes[0], bytes[1]);
                    operandValue = _memory.LoadByteFromMemory(twoBytesAddress);
                    break;
                case OpCodes.ADCIndY:
                    address = op.Operands[0];
                    bytes = _memory.Load2BytesFromMemory(address);
                    twoBytesAddress = Build2BytesAddress(bytes[0], bytes[1], IndexRegisterY);
                    operandValue = _memory.LoadByteFromMemory(twoBytesAddress);
                    break;
                default:
                    throw new InvalidByteCodeException((byte)op.OpCode);
            }
            int intNewValue = accum + operandValue + (CarryFlag ? 1 : 0);
            byte byteNewValue = (byte) intNewValue;
            Accumulator = byteNewValue;
            ZeroFlag = byteNewValue == 0;
            bool equalSign = (accum & 0x80 ^ operandValue & 0x80) == 0;
            OverflowFlag = equalSign && ((accum ^ byteNewValue) & 0x80) != 0;
            CarryFlag = intNewValue > byte.MaxValue;
            NegativeFlag = (intNewValue & 0x80) == 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Build2BytesAddress(Operator op, byte additionalOffset = 0)
        {
            return Build2BytesAddress(op.Operands[0], op.Operands[1], additionalOffset);
            //return (op.Operands[1] << 8) + op.Operands[0] + additionalOffset;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Build2BytesAddress(byte hiByte, byte lowByte, byte additionalOffset = 0)
        {
            return ((lowByte << 8) + hiByte + additionalOffset) & 0xFFFF;
        }
    }
}