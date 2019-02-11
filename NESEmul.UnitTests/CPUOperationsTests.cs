using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests
{
    [TestFixture]
    public class CPUOperationsTests
    {
        private CPU _cpu;
        private readonly Memory _memory;

        public CPUOperationsTests()
        {
            _memory = new Memory();
            _cpu = new CPU(0, 0, _memory);
        }

        [Test]
        public void ADCTest()
        {
            _memory.StoreByteInMemory(0, (byte)OpCodes.ADCIm);
            _cpu.Accumulator = 0x5;
            _memory.StoreByteInMemory(1, 0x10);
            _cpu.Do();

        }
    }
}