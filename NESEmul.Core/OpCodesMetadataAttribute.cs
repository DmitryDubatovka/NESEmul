using System;

namespace NESEmul.Core
{
    [AttributeUsage(AttributeTargets.Field)]
    sealed class OpCodesMetadataAttribute : Attribute
    {
        public AddressingMode AddressingMode { get; private set; }
        public int Cycles { get; set; }
        public bool HasExtraCycles { get; set; }

        public OpCodesMetadataAttribute(AddressingMode addressingMode, int cycles, bool hasExtraCycles = false)
        {
            AddressingMode = addressingMode;
            Cycles = cycles;
            HasExtraCycles = hasExtraCycles;
        }
       
    }
}