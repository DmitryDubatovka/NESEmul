using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class JMPOperationTests : OperationBaseTests
    {
        [Test]
        public void JmpAbsTest()
        {
            CPU.IndexRegisterX = 0x1;
            Memory.StoreByteInMemory(0, (byte)OpCodes.JmpAbs);
            Memory.StoreByteInMemory(1, 0x02);
            Memory.StoreByteInMemory(2, 0x03);
            Memory.StoreByteInMemory(0x0302, (byte)OpCodes.INX);
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0x2));
            Assert.That(CPU.ProgramCounter, Is.EqualTo(0x0304));
        }
        
        [Test]
        public void JmpIndTest()
        {
            CPU.IndexRegisterY = 0x1;
            Memory.StoreByteInMemory(0, (byte)OpCodes.JmpInd);
            Memory.StoreByteInMemory(1, 0x01);
            Memory.StoreByteInMemory(2, 0x02);
            Memory.StoreByteInMemory(0x0201, 0x03);
            Memory.StoreByteInMemory(0x0202, 0x04);
            Memory.StoreByteInMemory(0x0403, (byte)OpCodes.INY);
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(0x2));
            Assert.That(CPU.ProgramCounter, Is.EqualTo(0x0405));
        }
    }
}