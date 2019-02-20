using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class LDXAndLDYOperationTests : OperationBaseTests
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
            InitZPMode(OpCodes.LDXZP, 0x0);
            CPU.IndexRegisterX = 0x1;
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0x0));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void LDXAbsYTest()
        {
            InitAbsYMode(OpCodes.LDXAbsY, 0x3);
            CPU.IndexRegisterX = 0x1;
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
            InitAbsMode(OpCodes.LDXAbs, 0x00);
            CPU.IndexRegisterX = 0x1;
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0x00));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true));
        }

        [Test]
        public void LDYImmTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.LDYImm);
            CPU.IndexRegisterY = 0;
            Memory.StoreByteInMemory(1, 0x10);
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(0x10));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void LDYZPTest()
        {
            InitZPMode(OpCodes.LDYZP, 0x0);
            CPU.IndexRegisterY = 0x1;
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(0x0));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void LDYAbsXTest()
        {
            InitAbsXMode(OpCodes.LDYAbsX, 0x3);
            CPU.IndexRegisterY = 0xFF;
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(0x3));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void LDYZPYTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.LDYZPX);
            CPU.IndexRegisterY = 0x1;
            CPU.IndexRegisterX = 0x2;
            Memory.StoreByteInMemory(1, 0x02);
            Memory.StoreByteInMemory(0x04, 0xFE);
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(0xFE));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void LDYAbsTest()
        {
            InitAbsMode(OpCodes.LDYAbs, 0x00);
            CPU.IndexRegisterY = 0x1;
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(0x00));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true));
        }
    }
}