using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Better.Validation.EditorAddons.ContextResolver;
using Better.Validation.EditorAddons.Iteration;
using Better.Validation.EditorAddons.Utilities;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Better.Validation.EditorAddons
{
    public class ValidatorCommands
    {
        public List<ValidationCommandData> ValidateAttributesInProject()
        {
            var allAssets = AssetDatabase.GetAllAssetPaths();
            var objs = allAssets.SelectMany(a =>
                AssetDatabase.LoadAssetAtPath<Object>(a).GetAllChildren().Where(file =>
                    file is ScriptableObject || file is GameObject)).ToArray();

            Iterator.SetContext(AssetResolver.Instance);
            return Iterator.ObjectsIteration(objs, IteratorFilter.PropertyIterationWithAttributes);
        }

        public List<ValidationCommandData> ValidateAttributesInCurrentScene()
        {
            var scene = SceneManager.GetActiveScene();
            Iterator.SetContext(SceneResolver.Instance);
            return Iterator.ObjectsIteration(scene.GetRootGameObjects().SelectMany(x => x.GetAllChildren()).ToList(),
                IteratorFilter.PropertyIterationWithAttributes);
        }

        public List<ValidationCommandData> ValidateAttributesInAllScenes()
        {
            Iterator.SetContext(SceneResolver.Instance);
            var list = new List<ValidationCommandData>();
            foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled))
            {
                var sceneReference = EditorSceneManager.OpenScene(scene.path);
                list.AddRange(Iterator.ObjectsIteration(sceneReference.GetRootGameObjects(), IteratorFilter.PropertyIterationWithAttributes));
            }

            return list;
        }

        public List<ValidationCommandData> FindMissingInAllScenes()
        {
            Iterator.SetContext(SceneResolver.Instance);
            var list = new List<ValidationCommandData>();
            foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled))
            {
                var sceneReference = EditorSceneManager.OpenScene(scene.path);
                list.AddRange(Iterator.ObjectsIteration(sceneReference.GetRootGameObjects(), IteratorFilter.MissingPropertyIteration));
            }

            return list;
        }


        public List<ValidationCommandData> FindMissingReferencesInCurrentScene()
        {
            var countLoaded = SceneManager.sceneCount;

            Iterator.SetContext(SceneResolver.Instance);
            var list = new List<ValidationCommandData>();
            for (var i = 0; i < countLoaded; i++)
            {
                var data = Iterator.ObjectsIteration(SceneManager.GetSceneAt(i).GetRootGameObjects(), IteratorFilter.MissingPropertyIteration);
                list.AddRange(data);
            }

            return list;
        }

        public List<ValidationCommandData> FindMissingReferencesInProject()
        {
            var allAssets = AssetDatabase.GetAllAssetPaths();
            var objs = allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject).Where(a => a != null).ToArray();

            Iterator.SetContext(AssetResolver.Instance);
            return Iterator.ObjectsIteration(objs, IteratorFilter.MissingPropertyIteration);
        }
    }
}