using System.Collections.Generic;
using NUnit.Framework;

namespace NESEmul.UnitTests.TestCases
{
    public class CPUOperationsTestCases
    {
        public static IEnumerable<TestCaseData> ADCImmTestCases()
        {
            yield return new TestCaseData((byte)0x1, (byte)0x2, (byte)0x3, false, false, false, false);
            yield return new TestCaseData((byte)0xFF, (byte)0x1, (byte)0x0, true, true, false, false);
            yield return new TestCaseData((byte)0xFE, (byte)0x1, (byte)0xFF, false, false, false, true);
            yield return new TestCaseData((byte)0xFE, (byte)0x80, (byte)0x7E, true, false, true, false);
        }

        public static IEnumerable<TestCaseData> CMPImmTestCases()
        {
            yield return new TestCaseData((byte)0x10, (byte)0x5, true, false, false);
            yield return new TestCaseData((byte)0x10, (byte)0x10, true, true, false);
            yield return new TestCaseData((byte)0x1, (byte)0x10, false, false, true);
        }
        
        public static IEnumerable<TestCaseData> CPXAbsTestCases()
        {
            yield return new TestCaseData((byte)0x10, (byte)0x5, true, false, false);
            yield return new TestCaseData((byte)0x10, (byte)0x10, true, true, false);
            yield return new TestCaseData((byte)0x1, (byte)0x10, false, false, true);
        }
        
        public static IEnumerable<TestCaseData> CPYZPTestCases()
        {
            yield return new TestCaseData((byte)0x10, (byte)0x5, true, false, false);
            yield return new TestCaseData((byte)0x10, (byte)0x10, true, true, false);
            yield return new TestCaseData((byte)0x0, (byte)0x0, true, true, false);
        }

        public static IEnumerable<TestCaseData> SBCImmTestCases()
        {
            yield return new TestCaseData((byte)0x1, (byte)0x2, (byte)0xFE, false, false, true, true);
            yield return new TestCaseData((byte)0x02, (byte)0x1, (byte)0x0, true, true, false, false);
            yield return new TestCaseData((byte)0xFE, (byte)0x1, (byte)0xFC, true, false, false, true);
        }
    }
}