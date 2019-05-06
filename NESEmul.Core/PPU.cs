using NESEmul.Core.Exceptions;

namespace NESEmul.Core
{
    /// <summary>
    /// Picture processing unit
    /// </summary>
    public class PPU
    {
        private PPUSpriteMemory _spriteMemory;

        public static readonly PPU Instance;

        #region ctors

        private PPU()
        {
            _spriteMemory = PPUSpriteMemory.Instance;
        }

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
        public const int SpriteMemoryAddrAddress = 0x2003;

        /// <summary>
        /// OAM data read/write
        /// </summary>
        public const int SpriteDataAddress = 0x2004;

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

        #region PPUMaskAddress
        //zero byte is not used
        /// <summary>
        /// Image Mask, 0 = don't show left 8 columns of the screen. Bit 1
        /// </summary>
        private bool _imageMaskEnabled { get; set; }

        /// <summary>
        /// Sprite Mask, false = don't show sprites in left 8 columns. Bit 2
        /// </summary>
        private bool _spriteMaskEnabled { get; set; }

        /// <summary>
        /// Screen Enable, true = show picture, false = blank screen. Bit 3
        /// </summary>
        private bool _screenEnabled { get; set; }

        /// <summary>
        /// Sprites Enable, true = show sprites, false = hide sprites. Bit 4
        /// </summary>
        private bool _spritesEnabled { get; set; }

        /// <summary>
        /// Background Color, 0 = black, 1 = blue, 2 = green, 4 = red. Do not use any other numbers. Bits 5-7
        /// </summary>
        private byte _backgroundColor { get; set; }

        #endregion

        #region PPU Status Register

        /// <summary>
        /// Hit Flag, true = Sprite refresh has hit sprite #0. This flag resets to false when screen refresh starts
        /// </summary>
        private bool _hitFlag { get; set; }

        /// <summary>
        /// VBlank Flag, true = PPU is in VBlank state. This flag resets to false when VBlank ends or CPU reads $2002
        /// </summary>
        private bool _vblankFlag { get; set; }

        #endregion

        public byte PPUCTRL
        {
            get
            {
                byte result = (byte) (_nmiEnable ? 1 : 0);
                result <<= 1;
                
                result += (byte)(_ppuMasterSlave ? 1 : 0);
                result <<= 1;

                result += (byte)(_spriteHeight ? 1 : 0);
                result <<= 1;

                result += (byte)(_backgroundTileSelect ? 1 : 0);
                result <<= 1;

                result += (byte)(_spriteTileSelect ? 1 : 0);
                result <<= 1;

                result += (byte)(_incrementMode ? 1 : 0);
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

        public byte PPUMask
        {
            get
            {
                byte result = _backgroundColor;
                result <<= 3;
                
                result += (byte)(_spritesEnabled ? 1 : 0);
                result <<= 1;

                result += (byte)(_screenEnabled ? 1 : 0);
                result <<= 1;

                result += (byte)(_spriteMaskEnabled ? 1 : 0);
                result <<= 1;

                result += (byte)(_imageMaskEnabled ? 1 : 0);
                result <<= 1;

                result += 0; //zero bit is not used
                result <<= 1;

                return result;
            }
            set
            {
                _backgroundColor = (byte) ((value & 0xE0) >> 5);
                if(_backgroundColor > 4 || _backgroundColor == 3)
                    throw new ApplicationException($"Background color in the PPUCTRL2 equals {_backgroundColor}");
                _spritesEnabled = (value & 0x10) != 0;
                _screenEnabled = (value & 0x8) != 0;
                _spriteMaskEnabled = (value & 0x4) != 0;

                _imageMaskEnabled = (value & 0x2) != 0;
            }
        }

        public byte PPUStatusRegister
        {
            get
            {
                byte result = (byte)(_vblankFlag ? 1 : 0);
                result <<= 1;

                result += (byte)(_hitFlag ? 1 : 0);
                result <<= 1;

                result += 0; // Bits 0-5 are not used
                result <<= 5;
                return result;
            }
            private set
            {
                _vblankFlag = (value & 0x80) != 0;
                _hitFlag = (value & 0x40) != 0;
                if((value & 0x3F) > 0)
                    throw new ApplicationException($"PPU status register has values in bits 0-5. Value {value}");
            }
        }

        public byte SpriteMemoryAddress { private get; set; }
        public static bool InPPURegistersAddress(int address)
        {
            return address >= PPUCTRLAddress && address <= OAMDMAAddress;
        }

        public void WriteToRegister(int address, byte value)
        {
            switch (address)
            {
                case PPUCTRLAddress:
                    PPUCTRL = value;
                    return;
                case PPUMaskAddress:
                    PPUMask = value;
                    return;
                case SpriteMemoryAddrAddress:
                    SpriteMemoryAddress = value;
                    return;
                case  SpriteDataAddress:
                    _spriteMemory.StoreByteInMemory(SpriteMemoryAddress, value);
                    SpriteMemoryAddress++;
                    return;
            }
            
        }

        public byte ReadFromRegister(int address)
        {
            switch (address)
            {
                case PPUCTRLAddress:
                    return PPUCTRL;
                case PPUMaskAddress:
                    return PPUMask;
                case PPUStatusAddress:
                    return PPUStatusRegister;
                case  SpriteDataAddress:
                    var val = _spriteMemory.LoadByteFromMemory(SpriteMemoryAddress);
                    SpriteMemoryAddress++;
                    return val;
            }
            throw new ApplicationException($"Invalid address of PPU register {address}");
        }
    }
}