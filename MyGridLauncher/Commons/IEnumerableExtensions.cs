using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTray.Commons {
    public static class IEnumerableExtensions {
        private sealed class CommonSelector<T, TKey> : IEqualityComparer<T> {
            private Func<T, TKey> _selector;

            public CommonSelector(Func<T, TKey> selector) {
                _selector = selector;
            }

            public bool Equals(T x, T y) {
                return _selector(x).Equals(_selector(y));
            }

            public int GetHashCode(T obj) {
                return _selector(obj).GetHashCode();
            }
        }

        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector) {
            return source.Distinct(new CommonSelector<T, TKey>(selector));
        }
    }
}
