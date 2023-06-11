using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Better.EditorTools;
using Better.EditorTools.Drawers.Base;
using Better.EditorTools.Helpers.Caching;
using Better.Extensions.Runtime;
using Better.Validation.EditorAddons.ContextResolver;
using Better.Validation.EditorAddons.Utilities;
using Better.Validation.EditorAddons.ValidationWrappers;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Better.Validation.EditorAddons
{
    public class ValidatorCommands
    {
        public async Task<List<ValidationCommandData>> ValidateAttributesInProject()
        {
            var allAssets = AssetDatabase.GetAllAssetPaths();
            var objs = allAssets.SelectMany(a =>
                AssetDatabase.LoadAssetAtPath<Object>(a).GetAllChildren().Where(file =>
                    file is ScriptableObject || file is GameObject)).ToArray();

            Iterator.SetContext(AssetResolver.Instance);
            return await Iterator.ObjectsIteration(objs, IteratorFilter.PropertyIterationWithAttributes);
        }

        public async Task<List<ValidationCommandData>> ValidateAttributesInCurrentScene()
        {
            var scene = SceneManager.GetActiveScene();
            Iterator.SetContext(SceneResolver.Instance);
            return await Iterator.ObjectsIteration(scene.GetRootGameObjects().SelectMany(x => x.GetAllChildren()).ToList(),
                IteratorFilter.PropertyIterationWithAttributes);
        }

        public async Task<List<ValidationCommandData>> MissingInAllScenes()
        {
            Iterator.SetContext(AssetResolver.Instance);
            var list = new List<ValidationCommandData>();
            foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled))
            {
                var sceneReference = EditorSceneManager.OpenScene(scene.path);
                list.AddRange(await Iterator.ObjectsIteration(sceneReference.GetRootGameObjects(), IteratorFilter.MissingPropertyIteration));
            }

            return list;
        }


        public async Task<List<ValidationCommandData>> FindMissingReferencesInCurrentScene()
        {
            var countLoaded = SceneManager.sceneCount;

            Iterator.SetContext(AssetResolver.Instance);
            var list = new List<ValidationCommandData>();
            for (var i = 0; i < countLoaded; i++)
            {
                var data = await Iterator.ObjectsIteration(SceneManager.GetSceneAt(i).GetRootGameObjects(), IteratorFilter.MissingPropertyIteration);
                list.AddRange(data);
            }

            return list;
        }

        public async Task<List<ValidationCommandData>> MissingReferencesInProject()
        {
            var allAssets = AssetDatabase.GetAllAssetPaths();
            var objs = allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject).Where(a => a != null).ToArray();

            Iterator.SetContext(AssetResolver.Instance);
            return await Iterator.ObjectsIteration(objs, IteratorFilter.MissingPropertyIteration);
        }
    }
}