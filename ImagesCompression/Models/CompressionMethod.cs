using System.Collections.Generic;

namespace ImagesCompression.Models
{
    public static class CompressionMethod
    {
        public static string RLE = "RLE";
        public static string DPCM = "DPCM";
        public static string Huffman = "Huffman";

        public static List<string> CompressionMethodsList = new List<string> { RLE, Huffman, DPCM };
    }
}
