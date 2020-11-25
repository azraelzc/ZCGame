/***************************************************************

 *  类名称：        Fold

 *  描述：          折叠         				

 *  作者：          Chico(wuyuanbing)

 *  创建时间：      2020/4/22 20:20:14

 *  最后修改人：

 *  版权所有 （C）:   diandiangames

***************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCGame.Core.Functional
{
    public static class Fold
    {
        public static R FoldL<T, R>(this IEnumerable<T> list, Func<R, T, R> accumulator, R startVal)
        {
            R result = startVal;
            foreach (T sourceVal in list)
                result = accumulator(result, sourceVal);
            return result;
        }

        public static IEnumerable<T> Reverse<T>(this IEnumerable<T> list)
        {
            Stack<T> stack = new Stack<T>();
            foreach (T sourceVal in list)
            {
                stack.Push(sourceVal);
            }

            foreach (var element in stack)
            {
                yield return element;
            }
        }

        public static R FoldR<T, R>(this IEnumerable<T> list, Func<R, T, R> accumulator, R startVal)
        {
            return FoldL(list, accumulator, startVal);
        }
    }
}
