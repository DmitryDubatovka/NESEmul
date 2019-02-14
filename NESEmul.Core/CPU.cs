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

            BreakCommand = false;
        }

        public void Do()
        {
            while (!BreakCommand)
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
                case OpCodes.BRK:
                    BreakCommand = true;
                    break;

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

                case OpCodes.ANDAbs:
                case OpCodes.ANDIm:
                case OpCodes.ANDZP:
                case OpCodes.ANDZPX:
                case OpCodes.ANDAbsX:
                case OpCodes.ANDAbsY:
                case OpCodes.ANDIndX:
                case OpCodes.ANDIndY:
                    DoANDOperation(@operator);
                    break;

                case OpCodes.ASLAccum:
                case OpCodes.ASLZP:
                case OpCodes.ASLZPX:
                case OpCodes.ASLAbs:
                case OpCodes.ASLAbsX:
                    DoASLOperation(@operator);
                    break;

                case OpCodes.BPL:
                case OpCodes.BMI:
                case OpCodes.BVC:
                case OpCodes.BVS:
                case OpCodes.BCC:
                case OpCodes.BCS:
                case OpCodes.BNE:
                case OpCodes.BEQ:
                    DoBranchOperation(@operator);
                    break;

                case OpCodes.CMPAbs:
                case OpCodes.CMPIm:
                case OpCodes.CMPZP:
                case OpCodes.CMPZPX:
                case OpCodes.CMPAbsX:
                case OpCodes.CMPAbsY:
                case OpCodes.CMPIndX:
                case OpCodes.CMPIndY:
                    DoCMPOperation(@operator);
                    break;
            }
        }

        private void DoADCOperation(Operator op)
        {
            byte accum = Accumulator;
            byte operandValue = FetchOperandValue(op);

            int intNewValue = accum + operandValue + (CarryFlag ? 1 : 0);
            byte byteNewValue = (byte) intNewValue;
            Accumulator = byteNewValue;
            ZeroFlag = byteNewValue == 0;
            bool equalSign = (accum & 0x80 ^ operandValue & 0x80) == 0;
            OverflowFlag = equalSign && ((accum ^ byteNewValue) & 0x80) != 0;
            CarryFlag = intNewValue > byte.MaxValue;
            NegativeFlag = (byteNewValue & 0x80) == 0x80;
        }

        private void DoANDOperation(Operator op)
        {
            byte accum = Accumulator;
            byte operandValue = FetchOperandValue(op);
            byte byteNewValue = (byte) (accum & operandValue);
            Accumulator = byteNewValue;
            ZeroFlag = byteNewValue == 0;
            NegativeFlag = (byteNewValue & 0x80) == 0x80;
        }

        private void DoASLOperation(Operator op)
        {
            byte operandValue = FetchOperandValue(op);
            bool hasHiBit = (operandValue & 0x80) == 0x80;
            byte byteNewValue = (byte) (operandValue << 1);
            CarryFlag = hasHiBit;
            ZeroFlag = byteNewValue == 0;
            NegativeFlag = (byteNewValue & 0x80) == 0x80;
            if (op.AddressingMode == AddressingMode.Accumulator)
                Accumulator = byteNewValue;
            else
            {
                var address = ResolveAddress(op);
                _memory.StoreByteInMemory(address, byteNewValue);
            }
        }

        private void DoCMPOperation(Operator op)
        {
            byte operandValue = FetchOperandValue(op);
            CarryFlag = Accumulator >= operandValue;
            ZeroFlag = Accumulator == operandValue;
            NegativeFlag = ((Accumulator - operandValue) & 0x80) == 0x80;
        }

        private void DoBranchOperation(Operator op)
        {
            byte operandValue = FetchOperandValue(op);
            bool isNegative = (operandValue & 0x80) == 0x80;
            ushort offset;
            if(isNegative)
            {
                offset = (ushort) (operandValue - 256);
            }
            else
            {
                offset = operandValue;
            }

            bool performOperation = false;
            switch (op.OpCode)
            {
                case OpCodes.BPL:
                    performOperation = !NegativeFlag;
                    break;
                case OpCodes.BMI:
                    performOperation = NegativeFlag;
                    break;
                case OpCodes.BVC:
                    performOperation = !OverflowFlag;
                    break;
                case OpCodes.BVS:
                    performOperation = OverflowFlag;
                    break;
                case OpCodes.BCC:
                    performOperation = !CarryFlag;
                    break;
                case OpCodes.BCS:
                    performOperation = CarryFlag;
                    break;
                case OpCodes.BNE:
                    performOperation = !ZeroFlag;
                    break;
                case OpCodes.BEQ:
                    performOperation = ZeroFlag;
                    break;
            }

            if (performOperation)
                ProgramCounter += offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte FetchOperandValue(Operator op)
        {
            switch (op.AddressingMode)
            {
                case AddressingMode.Accumulator:
                    return Accumulator;
                case AddressingMode.Relative:
                case AddressingMode.Immediate:
                    return op.Operands[0];
                case AddressingMode.ZeroPage:
                case AddressingMode.ZeroPageX:
                case AddressingMode.ZeroPageY:
                case AddressingMode.Absolute:
                case AddressingMode.AbsoluteX:
                case AddressingMode.AbsoluteY:
                case AddressingMode.IndexedIndirect:
                case AddressingMode.IndirectIndexed:
                    var address = ResolveAddress(op);
                    return _memory.LoadByteFromMemory(address);
                default:
                    throw new InvalidByteCodeException((byte)op.OpCode);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ResolveAddress(Operator op)
        {
            switch (op.AddressingMode)
            {
                case AddressingMode.ZeroPage:
                    return op.Operands[0];
                case AddressingMode.ZeroPageX:
                    return (byte) (op.Operands[0] + IndexRegisterX);
                case AddressingMode.ZeroPageY:
                    return (byte) (op.Operands[0] + IndexRegisterY);
                case AddressingMode.Absolute:
                    return Build2BytesAddress(op);
                case AddressingMode.AbsoluteX:
                    return Build2BytesAddress(op, IndexRegisterX);
                case AddressingMode.AbsoluteY:
                    return Build2BytesAddress(op, IndexRegisterY);
                case AddressingMode.IndexedIndirect:
                    var address = (byte) (op.Operands[0] + IndexRegisterX);
                    var bytes = _memory.Load2BytesFromMemory(address);
                    return Build2BytesAddress(bytes[0], bytes[1]);
                case AddressingMode.IndirectIndexed:
                    address = op.Operands[0];
                    bytes = _memory.Load2BytesFromMemory(address);
                    return Build2BytesAddress(bytes[0], bytes[1], IndexRegisterY);
                default:
                    throw new InvalidByteCodeException((byte)op.OpCode);
            }
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