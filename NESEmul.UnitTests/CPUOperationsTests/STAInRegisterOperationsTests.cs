using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class STAInRegisterOperationsTests : OperationBaseTests
    {
        [Test]
        public void STAZPOperationTest()
        {
            InitZPMode(OpCodes.STAZP, 0x80);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x80));
        }
        
        [Test]
        public void STAZPXOperationTest()
        {
            InitZPXMode(OpCodes.STAZPX, 0x80);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x80));
        }
        
        [Test]
        public void STAAbsOperationTest()
        {
            InitAbsMode(OpCodes.STAAbs, 0x80);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x80));
        }
        
        [Test]
        public void STAAbsXOperationTest()
        {
            InitAbsXMode(OpCodes.STAAbsX, 0x80);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x80));
        }
        
        [Test]
        public void STAAbsYOperationTest()
        {
            InitAbsYMode(OpCodes.STAAbsY, 0x80);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x80));
        }
        
        [Test]
        public void STAIndXOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.STAIndX);
            CPU.IndexRegisterX = 0xFF;
            Memory.StoreByteInMemory(0x1, 0x10);
            Memory.StoreByteInMemory(0x0F, 0x02);
            Memory.StoreByteInMemory(0x10, 0x04);
            Memory.StoreByteInMemory(0x0402, 0x80);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x80));
        }
        
        [Test]
        public void STAIndYOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte) OpCodes.STAIndY);
            CPU.IndexRegisterY = 0xFF;
            Memory.StoreByteInMemory(0x1, 0x10);
            Memory.StoreByteInMemory(0x10, 0x04);
            Memory.StoreByteInMemory(0x11, 0x02);
            Memory.StoreByteInMemory(0x303, 0x80);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x80));
        }

        [Test]
        public void STXZPOperationTest()
        {
            InitZPMode(OpCodes.STXZP, 0x81);
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0x81));
        }
        
        [Test]
        public void STYZPOperationTest()
        {
            InitZPMode(OpCodes.STYZP, 0x82);
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(0x82));
        }

        [Test]
        public void STXAbsOperationTest()
        {
            InitAbsMode(OpCodes.STXAbs, 0x30);
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0x30));
        }
        
        [Test]
        public void STYAbsOperationTest()
        {
            InitAbsMode(OpCodes.STYAbs, 0x32);
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(0x32));
        }

        [Test]
        public void STXZPYOperationTest()
        {
            InitZPYMode(OpCodes.STXZPY, 0x72);
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0x72));
        }
        
        [Test]
        public void STYZPXOperationTest()
        {
            InitZPXMode(OpCodes.STYZPX, 0x70);
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(0x70));
        }
    }
}