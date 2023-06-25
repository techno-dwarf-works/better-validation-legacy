using UnityEngine;

namespace Better.Validation.EditorAddons.ContextResolver
{
    public interface IContextResolver
    {
        public string Resolve(Object obj);
    }
}