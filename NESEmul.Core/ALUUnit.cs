using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using NESEmul.Core.Exceptions;
using NLog;

namespace NESEmul.Core
{
    public class ALUUnit
    {
        private readonly CPU _cpu;
        private readonly Memory _memory;
        private readonly Stack _stack;
        private readonly OpCodesDecoder _decoder;
        private bool _hasBoundaryCross;
        private Logger _logger;


        internal int CurrentCycles { get; set; }

        public ALUUnit(CPU cpu, Memory memory, Stack stack)
        {
            _cpu = cpu;
            _memory = memory;
            _stack = stack;
            _decoder = new OpCodesDecoder(cpu, memory);
            CurrentCycles = 0;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Do()
        {
            int cycles = 0;
            while (!_cpu.BreakCommand)
            {
                _hasBoundaryCross = false;
                var @operator = _decoder.Decode(_memory.LoadByteFromMemory(_cpu.ProgramCounter));
                cycles += DoOperator(@operator);
                
                _cpu.ProgramCounter += @operator.Length;
            }
        }

        internal void DoSingleOperation()
        {
            _hasBoundaryCross = false;
            var @operator = _decoder.Decode(_memory.LoadByteFromMemory(_cpu.ProgramCounter));
            LogOperation(@operator);
            CurrentCycles += DoOperator(@operator);
            
            _cpu.ProgramCounter += @operator.Length;
        }

        private void LogOperation(Operator @operator)
        {
            var operands = string.Join(" ", @operator.Operands.Select(s => $"{s:X2}"));
            var operatorLog = $"${_cpu.ProgramCounter:X}:{@operator.OpCode, -2:X} {operands, -6} {@operator.OpCode, -10} ";
            _logger.Info($"{operatorLog} A: {_cpu.Accumulator, -3:X} X: {_cpu.IndexRegisterX, -3:X} Y: {_cpu.IndexRegisterY, -3:X}");
        }

        private int DoOperator(Operator @operator)
        {
            switch (@operator.OpCode)
            {
                case OpCodes.BRK:
                    _cpu.TriggerInterrupt(InterruptType.IRQ);
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
                    return DoBranchOperation(@operator);

                case OpCodes.BITAbs:
                case OpCodes.BITZP:
                    DoBITOperation(@operator);
                    break;

                case OpCodes.CMPAbs:
                case OpCodes.CMPImm:
                case OpCodes.CMPZP:
                case OpCodes.CMPZPX:
                case OpCodes.CMPAbsX:
                case OpCodes.CMPAbsY:
                case OpCodes.CMPIndX:
                case OpCodes.CMPIndY:
                    DoCMPOperation(@operator, _cpu.Accumulator);
                    break;
                
                case OpCodes.CPXAbs:
                case OpCodes.CPXImm:
                case OpCodes.CPXZP:
                    DoCMPOperation(@operator, _cpu.IndexRegisterX);
                    break;
                
                case OpCodes.CPYAbs:
                case OpCodes.CPYImm:
                case OpCodes.CPYZP:
                    DoCMPOperation(@operator, _cpu.IndexRegisterY);
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
                
                case OpCodes.LDAImm:
                case OpCodes.LDAZP:
                case OpCodes.LDAZPX:
                case OpCodes.LDAAbs:
                case OpCodes.LDAAbsX:
                case OpCodes.LDAAbsY:
                case OpCodes.LDAIndX:
                case OpCodes.LDAIndY:
                    DoLDAOperation(@operator);
                    break;
                
                case OpCodes.LDXImm:
                case OpCodes.LDXZP:
                case OpCodes.LDXZPY:
                case OpCodes.LDXAbs:
                case OpCodes.LDXAbsY:
                    DoLDXOperation(@operator);
                    break;

                case OpCodes.LDYImm:
                case OpCodes.LDYZP:
                case OpCodes.LDYZPX:
                case OpCodes.LDYAbs:
                case OpCodes.LDYAbsX:
                    DoLDYOperation(@operator);
                    break;

                case OpCodes.INCZP:
                case OpCodes.INCZPX:
                case OpCodes.INCAbs:
                case OpCodes.INCAbsX:
                case OpCodes.INX:
                case OpCodes.INY:
                    DoINCOperation(@operator);
                    break;
                
                case OpCodes.LSRAcc:
                case OpCodes.LSRZP:
                case OpCodes.LSRZPX:
                case OpCodes.LSRAbs:
                case OpCodes.LSRAbsX:
                    DoLSROperation(@operator);
                    break;

                case OpCodes.JmpInd:
                case OpCodes.JmpAbs:
                    DoJMPOperation(@operator);
                    break;

                case OpCodes.JSR:
                    DoJSROperation(@operator);
                    break;

                case OpCodes.RTS:
                    DoRTSOperation();
                    break;

                case OpCodes.ORAImm:
                case OpCodes.ORAZP:
                case OpCodes.ORAZPX:
                case OpCodes.ORAAbs:
                case OpCodes.ORAAbsX:
                case OpCodes.ORAAbsY:
                case OpCodes.ORAIndX:
                case OpCodes.ORAIndY:
                    DoORAOperation(@operator);
                    break;

                case OpCodes.ROLAccum:
                case OpCodes.ROLZP:
                case OpCodes.ROLZPX:
                case OpCodes.ROLAbs:
                case OpCodes.ROLAbsX:
                    DoROLOperation(@operator);
                    break;

                case OpCodes.RORAccum:
                case OpCodes.RORZP:
                case OpCodes.RORZPX:
                case OpCodes.RORAbs:
                case OpCodes.RORAbsX:
                    DoROROperation(@operator);
                    break;

                case OpCodes.PHA:
                    _stack.Push(_cpu.Accumulator);
                    break;

                case OpCodes.PLA:
                    _cpu.Accumulator = _stack.Pop();
                    SetNegativeFlag(_cpu.Accumulator);
                    _cpu.ZeroFlag = _cpu.Accumulator == 0;
                    break;

                case OpCodes.PHP:
                    _stack.PushFlags();
                    break;
                case OpCodes.PLP:
                    _stack.PopFlags();
                    break;

                case OpCodes.RTI:
                    _stack.PopFlags();
                    var lowByte = _stack.Pop();
                    var hiByte = _stack.Pop();
                    _cpu.ProgramCounter = (ushort) (Build2BytesAddress(lowByte, hiByte) - 1);
                    break;

                case OpCodes.SBCImm:
                case OpCodes.SBCAbs:
                case OpCodes.SBCZP:
                case OpCodes.SBCAbsX:
                case OpCodes.SBCAbsY:
                case OpCodes.SBCIndX:
                case OpCodes.SBCIndY:
                case OpCodes.SBCZPX:
                    DoSBCOperation(@operator);
                    break;
                
                case OpCodes.STAZP:
                case OpCodes.STAZPX:
                case OpCodes.STAAbs:
                case OpCodes.STAAbsX:
                case OpCodes.STAAbsY:
                case OpCodes.STAIndX:
                case OpCodes.STAIndY:
                    var address = ResolveAddress(@operator);
                    _memory.StoreByteInMemory(address, _cpu.Accumulator);
                    break;
                
                case OpCodes.STXAbs:
                case OpCodes.STXZP:
                case OpCodes.STXZPY:
                    address = ResolveAddress(@operator);
                    _memory.StoreByteInMemory(address, _cpu.IndexRegisterX);
                    break;
                
                case OpCodes.STYAbs:
                case OpCodes.STYZP:
                case OpCodes.STYZPX:
                    address = ResolveAddress(@operator);
                    _memory.StoreByteInMemory(address, _cpu.IndexRegisterY);
                    break;
                
                case OpCodes.TAX:
                case OpCodes.TAY:
                case OpCodes.TSX:
                case OpCodes.TXA:
                case OpCodes.TXS:
                case OpCodes.TYA:
                    DoTransferOperation(@operator);
                    break;
            }

            var cycles = @operator.Cycles;
            if (@operator.HasExtraCycle && _hasBoundaryCross)
                cycles++;
            return cycles;
        }

        private void DoADCOperation(Operator op)
        {
            byte accum = _cpu.Accumulator;
            byte operandValue = FetchOperandValue(op);

            int intNewValue = accum + operandValue + (_cpu.CarryFlag ? 1 : 0);
            byte byteNewValue = (byte) intNewValue;
            _cpu.Accumulator = byteNewValue;
            _cpu.ZeroFlag = byteNewValue == 0;
            bool equalSign = (accum & 0x80 ^ operandValue & 0x80) == 0;
            _cpu.OverflowFlag = equalSign && ((accum ^ byteNewValue) & 0x80) != 0;
            _cpu.CarryFlag = intNewValue > byte.MaxValue;
            SetNegativeFlag(byteNewValue);
        }

        private void DoANDOperation(Operator op)
        {
            byte accum = _cpu.Accumulator;
            byte operandValue = FetchOperandValue(op);
            byte byteNewValue = (byte) (accum & operandValue);
            _cpu.Accumulator = byteNewValue;
            _cpu.ZeroFlag = byteNewValue == 0;
            SetNegativeFlag(byteNewValue);
        }

        private void DoASLOperation(Operator op)
        {
            byte operandValue = FetchOperandValue(op);
            bool hasHiBit = (operandValue & 0x80) == 0x80;
            byte byteNewValue = (byte) (operandValue << 1);
            _cpu.CarryFlag = hasHiBit;
            _cpu.ZeroFlag = byteNewValue == 0;
            SetNegativeFlag(byteNewValue);
            if (op.AddressingMode == AddressingMode.Accumulator)
                _cpu.Accumulator = byteNewValue;
            else
            {
                var address = ResolveAddress(op);
                _memory.StoreByteInMemory(address, byteNewValue);
            }
        }

        private void DoBITOperation(Operator op)
        {
            var operandValue = FetchOperandValue(op);
            byte result = (byte)(_cpu.Accumulator & operandValue);
            _cpu.ZeroFlag = result == 0;
            SetNegativeFlag(operandValue);
            _cpu.OverflowFlag = (operandValue & 0x40) == 0x40;
        }

        private void DoCMPOperation(Operator op, byte registerValue)
        {
            byte operandValue = FetchOperandValue(op);
            _cpu.CarryFlag = registerValue >= operandValue;
            _cpu.ZeroFlag = registerValue == operandValue;
            SetNegativeFlag((byte) (registerValue - operandValue));
        }

        private void DoFlagOperation(Operator op)
        {
            switch (op.OpCode)
            {
                case OpCodes.CLC:
                    _cpu.CarryFlag = false;
                    break;
                case OpCodes.CLD:
                    _cpu.DecimalMode = false;
                    break;
                case OpCodes.CLI:
                    _cpu.InterruptDisable = false;
                    break;
                case OpCodes.CLV:
                    _cpu.OverflowFlag = false;
                    break;
                case OpCodes.SEC:
                    _cpu.CarryFlag = true;
                    break;
                case OpCodes.SED:
                    _cpu.DecimalMode = true;
                    break;
                case OpCodes.SEI:
                    _cpu.InterruptDisable = true;
                    break;
            }
            
        }

        private int DoBranchOperation(Operator op)
        {
            byte operandValue = FetchOperandValue(op);
            bool isNegative = (operandValue & 0x80) == 0x80;
            ushort offset;
            int currentPage = _cpu.ProgramCounter / 256;
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
                    performOperation = !_cpu.NegativeFlag;
                    break;
                case OpCodes.BMI:
                    performOperation = _cpu.NegativeFlag;
                    break;
                case OpCodes.BVC:
                    performOperation = !_cpu.OverflowFlag;
                    break;
                case OpCodes.BVS:
                    performOperation = _cpu.OverflowFlag;
                    break;
                case OpCodes.BCC:
                    performOperation = !_cpu.CarryFlag;
                    break;
                case OpCodes.BCS:
                    performOperation = _cpu.CarryFlag;
                    break;
                case OpCodes.BNE:
                    performOperation = !_cpu.ZeroFlag;
                    break;
                case OpCodes.BEQ:
                    performOperation = _cpu.ZeroFlag;
                    break;
            }

            var cycles = op.Cycles;
            if (performOperation)
            {
                _cpu.ProgramCounter += offset;
                cycles++;
            }

            var newPage = _cpu.ProgramCounter / 256;
            return cycles + (newPage == currentPage ? 0  : 1);
        }

