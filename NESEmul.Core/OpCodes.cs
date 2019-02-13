namespace NESEmul.Core
{
    public enum OpCodes : byte
    {
        /// <summary>
        /// Force break
        /// </summary>
        BRK = 0x0,

        //Add with Carry
        #region ADC

        /// <summary>
        /// Immediate addressing mode
        /// </summary>
        ADCIm = 0x69,
        
        /// <summary>
        /// Zero Page addressing mode
        /// </summary>
        ADCZP = 0x65,
        
        /// <summary>
        /// Zero Page X addressing mode
        /// </summary>
        ADCZPX = 0x75,
        
        /// <summary>
        /// Absolute addressing mode
        /// </summary>
        ADCAbs = 0x6D,
        
        /// <summary>
        /// Absolute addressing X mode
        /// </summary>
        ADCAbsX = 0x7D,
        
        /// <summary>
        /// Absolute addressing Y mode
        /// </summary>
        ADCAbsY = 0x79,
        
        /// <summary>
        /// Indexed Indirect addressing mode
        /// </summary>
        ADCIndX = 0x61,
        
        /// <summary>
        /// Indirect indexed addressing mode
        /// </summary>
        ADCIndY = 0x71,

        #endregion

        //bitwise AND with accumulator
        #region AND

        /// <summary>
        /// Immediate addressing mode
        /// </summary>
        ANDIm = 0x29,

        /// <summary>
        /// Zero page addressing mode
        /// </summary>
        ANDZP = 0x25,

        /// <summary>
        /// Zero page X addressing mode
        /// </summary>
        ANDZPX = 0x35,

        /// <summary>
        /// Absolute addressing mode
        /// </summary>
        ANDAbs = 0x2D,

        /// <summary>
        /// Absolute addressing X mode
        /// </summary>
        ANDAbsX = 0x3D,

        /// <summary>
        /// Absolute addressing Y mode
        /// </summary>
        ANDAbsY = 0x39,

        /// <summary>
        /// Indexed Indirect addressing mode
        /// </summary>
        ANDIndX = 0x21,
        
        /// <summary>
        /// Indirect indexed addressing mode
        /// </summary>
        ANDIndY = 0x31,



        #endregion

        //Arithmetic Shift Left
        //This operation shifts all the bits of the accumulator or memory contents one bit left. Bit 0 is set to 0 and bit 7 is placed in the carry flag.
        #region ASL

        /// <summary>
        /// Accumulator addressing mode
        /// </summary>
        ASLAccum = 0x0A,

        /// <summary>
        /// Zero Page addressing mode
        /// </summary>
        ASLZP = 0x06,

        /// <summary>
        /// Zero Page X addressing mode
        /// </summary>
        ASLZPX = 0x16,

        /// <summary>
        /// Absolute addressing mode
        /// </summary>
        ASLAbs = 0x0E,


        /// <summary>
        /// Absolute X addressing mode
        /// </summary>
        ASLAbsX = 0x1E,


        #endregion

        //Relative addressing mode is used by branch instructions (e.g. BEQ, BNE, etc.) which contain a signed 8 bit relative offset (e.g. -128 to +127) which is added to program counter if the condition is true.
        //As the program counter itself is incremented during instruction execution by two the effective address range for the target instruction must be with -126 to +129 bytes of the branch.
        #region Branches

        /// <summary>
        /// Branch on Plus
        /// </summary>
        BPL = 0x10,

        /// <summary>
        /// Branch on Minus
        /// </summary>
        BMI = 0x30,

        /// <summary>
        /// Branch on oVerflow Clear
        /// </summary>
        BVC = 0x50,

        /// <summary>
        /// Branch on oVerflow Set
        /// </summary>
        BVS = 0x70,

        /// <summary>
        /// Branch on Carry Clear
        /// </summary>
        BCC = 0x90,

        /// <summary>
        /// Branch on Carry Set
        /// </summary>
        BCS = 0xB0,

        /// <summary>
        /// Branch on Not Equal
        /// </summary>
        BNE = 0xD0,

        /// <summary>
        /// Branch on EQual
        /// </summary>
        BEQ = 0xF0
        #endregion
    }
}