namespace Utilities
{
    struct Algorithms
    {
        /// <summary>
        /// Swaps two values of any type
        /// </summary>
        public static void swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
}