        private void DoDECOperation(Operator op)
        {
            byte operandValue;
            if (op.OpCode == OpCodes.DEX)
                operandValue = _cpu.IndexRegisterX;
            else if (op.OpCode == OpCodes.DEY)
                operandValue = _cpu.IndexRegisterY;
            else
                operandValue = FetchOperandValue(op);
            operandValue--;
            _cpu.ZeroFlag = operandValue == 0;
            SetNegativeFlag(operandValue);
            if (op.OpCode == OpCodes.DEX)
                _cpu.IndexRegisterX = operandValue;
            else if (op.OpCode == OpCodes.DEY)
                _cpu.IndexRegisterY = operandValue;
            else
            {
                var address = ResolveAddress(op);
                _memory.StoreByteInMemory(address, operandValue);
            }
        }
        
        private void DoINCOperation(Operator op)
        {
            byte operandValue;
            if (op.OpCode == OpCodes.INX)
                operandValue = _cpu.IndexRegisterX;
            else if (op.OpCode == OpCodes.INY)
                operandValue = _cpu.IndexRegisterY;
            else
                operandValue = FetchOperandValue(op);
            operandValue++;
            _cpu.ZeroFlag = operandValue == 0;
            SetNegativeFlag(operandValue);
            if (op.OpCode == OpCodes.INX)
                _cpu.IndexRegisterX = operandValue;
            else if (op.OpCode == OpCodes.INY)
                _cpu.IndexRegisterY = operandValue;
            else
            {
                var address = ResolveAddress(op);
                _memory.StoreByteInMemory(address, operandValue);
            }
        }

