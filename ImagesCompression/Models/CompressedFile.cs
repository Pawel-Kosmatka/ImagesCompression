using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesCompression.Models
{
    class CompressedFile
    {
        public byte[] FileBitMap { get; set; }
        public int FileSize { get; set; }
        public double CompressionRatio { get; set; }
    }
}
