
namespace AccurPdfCrop
{
    public class BoundingRect
    {
        public int Left { get; private set; }
        public int Right { get; private set; }
        public int Top { get; private set; }
        public int Bottom { get; private set; }

        private BoundingRect() { }

        static internal BoundingRect DetectOnColor(byte[] pixels, int width, int height)
        {
            var result = new BoundingRect();
            int i;

            i = TopHitIndexColor(pixels);
            if (i >= pixels.Length)
                return null;        // Page is blank
            result.Top = i / 4 / width;

            i = BottomHitIndexColor(pixels);
            result.Bottom = i / 4 / width;

            i = LeftHitIndexColor(pixels, width * 4 - 2, (result.Bottom - result.Top + 1) * width * 4, result.Top * width * 4);
            result.Left = (i / 4) % width;

            i = RightHitIndexColor(pixels, width * 4 - 2, (result.Bottom - result.Top + 1) * width * 4, ((result.Top + 1) * width - 1) * 4);
            result.Right = (i / 4) % width;

            return result;
        }

        static internal BoundingRect DetectOnGrey(byte[] pixels, int width, int height)
        {
            var result = new BoundingRect();
            int i;

            i = TopHitIndexGrey(pixels);
            if (i >= pixels.Length)
                return null;        // Page is blank
            result.Top = i / width;

            i = BottomHitIndexGrey(pixels);
            result.Bottom = i / width;

            i = LeftHitIndexGrey(pixels, width, (result.Bottom - result.Top + 1) * width, result.Top * width);
            result.Left = i % width;

            i = RightHitIndexGrey(pixels, width, (result.Bottom - result.Top + 1) * width, (result.Top + 1) * width - 1);
            result.Right = i % width;

            return result;
        }

        static private int TopHitIndexColor(byte[] pixels)
        {
            int i;
            for (i = 0; i < pixels.Length; ++i)
            {
                if (pixels[i] + pixels[++i] + pixels[++i] < 765)
                    break;
                ++i;
            }

            return i;
        }

        static private int BottomHitIndexColor(byte[] pixels)
        {
            int i = pixels.Length;
            while (true)
            {
                --i;
                if (pixels[--i] + pixels[--i] + pixels[--i] < 765)
                    break;
            }

            return i;
        }

        static private int LeftHitIndexColor(byte[] pixels, int dw, int dy4, int start)
        {
            int i = start;
            int tempIndex = i + dy4;

            while (true)
            {
                while (i < tempIndex)
                {
                    if (pixels[i] + pixels[++i] + pixels[++i] < 765)
                        return i;       // RETURN

                    i += dw;
                }

                tempIndex = i + 4;
                i = tempIndex - dy4;
            }
        }

        static private int RightHitIndexColor(byte[] pixels, int dw, int dy4, int start)
        {
            int i = start;
            int tempIndex = i + dy4;

            while (true)
            {
                while (i < tempIndex)
                {
                    if (pixels[i] + pixels[++i] + pixels[++i] < 765)
                        return i;       // RETURN

                    i += dw;
                }

                tempIndex = i - 4;
                i = tempIndex - dy4;
            }
        }

        static private int TopHitIndexGrey(byte[] pixels)
        {
            int i;
            for (i = 0; i < pixels.Length; ++i)
                if (pixels[i] < 255)
                    break;

            return i;
        }

        static private int BottomHitIndexGrey(byte[] pixels)
        {
            int i = pixels.Length;
            while (true)
            {
                if (pixels[--i] < 255)
                    break;
            }

            return i;
        }

        static private int LeftHitIndexGrey(byte[] pixels, int width, int dy, int start)
        {
            int i = start;
            int tempIndex = i + dy;

            while (true)
            {
                while (i < tempIndex)
                {
                    if (pixels[i] < 255)
                        return i;       // RETURN

                    i += width;
                }

                tempIndex = i + 1;
                i = tempIndex - dy;
            }
        }

        static private int RightHitIndexGrey(byte[] pixels, int width, int dy, int start)
        {
            int i = start;
            int tempIndex = i + dy;

            while (true)
            {
                while (i < tempIndex)
                {
                    if (pixels[i] < 255)
                        return i;       // RETURN

                    i += width;
                }

                tempIndex = i - 1;
                i = tempIndex - dy;
            }
        }
    }
}
