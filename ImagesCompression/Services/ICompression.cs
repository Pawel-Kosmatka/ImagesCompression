namespace ImagesCompression.Services
{
    interface ICompression
    {
        byte[] CompressImage(byte[] sourceImage, string path);
        byte[] DecompressImage(byte[] compressedImage);
    }
}
