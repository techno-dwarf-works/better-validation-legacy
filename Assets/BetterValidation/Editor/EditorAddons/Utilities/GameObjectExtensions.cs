using System.Collections.Generic;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Better.Validation.EditorAddons.Utilities
{
    public static class GameObjectExtensions
    {
        private static readonly IReadOnlyList<GameObject> Empty = new List<GameObject>();

        public static string FullPath(this GameObject go)
        {
            return go.transform.parent.IsNullOrDestroyed()
                ? go.name
                : FullPath(go.transform.parent.gameObject) + "/" + go.name;
        }

        public static string FullPath(this Object obj)
        {
            if (!PrefabUtility.IsPartOfPrefabAsset(obj))
            {
                switch (obj)
                {
                    case GameObject go:
                        return FullPath(go);
                    case Component component:
                        return FullPath(component.gameObject);
                }
            }

            return AssetDatabase.GetAssetPath(obj) + "/" + obj.name;
        }

        public static string FullPathNoRoot(this GameObject go)
        {
            return go.transform.parent.IsNullOrDestroyed()
                ? string.Empty
                : FullPathNoRoot(go.transform.parent.gameObject) + "/" + go.name;
        }

        public static string FullPathNoRoot(this Object obj)
        {
            if (!PrefabUtility.IsPartOfPrefabAsset(obj))
            {
                switch (obj)
                {
                    case GameObject go:
                        return FullPathNoRoot(go);
                    case Component component:
                        return FullPathNoRoot(component.gameObject);
                }
            }

            return AssetDatabase.GetAssetPath(obj) + "/" + obj.name;
        }

        public static IReadOnlyList<GameObject> GetAllChildren(this GameObject root)
        {
            if (root.IsNullOrDestroyed()) return Empty;
            var children = new List<GameObject>();

            var queue = new Queue<Transform>();
            queue.Enqueue(root.transform);
            while (queue.Count > 0)
            {
                var transform = queue.Dequeue();

                children.Add(transform.gameObject);

                foreach (Transform item in transform)
                    queue.Enqueue(item);
            }

            return children;
        }

        public static IReadOnlyList<Object> GetAllChildren(this Object root)
        {
            if (root.IsNullOrDestroyed()) return Empty;
            if (root is GameObject gameObject) return gameObject.GetAllChildren();

            return new[] { root };
        }
    }
}