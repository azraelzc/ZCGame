using System;
using System.Linq;

namespace Battle.Arithmetic
{
    public class TestAction  {

	    // Use this for initialization
	    void Start () {
            ///Action<T>的用法
            ///这里的T为代理函数的传入类型,无返回值
            Action<string[]> action = delegate (string[] x)
            {
                var result = from p in x
                             where p.Contains("s")
                             select p;
                foreach (string s in result.ToList())
                {
                    Console.WriteLine(s);
                }
            };
            string[] str = { "charlies", "nancy", "alex", "jimmy", "selina" };
            action(str);

            Func<int,string, int> func = CallStringLength;
            Func<string> func1 = delegate ()
            {
                return "我是Func<TResult>委托返回的结果";
            };
            Console.WriteLine(func(11,"1111"));
            Console.WriteLine(func1());

    #region Predicate
            ///bool Predicate<T>的用法
            ///输入一个T类型的参数,返回值为bool类型
            Predicate<string[]> predicate = delegate(string[] x)
            {
                var result = from p in x
                             where p.Contains("s")
                             select p;
                if (result.ToList().Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            };
            string[] _value = { "charlies", "nancy", "alex", "jimmy", "selina" };
            if (predicate(_value))
            {
                    Console.WriteLine("They contain.");
            }
            else
            {
                    Console.WriteLine("They don't contain.");
            }
    #endregion
        }

    

        int CallStringLength(int i,string str)
        {
            return str.Length;
        }

        // Update is called once per frame
        void Update () {
		
	    }
    }
}

