
namespace Battle.Arithmetic
{
    public class SelectionSort : SortBase
    {
        // Use this for initialization
        void Start()
        {
            int[] a = { 2, 1, 5, 9, 0, 6, 8, 7, 3 };
            print("result", (new SelectionSort()).sort(a));
        }

        public override int[] sort(int[] a)
        {
            print("init",a);
            int minIndex = 0;
            int temp = 0;
            for (int i = 0; i < a.Length; i++)
            {
                minIndex = i;
                for (int j = i + 1; j < a.Length; j++)
                {
                    if (a[j] < a[minIndex])
                    {
                        minIndex = j;
                    }
                }
                temp = a[i];
                a[i] = a[minIndex];
                a[minIndex] = temp;

                print((i + 1) + "", a);
            }
            return a;
        }
    }
}