        private void DoEOROperation(Operator op)
        {
            byte operandValue = FetchOperandValue(op);
            _cpu.Accumulator ^= operandValue;
            _cpu.ZeroFlag = _cpu.Accumulator == 0;
            SetNegativeFlag(_cpu.Accumulator);
        }

        private void DoJMPOperation(Operator op)
        {
            ushort newProgramCounter;
            if (op.OpCode == OpCodes.JmpAbs)
            {
                var address = ResolveAddress(op);
                newProgramCounter = (ushort) (address - op.Length);
            }
            else if(op.OpCode == OpCodes.JmpInd)
            {
                var address = ResolveAddress(op);
                var bytes = _memory.Load2BytesFromMemory(address);
                address = Build2BytesAddress(bytes[0], bytes[1]);
                newProgramCounter = (ushort) (address - op.Length);
            }
            else
            {
                throw new InvalidByteCodeException((byte) op.OpCode);
            }

            _cpu.ProgramCounter = newProgramCounter;
        }

        private void DoJSROperation(Operator op)
        {
            var currentPC = _cpu.ProgramCounter;
            currentPC += (ushort)(op.Length - 1);
            byte hiByte = (byte) ((currentPC & 0xFF00) >> 8);
            byte loByte = (byte) (currentPC & 0xFF);
            _stack.Push(hiByte);
            _stack.Push(loByte);
            var address = ResolveAddress(op);
            _cpu.ProgramCounter = (ushort) (address - op.Length);
        }

