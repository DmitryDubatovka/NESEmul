using NESEmul.Core;
using NESEmul.Core.Exceptions;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    [TestFixture]
    public class CommonOperationsTests : OperationBaseTests
    {
        [Test]
        public void FlagsOperationsTest()
        {
            var commandPointer = CPU.ProgramCounter;
            Memory.StoreByteInMemory(commandPointer++, (byte)OpCodes.SEC);
            Memory.StoreByteInMemory(commandPointer++, (byte)OpCodes.SED);
            Memory.StoreByteInMemory(commandPointer++, (byte)OpCodes.SEI);
            Memory.StoreByteInMemory(commandPointer++, (byte)OpCodes.BRK);
            CPU.Do();
            CPU.BreakCommand = false;
            Assert.That(CPU.CarryFlag, Is.EqualTo(true));
            Assert.That(CPU.DecimalMode, Is.EqualTo(true));
            Assert.That(CPU.InterruptDisable, Is.EqualTo(true));
            Assert.That(CPU.OverflowFlag, Is.EqualTo(false));
            CPU.OverflowFlag = true;
            Memory.StoreByteInMemory(commandPointer++, (byte)OpCodes.CLC);
            Memory.StoreByteInMemory(commandPointer++, (byte)OpCodes.CLD);
            Memory.StoreByteInMemory(commandPointer++, (byte)OpCodes.CLI);
            Memory.StoreByteInMemory(commandPointer, (byte)OpCodes.CLV);
            CPU.Do();
            Assert.That(CPU.CarryFlag, Is.EqualTo(false));
            Assert.That(CPU.DecimalMode, Is.EqualTo(false));
            Assert.That(CPU.InterruptDisable, Is.EqualTo(false));
            Assert.That(CPU.OverflowFlag, Is.EqualTo(false));
        }

        [Test]
        public void NOPOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.NOP);
            Memory.StoreByteInMemory(1, (byte)OpCodes.NOP);
            CPU.Do();
            Assert.That(CPU.ProgramCounter, Is.EqualTo(3));
        }

        [Test]
        public void FakeOperatorTest()
        {
            Memory.StoreByteInMemory(0, 0xFF);
            Assert.Catch<InvalidByteCodeException>(() => CPU.Do());
        }
    }
}