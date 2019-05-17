using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NESEmul.Core;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace NESEmul.PatternTablesViewer
{
    public partial class Form1 : Form
    {
        private readonly List<Tile> _tileList;

        private Emulator _emulator;
        public Form1()
        {
            InitializeComponent();
            _tileList = new List<Tile>(512);
            _emulator = new Emulator();
        }


        private void OnSkControlOnPaint(object sender, SKPaintSurfaceEventArgs args)
        {
            if(!_tileList.Any())
                return;
            var stopWatch = Stopwatch.StartNew();
            int cellSize = 3;
            var surface = args.Surface;

            SKCanvas canvas = surface.Canvas;
            canvas.DrawColor(SKColors.White);
            canvas.Clear(SKColors.White);
            
            using (SKPaint paint = new SKPaint())
            {
                paint.StrokeWidth = 3;
                paint.Style = SKPaintStyle.Fill;

                List<SKPoint> points1 = new List<SKPoint>(64 * 64);
                List<SKPoint> points2 = new List<SKPoint>(64 * 64);
                List<SKPoint> points3 = new List<SKPoint>(64 * 64);
                for (int tileIndex = 0; tileIndex < _tileList.Count; tileIndex++)
                {
                    var tile = _tileList[tileIndex];
                    int y = tileIndex / 16;
                    int x = tileIndex - y * 16;

                    TileColors[,] colors = tile.As8X8Array();
                    for (int j = 0; j < colors.GetLength(0); j++)
                    {
                        for (int i = 0; i < colors.GetLength(1); i++)
                        {
                            var p = new SKPoint(8 * x * cellSize + i * cellSize, 8 * y * cellSize + j * cellSize);
                            var tilePixelColor = colors[i, j];
                            if(tilePixelColor == TileColors.FirstColor)
                                points1.Add(p);
                            else if(tilePixelColor == TileColors.SecondColor)
                                points2.Add(p);
                            else if(tilePixelColor == TileColors.ThirdColor)
                                points3.Add(p);
                        }
                    }

                }

                paint.Color = SKColors.Red;
                canvas.DrawPoints(SKPointMode.Points, points1.ToArray(), paint);
                paint.Color = SKColors.Blue;
                canvas.DrawPoints(SKPointMode.Points, points2.ToArray(), paint);
                paint.Color = SKColors.Black;
                canvas.DrawPoints(SKPointMode.Points, points3.ToArray(), paint);

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.ElapsedMilliseconds);
            }
        }

        private void openFileMenuItem_Click(object sender, EventArgs e)
        {
            var romImage = new ROMImage();
            if (openROMFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var stream = openROMFileDialog.OpenFile())
                {
                    _emulator.Run(stream);
                    //romImage.Load(stream);
                }

                if(romImage.CHRROMBanksNumber < 1)
                    return;
                byte[] vromBank = romImage.CHRROMBanks.Single();
                _tileList.Clear();
                for (int i = 0; i < vromBank.Length / 16; i++)
                {
                    _tileList.Add(new Tile(vromBank.Skip(i * 16).Take(16).ToArray()));
                }
                _skControl.Invalidate();
            }

            
        }
    }
}
