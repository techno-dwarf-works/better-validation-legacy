using Better.Validation.EditorAddons.Utilities;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.ContextResolver
{
    public class AssetResolver : IContextResolver
    {
        public static IContextResolver Instance { get; } = new AssetResolver();

        public string Resolve(Object obj)
        {
            if (obj is GameObject gameObject)
            {
                var hasParent = gameObject.transform.parent != null;
                return AssetDatabase.GetAssetPath(obj) + (hasParent ? obj.FullPathNoRoot() : string.Empty);
            }

            return obj.FullPathNoRoot();
        }
    }
}