        private void DoRTSOperation()
        {
            var address = Build2BytesAddress(_stack.Pop(), _stack.Pop()); //additional byte offset is added in the main loop by ProgramCounter += @operator.Length;
            _cpu.ProgramCounter = (ushort) address;
        }

        private void DoLDAOperation(Operator op)
        {
            var operandValue = FetchOperandValue(op);
            _cpu.Accumulator = operandValue;
            _cpu.ZeroFlag = _cpu.Accumulator == 0;
            SetNegativeFlag(_cpu.Accumulator);
        }
        
        private void DoLDXOperation(Operator op)
        {
            var operandValue = FetchOperandValue(op);
            _cpu.IndexRegisterX = operandValue;
            _cpu.ZeroFlag = _cpu.IndexRegisterX == 0;
            SetNegativeFlag(_cpu.IndexRegisterX);
        }
        
        private void DoLDYOperation(Operator op)
        {
            var operandValue = FetchOperandValue(op);
            _cpu.IndexRegisterY = operandValue;
            _cpu.ZeroFlag = _cpu.IndexRegisterY == 0;
            SetNegativeFlag(_cpu.IndexRegisterY);
        }

        private void DoLSROperation(Operator op)
        {
            var operandValue = FetchOperandValue(op);
            _cpu.CarryFlag = (operandValue & 0x1) == 1;
            operandValue >>= 1;
            if (op.AddressingMode == AddressingMode.Accumulator)
                _cpu.Accumulator = operandValue;
            else
            {
                var address = ResolveAddress(op);
                _memory.StoreByteInMemory(address, operandValue);
            }
            _cpu.ZeroFlag = operandValue == 0;
            _cpu.NegativeFlag = false; // the 7th bit of the result is always 0
        }

