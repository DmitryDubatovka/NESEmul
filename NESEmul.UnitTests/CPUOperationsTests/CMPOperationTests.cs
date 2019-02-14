using NESEmul.Core;
using NESEmul.UnitTests.TestCases;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class CMPOperationTests : OperationBaseTests
    {
        [TestCaseSource(typeof(CPUOperationsTestCases), nameof(CPUOperationsTestCases.CMPImTestCases))]
        public void CMPImTest(byte accumValue, byte operandValue, byte accumResultValue, bool carryFlag, bool zeroFlag, bool negativeFlag)
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.CMPIm);
            CPU.Accumulator = accumValue;
            Memory.StoreByteInMemory(1, operandValue);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(accumResultValue));
            Assert.That(CPU.CarryFlag, Is.EqualTo(carryFlag), "Carry flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(zeroFlag), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(negativeFlag), "Negative flag");
        }
    }
}