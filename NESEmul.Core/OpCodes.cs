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
        EORIndY = 0x51

        #endregion
    }
}