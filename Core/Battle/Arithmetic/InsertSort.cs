
namespace Battle.Arithmetic
{
    public class InsertSort : SortBase
    {
        // Use this for initialization
        void Start()
        {
            int[] a = { 2, 1, 5, 9, 0, 6, 8, 7, 3 };
            print("result", (new InsertSort()).sort(a));
        }

        public override int[] sort(int[] a)
        {
            print("init", a);
            int temp = 0;
            for (int i = 1; i < a.Length; i++)
            {
                //只能从当前索引往前循环，因为索引前的数组皆为有序的，索引只要确定当前索引的数据的为止即可
                for (int j = i; j > 0 && a[j] < a[j - 1]; j--)
                {
                    temp = a[j];
                    a[j] = a[j - 1];
                    a[j - 1] = temp;
                }
                print(i + "", a);
            }

            print("result", a);
            return a;
        }
    }
}


