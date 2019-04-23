using System.Collections.Generic;
using NESEmul.Core.Exceptions;

namespace NESEmul.Core
{
    public static class InterruptsVectorTable
    {
        private static Dictionary<InterruptType, int> _table = new Dictionary<InterruptType, int>
        {
            {InterruptType.IRQ, 0xFFFE},
            {InterruptType.NMI, 0xFFFA},
            {InterruptType.Reset, 0xFFFC}
        };

        public static int GetHandlingRoutineAddress(InterruptType interruptType)
        {
            if(!_table.ContainsKey(interruptType))
                throw new ApplicationException($"Interrupt type {interruptType} not defined");
            return _table[interruptType];
        }
    }
}