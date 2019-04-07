using ImagesCompression.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesCompression.Services
{
    interface ICompressionService
    {
        CompressedFile CompressImage(byte[] sourceImage);
        byte[] DecompressImage(byte[] compressedImage);
    }
}
