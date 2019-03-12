using System;
using System.Collections.Generic;

namespace NESEmul.Core
{
    public enum TileColors : byte
    {
        Transparent = 0x0,
        FirstColor = 0x1,
        SecondColor = 0x2,
        ThirdColor = 0x3
    }
    /// <summary>
    /// Represents 8x8 pixels block
    /// </summary>
    public class Tile
    {
        private readonly byte[] _data;
        public List<TileColors> Colors { get; private set; }

        public Tile(byte[] data)
        {
            _data = data;
            if(data == null || data.Length != 16)
                throw new ApplicationException("Invalid tile data");
            Colors = new List<TileColors>(64);
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < 8; i++)
            {
                var lowByte = _data[i];
                var hiByte = _data[i + 8];
                for (int bitIndex = 0; bitIndex < 8; bitIndex++) //iterate through all bits in byte
                {
                    var lowBit = (lowByte & 0x80) == 0x80 ? 1 : 0;
                    var hiBit = (hiByte & 0x80) == 0x80 ? 1 : 0;
                    var result = (byte)((hiBit << 1) + lowBit);
                    Colors.Add((TileColors)result);
                    lowByte <<= 1;
                    hiByte <<= 1;
                }
            }
        }
    }
}