namespace NESEmul.Core
{
    public enum OpCodes : byte
    {
        /// <summary>
        /// Force break
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        BRK = 0x0,

        /// <summary>
        /// No Operation
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        NOP = 0xEA,

        //Jump to Subroutine. The JSR instruction pushes the address (minus one) of the return point on to the stack and then sets the program counter to the target memory address.
        [OpCodesAddressingMode(AddressingMode.Absolute)]
        JSR = 0x20,

        /// <summary>
        /// RTS pulls the top two bytes off the stack (low byte first) and transfers program control to that address+1.
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        RTS = 0x60,

        /// <summary>
        /// Push Accumulator. Pushes a copy of the accumulator on to the stack.
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        PHA = 0x48,

        /// <summary>
        /// Pull Accumulator. Pulls an 8 bit value from the stack and into the accumulator. The zero and negative flags are set as appropriate.
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        PLA = 0x68,

        /// <summary>
        /// Push Processor Status. Pushes a copy of the status flags on to the stack.
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        PHP = 0x08,

        /// <summary>
        /// Pull Processor Status. Pulls an 8 bit value from the stack and into the processor flags. The flags will take on new states as determined by the value pulled.
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        PLP = 0x28,

        /// <summary>
        /// Return from Interrupt. The RTI instruction is used at the end of an interrupt processing routine. It pulls the processor flags from the stack followed by the program counter.
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        RTI = 0x40,

        //Add with Carry
        #region ADC

        /// <summary>
        /// Immediate addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Immediate)]
        ADCImm = 0x69,
        
        /// <summary>
        /// Zero Page addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        ADCZP = 0x65,
        
        /// <summary>
        /// Zero Page X addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        ADCZPX = 0x75,
        
        /// <summary>
        /// Absolute addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Absolute)]
        ADCAbs = 0x6D,
        
        /// <summary>
        /// Absolute addressing X mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        ADCAbsX = 0x7D,
        
        /// <summary>
        /// Absolute addressing Y mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.AbsoluteY)]
        ADCAbsY = 0x79,
        
        /// <summary>
        /// Indexed Indirect addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.IndexedIndirect)]
        ADCIndX = 0x61,
        
        /// <summary>
        /// Indirect indexed addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.IndirectIndexed)]
        ADCIndY = 0x71,

        #endregion

        //Bitwise AND with accumulator
        #region AND

        /// <summary>
        /// Immediate addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Immediate)]
        ANDImm = 0x29,

        /// <summary>
        /// Zero page addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        ANDZP = 0x25,

        /// <summary>
        /// Zero page X addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        ANDZPX = 0x35,

        /// <summary>
        /// Absolute addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Absolute)]
        ANDAbs = 0x2D,

        /// <summary>
        /// Absolute addressing X mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        ANDAbsX = 0x3D,

        /// <summary>
        /// Absolute addressing Y mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.AbsoluteY)]
        ANDAbsY = 0x39,

        /// <summary>
        /// Indexed Indirect addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.IndexedIndirect)]
        ANDIndX = 0x21,
        
        /// <summary>
        /// Indirect indexed addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.IndirectIndexed)]
        ANDIndY = 0x31,



        #endregion

        //Arithmetic Shift Left
        //This operation shifts all the bits of the accumulator or memory contents one bit left. Bit 0 is set to 0 and bit 7 is placed in the carry flag.
        #region ASL

        /// <summary>
        /// Accumulator addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Accumulator)]
        ASLAccum = 0x0A,

        /// <summary>
        /// Zero Page addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        ASLZP = 0x06,

        /// <summary>
        /// Zero Page X addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        ASLZPX = 0x16,

        /// <summary>
        /// Absolute addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Absolute)]
        ASLAbs = 0x0E,


        /// <summary>
        /// Absolute X addressing mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        ASLAbsX = 0x1E,


        #endregion

