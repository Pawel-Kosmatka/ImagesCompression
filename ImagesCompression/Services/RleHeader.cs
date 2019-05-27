using System;
using System.Linq;

namespace ImagesCompression.Services
{
    class RleHeader
    {
        private byte[] _header = new byte[28];
        private static readonly byte SizeOfFileAdress = 0;
        private static readonly byte PixelFormatAdress = 4;
        private static readonly byte WidthAdress = 8;
        private static readonly byte HeightAdress = 12;
        private static readonly byte RedArrayAddress = 16;
        private static readonly byte GreenArrayAddress = 20;
        private static readonly byte BlueArrayAddress = 24;
        private static readonly byte PropertyByteSize = 4;

        public RleHeader()
        {
        }

        public RleHeader(byte[] source)
        {
            _header = source.Take(_header.Length).ToArray();
        }

        public byte[] Header { get => _header; }
        public int SizeOfFile
        {
            get { return ByteArrayToInt(SizeOfFileAdress, PropertyByteSize); }
            set { IntToByteInHeader(value + _header.Length, SizeOfFileAdress); }
        }

        public int PixelFormat
        {
            get { return ByteArrayToInt(PixelFormatAdress, PropertyByteSize); }
            set { IntToByteInHeader(value, PixelFormatAdress); }
        }

        public int Width
        {
            get { return ByteArrayToInt(WidthAdress, PropertyByteSize); }
            set { IntToByteInHeader(value, WidthAdress); }
        }

        public int Height
        {
            get { return ByteArrayToInt(HeightAdress, PropertyByteSize); }
            set { IntToByteInHeader(value, HeightAdress); }
        }

        public int RedColorArrayAddress
        {
            get { return ByteArrayToInt(RedArrayAddress, PropertyByteSize); }
        }

        public int GreenColorArrayAddress
        {
            get { return ByteArrayToInt(GreenArrayAddress, PropertyByteSize); }
        }

        public int BlueColorArrayAddress
        {
            get { return ByteArrayToInt(BlueArrayAddress, PropertyByteSize); }
        }

        public void SetColorArraysAddresses(int redSize, int greenSize)
        {
            IntToByteInHeader(_header.Length, RedArrayAddress);
            IntToByteInHeader(_header.Length + redSize, GreenArrayAddress);
            IntToByteInHeader(_header.Length + redSize + greenSize, BlueArrayAddress);
        }

        private int ByteArrayToInt(int skip, int take)
        {
            return BitConverter.ToInt32(_header.Skip(skip).Take(take).ToArray(), 0);
        }

        private void IntToByteInHeader(int value, byte position)
        {
            var bytes = BitConverter.GetBytes(value);
            for (int i = 0; i < bytes.Length; i++)
            {
                _header[position + i] = bytes[i];
            }
        }
    }
}
