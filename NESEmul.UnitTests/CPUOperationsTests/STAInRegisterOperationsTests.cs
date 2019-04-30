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
            CPU.Accumulator = 0x20;
            InitZPMode(OpCodes.STAZP, 0x0, 0, 0x10);
            Assert.That(Memory.LoadByteFromMemory(0x10), Is.EqualTo(0x0));
            CPU.Do();
            Assert.That(Memory.LoadByteFromMemory(0x10), Is.EqualTo(0x20));
        }
        
        [Test]
        public void STAZPXOperationTest()
        {
            CPU.Accumulator = 0x20;
            InitZPXMode(OpCodes.STAZPX, 0x0, 0x1, 0, 0xF);
            Assert.That(Memory.LoadByteFromMemory(0x10), Is.EqualTo(0x0));
            CPU.Do();
            Assert.That(Memory.LoadByteFromMemory(0x10), Is.EqualTo(0x20));
        }
        
        [Test]
        public void STAAbsOperationTest()
        {
            CPU.Accumulator = 0x20;
            InitAbsMode(OpCodes.STAAbs, 0, 0, 1, 2);
            Assert.That(Memory.LoadByteFromMemory(0x0102), Is.EqualTo(0x0));
            CPU.Do();
            Assert.That(Memory.LoadByteFromMemory(0x0102), Is.EqualTo(0x20));
        }
        
        [Test]
        public void STAAbsXOperationTest()
        {
            CPU.Accumulator = 0x20;
            InitAbsXMode(OpCodes.STAAbsX, 0x0, 1, 0, 0x01, 0x02);
            Assert.That(Memory.LoadByteFromMemory(0x0103), Is.EqualTo(0x0));
            CPU.Do();
            Assert.That(Memory.LoadByteFromMemory(0x0103), Is.EqualTo(0x20));
        }

        [Test]
        public void STAAbsYOperationTest()
        {
            CPU.Accumulator = 0x20;
            InitAbsYMode(OpCodes.STAAbsY, 0x0, 1, 0, 0x01, 0x02);
            Assert.That(Memory.LoadByteFromMemory(0x0103), Is.EqualTo(0x0));
            CPU.Do();
            Assert.That(Memory.LoadByteFromMemory(0x0103), Is.EqualTo(0x20));
        }
        
        [Test]
        public void STAIndXOperationTest()
        {
            CPU.Accumulator = 0x20;
            Memory.StoreByteInMemory(0, (byte)OpCodes.STAIndX);
            CPU.IndexRegisterX = 0xFF;
            Memory.StoreByteInMemory(0x1, 0x10);
            Memory.StoreByteInMemory(0x0F, 0x02);
            Memory.StoreByteInMemory(0x10, 0x04);
            Assert.That(Memory.LoadByteFromMemory(0x0402), Is.EqualTo(0x0));
            CPU.Do();
            Assert.That(Memory.LoadByteFromMemory(0x0402), Is.EqualTo(0x20));
        }
        
        [Test]
        public void STAIndYOperationTest()
        {
            CPU.Accumulator = 0x20;
            Memory.StoreByteInMemory(0, (byte) OpCodes.STAIndY);
            CPU.IndexRegisterY = 0xFF;
            Memory.StoreByteInMemory(0x1, 0x10);
            Memory.StoreByteInMemory(0x10, 0x04);
            Memory.StoreByteInMemory(0x11, 0x02);
            Assert.That(Memory.LoadByteFromMemory(0x0303), Is.EqualTo(0x0));
            CPU.Do();
            Assert.That(Memory.LoadByteFromMemory(0x0303), Is.EqualTo(0x20));
        }

        [Test]
        public void STXZPOperationTest()
        {
            CPU.IndexRegisterX = 0x81;
            InitZPMode(OpCodes.STXZP, 0x0, 0, 0x10);
            Assert.That(Memory.LoadByteFromMemory(0x10), Is.EqualTo(0x0));
            CPU.Do();
            Assert.That(Memory.LoadByteFromMemory(0x10), Is.EqualTo(0x81));
        }
        
        [Test]
        public void STYZPOperationTest()
        {
            CPU.IndexRegisterY = 0x82;
            InitZPMode(OpCodes.STYZP, 0x0, 0, 0x10);
            Assert.That(Memory.LoadByteFromMemory(0x10), Is.EqualTo(0x0));
            CPU.Do();
            Assert.That(Memory.LoadByteFromMemory(0x10), Is.EqualTo(0x82));
        }

        [Test]
        public void STXAbsOperationTest()
        {
            CPU.IndexRegisterX = 0x30;
            InitAbsMode(OpCodes.STXAbs, 0, 0, 1, 2);
            Assert.That(Memory.LoadByteFromMemory(0x0102), Is.EqualTo(0x0));
            CPU.Do();
            Assert.That(Memory.LoadByteFromMemory(0x0102), Is.EqualTo(0x30));
        }
        
        [Test]
        public void STYAbsOperationTest()
        {
            CPU.IndexRegisterY = 0x32;
            InitAbsMode(OpCodes.STYAbs, 0, 0, 1, 2);
            Assert.That(Memory.LoadByteFromMemory(0x0102), Is.EqualTo(0x0));
            CPU.Do();
            Assert.That(Memory.LoadByteFromMemory(0x0102), Is.EqualTo(0x32));
        }

        [Test]
        public void STXZPYOperationTest()
        {
            CPU.IndexRegisterX = 0x72;
            InitZPYMode(OpCodes.STXZPY, 0x0, 0x1, 0, 0xF);
            Assert.That(Memory.LoadByteFromMemory(0x10), Is.EqualTo(0x0));
            CPU.Do();
            Assert.That(Memory.LoadByteFromMemory(0x10), Is.EqualTo(0x72));

           
        }
        
        [Test]
        public void STYZPXOperationTest()
        {
            CPU.IndexRegisterY = 0x70;
            InitZPXMode(OpCodes.STYZPX, 0x0, 0x1, 0, 0xF);
            Assert.That(Memory.LoadByteFromMemory(0x10), Is.EqualTo(0x0));
            CPU.Do();
            Assert.That(Memory.LoadByteFromMemory(0x10), Is.EqualTo(0x70));
        }
    }
}