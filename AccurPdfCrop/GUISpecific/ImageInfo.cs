using System;

namespace AccurPdfCrop.GUISpecific
{
    public struct ImageInfo
    {
        public ImageInfo(int width, int height, byte[] pixelsData) : this()
        {
            if (width <= 0)
                throw new ArgumentException("Must be more than 0", "width");
            if (width <= 0)
                throw new ArgumentException("Must be more than 0", "height");

            Width = width;
            Height = height;
            PixelsData = pixelsData;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public byte[] PixelsData { get; private set; }      // Pixels format is always RGBA in my case
    }
}
