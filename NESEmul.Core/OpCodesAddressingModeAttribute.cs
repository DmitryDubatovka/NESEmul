using System;

namespace NESEmul.Core
{
    [AttributeUsage(AttributeTargets.Field)]
    sealed class OpCodesAddressingModeAttribute : Attribute
    {
        public AddressingMode AddressingMode { get; private set; }

        public OpCodesAddressingModeAttribute(AddressingMode addressingMode)
        {
            AddressingMode = addressingMode;
        }
       
    }
}