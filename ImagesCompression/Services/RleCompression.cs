using ImagesCompression.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace ImagesCompression.Services
{
    class RleCompression : ICompression
    {
        public CompressedFile CompressImage(byte[] sourceImage, string path)
        {
            var image = new Bitmap(path);

            var red = new List<byte>();
            var green = new List<byte>();
            var blue = new List<byte>();

            for (int w = 0; w < image.Width; w++)
            {
                for (int h = 0; h < image.Height; h++)
                {
                    red.Add(image.GetPixel(w, h).R);
                    green.Add(image.GetPixel(w, h).G);
                    blue.Add(image.GetPixel(w, h).B);
                }
            }

            var compressedRed = compressColorList(red);
            var compressedGreen = compressColorList(green);
            var compressedBlue = compressColorList(blue);
            var header = new RleHeader
            {
                PixelFormat = (int)image.PixelFormat,
                Width = image.Width,
                Height = image.Height,
                SizeOfFile = compressedRed.Count + compressedGreen.Count + compressedBlue.Count
            };
            header.SetColorArraysAddresses(compressedRed.Count, compressedGreen.Count);

            image.Dispose();

            var result = new List<byte>();
            result.AddRange(header.Header);
            result.AddRange(compressedRed);
            result.AddRange(compressedGreen);
            result.AddRange(compressedBlue);

            var compressionRatio = (double)BmpHeader.GetFileSizeFromHeader(sourceImage) / result.Count;

            return new CompressedFile
            {
                File = result.ToArray(),
                FileSize = result.Count,
                CompressionRatio = compressionRatio
            };
        }

        private List<byte> compressColorList(List<byte> colorList)
        {
            var compressedList = new List<byte>();

            byte count = 1;
            for (int i = 0; i < colorList.Count(); i++)
            {
                if (i == colorList.Count() - 1 || colorList[i] != colorList[i + 1] || count == 255)
                {
                    compressedList.Add(count);
                    compressedList.Add(colorList[i]);
                    count = 1;
                    continue;
                }
                count++;
            }

            return compressedList;
        }

        public byte[] DecompressImage(byte[] compressedImage)
        {
            var rleHeader = new RleHeader(compressedImage);

            var image = new Bitmap(rleHeader.Width, rleHeader.Height, (PixelFormat)rleHeader.PixelFormat);
            var image2 = new Bitmap(rleHeader.Width, rleHeader.Height, PixelFormat.Format1bppIndexed);
            image2.SetPixel(1, 1, new Color());
            var red = new List<byte>(compressedImage.Skip(rleHeader.RedColorArrayAddress).Take(rleHeader.GreenColorArrayAddress - rleHeader.RedColorArrayAddress));
            var green = new List<byte>(compressedImage.Skip(rleHeader.GreenColorArrayAddress).Take(rleHeader.BlueColorArrayAddress - rleHeader.GreenColorArrayAddress));
            var blue = new List<byte>(compressedImage.Skip(rleHeader.BlueColorArrayAddress).Take(compressedImage.Length - rleHeader.BlueColorArrayAddress));

            var decodedRed = DecodeColorLists(red);
            var decodedGreen = DecodeColorLists(green);
            var decodedBlue = DecodeColorLists(blue);

            int count = 0;
            for (int w = 0; w < rleHeader.Width; w++)
            {
                for (int h = 0; h < rleHeader.Height; h++)
                {
                    image.SetPixel(w, h, Color.FromArgb(decodedRed[count], decodedGreen[count], decodedBlue[count]));
                    count++;
                }
            }
            var path = AppDomain.CurrentDomain.BaseDirectory + @"\decoded" + nameof(CompressionMethod.RLE) + ".bmp";
            byte[] r = null;
            
            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Bmp);
                r = stream.ToArray();
                var file = File.Create(path);
                image.Save(file, ImageFormat.Bmp);
                //file.Write(r, 0, r.Length);
                file.Dispose();
            }
            image.Dispose();
            return r;
        }

        private List<byte> DecodeColorLists(List<byte> colorList)
        {
            var decodedList = new List<byte>();

            for (int i = 0; i < colorList.Count; i += 2)
            {
                decodedList.AddRange(Enumerable.Repeat(colorList[i + 1], colorList[i]));
            }
            return decodedList;
        }
    }
}
