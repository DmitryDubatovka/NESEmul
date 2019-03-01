using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NESEmul.Core.Exceptions;

namespace NESEmul.Core
{
    /// <summary>
    /// Decodes operations from their hex value into the <see cref="OpCodes"/>
    /// </summary>
    public class OpCodesDecoder
    {
        private Dictionary<byte, OpCodesInfo> _opCodesDictionary;
        private readonly Lazy<Dictionary<byte, OpCodesInfo>> _opCodesDictionaryTask = new Lazy<Dictionary<byte, OpCodesInfo>>(BuildOpCodesDictionary);
        private readonly CPU _cpu;
        private readonly Memory _memory;

        private class OpCodesInfo
        {
            public OpCodesInfo(OpCodes opCode, AddressingMode addressingMode, int cycles, bool hasExtraCycle)
            {
                OpCode = opCode;
                AddressingMode = addressingMode;
                Cycles = cycles;
                HasExtraCycle = hasExtraCycle;
            }
            public OpCodes OpCode { get; private set; }
            public AddressingMode AddressingMode { get; private set; }
            public int Cycles { get; private set; }
            public bool HasExtraCycle { get; private set; }
        }

        public OpCodesDecoder(CPU cpu, Memory memory)
        {
            _cpu = cpu;
            _memory = memory;
        }
        private Dictionary<byte, OpCodesInfo> OpCodesDictionary => _opCodesDictionary ?? (_opCodesDictionary = _opCodesDictionaryTask.Value);

        private static Dictionary<byte, OpCodesInfo> BuildOpCodesDictionary()
        {
            Type opCodesType = typeof(OpCodes);
            var fields = opCodesType.GetFields();
            var result = new Dictionary<byte, OpCodesInfo>(256);
            Array names = Enum.GetNames(typeof(OpCodes));
            foreach (string name in names)
            {
                var propertyInfo = fields.Single(p => p.Name == name);
                OpCodesMetadataAttribute attribute = propertyInfo.GetCustomAttributes<OpCodesMetadataAttribute>().Single();

                Enum.TryParse(name, out OpCodes enumValue);
                result.Add((byte)enumValue, new OpCodesInfo(enumValue, attribute.AddressingMode, attribute.Cycles, attribute.HasExtraCycles) );
            }

            return result;
        }

        public Operator Decode(byte code)
        {
            if (OpCodesDictionary.ContainsKey(code))
            {
                OpCodesInfo pair = OpCodesDictionary[code];
                switch (pair.AddressingMode)
                {
                    case AddressingMode.Accumulator:
                    case AddressingMode.Implicit:
                        _memory.LoadByteFromMemory(_cpu.ProgramCounter + 1); //dummy read
                        return new Operator(pair.OpCode, new byte[0], pair.AddressingMode, pair.Cycles, pair.HasExtraCycle);

                    case AddressingMode.Immediate:
                    case AddressingMode.ZeroPage:
                    case AddressingMode.ZeroPageX:
                    case AddressingMode.ZeroPageY:
                    case AddressingMode.IndexedIndirect:
                    case AddressingMode.IndirectIndexed:
                    case AddressingMode.Relative:
                        return new Operator(pair.OpCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)},
                            pair.AddressingMode, pair.Cycles, pair.HasExtraCycle);

                    case AddressingMode.Absolute:
                    case AddressingMode.AbsoluteX:
                    case AddressingMode.AbsoluteY:
                    case AddressingMode.Indirect:
                        return new Operator(pair.OpCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1),
                            pair.AddressingMode, pair.Cycles, pair.HasExtraCycle);
                    default:
                        throw new ArgumentException($"Invalid addressing mode {pair.AddressingMode}");
                }
            }
            throw new InvalidByteCodeException(code);
        }
    }
}