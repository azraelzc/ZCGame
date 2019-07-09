
namespace Battle.Arithmetic
{
    public class ShellSort : SortBase
    {
        public void Start()
        {
            int[] a = { 2, 1,105, 5, 9, 0, 6, 8, 7, 3 };
            print("result", (new ShellSort()).sort(a));
        }
        public override int[] sort(int[] a)
        {
            print("init", a);
            int h = a.Length;
            int temp = 0;
            while (h >= 1)
            {
                for (int i = h; i < a.Length; i++)
                {
                    for (int j = i; j >= h && a[j] < a[j - h]; j -= h)
                    {
                        temp = a[j];
                        a[j] = a[j - h];
                        a[j - h] = temp;

                    }
                }
                h /= 9;
            }
            return a;
        }
    }
}

