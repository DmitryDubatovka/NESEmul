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
            
        }
    }
}