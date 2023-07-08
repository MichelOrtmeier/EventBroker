using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBroker
{
    internal class IEnumerableIgnoringOrderComparer<TIEnumerableContent> : IEqualityComparer<IEnumerable<TIEnumerableContent>>
    {
        public bool Equals(IEnumerable<TIEnumerableContent> x, IEnumerable<TIEnumerableContent> y)
        {
            return OrderIEnumerableByHashCode(x).SequenceEqual(OrderIEnumerableByHashCode(y));
        }

        public int GetHashCode([DisallowNull] IEnumerable<TIEnumerableContent> obj)
        {
            return obj.GetHashCode();
        }

        private static IEnumerable<TIEnumerableContent> OrderIEnumerableByHashCode(IEnumerable<TIEnumerableContent> enumerable)
        {
            return enumerable.OrderBy((obj) => obj?.GetHashCode());
        }
    }
}
