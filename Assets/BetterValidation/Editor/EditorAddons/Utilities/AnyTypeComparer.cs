using System;
using System.Collections.Generic;
using Better.EditorTools.Comparers;

namespace Better.Validation.EditorAddons.Utilities
{
    public class AnyTypeComparer :  BaseComparer<AnyTypeComparer, Type>, IEqualityComparer<Type>
    {
        public bool Equals(Type x, Type y)
        {
            return true;
        }

        public int GetHashCode(Type obj)
        {
            return 0;
        }
    }
}