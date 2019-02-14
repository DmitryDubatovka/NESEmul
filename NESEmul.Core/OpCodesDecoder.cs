using System;
using System.Collections.Generic;
using NESEmul.Core.Exceptions;

namespace NESEmul.Core
{
    /// <summary>
    /// Decodes operations from their hex value into the <see cref="OpCodes"/>
    /// </summary>
    public class OpCodesDecoder
    {
        private Dictionary<byte, OpCodes> _opCodesDictionary;
        private readonly Lazy<Dictionary<byte, OpCodes>> _opCodesDictionaryTask = new Lazy<Dictionary<byte, OpCodes>>(BuildOpCodesDictionary);
        private readonly CPU _cpu;
        private readonly Memory _memory;

        public OpCodesDecoder(CPU cpu, Memory memory)
        {
            _cpu = cpu;
            _memory = memory;
        }
        private Dictionary<byte, OpCodes> OpCodesDictionary => _opCodesDictionary ?? (_opCodesDictionary = _opCodesDictionaryTask.Value);

        private static Dictionary<byte, OpCodes> BuildOpCodesDictionary()
        {
            var result = new Dictionary<byte, OpCodes>(256);
            Array names = Enum.GetNames(typeof(OpCodes));
            foreach (object name in names)
            {
                Enum.TryParse(name.ToString(), out OpCodes enumValue);
                result.Add((byte)enumValue, enumValue);
            }

            return result;
        }

        public Operator Decode(byte code)
        {
            OpCodes opCode = OpCodesDictionary[code];
            switch (opCode)
            {
                case OpCodes.BRK:
                    return new Operator(opCode, new byte[0], AddressingMode.Implicit);

                case OpCodes.ADCImm:
                case OpCodes.ADCAbs:
                case OpCodes.ADCZP:
                case OpCodes.ADCAbsX:
                case OpCodes.ADCAbsY:
                case OpCodes.ADCIndX:
                case OpCodes.ADCIndY:
                case OpCodes.ADCZPX:
                    return DecodeADCOperator(opCode);

                case OpCodes.ANDImm:
                case OpCodes.ANDZP:
                case OpCodes.ANDZPX:
                case OpCodes.ANDAbs:
                case OpCodes.ANDAbsX:
                case OpCodes.ANDAbsY:
                case OpCodes.ANDIndX:
                case OpCodes.ANDIndY:
                    return DecodeANDOperator(opCode);

                case OpCodes.ASLAccum:
                case OpCodes.ASLZP:
                case OpCodes.ASLZPX:
                case OpCodes.ASLAbs:
                case OpCodes.ASLAbsX:
                    return DecodeASLOperator(opCode);

                case OpCodes.BPL:
                case OpCodes.BMI:
                case OpCodes.BVC:
                case OpCodes.BVS:
                case OpCodes.BCC:
                case OpCodes.BCS:
                case OpCodes.BNE:
                case OpCodes.BEQ:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.Relative);

                case OpCodes.CMPImm:
                case OpCodes.CMPZP:
                case OpCodes.CMPZPX:
                case OpCodes.CMPAbs:
                case OpCodes.CMPAbsX:
                case OpCodes.CMPAbsY:
                case OpCodes.CMPIndX:
                case OpCodes.CMPIndY:
                    return DecodeCMPOperator(opCode);

                case OpCodes.CPXImm:
                case OpCodes.CPXZP:
                case OpCodes.CPXAbs:
                    return DecodeCPXOperator(opCode);

                case OpCodes.CPYImm:
                case OpCodes.CPYZP:
                case OpCodes.CPYAbs:
                    return DecodeCPYOperator(opCode);

                case OpCodes.CLC:
                case OpCodes.CLD:
                case OpCodes.CLI:
                case OpCodes.CLV:
                case OpCodes.SEC:
                case OpCodes.SED:
                case OpCodes.SEI:
                    return new Operator(opCode, new byte[0], AddressingMode.Implicit);
            }
            throw new InvalidByteCodeException(code);
        }

        private Operator DecodeADCOperator(OpCodes opCode)
        {
            switch (opCode)
            {
                case OpCodes.ADCImm:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.Immediate);
                case OpCodes.ADCZP:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.ZeroPage);
                case OpCodes.ADCZPX:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.ZeroPageX);
                case OpCodes.ADCIndX:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.IndexedIndirect);
                case OpCodes.ADCIndY:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.IndirectIndexed);
                case OpCodes.ADCAbs:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1), AddressingMode.Absolute);
                case OpCodes.ADCAbsX:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1), AddressingMode.AbsoluteX);
                case OpCodes.ADCAbsY:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1), AddressingMode.AbsoluteY);
            }
            throw new InvalidByteCodeException((byte) opCode);
        }

        private Operator DecodeANDOperator(OpCodes opCode)
        {
            switch (opCode)
            {
                case OpCodes.ANDImm:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.Immediate);
                case OpCodes.ANDZP:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.ZeroPage);
                case OpCodes.ANDZPX:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.ZeroPageX);
                case OpCodes.ANDIndX:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.IndexedIndirect);
                case OpCodes.ANDIndY:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.IndirectIndexed);
                case OpCodes.ANDAbs:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1), AddressingMode.Absolute);
                case OpCodes.ANDAbsX:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1), AddressingMode.AbsoluteX);
                case OpCodes.ANDAbsY:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1), AddressingMode.AbsoluteY);
            }
            throw new InvalidByteCodeException((byte) opCode);
        }

        private Operator DecodeASLOperator(OpCodes opCode)
        {
            switch (opCode)
            {
                case OpCodes.ASLAccum:
                    return new Operator(opCode, new byte[0], AddressingMode.Accumulator);
                case OpCodes.ASLZP:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.ZeroPage);
                case OpCodes.ASLZPX:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.ZeroPageX);
                case OpCodes.ASLAbs:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1), AddressingMode.Absolute);
                case OpCodes.ASLAbsX:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1), AddressingMode.AbsoluteX);
            }
            throw new InvalidByteCodeException((byte) opCode);
        }

        private Operator DecodeCMPOperator(OpCodes opCode)
        {
            switch (opCode)
            {
                case OpCodes.CMPImm:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.Immediate);
                case OpCodes.CMPZP:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.ZeroPage);
                case OpCodes.CMPZPX:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.ZeroPageX);
                case OpCodes.CMPIndX:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.IndexedIndirect);
                case OpCodes.CMPIndY:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.IndirectIndexed);
                case OpCodes.CMPAbs:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1), AddressingMode.Absolute);
                case OpCodes.CMPAbsX:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1), AddressingMode.AbsoluteX);
                case OpCodes.CMPAbsY:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1), AddressingMode.AbsoluteY);
            }
            throw new InvalidByteCodeException((byte) opCode);
        }
        
        private Operator DecodeCPXOperator(OpCodes opCode)
        {
            switch (opCode)
            {
                case OpCodes.CPXImm:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.Immediate);
                case OpCodes.CPXZP:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.ZeroPage);
                case OpCodes.CPXAbs:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1), AddressingMode.Absolute);
            }
            throw new InvalidByteCodeException((byte) opCode);
        }
        
        private Operator DecodeCPYOperator(OpCodes opCode)
        {
            switch (opCode)
            {
                case OpCodes.CPYImm:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.Immediate);
                case OpCodes.CPYZP:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)}, AddressingMode.ZeroPage);
                case OpCodes.CPYAbs:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1), AddressingMode.Absolute);
            }
            throw new InvalidByteCodeException((byte) opCode);
        }
    }
}