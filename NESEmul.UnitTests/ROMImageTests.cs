using System;
using System.IO;
using NESEmul.Core;
using NUnit.Framework;

namespace NESEmul.UnitTests
{
    [TestFixture]
    public class ROMImageTests
    {
        [Test]
        public void LoadTest()
        {
            var image = new ROMImage();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../ROMs/Defender II (U) [!].nes");
            using (var stream = File.OpenRead(path))
            {
                image.Load(stream);
            }
            Assert.That(image.MapperType, Is.EqualTo(0));
            Assert.That(image.VROMBanksNumber, Is.EqualTo(1));
            Assert.That(image.ROMBanksNumber, Is.EqualTo(1));
            
        }
    }
}