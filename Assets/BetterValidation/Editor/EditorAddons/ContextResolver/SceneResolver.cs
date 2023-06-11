using Better.Validation.EditorAddons.Utilities;
using UnityEngine;

namespace Better.Validation.EditorAddons.ContextResolver
{
    public class SceneResolver : IContextResolver
    {
        public static IContextResolver Instance { get; } = new SceneResolver();

        public string Resolve(Object obj)
        {
            return obj.FullPath();
        }
    }
}