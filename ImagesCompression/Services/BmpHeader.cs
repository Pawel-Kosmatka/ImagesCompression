using System;
using System.Linq;

namespace ImagesCompression.Services
{
    class BmpHeader
    {
        private static readonly byte SizeOfFileAdress = 2;
        private static readonly byte PixelArrayAddress = 10;
        private static readonly byte CompressionMethodTypeAddress = 30;
        private static readonly byte RawImageSizeAddress = 34;
        private static readonly byte PropertyByteSize = 4;

        public static int GetImageArrayAddress(byte[] bmpArray)
        {
            return ByteArrayToInt(bmpArray, PixelArrayAddress, PropertyByteSize);
        }

        public static int GetFileSizeFromHeader(byte[] bmpArray)
        {
            return ByteArrayToInt(bmpArray, SizeOfFileAdress, PropertyByteSize);
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
            return ByteArrayToInt(bmpArray, RawImageSizeAddress, PropertyByteSize);
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
