using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class LDXOperationTests : OperationBaseTests
    {
        [Test]
        public void LDXImmTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.LDXImm);
            CPU.IndexRegisterX = 0;
            Memory.StoreByteInMemory(1, 0x10);
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0x10));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void LDXZPTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.LDXZP);
            CPU.IndexRegisterX = 0x1;
            Memory.StoreByteInMemory(1, 0xF);
            Memory.StoreByteInMemory(0xF, 0x0);
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0x0));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void LDXAbsYTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.LDXAbsY);
            CPU.IndexRegisterY = 0x1;
            CPU.IndexRegisterX = 0x1;
            Memory.StoreByteInMemory(1, 0x02);
            Memory.StoreByteInMemory(2, 0x01);
            Memory.StoreByteInMemory(0x0103, 0x3);
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0x3));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void LDXZPYTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.LDXZPY);
            CPU.IndexRegisterY = 0x2;
            CPU.IndexRegisterX = 0x1;
            Memory.StoreByteInMemory(1, 0x02);
            Memory.StoreByteInMemory(0x04, 0xFE);
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0xFE));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void LDXAbsTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.LDXAbs);
            CPU.IndexRegisterX = 0x1;
            Memory.StoreByteInMemory(1, 0x02);
            Memory.StoreByteInMemory(2, 0x01);
            Memory.StoreByteInMemory(0x0102, 0x00);
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0x00));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true));
        }
    }
}