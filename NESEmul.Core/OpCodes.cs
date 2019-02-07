namespace NESEmul.Core
{
    public enum OpCodes : byte
    {
        #region ADC

        ADCIm = 0x69,
        ADCZP = 0x65,
        ADCZPX = 0x75,
        ADCAbs = 0x6D,
        ADCAbsX = 0x7D,
        ADCAbsY = 0x79,
        ADCIndX = 0x61,
        ADCIndY = 0x71

        #endregion
        
    }
}