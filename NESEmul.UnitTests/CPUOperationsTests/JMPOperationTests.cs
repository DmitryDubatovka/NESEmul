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
            InitAbsMode(OpCodes.JmpAbs, (byte)OpCodes.INX, 0, 0x03);
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

        [Test]
        public void JSRAndRTSTest()
        {
            var cpu = new CPU(0x0600, Memory) {IndexRegisterX = 0x1, IndexRegisterY = 0x5};
            Memory.StoreByteInMemory(0x0600, (byte)OpCodes.NOP);
            Memory.StoreByteInMemory(0x0601, (byte)OpCodes.NOP);
            Memory.StoreByteInMemory(0x0602, (byte)OpCodes.JSR);
            Memory.StoreByteInMemory(0x0603, 0x09);
            Memory.StoreByteInMemory(0x0604, 0x06);
            Memory.StoreByteInMemory(0x0605, (byte)OpCodes.DEY);
            Memory.StoreByteInMemory(0x0606, (byte)OpCodes.BRK);
            Memory.StoreByteInMemory(0x0609, (byte)OpCodes.INX);
            Memory.StoreByteInMemory(0x060A, (byte)OpCodes.RTS);
            cpu.Do();
            Assert.That(cpu.IndexRegisterX, Is.EqualTo(0x2));
            Assert.That(cpu.IndexRegisterY, Is.EqualTo(0x4));
        }

        [Test]
        public void RTIOperationTest()
        {
            var cpu = new CPU(0x0600, Memory) {Accumulator = 0x6};
            Memory.StoreByteInMemory(0x0600, (byte)OpCodes.PHA);
            Memory.StoreByteInMemory(0x0601, (byte)OpCodes.EORImm);
            Memory.StoreByteInMemory(0x0602, 0xD); //0x6 ^ 0xD = 0xB
            Memory.StoreByteInMemory(0x0603, (byte)OpCodes.PHA);
            Memory.StoreByteInMemory(0x0604, (byte)OpCodes.ORAImm);
            Memory.StoreByteInMemory(0x0605, 0x06);
            Memory.StoreByteInMemory(0x0606, (byte)OpCodes.PHA);
            Memory.StoreByteInMemory(0x0607, (byte)OpCodes.RTI);
            Memory.StoreByteInMemory(0x060B, (byte)OpCodes.DEX);
            cpu.Do();

            Assert.That(cpu.IndexRegisterX, Is.EqualTo(0xFF));
            Assert.That(cpu.CarryFlag, Is.EqualTo(true));
            Assert.That(cpu.InterruptDisable, Is.EqualTo(true));
            Assert.That(cpu.DecimalMode, Is.EqualTo(true));
        }
    }
}