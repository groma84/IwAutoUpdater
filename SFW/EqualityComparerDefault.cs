using System;
using System.Collections.Generic;

namespace IwAutoUpdater.CrossCutting.SFW
{
    public class EqualityComparerDefault<T> : IEqualityComparer<T>
    {
        Func<T, T, bool> _equal;
        Func<T, int> _hash;

        public EqualityComparerDefault(Func<T, T, bool> equal = null, Func<T, int> hash = null)
        {
            if (equal != null)
            {
                _equal = equal;
            }
            else
            {
                _equal = (x, y) => x.Equals(y);
            }

            if (hash != null)
            {
                _hash = hash;
            }
            else if (typeof(T).IsPrimitive)
            {
                _hash = a => a.GetHashCode();
            }
            else
            {
                _hash = a => 0;
            }
        }

        #region IEqualityComparer<T> Members

        bool IEqualityComparer<T>.Equals(T x, T y)
        {
            return _equal(x, y);
        }

        int IEqualityComparer<T>.GetHashCode(T obj)
        {
            return _hash(obj);
        }

        #endregion
    }
}
