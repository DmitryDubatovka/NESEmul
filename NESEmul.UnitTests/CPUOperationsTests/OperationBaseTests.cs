using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests.CPUOperationTests
{
    public class OperationBaseTests
    {
        protected CPU CPU;
        protected Memory Memory;

        [SetUp]
        protected void Setup()
        {
            Memory = new Memory();
            CPU = new CPU(0, Memory);
        }
    }
}