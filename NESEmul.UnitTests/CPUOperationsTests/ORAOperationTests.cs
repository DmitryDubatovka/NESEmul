using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class ORAOperationTests : OperationBaseTests
    {
        [Test]
        public void ORAImmTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.ORAImm);
            CPU.Accumulator = 0x01;
            Memory.StoreByteInMemory(1, 0x10);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x11));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }
        
        [Test]
        public void ORAZPTest()
        {
            InitZPMode(OpCodes.ORAZP, 0xFF);
            CPU.Accumulator = 0x01;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0xFF));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true), "Negative flag");
        }
        
        [Test]
        public void ORAZPXTest()
        {
            InitZPXMode(OpCodes.ORAZPX, 0x0);
            CPU.Accumulator = 0x00;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x00));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false), "Negative flag");
        }

        [Test]
        public void ORAAbsTest()
        {
            InitAbsMode(OpCodes.ORAAbs, 0x80);
            CPU.Accumulator = 0x01;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x81));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true), "Negative flag");
        }
    }
}