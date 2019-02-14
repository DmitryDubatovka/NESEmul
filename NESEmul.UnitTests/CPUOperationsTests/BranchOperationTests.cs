using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class BranchOperationTests : OperationBaseTests
    {
        private const ushort StartProgramCounter = 0x1000;

        [Test]
        [Theory]
        public void BPLTest(bool isNegativeFlagSet)
        {
            var memoryPointer = StartProgramCounter;
            CPU.ProgramCounter = StartProgramCounter;
            CPU.NegativeFlag = isNegativeFlagSet;
            Memory.StoreByteInMemory(memoryPointer++, (byte)OpCodes.BPL);
            Memory.StoreByteInMemory(memoryPointer, 0x7F);//127
            CPU.Do();
            var result = StartProgramCounter + 0x2 + 0x1; //0x2 - length of BPL command, 0x1 - last BRK command
            if (!isNegativeFlagSet)
                result += 0x7F;
            Assert.That(CPU.ProgramCounter, Is.EqualTo(result));
        }
        
        [Test]
        [Theory]
        public void BMITest(bool isNegativeFlag)
        {
            var memoryPointer = StartProgramCounter;
            CPU.ProgramCounter = StartProgramCounter;
            CPU.NegativeFlag = isNegativeFlag;
            Memory.StoreByteInMemory(memoryPointer++, (byte)OpCodes.BMI);
            Memory.StoreByteInMemory(memoryPointer, 0xF6);//-10
            CPU.Do();
            var result = StartProgramCounter + 0x2 + 0x1; //0x2 - length of BMI command, 0x1 - last BRK command
            if (isNegativeFlag)
                result -= 0xFF + 1 - 0xF6;
            Assert.That(CPU.ProgramCounter, Is.EqualTo(result));
        }
        
        [Test]
        [Theory]
        public void BVCTest(bool isOverflowFlagClear)
        {
            var memoryPointer = StartProgramCounter;
            CPU.ProgramCounter = StartProgramCounter;
            CPU.OverflowFlag = !isOverflowFlagClear;
            Memory.StoreByteInMemory(memoryPointer++, (byte)OpCodes.BVC);
            Memory.StoreByteInMemory(memoryPointer, 0xF6);//-10
            CPU.Do();
            var result = StartProgramCounter + 0x2 + 0x1; //0x2 - length of BMI command, 0x1 - last BRK command
            if (isOverflowFlagClear)
                result -= 0xFF + 1 - 0xF6;
            Assert.That(CPU.ProgramCounter, Is.EqualTo(result));
        }
        
        [Test]
        [Theory]
        public void BVSTest(bool isOverflowFlagSet)
        {
            var memoryPointer = StartProgramCounter;
            CPU.ProgramCounter = StartProgramCounter;
            CPU.OverflowFlag = isOverflowFlagSet;
            Memory.StoreByteInMemory(memoryPointer++, (byte)OpCodes.BVS);
            Memory.StoreByteInMemory(memoryPointer, 0xF6);//-10
            CPU.Do();
            var result = StartProgramCounter + 0x2 + 0x1; //0x2 - length of BMI command, 0x1 - last BRK command
            if (isOverflowFlagSet)
                result -= 0xFF + 1 - 0xF6;
            Assert.That(CPU.ProgramCounter, Is.EqualTo(result));
        }

        [Test]
        [Theory]
        public void BCCTest(bool isCarryFlagClear)
        {
            var memoryPointer = StartProgramCounter;
            CPU.ProgramCounter = StartProgramCounter;
            CPU.CarryFlag = !isCarryFlagClear;
            Memory.StoreByteInMemory(memoryPointer++, (byte)OpCodes.BCC);
            Memory.StoreByteInMemory(memoryPointer, 0x7F);//127
            CPU.Do();
            var result = StartProgramCounter + 0x2 + 0x1; //0x2 - length of BPL command, 0x1 - last BRK command
            if (isCarryFlagClear)
                result += 0x7F;
            Assert.That(CPU.ProgramCounter, Is.EqualTo(result));
        }
        
        [Test]
        [Theory]
        public void BCSTest(bool isCarryFlagSet)
        {
            var memoryPointer = StartProgramCounter;
            CPU.ProgramCounter = StartProgramCounter;
            CPU.CarryFlag = isCarryFlagSet;
            Memory.StoreByteInMemory(memoryPointer++, (byte)OpCodes.BCS);
            Memory.StoreByteInMemory(memoryPointer, 0x7F);//127
            CPU.Do();
            var result = StartProgramCounter + 0x2 + 0x1; //0x2 - length of BPL command, 0x1 - last BRK command
            if (isCarryFlagSet)
                result += 0x7F;
            Assert.That(CPU.ProgramCounter, Is.EqualTo(result));
        }
        
        [Test]
        [Theory]
        public void BNETest(bool isZeroFlagClear)
        {
            var memoryPointer = StartProgramCounter;
            CPU.ProgramCounter = StartProgramCounter;
            CPU.ZeroFlag = !isZeroFlagClear;
            Memory.StoreByteInMemory(memoryPointer++, (byte)OpCodes.BNE);
            Memory.StoreByteInMemory(memoryPointer, 0x7F);//127
            CPU.Do();
            var result = StartProgramCounter + 0x2 + 0x1; //0x2 - length of BPL command, 0x1 - last BRK command
            if (isZeroFlagClear)
                result += 0x7F;
            Assert.That(CPU.ProgramCounter, Is.EqualTo(result));
        }
        
        [Test]
        [Theory]
        public void BEQTest(bool isZeroFlagSet)
        {
            var memoryPointer = StartProgramCounter;
            CPU.ProgramCounter = StartProgramCounter;
            CPU.ZeroFlag = isZeroFlagSet;
            Memory.StoreByteInMemory(memoryPointer++, (byte)OpCodes.BEQ);
            Memory.StoreByteInMemory(memoryPointer, 0x7F);//127
            CPU.Do();
            var result = StartProgramCounter + 0x2 + 0x1; //0x2 - length of BPL command, 0x1 - last BRK command
            if (isZeroFlagSet)
                result += 0x7F;
            Assert.That(CPU.ProgramCounter, Is.EqualTo(result));
        }
        
        [Test]
        [Theory]
        public void ZeroOffsetBEQTest(bool isZeroFlagSet)
        {
            var memoryPointer = StartProgramCounter;
            CPU.ProgramCounter = StartProgramCounter;
            CPU.ZeroFlag = isZeroFlagSet;
            Memory.StoreByteInMemory(memoryPointer++, (byte)OpCodes.BEQ);
            Memory.StoreByteInMemory(memoryPointer, 0x0);//127
            CPU.Do();
            var result = StartProgramCounter + 0x2 + 0x1; //0x2 - length of BPL command, 0x1 - last BRK command
            Assert.That(CPU.ProgramCounter, Is.EqualTo(result));
        }
    }
}