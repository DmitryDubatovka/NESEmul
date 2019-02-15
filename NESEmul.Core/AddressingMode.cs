namespace NESEmul.Core
{
    public enum AddressingMode
    {
        /// <summary>
        /// directly specify an 8 bit constant within the instruction.
        /// </summary>
        Immediate = 1,

        /// <summary>
        /// An instruction using zero page addressing mode has only an 8 bit address operand.
        /// This limits it to addressing only the first 256 bytes of memory (e.g. $0000 to $00FF) 
        /// </summary>
        ZeroPage = 2,

        /// <summary>
        /// The address to be accessed by an instruction using indexed zero page addressing is calculated by taking the 8 bit zero page address from the instruction and adding the current value
        /// of the X register to it. For example if the X register contains $0F and the instruction LDA $80,X is executed then the accumulator will be loaded from $008F (e.g. $80 + $0F => $8F)
        /// </summary>
        ZeroPageX = 3,

        /// <summary>
        /// The address to be accessed by an instruction using indexed zero page addressing is calculated by taking the 8 bit zero page address from the instruction and adding the current value
        /// of the Y register to it. This mode can only be used with the LDX and STX instructions.
        /// </summary>
        ZeroPageY = 4,

        /// <summary>
        /// Instructions using absolute addressing contain a full 16 bit address to identify the target location.
        /// </summary>
        Absolute = 5,

        /// <summary>
        /// The address to be accessed by an instruction using X register indexed absolute addressing is computed by taking the 16 bit address from the instruction and added the contents of the X register.
        /// For example if X contains $92 then an STA $2000,X instruction will store the accumulator at $2092 (e.g. $2000 + $92).
        /// </summary>
        AbsoluteX = 6,

        /// <summary>
        /// The Y register indexed absolute addressing mode is the same as the previous mode only with the contents of the Y register added to the 16 bit address from the instruction.
        /// </summary>
        AbsoluteY = 7,

        /// <summary>
        /// The address of the table is taken from the instruction and the X register added to it (with zero page wrap around)
        /// to give the location of the least significant byte of the target address.
        /// </summary>
        IndexedIndirect = 8,

        /// <summary>
        /// In instruction contains the zero page location of the least significant byte of 16 bit address.
        /// The Y register is dynamically added to this value to generated the actual target address for operation.
        /// </summary>
        IndirectIndexed = 9,

        /// <summary>
        /// The source and destination of the information to be manipulated is implied directly by the function of the instruction itself and no further operand needs to be specified.
        /// </summary>
        Implicit = 10,

        /// <summary>
        /// Operate directly upon the accumulator
        /// </summary>
        Accumulator = 11,

        /// <summary>
        /// Relative addressing mode is used by branch instructions
        /// </summary>
        Relative = 12,

        /// <summary>
        /// JMP is the only 6502 instruction to support indirection. The instruction contains a 16 bit address which identifies the location of the
        /// least significant byte of another 16 bit memory address which is the real target of the instruction.
        /// For example if location $0120 contains $FC and location $0121 contains $BA then the instruction JMP ($0120) will cause the next instruction execution to occur
        /// at $BAFC (e.g. the contents of $0120 and $0121).
        /// </summary>
        Indirect = 13
    }
}