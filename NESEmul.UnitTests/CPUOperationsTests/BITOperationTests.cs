using NESEmul.Core;
using NESEmul.UnitTests.TestCases;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class BITOperationTests : OperationBaseTests
    {
        [TestCaseSource(typeof(CPUOperationsTestCases), nameof(CPUOperationsTestCases.BITZPTestCases))]
        public void BITZPTest(byte accumValue, byte operandValue, bool zeroFlag, bool negativeFlag, bool overflowFlag)
        {
            InitZPMode(OpCodes.BITZP, operandValue);
            CPU.Accumulator = accumValue;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(accumValue));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(zeroFlag));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(negativeFlag));
            Assert.That(CPU.OverflowFlag, Is.EqualTo(overflowFlag));
        }

        [Test]
        public void BITAbsTest()
        {
            InitAbsMode(OpCodes.BITAbs, 0xE0);
            CPU.Accumulator = 0xFF;
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0xFF));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
            Assert.That(CPU.OverflowFlag, Is.EqualTo(true));
        }
    }
}