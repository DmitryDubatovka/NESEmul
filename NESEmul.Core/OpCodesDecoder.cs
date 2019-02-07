using System;
using System.Collections.Generic;

namespace NESEmul.Core
{
    /// <summary>
    /// Decodes operations from their hex value into the <see cref="OpCodes"/>
    /// </summary>
    public class OpCodesDecoder
    {
        
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
            switch (code)
            {
                case OpCodes.ADCIm:
                case OpCodes.ADCAbs:
                case OpCodes.ADCZP:
            }
        }

        private Operator DecodeADCOperators(byte code)
        {
            
        }
    }
}