        //Relative addressing mode is used by branch instructions (e.g. BEQ, BNE, etc.) which contain a signed 8 bit relative offset (e.g. -128 to +127) which is added to program counter if the condition is true.
        //As the program counter itself is incremented during instruction execution by two the effective address range for the target instruction must be with -126 to +129 bytes of the branch.
        #region Branches

        /// <summary>
        /// Branch on Plus
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Relative)]
        BPL = 0x10,

        /// <summary>
        /// Branch on Minus
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Relative)]
        BMI = 0x30,

        /// <summary>
        /// Branch on oVerflow Clear
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Relative)]
        BVC = 0x50,

        /// <summary>
        /// Branch on oVerflow Set
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Relative)]
        BVS = 0x70,

        /// <summary>
        /// Branch on Carry Clear
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Relative)]
        BCC = 0x90,

        /// <summary>
        /// Branch on Carry Set
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Relative)]
        BCS = 0xB0,

        /// <summary>
        /// Branch on Not Equal
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Relative)]
        BNE = 0xD0,

        /// <summary>
        /// Branch on EQual
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Relative)]
        BEQ = 0xF0,
        #endregion

        //CMP (CoMPare accumulator)  If the value in the accumulator is equal or greater than the compared value, the Carry will be set.
        //The equal (Z) and sign (S) flags will be set based on equality or lack thereof and the sign (i.e. A>=$80) of the accumulator.
        #region CMP
        [OpCodesAddressingMode(AddressingMode.Immediate)]
        CMPImm = 0xC9,

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        CMPZP = 0xC5,

        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        CMPZPX = 0xD5,

        [OpCodesAddressingMode(AddressingMode.Absolute)]
        CMPAbs = 0xCD,

        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        CMPAbsX = 0xDD,

        [OpCodesAddressingMode(AddressingMode.AbsoluteY)]
        CMPAbsY = 0xD9,

        [OpCodesAddressingMode(AddressingMode.IndexedIndirect)]
        CMPIndX = 0xC1,

        [OpCodesAddressingMode(AddressingMode.IndirectIndexed)]
        CMPIndY = 0xD1,

        #endregion

        //ComPare X register. Operation and flag results are identical to equivalent mode accumulator CMP ops.
        #region CPX
        [OpCodesAddressingMode(AddressingMode.Immediate)]
        CPXImm = 0xE0,

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        CPXZP = 0xE4,
        
        [OpCodesAddressingMode(AddressingMode.Absolute)]
        CPXAbs = 0xEC,

        #endregion

        //ComPare Y register. Operation and flag results are identical to equivalent mode accumulator CMP ops.
        #region CPY
        [OpCodesAddressingMode(AddressingMode.Immediate)]
        CPYImm = 0xC0,

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        CPYZP = 0xC4,

        [OpCodesAddressingMode(AddressingMode.Absolute)]
        CPYAbs = 0xCC,

        #endregion

        #region Flags

        /// <summary>
        /// Clear Carry flag
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        CLC = 0x18,

        /// <summary>
        /// Clear Decimal Mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        CLD = 0xD8,

        /// <summary>
        /// Clear Interrupt Disable
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        CLI = 0x58,

        /// <summary>
        /// Clear Overflow flag
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        CLV = 0xB8,

        /// <summary>
        /// Set Carry flag
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        SEC = 0x38,

        /// <summary>
        /// Set Interrupt Disable
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        SEI = 0x78,

        /// <summary>
        /// Set Decimal Mode
        /// </summary>
        [OpCodesAddressingMode(AddressingMode.Implicit)]
        SED = 0xF8,


        #endregion

        //DECrement memory
        #region DEC
        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        DECZP = 0xC6,

        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        DECZPX = 0xD6,

        [OpCodesAddressingMode(AddressingMode.Absolute)]
        DECAbs = 0xCE,

        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        DECAbsX = 0xDE,

        [OpCodesAddressingMode(AddressingMode.Implicit)]
        DEX = 0xCA,

        [OpCodesAddressingMode(AddressingMode.Implicit)]
        DEY = 0x88,

        #endregion

        #region EOR (XOR) An exclusive OR is performed, bit by bit, on the accumulator contents using the contents of a byte of memory.
        [OpCodesAddressingMode(AddressingMode.Immediate)]
        EORImm = 0x49,

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        EORZP = 0x45,

        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        EORZPX = 0x55,

        [OpCodesAddressingMode(AddressingMode.Absolute)]
        EORAbs = 0x4D,

        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        EORAbsX = 0x5D,

        [OpCodesAddressingMode(AddressingMode.AbsoluteY)]
        EORAbsY = 0x59,

        [OpCodesAddressingMode(AddressingMode.IndexedIndirect)]
        EORIndX = 0x41,

        [OpCodesAddressingMode(AddressingMode.IndirectIndexed)]
        EORIndY = 0x51,

        #endregion

        //Adds one to the value held at a specified memory location setting the zero and negative flags as appropriate.
        #region INC

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        INCZP = 0xE6,
        
        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        INCZPX = 0xF6,

        [OpCodesAddressingMode(AddressingMode.Absolute)]
        INCAbs = 0xEE,
        
        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        INCAbsX = 0xFE,

        [OpCodesAddressingMode(AddressingMode.Implicit)]
        INX = 0xE8,

        [OpCodesAddressingMode(AddressingMode.Implicit)]
        INY = 0xC8,

        #endregion

        //Sets the program counter to the address specified by the operand.
        #region JMP

        [OpCodesAddressingMode(AddressingMode.Absolute)]
        JmpAbs = 0x4C,

        [OpCodesAddressingMode(AddressingMode.Indirect)]
        JmpInd = 0x6C,

        #endregion

        //Loads a byte of memory into the accumulator setting the zero and negative flags as appropriate.
        #region LDA

        [OpCodesAddressingMode(AddressingMode.Immediate)]
        LDAImm = 0xA9,

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        LDAZP = 0xA5,
        
        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        LDAZPX = 0xB5,
        
        [OpCodesAddressingMode(AddressingMode.Absolute)]
        LDAAbs = 0xAD,

        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        LDAAbsX = 0xBD,

        [OpCodesAddressingMode(AddressingMode.AbsoluteY)]
        LDAAbsY = 0xB9,

        [OpCodesAddressingMode(AddressingMode.IndexedIndirect)]
        LDAIndX = 0xA1,

        [OpCodesAddressingMode(AddressingMode.IndirectIndexed)]
        LDAIndY = 0xB1,

        #endregion

        //Loads a byte of memory into the X register setting the zero and negative flags as appropriate.
        #region LDX

        [OpCodesAddressingMode(AddressingMode.Immediate)]
        LDXImm = 0xA2,

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        LDXZP = 0xA6,
        
        [OpCodesAddressingMode(AddressingMode.ZeroPageY)]
        LDXZPY = 0xB6,
        
        [OpCodesAddressingMode(AddressingMode.Absolute)]
        LDXAbs = 0xAE,

        [OpCodesAddressingMode(AddressingMode.AbsoluteY)]
        LDXAbsY = 0xBE,

        #endregion

        //Loads a byte of memory into the Y register setting the zero and negative flags as appropriate.
        #region LDY

        [OpCodesAddressingMode(AddressingMode.Immediate)]
        LDYImm = 0xA0,

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        LDYZP = 0xA4,
        
        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        LDYZPX = 0xB4,
        
        [OpCodesAddressingMode(AddressingMode.Absolute)]
        LDYAbs = 0xAC,

        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        LDYAbsX = 0xBC,

        #endregion

        //Each of the bits in A or M is shift one place to the right. The bit that was in bit 0 is shifted into the carry flag. Bit 7 is set to zero.
        #region LSR - Logical Shift Right

        [OpCodesAddressingMode(AddressingMode.Accumulator)]
        LSRAcc = 0x4A,

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        LSRZP = 0x46,

        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        LSRZPX = 0x56,

        [OpCodesAddressingMode(AddressingMode.Absolute)]
        LSRAbs = 0x4E,

        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        LSRAbsX = 0x5E,

        #endregion

        //An inclusive OR is performed, bit by bit, on the accumulator contents using the contents of a byte of memory.
        #region ORA Logical Inclusive OR

        [OpCodesAddressingMode(AddressingMode.Immediate)]
        ORAImm = 0x09,

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        ORAZP = 0x05,

        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        ORAZPX = 0x15,

        [OpCodesAddressingMode(AddressingMode.Absolute)]
        ORAAbs = 0x0D,

        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        ORAAbsX = 0x1D,

        [OpCodesAddressingMode(AddressingMode.AbsoluteY)]
        ORAAbsY = 0x19,

        [OpCodesAddressingMode(AddressingMode.IndexedIndirect)]
        ORAIndX = 0x01,

        [OpCodesAddressingMode(AddressingMode.IndirectIndexed)]
        ORAIndY = 0x11,


        #endregion

        //Move each of the bits in either A or M one place to the left. Bit 0 is filled with the current value of the carry flag whilst the old bit 7 becomes the new carry flag value.
        #region ROL

        [OpCodesAddressingMode(AddressingMode.Accumulator)]
        ROLAccum = 0x2A,

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        ROLZP = 0x26,

        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        ROLZPX = 0x36,

        [OpCodesAddressingMode(AddressingMode.Absolute)]
        ROLAbs = 0x2E,

        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        ROLAbsX = 0x3E,

        #endregion

        //Rotate Right. Move each of the bits in either A or M one place to the right. Bit 7 is filled with the current value of the carry flag whilst the old bit 0 becomes the new carry flag value.
        #region ROR

        [OpCodesAddressingMode(AddressingMode.Accumulator)]
        RORAccum = 0x6A,

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        RORZP = 0x66,

        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        RORZPX = 0x76,

        [OpCodesAddressingMode(AddressingMode.Absolute)]
        RORAbs = 0x6E,

        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        RORAbsX = 0x7E,

        #endregion

        //A,Z,C,N = A-M-(1-C) This instruction subtracts the contents of a memory location to the accumulator together with
        //the not of the carry bit. If overflow occurs the carry bit is clear, this enables multiple byte subtraction to be performed.
        //Processor Status after use:
        #region SBC Subtract with Carry

        [OpCodesAddressingMode(AddressingMode.Immediate)]
        SBCImm = 0xE9,

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        SBCZP = 0xE5,

        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        SBCZPX = 0xF5,

        [OpCodesAddressingMode(AddressingMode.Absolute)]
        SBCAbs = 0xED,

        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        SBCAbsX = 0xFD,

        [OpCodesAddressingMode(AddressingMode.AbsoluteY)]
        SBCAbsY = 0xF9,

        [OpCodesAddressingMode(AddressingMode.IndexedIndirect)]
        SBCIndX = 0xE1,

        [OpCodesAddressingMode(AddressingMode.IndirectIndexed)]
        SBCIndY = 0xF1,


        #endregion

        /// <summary>
        /// Stores the contents of the accumulator into memory.
        /// </summary>
        #region STA - Store Accumulator

        [OpCodesAddressingMode(AddressingMode.ZeroPage)]
        STAZP = 0x85,

        [OpCodesAddressingMode(AddressingMode.ZeroPageX)]
        STAZPX = 0x95,

        [OpCodesAddressingMode(AddressingMode.Absolute)]
        STAAbs = 0x8D,

        [OpCodesAddressingMode(AddressingMode.AbsoluteX)]
        STAAbsX = 0x9D,

        [OpCodesAddressingMode(AddressingMode.AbsoluteY)]
        STAAbsY = 0x99,

        [OpCodesAddressingMode(AddressingMode.IndexedIndirect)]
        STAIndX = 0x81,

        [OpCodesAddressingMode(AddressingMode.IndirectIndexed)]
        STAIndY = 0x91,

        #endregion
    }
}