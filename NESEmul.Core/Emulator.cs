using System.IO;
using System.Threading;

namespace NESEmul.Core
{
    public class Emulator
    {
        private ROMImage _romImage;
        private CPU _cpu;
        private readonly Memory _memory;
        private PPU _ppu;

        public Emulator()
        {
            _memory = new Memory();
            _cpu = new CPU(_memory);
            _ppu = PPU.Instance;
        }

        public void Run(Stream romImageStream)
        {
            _romImage = new ROMImage();
            _romImage.Load(romImageStream);

            _memory.WriteBytes(0x8000, _romImage.ROMBanks[0]);
            _memory.WriteBytes(0xC000, _romImage.ROMBanks[_romImage.ROMBanksNumber > 1 ? 1 : 0]);
            Run();
        }

        private void Run()
        {
            Thread t = new Thread(() =>
            {
                _cpu.TriggerInterrupt(InterruptType.Reset);
                int cpuCycles = 0;
                while (true)
                {

                    for (int frame = 0; frame < 60; frame++)
                    {
                        for (int scanLine = 0; scanLine < 262; scanLine++)
                        {
                            for (int cycle = 0; cycle < 341; cycle++)
                            {
                                if (++cpuCycles == 3)
                                {
                                    _cpu.Tick();
                                    cpuCycles = 0;
                                }
                            }

                            if(scanLine == 240)
                                _cpu.TriggerInterrupt(InterruptType.NMI);
                        }

                        Thread.Sleep(10);
                    }
                }
            }){IsBackground = true};
            t.Start();
        }
    }
}