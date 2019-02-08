namespace NESEmul.Core
{
    public enum OpCodes : byte
    {
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
        
        ADCIndY = 0x71

        #endregion
        
    }
}