        private void DoORAOperation(Operator op)
        {
            var operandValue = FetchOperandValue(op);
            _cpu.Accumulator |= operandValue;
            _cpu.ZeroFlag = _cpu.Accumulator == 0;
            SetNegativeFlag(_cpu.Accumulator);
        }

        private void DoROLOperation(Operator op)
        {
            var operandValue = FetchOperandValue(op);
            var newCarryFlagValueSet = (operandValue & 0x80) == 0x80;
            operandValue <<= 1;
            if (_cpu.CarryFlag)
                operandValue |= 0x01;
            else
                operandValue &= 0xFE;
            _cpu.CarryFlag = newCarryFlagValueSet;
            if (op.AddressingMode == AddressingMode.Accumulator)
                _cpu.Accumulator = operandValue;
            else
            {
                var address = ResolveAddress(op);
                _memory.StoreByteInMemory(address, operandValue);
            }

            _cpu.ZeroFlag = operandValue == 0;
            SetNegativeFlag(operandValue);
        }
        
        private void DoROROperation(Operator op)
        {
            var operandValue = FetchOperandValue(op);
            var newCarryFlagValueSet = (operandValue & 0x01) == 0x01;
            operandValue >>= 1;
            if (_cpu.CarryFlag)
                operandValue |= 0x80;
            else
                operandValue &= 0x7F;
            _cpu.CarryFlag = newCarryFlagValueSet;
            if (op.AddressingMode == AddressingMode.Accumulator)
                _cpu.Accumulator = operandValue;
            else
            {
                var address = ResolveAddress(op);
                _memory.StoreByteInMemory(address, operandValue);
            }

            _cpu.ZeroFlag = operandValue == 0;
            SetNegativeFlag(operandValue);
        }

        private void DoSBCOperation(Operator op)
        {
            byte accum = _cpu.Accumulator;
            byte operandValue = FetchOperandValue(op);
            byte result = (byte) (_cpu.Accumulator - operandValue - (_cpu.CarryFlag ? 0 : 1));
            _cpu.Accumulator = result;
            _cpu.ZeroFlag = _cpu.Accumulator == 0;
            SetNegativeFlag(_cpu.Accumulator);
            bool equalSign = (accum & 0x80 ^ operandValue & 0x80) == 0;
            _cpu.OverflowFlag = equalSign && ((accum ^ result) & 0x80) != 0;
            _cpu.CarryFlag = !_cpu.OverflowFlag;
        }

