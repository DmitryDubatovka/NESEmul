using NESEmul.Core.Exceptions;

namespace NESEmul.Core
{
    /// <summary>
    /// Picture processing unit
    /// </summary>
    public class PPU
    {
        public static readonly PPU Instance;

        #region ctors

        private PPU(){}

        static PPU()
        {
            Instance = new PPU();
        }

        #endregion

        #region constanst

        public const int PPUCTRLAddress = 0x2000;

        public const int PPUMaskAddress = 0x2001;

        /// <summary>
        /// read resets write pair for $2005/$2006
        /// </summary>
        public const int PPUStatusAddress = 0x2002;

        /// <summary>
        /// OAM read/write address
        /// </summary>
        public const int OAMAddrAddress = 0x2003;

        /// <summary>
        /// OAM data read/write
        /// </summary>
        public const int OAMDataAddress = 0x2004;

        /// <summary>
        /// fine scroll position (two writes: X scroll, Y scroll)
        /// </summary>
        public const int PPUScrollAddress = 0x2005;
        
        /// <summary>
        /// PPU read/write address (two writes: most significant byte, least significant byte)
        /// </summary>
        public const int PPUAddrAddress = 0x2006;
        
        /// <summary>
        /// PPU data read/write
        /// </summary>
        public const int PPUDataAddress = 0x2007;
        
        /// <summary>
        /// OAM DMA high address
        /// </summary>
        public const int OAMDMAAddress = 0x2008;

        #endregion
        

        #region PPUCTRL flags

        /// <summary>
        /// 7th bit of the PPUCTRL register
        /// </summary>
        private bool _nmiEnable;

        /// <summary>
        /// 6th bit of the PPUCTRL register
        /// </summary>
        private bool _ppuMasterSlave;

        /// <summary>
        /// 5th bit of the PPUCTRL register
        /// </summary>
        private bool _spriteHeight;

        /// <summary>
        /// 4th bit of the PPUCTRL register
        /// </summary>
        private bool _backgroundTileSelect;
        
        /// <summary>
        /// 3rd bit of the PPUCTRL register
        /// </summary>
        private bool _spriteTileSelect;
        
        /// <summary>
        /// 2nd bit of the PPUCTRL register
        /// </summary>
        private bool _incrementMode;

        /// <summary>
        /// 1st and 0 bits of the PPUCTRL register
        /// </summary>
        private byte _nameTableSelect;

        #endregion

        public byte PPUCTRL
        {
            get
            {
                byte result = (byte) (_nmiEnable ? 1 : 0);
                result <<= 1;
                
                result += (byte)(_ppuMasterSlave ? 0 : 1);
                result <<= 1;

                result += (byte)(_spriteHeight ? 0 : 1);
                result <<= 1;

                result += (byte)(_backgroundTileSelect ? 0 : 1);
                result <<= 1;

                result += (byte)(_spriteTileSelect ? 0 : 1);
                result <<= 1;

                result += (byte)(_incrementMode ? 0 : 1);
                result <<= 2;

                if(_nameTableSelect > 3)
                    throw new ApplicationException($"Invalid nametable value {_nameTableSelect}");
                
                result += _nameTableSelect;
                return result;
            }
            set
            {
                _nmiEnable = (value & 0x80) != 0;
                _ppuMasterSlave = (value & 0x40) != 0;
                _spriteHeight = (value & 0x20) != 0;
                _backgroundTileSelect = (value & 0x10) != 0;
                _spriteTileSelect = (value & 0x8) != 0;
                _incrementMode = (value & 0x4) != 0;
                _nameTableSelect = (byte) (value & 0x3);
            }
        }

        public static bool InPPURegistersAddress(int address)
        {
            return address >= PPUAddrAddress && address <= OAMDMAAddress;
        }
    }
}