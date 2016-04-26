using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpracheBlog
{

    public static class CollectionExtensions
    {
        public static IEnumerable<T> Concatenate<T>(this T first, IEnumerable<T> rest)
        {
            yield return first;
            foreach (T item in rest)
            {
                yield return item;
            }
        }
    }

}