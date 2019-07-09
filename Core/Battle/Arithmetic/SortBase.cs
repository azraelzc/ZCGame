using System.Text;

namespace Battle.Arithmetic
{
    public abstract class SortBase
    {
        public int[] array = { 14, 2, 50, 4, 111, 504, 23 };

        public abstract int[] sort(int[] a);

        public static void print(int[] arrayForSort)
        {
            StringBuilder str = new StringBuilder("[");
            for (int i = 0; i < arrayForSort.Length; i++)
            {
                if (i == arrayForSort.Length - 1)
                {
                    str.Append(arrayForSort[i]);
                }
                else
                {
                    str.Append(arrayForSort[i] + " ,");
                }
            }
            str.Append("]");
        }

        public static void print(string prefix, int[] arrayForSort)
        {
            StringBuilder str = new StringBuilder(prefix + ": [");
            for (int i = 0; i < arrayForSort.Length; i++)
            {
                if (i == arrayForSort.Length - 1)
                {
                    str.Append(arrayForSort[i]);
                }
                else
                {
                    str.Append(arrayForSort[i] + " ,");
                }
            }
            str.Append("]");
        }
    }
}


