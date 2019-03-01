namespace NESEmul.Core
{
    public enum OpCodes : byte
    {
        /// <summary>
        /// Force break
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 7)]
        BRK = 0x0,

        /// <summary>
        /// No Operation
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        NOP = 0xEA,

        //Jump to Subroutine. The JSR instruction pushes the address (minus one) of the return point on to the stack and then sets the program counter to the target memory address.
        [OpCodesMetadata(AddressingMode.Absolute, 6)]
        JSR = 0x20,

        /// <summary>
        /// RTS pulls the top two bytes off the stack (low byte first) and transfers program control to that address+1.
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 6)]
        RTS = 0x60,

        /// <summary>
        /// Push Accumulator. Pushes a copy of the accumulator on to the stack.
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 3)]
        PHA = 0x48,

        /// <summary>
        /// Pull Accumulator. Pulls an 8 bit value from the stack and into the accumulator. The zero and negative flags are set as appropriate.
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 4)]
        PLA = 0x68,

        /// <summary>
        /// Push Processor Status. Pushes a copy of the status flags on to the stack.
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 3)]
        PHP = 0x08,

        /// <summary>
        /// Pull Processor Status. Pulls an 8 bit value from the stack and into the processor flags. The flags will take on new states as determined by the value pulled.
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 4)]
        PLP = 0x28,

        /// <summary>
        /// Return from Interrupt. The RTI instruction is used at the end of an interrupt processing routine. It pulls the processor flags from the stack followed by the program counter.
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 6)]
        RTI = 0x40,

        //Add with Carry
        #region ADC

        /// <summary>
        /// Immediate addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.Immediate, 2)]
        ADCImm = 0x69,
        
        /// <summary>
        /// Zero Page addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        ADCZP = 0x65,
        
        /// <summary>
        /// Zero Page X addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.ZeroPageX, 4)]
        ADCZPX = 0x75,
        
        /// <summary>
        /// Absolute addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        ADCAbs = 0x6D,
        
        /// <summary>
        /// Absolute addressing X mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.AbsoluteX, 4, true)]
        ADCAbsX = 0x7D,
        
        /// <summary>
        /// Absolute addressing Y mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.AbsoluteY, 4, true)]
        ADCAbsY = 0x79,
        
        /// <summary>
        /// Indexed Indirect addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.IndexedIndirect, 6)]
        ADCIndX = 0x61,
        
        /// <summary>
        /// Indirect indexed addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.IndirectIndexed, 5, true)]
        ADCIndY = 0x71,

        #endregion

        //Bitwise AND with accumulator
        #region AND

        /// <summary>
        /// Immediate addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.Immediate, 2)]
        ANDImm = 0x29,

        /// <summary>
        /// Zero page addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        ANDZP = 0x25,

        /// <summary>
        /// Zero page X addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.ZeroPageX, 4)]
        ANDZPX = 0x35,

        /// <summary>
        /// Absolute addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        ANDAbs = 0x2D,

        /// <summary>
        /// Absolute addressing X mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.AbsoluteX, 4, true)]
        ANDAbsX = 0x3D,

        /// <summary>
        /// Absolute addressing Y mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.AbsoluteY, 4, true)]
        ANDAbsY = 0x39,

        /// <summary>
        /// Indexed Indirect addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.IndexedIndirect, 6)]
        ANDIndX = 0x21,
        
        /// <summary>
        /// Indirect indexed addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.IndirectIndexed, 5, true)]
        ANDIndY = 0x31,



        #endregion

        //Arithmetic Shift Left
        //This operation shifts all the bits of the accumulator or memory contents one bit left. Bit 0 is set to 0 and bit 7 is placed in the carry flag.
        #region ASL

        /// <summary>
        /// Accumulator addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.Accumulator, 2)]
        ASLAccum = 0x0A,

        /// <summary>
        /// Zero Page addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.ZeroPage, 5)]
        ASLZP = 0x06,

        /// <summary>
        /// Zero Page X addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.ZeroPageX, 6)]
        ASLZPX = 0x16,

        /// <summary>
        /// Absolute addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.Absolute, 6)]
        ASLAbs = 0x0E,


        /// <summary>
        /// Absolute X addressing mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.AbsoluteX, 7)]
        ASLAbsX = 0x1E,


        #endregion

        //A & M, N = M7, V = M6 
        //This instructions is used to test if one or more bits are set in a target memory location. The mask pattern in A is ANDed with the value in memory to set or clear the zero flag,
        //but the result is not kept. Bits 7 and 6 of the value from memory are copied into the N and V flags.
        #region BIT Bit Test

        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        BITZP = 0x24,

        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        BITAbs = 0x2C,

        #endregion
        //Relative addressing mode is used by branch instructions (e.g. BEQ, BNE, etc.) which contain a signed 8 bit relative offset (e.g. -128 to +127) which is added to program counter if the condition is true.
        //As the program counter itself is incremented during instruction execution by two the effective address range for the target instruction must be with -126 to +129 bytes of the branch.
        #region Branches

        /// <summary>
        /// Branch on Plus
        /// </summary>
        [OpCodesMetadata(AddressingMode.Relative, 2, true)]
        BPL = 0x10,

        /// <summary>
        /// Branch on Minus
        /// </summary>
        [OpCodesMetadata(AddressingMode.Relative, 2, true)]
        BMI = 0x30,

        /// <summary>
        /// Branch on oVerflow Clear
        /// </summary>
        [OpCodesMetadata(AddressingMode.Relative, 2, true)]
        BVC = 0x50,

        /// <summary>
        /// Branch on oVerflow Set
        /// </summary>
        [OpCodesMetadata(AddressingMode.Relative, 2, true)]
        BVS = 0x70,

        /// <summary>
        /// Branch on Carry Clear
        /// </summary>
        [OpCodesMetadata(AddressingMode.Relative, 2, true)]
        BCC = 0x90,

        /// <summary>
        /// Branch on Carry Set
        /// </summary>
        [OpCodesMetadata(AddressingMode.Relative, 2, true)]
        BCS = 0xB0,

        /// <summary>
        /// Branch on Not Equal
        /// </summary>
        [OpCodesMetadata(AddressingMode.Relative, 2, true)]
        BNE = 0xD0,

        /// <summary>
        /// Branch on EQual
        /// </summary>
        [OpCodesMetadata(AddressingMode.Relative, 2, true)]
        BEQ = 0xF0,
        #endregion

        //CMP (CoMPare accumulator)  If the value in the accumulator is equal or greater than the compared value, the Carry will be set.
        //The equal (Z) and sign (S) flags will be set based on equality or lack thereof and the sign (i.e. A>=$80) of the accumulator.
        #region CMP
        [OpCodesMetadata(AddressingMode.Immediate, 2)]
        CMPImm = 0xC9,

        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        CMPZP = 0xC5,

        [OpCodesMetadata(AddressingMode.ZeroPageX, 4)]
        CMPZPX = 0xD5,

        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        CMPAbs = 0xCD,

        [OpCodesMetadata(AddressingMode.AbsoluteX, 4, true)]
        CMPAbsX = 0xDD,

        [OpCodesMetadata(AddressingMode.AbsoluteY, 4, true)]
        CMPAbsY = 0xD9,

        [OpCodesMetadata(AddressingMode.IndexedIndirect, 6)]
        CMPIndX = 0xC1,

        [OpCodesMetadata(AddressingMode.IndirectIndexed, 5, true)]
        CMPIndY = 0xD1,

        #endregion

        //ComPare X register. Operation and flag results are identical to equivalent mode accumulator CMP ops.
        #region CPX
        [OpCodesMetadata(AddressingMode.Immediate, 2)]
        CPXImm = 0xE0,

        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        CPXZP = 0xE4,
        
        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        CPXAbs = 0xEC,

        #endregion

        //ComPare Y register. Operation and flag results are identical to equivalent mode accumulator CMP ops.
        #region CPY
        [OpCodesMetadata(AddressingMode.Immediate, 2)]
        CPYImm = 0xC0,

        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        CPYZP = 0xC4,

        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        CPYAbs = 0xCC,

        #endregion

        #region Flags

        /// <summary>
        /// Clear Carry flag
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        CLC = 0x18,

        /// <summary>
        /// Clear Decimal Mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        CLD = 0xD8,

        /// <summary>
        /// Clear Interrupt Disable
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        CLI = 0x58,

        /// <summary>
        /// Clear Overflow flag
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        CLV = 0xB8,

        /// <summary>
        /// Set Carry flag
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        SEC = 0x38,

        /// <summary>
        /// Set Interrupt Disable
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        SEI = 0x78,

        /// <summary>
        /// Set Decimal Mode
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        SED = 0xF8,


        #endregion

        //DECrement memory
        #region DEC
        [OpCodesMetadata(AddressingMode.ZeroPage, 5)]
        DECZP = 0xC6,

        [OpCodesMetadata(AddressingMode.ZeroPageX, 6)]
        DECZPX = 0xD6,

        [OpCodesMetadata(AddressingMode.Absolute, 6)]
        DECAbs = 0xCE,

        [OpCodesMetadata(AddressingMode.AbsoluteX, 7)]
        DECAbsX = 0xDE,

        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        DEX = 0xCA,

        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        DEY = 0x88,

        #endregion

        #region EOR (XOR) An exclusive OR is performed, bit by bit, on the accumulator contents using the contents of a byte of memory.
        [OpCodesMetadata(AddressingMode.Immediate, 2)]
        EORImm = 0x49,

        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        EORZP = 0x45,

        [OpCodesMetadata(AddressingMode.ZeroPageX, 4)]
        EORZPX = 0x55,

        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        EORAbs = 0x4D,

        [OpCodesMetadata(AddressingMode.AbsoluteX, 4, true)]
        EORAbsX = 0x5D,

        [OpCodesMetadata(AddressingMode.AbsoluteY, 4, true)]
        EORAbsY = 0x59,

        [OpCodesMetadata(AddressingMode.IndexedIndirect, 6)]
        EORIndX = 0x41,

        [OpCodesMetadata(AddressingMode.IndirectIndexed, 5, true)]
        EORIndY = 0x51,

        #endregion

        //Adds one to the value held at a specified memory location setting the zero and negative flags as appropriate.
        #region INC

        [OpCodesMetadata(AddressingMode.ZeroPage, 5)]
        INCZP = 0xE6,
        
        [OpCodesMetadata(AddressingMode.ZeroPageX, 6)]
        INCZPX = 0xF6,

        [OpCodesMetadata(AddressingMode.Absolute, 6)]
        INCAbs = 0xEE,
        
        [OpCodesMetadata(AddressingMode.AbsoluteX, 7)]
        INCAbsX = 0xFE,

        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        INX = 0xE8,

        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        INY = 0xC8,

        #endregion

        //Sets the program counter to the address specified by the operand.
        #region JMP

        [OpCodesMetadata(AddressingMode.Absolute, 3)]
        JmpAbs = 0x4C,

        [OpCodesMetadata(AddressingMode.Indirect, 5)]
        JmpInd = 0x6C,

        #endregion

        //Loads a byte of memory into the accumulator setting the zero and negative flags as appropriate.
        #region LDA

        [OpCodesMetadata(AddressingMode.Immediate, 2)]
        LDAImm = 0xA9,

        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        LDAZP = 0xA5,
        
        [OpCodesMetadata(AddressingMode.ZeroPageX, 4)]
        LDAZPX = 0xB5,
        
        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        LDAAbs = 0xAD,

        [OpCodesMetadata(AddressingMode.AbsoluteX, 4, true)]
        LDAAbsX = 0xBD,

        [OpCodesMetadata(AddressingMode.AbsoluteY, 4, true)]
        LDAAbsY = 0xB9,

        [OpCodesMetadata(AddressingMode.IndexedIndirect, 6)]
        LDAIndX = 0xA1,

        [OpCodesMetadata(AddressingMode.IndirectIndexed, 5, true)]
        LDAIndY = 0xB1,

        #endregion

        //Loads a byte of memory into the X register setting the zero and negative flags as appropriate.
        #region LDX

        [OpCodesMetadata(AddressingMode.Immediate, 2)]
        LDXImm = 0xA2,

        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        LDXZP = 0xA6,
        
        [OpCodesMetadata(AddressingMode.ZeroPageY, 4)]
        LDXZPY = 0xB6,
        
        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        LDXAbs = 0xAE,

        [OpCodesMetadata(AddressingMode.AbsoluteY, 4, true)]
        LDXAbsY = 0xBE,

        #endregion

        //Loads a byte of memory into the Y register setting the zero and negative flags as appropriate.
        #region LDY

        [OpCodesMetadata(AddressingMode.Immediate, 2)]
        LDYImm = 0xA0,

        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        LDYZP = 0xA4,
        
        [OpCodesMetadata(AddressingMode.ZeroPageX, 4)]
        LDYZPX = 0xB4,
        
        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        LDYAbs = 0xAC,

        [OpCodesMetadata(AddressingMode.AbsoluteX, 4, true)]
        LDYAbsX = 0xBC,

        #endregion

        //Each of the bits in A or M is shift one place to the right. The bit that was in bit 0 is shifted into the carry flag. Bit 7 is set to zero.
        #region LSR - Logical Shift Right

        [OpCodesMetadata(AddressingMode.Accumulator, 2)]
        LSRAcc = 0x4A,

        [OpCodesMetadata(AddressingMode.ZeroPage, 5)]
        LSRZP = 0x46,

        [OpCodesMetadata(AddressingMode.ZeroPageX, 6)]
        LSRZPX = 0x56,

        [OpCodesMetadata(AddressingMode.Absolute, 6)]
        LSRAbs = 0x4E,

        [OpCodesMetadata(AddressingMode.AbsoluteX, 7)]
        LSRAbsX = 0x5E,

        #endregion

        //An inclusive OR is performed, bit by bit, on the accumulator contents using the contents of a byte of memory.
        #region ORA Logical Inclusive OR

        [OpCodesMetadata(AddressingMode.Immediate, 2)]
        ORAImm = 0x09,

        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        ORAZP = 0x05,

        [OpCodesMetadata(AddressingMode.ZeroPageX, 4)]
        ORAZPX = 0x15,

        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        ORAAbs = 0x0D,

        [OpCodesMetadata(AddressingMode.AbsoluteX, 4, true)]
        ORAAbsX = 0x1D,

        [OpCodesMetadata(AddressingMode.AbsoluteY, 4, true)]
        ORAAbsY = 0x19,

        [OpCodesMetadata(AddressingMode.IndexedIndirect, 6)]
        ORAIndX = 0x01,

        [OpCodesMetadata(AddressingMode.IndirectIndexed, 5, true)]
        ORAIndY = 0x11,


        #endregion

        //Move each of the bits in either A or M one place to the left. Bit 0 is filled with the current value of the carry flag whilst the old bit 7 becomes the new carry flag value.
        #region ROL

        [OpCodesMetadata(AddressingMode.Accumulator, 2)]
        ROLAccum = 0x2A,

        [OpCodesMetadata(AddressingMode.ZeroPage, 5)]
        ROLZP = 0x26,

        [OpCodesMetadata(AddressingMode.ZeroPageX, 6)]
        ROLZPX = 0x36,

        [OpCodesMetadata(AddressingMode.Absolute, 6)]
        ROLAbs = 0x2E,

        [OpCodesMetadata(AddressingMode.AbsoluteX, 7)]
        ROLAbsX = 0x3E,

        #endregion

        //Rotate Right. Move each of the bits in either A or M one place to the right. Bit 7 is filled with the current value of the carry flag whilst the old bit 0 becomes the new carry flag value.
        #region ROR

        [OpCodesMetadata(AddressingMode.Accumulator, 2)]
        RORAccum = 0x6A,

        [OpCodesMetadata(AddressingMode.ZeroPage, 5)]
        RORZP = 0x66,

        [OpCodesMetadata(AddressingMode.ZeroPageX, 6)]
        RORZPX = 0x76,

        [OpCodesMetadata(AddressingMode.Absolute, 6)]
        RORAbs = 0x6E,

        [OpCodesMetadata(AddressingMode.AbsoluteX, 7)]
        RORAbsX = 0x7E,

        #endregion

        //A,Z,C,N = A-M-(1-C) This instruction subtracts the contents of a memory location to the accumulator together with
        //the not of the carry bit. If overflow occurs the carry bit is clear, this enables multiple byte subtraction to be performed.
        //Processor Status after use:
        #region SBC Subtract with Carry

        [OpCodesMetadata(AddressingMode.Immediate, 2)]
        SBCImm = 0xE9,

        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        SBCZP = 0xE5,

        [OpCodesMetadata(AddressingMode.ZeroPageX, 4)]
        SBCZPX = 0xF5,

        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        SBCAbs = 0xED,

        [OpCodesMetadata(AddressingMode.AbsoluteX, 4, true)]
        SBCAbsX = 0xFD,

        [OpCodesMetadata(AddressingMode.AbsoluteY, 4, true)]
        SBCAbsY = 0xF9,

        [OpCodesMetadata(AddressingMode.IndexedIndirect, 6)]
        SBCIndX = 0xE1,

        [OpCodesMetadata(AddressingMode.IndirectIndexed, 5, true)]
        SBCIndY = 0xF1,


        #endregion

        /// <summary>
        /// Stores the contents of the accumulator into memory.
        /// </summary>
        #region STA - Store Accumulator

        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        STAZP = 0x85,

        [OpCodesMetadata(AddressingMode.ZeroPageX, 4)]
        STAZPX = 0x95,

        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        STAAbs = 0x8D,

        [OpCodesMetadata(AddressingMode.AbsoluteX, 5)]
        STAAbsX = 0x9D,

        [OpCodesMetadata(AddressingMode.AbsoluteY, 5)]
        STAAbsY = 0x99,

        [OpCodesMetadata(AddressingMode.IndexedIndirect, 6)]
        STAIndX = 0x81,

        [OpCodesMetadata(AddressingMode.IndirectIndexed, 6)]
        STAIndY = 0x91,

        #endregion

        //Stores the contents of the X/Y register into memory.
        #region STX/Y Store X/Y Register

        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        STXZP = 0x86,

        [OpCodesMetadata(AddressingMode.ZeroPageY, 4)]
        STXZPY = 0x96,

        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        STXAbs = 0x8E,

        [OpCodesMetadata(AddressingMode.ZeroPage, 3)]
        STYZP = 0x84,

        [OpCodesMetadata(AddressingMode.ZeroPageX, 4)]
        STYZPX = 0x94,

        [OpCodesMetadata(AddressingMode.Absolute, 4)]
        STYAbs = 0x8C,

        #endregion

        #region Transfers

        /// <summary>
        /// Transfer Accumulator to X. X = A. Copies the current contents of the accumulator into the X register and sets the zero and negative flags as appropriate.
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        TAX = 0xAA,

        /// <summary>
        /// Transfer Accumulator to Y. Y = A. Copies the current contents of the accumulator into the Y register and sets the zero and negative flags as appropriate.
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        TAY = 0xA8,

        /// <summary>
        /// Transfer Stack Pointer to X. X = S. Copies the current contents of the stack register into the X register and sets the zero and negative flags as appropriate.
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        TSX = 0xBA,

        /// <summary>
        /// Transfer X to Accumulator. A = X. Copies the current contents of the X register into the accumulator and sets the zero and negative flags as appropriate.
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        TXA = 0x8A,
        
        /// <summary>
        /// Transfer X to Stack Pointer. S = X. Copies the current contents of the X register into the stack register.
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        TXS = 0x9A,

        /// <summary>
        /// Transfer Y to Accumulator. A = Y. Copies the current contents of the Y register into the accumulator and sets the zero and negative flags as appropriate.
        /// </summary>
        [OpCodesMetadata(AddressingMode.Implicit, 2)]
        TYA  = 0x98,
        #endregion
    }
}