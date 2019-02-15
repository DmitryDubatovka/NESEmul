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
        private Dictionary<byte, OpCodesAndAddressingModePair> _opCodesDictionary;
        private readonly Lazy<Dictionary<byte, OpCodesAndAddressingModePair>> _opCodesDictionaryTask = new Lazy<Dictionary<byte, OpCodesAndAddressingModePair>>(BuildOpCodesDictionary);
        private readonly CPU _cpu;
        private readonly Memory _memory;

        private class OpCodesAndAddressingModePair
        {
            public OpCodesAndAddressingModePair(OpCodes opCode, AddressingMode addressingMode)
            {
                OpCode = opCode;
                AddressingMode = addressingMode;
            }
            public OpCodes OpCode { get; private set; }
            public AddressingMode AddressingMode { get; private set; }
        }

        public OpCodesDecoder(CPU cpu, Memory memory)
        {
            _cpu = cpu;
            _memory = memory;
        }
        private Dictionary<byte, OpCodesAndAddressingModePair> OpCodesDictionary => _opCodesDictionary ?? (_opCodesDictionary = _opCodesDictionaryTask.Value);

        private static Dictionary<byte, OpCodesAndAddressingModePair> BuildOpCodesDictionary()
        {
            Type opCodesType = typeof(OpCodes);
            var fields = opCodesType.GetFields();
            var result = new Dictionary<byte, OpCodesAndAddressingModePair>(256);
            Array names = Enum.GetNames(typeof(OpCodes));
            foreach (string name in names)
            {
                var propertyInfo = fields.Single(p => p.Name == name);
                OpCodesAddressingModeAttribute attribute = propertyInfo.GetCustomAttributes<OpCodesAddressingModeAttribute>().Single();

                Enum.TryParse(name, out OpCodes enumValue);
                result.Add((byte)enumValue, new OpCodesAndAddressingModePair(enumValue, attribute.AddressingMode) );
            }

            return result;
        }

        public Operator Decode(byte code)
        {
            if (OpCodesDictionary.ContainsKey(code))
            {
                OpCodesAndAddressingModePair pair = OpCodesDictionary[code];
                switch (pair.AddressingMode)
                {
                    case AddressingMode.Accumulator:
                    case AddressingMode.Implicit:
                        return new Operator(pair.OpCode, new byte[0], pair.AddressingMode);

                    case AddressingMode.Immediate:
                    case AddressingMode.ZeroPage:
                    case AddressingMode.ZeroPageX:
                    case AddressingMode.ZeroPageY:
                    case AddressingMode.IndexedIndirect:
                    case AddressingMode.IndirectIndexed:
                    case AddressingMode.Relative:
                        return new Operator(pair.OpCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)},
                            pair.AddressingMode);

                    case AddressingMode.Absolute:
                    case AddressingMode.AbsoluteX:
                    case AddressingMode.AbsoluteY:
                    case AddressingMode.Indirect:
                        return new Operator(pair.OpCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1),
                            pair.AddressingMode);
                    default:
                        throw new ArgumentException($"Invalid addressing mode {pair.AddressingMode}");
                }
            }
            throw new InvalidByteCodeException(code);
        }
    }
}