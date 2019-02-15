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
        private const short StackMemoryOffset = 0x100;

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
            _stackPointer = 0xFF;
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

        private void Push(byte value)
        {
            var address = StackMemoryOffset + _stackPointer;
            _memory.StoreByteInMemory(address, value);
            _stackPointer--;
        }

        private void PushFlags()
        {
            byte value = (byte) (NegativeFlag ? 1 : 0);
            value <<= 1;
            value += (byte) (OverflowFlag ? 1 : 0);
            value <<= 1;
            value += 1; // unused bit
            value <<= 1;
            value += (byte) (BreakCommand ? 1 : 0);
            value <<= 1;
            value += (byte) (DecimalMode ? 1 : 0);
            value <<= 1;
            value += (byte) (InterruptDisable ? 1 : 0);
            value <<= 1;
            value += (byte) (ZeroFlag ? 1 : 0);
            value <<= 1;
            value += (byte) (CarryFlag ? 1 : 0);
            Push(value);
        }

        private void PopFlags()
        {
            byte value = Pop();
            NegativeFlag = (value & 0x80) != 0;
            OverflowFlag = (value & 0x40) != 0;
            BreakCommand = (value & 0x10) != 0;
            DecimalMode = (value & 0x08) != 0;
            InterruptDisable = (value & 0x04) != 0;
            ZeroFlag = (value & 0x02) != 0;
            CarryFlag = (value & 0x01) != 0;
        }

        private byte Pop()
        {
            var address = StackMemoryOffset + ++_stackPointer;
            return _memory.LoadByteFromMemory(address);
        }

        private void DoOperator(Operator @operator)
        {
            switch (@operator.OpCode)
            {
                case OpCodes.BRK:
                    BreakCommand = true;
                    break;
                case OpCodes.NOP:
                    break;

                case OpCodes.ADCImm:
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
                case OpCodes.ANDImm:
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
                case OpCodes.CMPImm:
                case OpCodes.CMPZP:
                case OpCodes.CMPZPX:
                case OpCodes.CMPAbsX:
                case OpCodes.CMPAbsY:
                case OpCodes.CMPIndX:
                case OpCodes.CMPIndY:
                    DoCMPOperation(@operator, Accumulator);
                    break;
                
                case OpCodes.CPXAbs:
                case OpCodes.CPXImm:
                case OpCodes.CPXZP:
                    DoCMPOperation(@operator, IndexRegisterX);
                    break;
                
                case OpCodes.CPYAbs:
                case OpCodes.CPYImm:
                case OpCodes.CPYZP:
                    DoCMPOperation(@operator, IndexRegisterY);
                    break;

                case OpCodes.CLC:
                case OpCodes.CLD:
                case OpCodes.CLI:
                case OpCodes.CLV:
                case OpCodes.SEC:
                case OpCodes.SED:
                case OpCodes.SEI:
                    DoFlagOperation(@operator);
                    break;
                
                case OpCodes.DECZP:
                case OpCodes.DECZPX:
                case OpCodes.DECAbs:
                case OpCodes.DECAbsX:
                case OpCodes.DEX:
                case OpCodes.DEY:
                    DoDECOperation(@operator);
                    break;

                case OpCodes.EORImm:
                case OpCodes.EORZP:
                case OpCodes.EORZPX:
                case OpCodes.EORAbs:
                case OpCodes.EORAbsX:
                case OpCodes.EORAbsY:
                case OpCodes.EORIndX:
                case OpCodes.EORIndY:
                    DoEOROperation(@operator);
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
            SetNegativeFlag(byteNewValue);
        }

        private void DoANDOperation(Operator op)
        {
            byte accum = Accumulator;
            byte operandValue = FetchOperandValue(op);
            byte byteNewValue = (byte) (accum & operandValue);
            Accumulator = byteNewValue;
            ZeroFlag = byteNewValue == 0;
            SetNegativeFlag(byteNewValue);
        }

        private void DoASLOperation(Operator op)
        {
            byte operandValue = FetchOperandValue(op);
            bool hasHiBit = (operandValue & 0x80) == 0x80;
            byte byteNewValue = (byte) (operandValue << 1);
            CarryFlag = hasHiBit;
            ZeroFlag = byteNewValue == 0;
            SetNegativeFlag(byteNewValue);
            if (op.AddressingMode == AddressingMode.Accumulator)
                Accumulator = byteNewValue;
            else
            {
                var address = ResolveAddress(op);
                _memory.StoreByteInMemory(address, byteNewValue);
            }
        }

        private void DoCMPOperation(Operator op, byte registerValue)
        {
            byte operandValue = FetchOperandValue(op);
            CarryFlag = registerValue >= operandValue;
            ZeroFlag = registerValue == operandValue;
            SetNegativeFlag(registerValue - operandValue);
        }

        private void DoFlagOperation(Operator op)
        {
            switch (op.OpCode)
            {
                case OpCodes.CLC:
                    CarryFlag = false;
                    break;
                case OpCodes.CLD:
                    DecimalMode = false;
                    break;
                case OpCodes.CLI:
                    InterruptDisable = false;
                    break;
                case OpCodes.CLV:
                    OverflowFlag = false;
                    break;
                case OpCodes.SEC:
                    CarryFlag = true;
                    break;
                case OpCodes.SED:
                    DecimalMode = true;
                    break;
                case OpCodes.SEI:
                    InterruptDisable = true;
                    break;
            }
            
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

        private void DoDECOperation(Operator op)
        {
            byte operandValue = 0;
            if (op.OpCode == OpCodes.DEX)
                operandValue = IndexRegisterX;
            else if (op.OpCode == OpCodes.DEY)
                operandValue = IndexRegisterY;
            else
                operandValue = FetchOperandValue(op);
            operandValue--;
            ZeroFlag = operandValue == 0;
            SetNegativeFlag(operandValue);
            if (op.OpCode == OpCodes.DEX)
                IndexRegisterX = operandValue;
            else if (op.OpCode == OpCodes.DEY)
                IndexRegisterY = operandValue;
            else
            {
                var address = ResolveAddress(op);
                _memory.StoreByteInMemory(address, operandValue);
            }
        }

        private void DoEOROperation(Operator op)
        {
            byte operandValue = FetchOperandValue(op);
            Accumulator ^= operandValue;
            ZeroFlag = Accumulator == 0;
            SetNegativeFlag(Accumulator);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetNegativeFlag(int value)
        {
            NegativeFlag = (value & 0x80) == 0x80;
        }
    }
}