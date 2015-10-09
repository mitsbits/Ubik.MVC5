using System;
using System.Linq.Expressions;

namespace Ubik.Infra.DataManagement
{
    public struct OrderByInfo<T> where T : class
    {
        public Expression<Func<T, dynamic>> Property;
        public bool Ascending;
    }
}
