using System;
using System.Collections.Generic;
using System.Text;

namespace bd.swrm.entidades.Comparadores
{
    public class MyComparer<T> : IEqualityComparer<T>
    {
        Func<T, T, bool> myCompare;
        Func<T, int> myGetHashCode;

        public MyComparer(Func<T, T, bool> myCompare, Func<T, int> myGetHashCode)
        {
            this.myCompare = myCompare;
            this.myGetHashCode = myGetHashCode;
        }

        public bool Equals(T x, T y)
        {
            return myCompare(x, y);
        }

        public int GetHashCode(T obj)
        {
            return myGetHashCode(obj);
        }
    }
}
