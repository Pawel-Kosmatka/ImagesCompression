using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesCompression.Services
{
    class BmpHeaderService
    {
        private static readonly byte SizeOfFileAdress = 2;
        private static readonly byte SizeOfFileSize = 4;
        private static readonly byte PixelArrayAddress = 10;
        private static readonly byte PixelArrayAddresSize = 4;
        private static readonly byte CompressionMethodTypeAddress = 30;
        private static readonly byte CompressionMethodTypeSize = 4;
        private static readonly byte RawImageSizeAddress = 34;
        private static readonly byte RawImageSizeSize = 4;

        public static int GetImageArrayAddress(byte[] bmpArray)
        {
            return ByteArrayToInt(bmpArray, PixelArrayAddress, PixelArrayAddresSize);
        }

        public static int GetFileSizeFromHeader(byte[] bmpArray)
        {
            return ByteArrayToInt(bmpArray, SizeOfFileAdress, SizeOfFileSize);
        }

        public static void SetFileSizeInHeader(byte[] bmpArray, int size) 
        {
            var bytes = BitConverter.GetBytes(size);
            for (int i = 0; i < bytes.Length; i++)
            {
                bmpArray[SizeOfFileAdress + i] = bytes[i];
            }
        }

        public static int GetRawImageSizeFromHeader(byte[] bmpArray)
        {
            return ByteArrayToInt(bmpArray, RawImageSizeAddress, RawImageSizeSize);
        }

        public static void SetRawImageSizeInHeader(byte[] bmpArray, int size)
        {
            var bytes = BitConverter.GetBytes(size);
            for (int i = 0; i < bytes.Length; i++)
            {
                bmpArray[RawImageSizeAddress + i] = bytes[i];
            }
        }

        public static void SetCompressionMethodType(byte[] bmpArray, byte type)
        {
            bmpArray[CompressionMethodTypeAddress] = type;
        }

        private static int ByteArrayToInt(byte[] bytes, int skip, int take)
        {
            return BitConverter.ToInt32(bytes.Skip(skip).Take(take).ToArray(), 0);
        }
    }
}
