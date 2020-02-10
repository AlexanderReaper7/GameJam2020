namespace Tools_XNA_dotNET_Framework
{
    public static class ArrayDimensionHelper
    {
        public static T[,] ConvertTo2D<T>(this T[] array, int width, int height)
        {
            T[,] output = new T[width, height];

            for (int y = 0; y < height; y++)
            {
                // For each pixel in this row
                for (int x = 0; x < width; x++)
                {
                    // Set color of this coordinate from array into TextureData
                    output[x, y] = array[x + y * width];
                }
            }

            return output;
        }

        public static T[] ConvertTo1D<T>(this T[,] array)
        {
            T[] output = new T[array.Length];

            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    output[x + (y * array.GetLength(0))] = array[x, y];
                }
            }

            return output;
        }
    }
}

