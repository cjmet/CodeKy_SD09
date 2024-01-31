using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeKY_SD01.Products;

namespace CodeKY_SD01.Extensions
{
    static class ListExtensions
    {
        public static List<T> InStock<T>(this List<T> list) where T : Product
        {
            return list.Where(item => item.Quantity > 0).ToList();
        }
    }
}
