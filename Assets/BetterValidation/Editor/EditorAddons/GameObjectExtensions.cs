using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Better.Validation.EditorAddons
{
    public static class GameObjectExtensions
    {
        public static string FullPath(this GameObject go)
        {
            return go.transform.parent == null
                ? go.name
                : FullPath(go.transform.parent.gameObject) + "/" + go.name;
        }

        public static string FullPathNoRoot(this GameObject go)
        {
            return go.transform.parent == null
                ? string.Empty
                : FullPathNoRoot(go.transform.parent.gameObject) + "/" + go.name;
        }

        public static List<GameObject> GetAllChildren(this GameObject root)
        {
            if (root == null) return new List<GameObject>();
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
    }
}