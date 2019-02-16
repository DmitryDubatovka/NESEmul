﻿using NESEmul.Core;
using NESEmul.UnitTests.TestCases;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class CMPOperationTests : OperationBaseTests
    {
        [TestCaseSource(typeof(CPUOperationsTestCases), nameof(CPUOperationsTestCases.CMPImmTestCases))]
        public void CMPImmTest(byte accumValue, byte operandValue, bool carryFlag, bool zeroFlag, bool negativeFlag)
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.CMPImm);
            CPU.Accumulator = accumValue;
            Memory.StoreByteInMemory(1, operandValue);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(accumValue));
            Assert.That(CPU.CarryFlag, Is.EqualTo(carryFlag), "Carry flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(zeroFlag), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(negativeFlag), "Negative flag");
        }
        
        [TestCaseSource(typeof(CPUOperationsTestCases), nameof(CPUOperationsTestCases.CPXAbsTestCases))]
        public void CPXAbsTest(byte xRegisterValue, byte operandValue, bool carryFlag, bool zeroFlag, bool negativeFlag)
        {
            InitAbsMode(OpCodes.CPXAbs, operandValue);
            CPU.IndexRegisterX = xRegisterValue;
            CPU.Do();
            Assert.That(CPU.IndexRegisterX, Is.EqualTo(xRegisterValue));
            Assert.That(CPU.CarryFlag, Is.EqualTo(carryFlag), "Carry flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(zeroFlag), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(negativeFlag), "Negative flag");
        }
        
        [TestCaseSource(typeof(CPUOperationsTestCases), nameof(CPUOperationsTestCases.CPYZPTestCases))]
        public void CPYZPTest(byte xRegisterValue, byte operandValue, bool carryFlag, bool zeroFlag, bool negativeFlag)
        {
            InitZPMode(OpCodes.CPYZP, operandValue);
            CPU.IndexRegisterY = xRegisterValue;
            CPU.Do();
            Assert.That(CPU.IndexRegisterY, Is.EqualTo(xRegisterValue));
            Assert.That(CPU.CarryFlag, Is.EqualTo(carryFlag), "Carry flag");
            Assert.That(CPU.ZeroFlag, Is.EqualTo(zeroFlag), "Zero flag");
            Assert.That(CPU.NegativeFlag, Is.EqualTo(negativeFlag), "Negative flag");
        }
    }
}