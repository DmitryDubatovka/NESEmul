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
                    return new Operator(opCode, new byte[0]);

                case OpCodes.ADCIm:
                case OpCodes.ADCAbs:
                case OpCodes.ADCZP:
                case OpCodes.ADCAbsX:
                case OpCodes.ADCAbsY:
                case OpCodes.ADCIndX:
                case OpCodes.ADCIndY:
                case OpCodes.ADCZPX:
                    return DecodeADCOperators(opCode);
            }
            throw new InvalidByteCodeException();
        }

        private Operator DecodeADCOperators(OpCodes opCode)
        {
            switch (opCode)
            {
                case OpCodes.ADCIm:
                case OpCodes.ADCZP:
                case OpCodes.ADCZPX:
                case OpCodes.ADCIndX:
                case OpCodes.ADCIndY:
                    return new Operator(opCode, new[] {_memory.LoadByteFromMemory(_cpu.ProgramCounter + 1)});
                default:
                    return new Operator(opCode, _memory.Load2BytesFromMemory(_cpu.ProgramCounter + 1));
            }
        }
    }
}