        private void DoTransferOperation(Operator op)
        {
            bool updateFlags = true;
            switch (op.OpCode)
            {
                case OpCodes.TXA:
                    _cpu.Accumulator = _cpu.IndexRegisterX;
                    break;
                case OpCodes.TAX:
                    _cpu.IndexRegisterX = _cpu.Accumulator;
                    break;
                case OpCodes.TYA:
                    _cpu.Accumulator = _cpu.IndexRegisterY;
                    break;
                case OpCodes.TAY:
                    _cpu.IndexRegisterY = _cpu.Accumulator;
                    break;
                case OpCodes.TSX:
                    _cpu.IndexRegisterX = _stack.StackPointer;
                    _cpu.ZeroFlag = _cpu.IndexRegisterX == 0;
                    SetNegativeFlag(_cpu.IndexRegisterX);
                    updateFlags = false;
                    break;
                case OpCodes.TXS:
                    _stack.StackPointer = _cpu.IndexRegisterX;
                    updateFlags = false;
                    break;
            }

            if (updateFlags)
            {
                _cpu.ZeroFlag = _cpu.Accumulator == 0;
                SetNegativeFlag(_cpu.Accumulator);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte FetchOperandValue(Operator op)
        {
            switch (op.AddressingMode)
            {
                case AddressingMode.Accumulator:
                    return _cpu.Accumulator;
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
                    return (byte) (op.Operands[0] + _cpu.IndexRegisterX);
                case AddressingMode.ZeroPageY:
                    return (byte) (op.Operands[0] + _cpu.IndexRegisterY);
                case AddressingMode.Absolute:
                    return Build2BytesAddress(op);
                case AddressingMode.AbsoluteX:
                    return Build2BytesAddress(op.Operands[0], op.Operands[1], out _hasBoundaryCross, _cpu.IndexRegisterX);
                case AddressingMode.AbsoluteY:
                    return Build2BytesAddress(op.Operands[0], op.Operands[1], out _hasBoundaryCross, _cpu.IndexRegisterY);
                case AddressingMode.Indirect:
                    var bytes = _memory.Load2BytesFromMemory(op.Operands[0]);
                    return Build2BytesAddress(bytes[0], bytes[1]);
                case AddressingMode.IndexedIndirect:
                    var address = (byte) (op.Operands[0] + _cpu.IndexRegisterX);
                    bytes = _memory.Load2BytesFromMemory(address);
                    return Build2BytesAddress(bytes[0], bytes[1]);
                case AddressingMode.IndirectIndexed:
                    address = op.Operands[0];
                    bytes = _memory.Load2BytesFromMemory(address);
                    return Build2BytesAddress(bytes[0], bytes[1], out _hasBoundaryCross, _cpu.IndexRegisterY);
                default:
                    throw new InvalidByteCodeException((byte)op.OpCode);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Build2BytesAddress(Operator op, byte additionalOffset = 0)
        {
            return Build2BytesAddress(op.Operands[0], op.Operands[1], additionalOffset);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Build2BytesAddress(byte lowByte, byte hiByte, out bool hasOverflow, byte additionalOffset = 0)
        {
            int hiByteRes = lowByte + additionalOffset;
            hasOverflow = hiByteRes > 255;
            return ((hiByte << 8) + hiByteRes) & 0xFFFF;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lowByte">in the address 0xAABB, the low byte is BB</param>
        /// <param name="hiByte">in the address 0xAABB, the low byte is AA</param>
        /// <param name="additionalOffset"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Build2BytesAddress(byte lowByte, byte hiByte, byte additionalOffset = 0)
        {
            return Build2BytesAddress(lowByte, hiByte, out bool unused, additionalOffset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetNegativeFlag(byte value)
        {
            _cpu.NegativeFlag = (value & 0x80) == 0x80;
        }
    
    }
}