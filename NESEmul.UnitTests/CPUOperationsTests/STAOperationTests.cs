using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class STAOperationTests : OperationBaseTests
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
    }
}