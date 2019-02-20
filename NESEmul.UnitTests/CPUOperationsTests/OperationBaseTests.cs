using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    public class OperationBaseTests
    {
        protected CPU CPU;
        protected Memory Memory;

        [SetUp]
        protected void Setup()
        {
            Memory = new Memory();
            CPU = new CPU(0, Memory);
        }

        protected void InitZPMode(OpCodes opCode, byte value, int opCodeAddress = 0x0, byte operandAddress = 0xF)
        {
            Memory.StoreByteInMemory(opCodeAddress, (byte)opCode);
            Memory.StoreByteInMemory(opCodeAddress + 1, operandAddress);
            Memory.StoreByteInMemory(operandAddress, value);
        }

        protected void InitZPXMode(OpCodes opCode, byte value, byte xRegisterValue = 0x1, int opCodeAddress = 0x0, byte operandAddress = 0xE)
        {
            Memory.StoreByteInMemory(opCodeAddress, (byte)opCode);
            CPU.IndexRegisterX = xRegisterValue;
            Memory.StoreByteInMemory(opCodeAddress + 1, operandAddress);
            Memory.StoreByteInMemory(operandAddress + xRegisterValue, value);
        }
        
        protected void InitZPYMode(OpCodes opCode, byte value, byte yRegisterValue = 0x1, int opCodeAddress = 0x0, byte operandAddress = 0xE)
        {
            Memory.StoreByteInMemory(opCodeAddress, (byte)opCode);
            CPU.IndexRegisterY = yRegisterValue;
            Memory.StoreByteInMemory(opCodeAddress + 1, operandAddress);
            Memory.StoreByteInMemory(operandAddress + yRegisterValue, value);
        }

        protected void InitAbsMode(OpCodes opCode, byte value, int opCodeAddress = 0x0, byte hiByte = 0x01, byte loByte = 0x02)
        {
            Memory.StoreByteInMemory(opCodeAddress, (byte)opCode);
            Memory.StoreByteInMemory(opCodeAddress + 1, loByte);
            Memory.StoreByteInMemory(opCodeAddress + 2, hiByte);
            Memory.StoreByteInMemory((hiByte << 8) + loByte, value);
        }
        
        protected void InitAbsXMode(OpCodes opCode, byte value, byte xRegisterValue = 0x1, int opCodeAddress = 0x0, byte hiByte = 0x01, byte loByte = 0x02)
        {
            CPU.IndexRegisterX = xRegisterValue;
            Memory.StoreByteInMemory(opCodeAddress, (byte)opCode);
            Memory.StoreByteInMemory(opCodeAddress + 1, loByte);
            Memory.StoreByteInMemory(opCodeAddress + 2, hiByte);
            Memory.StoreByteInMemory((hiByte << 8) + loByte + xRegisterValue, value);
        }
        
        protected void InitAbsYMode(OpCodes opCode, byte value, byte yRegisterValue = 0x1, int opCodeAddress = 0x0, byte hiByte = 0x01, byte loByte = 0x02)
        {
            CPU.IndexRegisterY = yRegisterValue;
            Memory.StoreByteInMemory(opCodeAddress, (byte)opCode);
            Memory.StoreByteInMemory(opCodeAddress + 1, loByte);
            Memory.StoreByteInMemory(opCodeAddress + 2, hiByte);
            Memory.StoreByteInMemory((hiByte << 8) + loByte + yRegisterValue, value);
        }
    }
}