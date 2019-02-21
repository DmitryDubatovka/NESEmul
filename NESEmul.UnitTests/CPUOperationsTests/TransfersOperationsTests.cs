using NESEmul.Core;
using NESEmul.UnitTests.TestCases;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class TransfersOperationsTests : OperationBaseTests
    {
        [TestCaseSource(typeof(CPUOperationsTestCases), nameof(CPUOperationsTestCases.TransferOperationsTestCases))]
        public void TAXOperationTest(byte value, bool zeroFlag, bool negativeFlag)
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.TAX);
            CPU.Accumulator = value;
            CPU.IndexRegisterX = 0xFF;
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(value));
            Assert.That(CPU.Accumulator, Is.EqualTo(value));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(zeroFlag));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(negativeFlag));
        }
        
        [TestCaseSource(typeof(CPUOperationsTestCases), nameof(CPUOperationsTestCases.TransferOperationsTestCases))]
        public void TXAOperationTest(byte value, bool zeroFlag, bool negativeFlag)
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.TXA);
            CPU.Accumulator = 0xFE;
            CPU.IndexRegisterX = value;
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(value));
            Assert.That(CPU.Accumulator, Is.EqualTo(value));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(zeroFlag));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(negativeFlag));
        }

        [TestCaseSource(typeof(CPUOperationsTestCases), nameof(CPUOperationsTestCases.TransferOperationsTestCases))]
        public void TAYOperationTest(byte value, bool zeroFlag, bool negativeFlag)
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.TAY);
            CPU.Accumulator = value;
            CPU.IndexRegisterY = 0xFD;
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(value));
            Assert.That(CPU.Accumulator, Is.EqualTo(value));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(zeroFlag));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(negativeFlag));
        }

        [TestCaseSource(typeof(CPUOperationsTestCases), nameof(CPUOperationsTestCases.TransferOperationsTestCases))]
        public void TYAOperationTest(byte value, bool zeroFlag, bool negativeFlag)
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.TYA);
            CPU.Accumulator = 0x0F;
            CPU.IndexRegisterY = value;
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(value));
            Assert.That(CPU.Accumulator, Is.EqualTo(value));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(zeroFlag));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(negativeFlag));
        }

        [Test]
        public void TSXOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte) OpCodes.PHA);
            Memory.StoreByteInMemory(1, (byte) OpCodes.PHA);
            Memory.StoreByteInMemory(2, (byte)OpCodes.TSX);
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(0xFD));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
            
        }
        
        [Test]
        public void TXSOperationTest()
        {
            CPU.Accumulator = 0x20;
            CPU.IndexRegisterX = 0xFE;
            Memory.StoreByteInMemory(0, (byte) OpCodes.PHA);
            Memory.StoreByteInMemory(1, (byte) OpCodes.PHP);
            Memory.StoreByteInMemory(2, (byte) OpCodes.PHP);
            Memory.StoreByteInMemory(3, (byte)OpCodes.LDAImm);
            Memory.StoreByteInMemory(4, 0x01);
            Memory.StoreByteInMemory(5, (byte)OpCodes.TXS);
            Memory.StoreByteInMemory(6, (byte)OpCodes.PLA);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x20));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
            
        }
    }
}