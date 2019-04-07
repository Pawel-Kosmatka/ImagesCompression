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
    class RleCompressionService : ICompressionService
    {
        public CompressedFile CompressImage(byte[] sourceImage)
        {
            var imageAddress = BmpHeaderService.GetImageArrayAddress(sourceImage);

            var compressedImage = new List<byte>();

            var compressedHeader = sourceImage.Take(imageAddress).ToArray();

            var image = sourceImage.Skip(imageAddress).ToArray();

            byte count = 1;
            byte prev = image.First();
            for (int i = 0; i < image.Count(); i++)
            {
                if (i == image.Count() - 1 || image[i] != image[i + 1] || count == 255)
                {
                    compressedImage.Add(count);
                    compressedImage.Add(image[i]);
                    count = 1;
                    continue;
                }
                count++;
            }

            var resultArray = compressedHeader.Concat(compressedImage).ToArray();
            setHeaderParams(compressedHeader, 1, compressedImage, resultArray);

            saveToFile(AppDomain.CurrentDomain.BaseDirectory + @"\encoded" + nameof(CompressionMethod.RLE), resultArray);

            var compressionRatio = (double)BmpHeaderService.GetFileSizeFromHeader(sourceImage) / resultArray.Length;
            return new CompressedFile { FileBitMap = resultArray, CompressionRatio = compressionRatio, FileSize = resultArray.Length };
        }

        public byte[] DecompressImage(byte[] compressedImage)
        {
            var imageAddress = BmpHeaderService.GetImageArrayAddress(compressedImage);

            var decodedImage = new List<byte>();

            var decodedHeader = compressedImage.Take(imageAddress).ToArray();

            var image = compressedImage.Skip(imageAddress).ToArray();

            for (int i = 0; i < image.Count(); i += 2)
            {
                decodedImage.AddRange(Enumerable.Repeat(image[i + 1], image[i]));
            }

            var resultArray = decodedHeader.Concat(decodedImage).ToArray();
            setHeaderParams(decodedHeader, 0, decodedImage, resultArray);

            saveToFile(AppDomain.CurrentDomain.BaseDirectory + @"\decoded" + nameof(CompressionMethod.RLE) + ".bmp", resultArray);

            return resultArray;
        }

        private void setHeaderParams(byte[] header, byte comppressionMethod, List<byte> rawImageArray, byte[] fileByteArray)
        {
            BmpHeaderService.SetCompressionMethodType(header, comppressionMethod);
            BmpHeaderService.SetRawImageSizeInHeader(header, rawImageArray.Count());
            BmpHeaderService.SetFileSizeInHeader(fileByteArray, fileByteArray.Length);
        }

        private void saveToFile(string path, byte[] fileByteArray)
        {
            var fileStream = File.Create(path);
            fileStream.Write(fileByteArray, 0, fileByteArray.Length);
            fileStream.Dispose();
        }
    }
}
