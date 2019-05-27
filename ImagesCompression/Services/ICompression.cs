using ImagesCompression.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesCompression.Services
{
    interface ICompression
    {
        CompressedFile CompressImage(byte[] sourceImage, string path);
        byte[] DecompressImage(byte[] compressedImage);
    }
}
