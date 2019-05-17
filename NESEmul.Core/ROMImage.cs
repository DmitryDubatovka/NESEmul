using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NESEmul.Core.Exceptions;

namespace NESEmul.Core
{
    public class ROMImage
    {
        #region properties

        /// <summary>
        /// Number of 16kB ROM banks.
        /// </summary>
        public byte ROMBanksNumber { get; private set; }

        /// <summary>
        /// Number of 8kB CHRROM banks
        /// </summary>
        public byte CHRROMBanksNumber { get; private set; }

        public byte RAMBanksNumber { get; private set; }

        /// <summary>
        /// true for vertical mirroring, false for horizontal mirroring.
        /// </summary>
        public bool IsVerticalMirroring { get; private set; }

        /// <summary>
        /// true for battery-backed RAM at $6000-$7FFF.
        /// </summary>
        public bool IsBatteryBackedRAM { get; private set; }

        /// <summary>
        /// true for a four-screen VRAM layout.
        /// </summary>
        public bool FourScreenVRAM { get; private set; }

        /// <summary>
        /// true for a 512-byte trainer at $7000-$71FF.
        /// </summary>
        public bool TrainerPresent { get; private set; }

        /// <summary>
        /// true for VS-System cartridges.
        /// </summary>
        public bool VSSystemCartridge { get; private set; }

        /// <summary>
        ///  true for PAL cartridges, otherwise assume NTSC.
        /// </summary>
        public bool IsPALCartridge { get; private set; }

        public byte MapperType { get; private set; }

        public List<byte[]> ROMBanks { get; private set; }

        //8kB CHRROM banks
        public List<byte[]> CHRROMBanks { get; private set; }
        #endregion

        public void Load(Stream inputFile)
        {
            using (BinaryReader r = new BinaryReader(inputFile))
            {
                ReadHeader(r);
                if (TrainerPresent)
                    r.ReadBytes(512);
                ROMBanks = new List<byte[]>(ROMBanksNumber);
                while (ROMBanks.Count < ROMBanksNumber)
                {
                    var romBank = r.ReadBytes(16 * 1024);
                    ROMBanks.Add(romBank);
                }
                CHRROMBanks = new List<byte[]>(CHRROMBanksNumber);
                while (CHRROMBanks.Count < CHRROMBanksNumber)
                {
                    CHRROMBanks.Add(r.ReadBytes(8 *1024));
                }
            }

        }

        private void ReadHeader(BinaryReader reader)
        {
            byte[] header = reader.ReadBytes(16);
            var nesString = Encoding.ASCII.GetString(header, 0, 3);
            if (nesString != "NES" || header[3] != 0x1A)
                throw new InvalidROMFile($"Invalid header string {nesString}");
            ROMBanksNumber = header[4];
            CHRROMBanksNumber = header[5];
            var flags = header[6];
            IsVerticalMirroring = (flags & 0x01) == 0x01;
            IsBatteryBackedRAM = (flags & 0x02) == 0x02;
            TrainerPresent = (flags & 0x04) == 0x04;
            FourScreenVRAM = (flags & 0x08) == 0x08;
            MapperType = (byte) (flags & 0xF0);
            flags = header[7];
            VSSystemCartridge = (flags & 0x01) == 0x01;
            if((flags & 0x0E) != 0)
                throw new InvalidROMFile("Bits 1-3 of the 7th byte of header are not zeroes");
            byte lowerBytesOfMapperType = (byte) (MapperType >> 4);
            MapperType = (byte) (flags & 0xF0);
            MapperType += lowerBytesOfMapperType;
            RAMBanksNumber = header[8];
            flags = header[9];
            IsPALCartridge = (flags & 0x01) == 0x01;
            if((flags & 0xFE) != 0)
                throw new InvalidROMFile("Bits 1-7 of the 9th byte of header are not zeroes");
            if(header.Skip(10).Any(p => p != 0))
                throw new InvalidROMFile("Bytes 10-15 of header are not zeroes");
        }
    }
}