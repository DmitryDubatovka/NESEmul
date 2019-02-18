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

        [Test]
        public void PHAOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.PHA);
            CPU.Accumulator = 0x80;
            var value = Memory.LoadByteFromMemory(0x1FF);
            Assert.That(value, Is.EqualTo(0x00));
            CPU.Do();
            value = Memory.LoadByteFromMemory(0x1FF);
            Assert.That(value, Is.EqualTo(0x80));
        }

        [Test]
        public void PLAOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.PHA);
            CPU.Accumulator = 0x81;
            Memory.StoreByteInMemory(1, (byte)OpCodes.LDAImm);
            Memory.StoreByteInMemory(2, 0x0);
            Memory.StoreByteInMemory(3, (byte)OpCodes.PLA);
            CPU.Do();
            Assert.That(CPU.Accumulator, Is.EqualTo(0x81));
        }

        [Test]
        public void PHPOperationTest()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.PHP);
            CPU.CarryFlag = false;
            CPU.ZeroFlag = true;
            CPU.InterruptDisable = true;
            CPU.DecimalMode = false;
            CPU.BreakCommand = false;
            CPU.OverflowFlag = true;
            CPU.NegativeFlag = true;
            CPU.Do();
            var value = Memory.LoadByteFromMemory(0x1FF);
            Assert.That(value, Is.EqualTo(0xE6));
        }
        
        [Test]
        public void PHPOperationTest2()
        {
            Memory.StoreByteInMemory(0, (byte)OpCodes.PHP);
            CPU.CarryFlag = true;
            CPU.ZeroFlag = false;
            CPU.InterruptDisable = false;
            CPU.DecimalMode = true;
            CPU.BreakCommand = false;
            CPU.OverflowFlag = false;
            CPU.NegativeFlag = false;
            CPU.Do();
            var value = Memory.LoadByteFromMemory(0x1FF);
            Assert.That(value, Is.EqualTo(0x29));
        }

        [Test]
        public void PLPOperationTest()
        {
            CPU.Accumulator = 0x29;
            Memory.StoreByteInMemory(0, (byte)OpCodes.PHA);
            Memory.StoreByteInMemory(1, (byte)OpCodes.PLP);
            CPU.Do();
            Assert.That(CPU.CarryFlag, Is.EqualTo(true));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(false));
            Assert.That(CPU.InterruptDisable, Is.EqualTo(false));
            Assert.That(CPU.DecimalMode, Is.EqualTo(true));
            Assert.That(CPU.BreakCommand, Is.EqualTo(true));
            Assert.That(CPU.OverflowFlag, Is.EqualTo(false));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(false));
        }
        
        [Test]
        public void PLPOperationTest2()
        {
            CPU.Accumulator = 0xE6;
            Memory.StoreByteInMemory(0, (byte)OpCodes.PHA);
            Memory.StoreByteInMemory(1, (byte)OpCodes.PLP);
            CPU.Do();
            Assert.That(CPU.CarryFlag, Is.EqualTo(false));
            Assert.That(CPU.ZeroFlag, Is.EqualTo(true));
            Assert.That(CPU.InterruptDisable, Is.EqualTo(true));
            Assert.That(CPU.DecimalMode, Is.EqualTo(false));
            Assert.That(CPU.BreakCommand, Is.EqualTo(true));
            Assert.That(CPU.OverflowFlag, Is.EqualTo(true));
            Assert.That(CPU.NegativeFlag, Is.EqualTo(true));
        }
    }
}