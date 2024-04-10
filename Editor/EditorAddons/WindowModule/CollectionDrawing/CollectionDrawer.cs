using System.Collections.Generic;
using Better.Commons.EditorAddons.Enums;
using Better.Commons.EditorAddons.Utility;
using Better.Validation.EditorAddons.Utility;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.WindowModule.CollectionDrawing
{
    public abstract class CollectionDrawer
    {
        private const string ScriptIconName = "cs Script Icon";
        protected ValidationCommandData _currentItem = null;
        public abstract int Count { get; }
        
        public abstract int Order { get; }

        public abstract string GetOptionName();

        public abstract CollectionDrawer Initialize(List<ValidationCommandData> data);

        public virtual CollectionDrawer CopyFrom(CollectionDrawer collectionDrawer)
        {
            _currentItem = collectionDrawer._currentItem;
            Initialize(collectionDrawer.GetRemaining());
            return this;
        }

        public abstract void DrawCollection();

        protected virtual void DrawBox(ValidationCommandData data)
        {
            var bufferColor = GUI.backgroundColor;

            using (var verticalScore = new EditorGUILayout.VerticalScope())
            {
                if (_currentItem == data)
                {
                    GUI.backgroundColor = Color.yellow;
                }

                GUI.Box(verticalScore.rect, GUIContent.none, EditorStyles.helpBox);

                EditorGUILayout.Space(ExtendedGUIUtility.SpaceHeight);

                DrawLabel(data);

                EditorGUILayout.Space(ExtendedGUIUtility.SpaceHeight);
                var iconType = data.Type.GetIconType();
                if (data.IsValid)
                {
                    GUI.backgroundColor = Color.green;
                    iconType = IconType.Checkmark;
                }

                ExtendedGUIUtility.HelpBox(data.Result, iconType, false);
                EditorGUILayout.Space(ExtendedGUIUtility.SpaceHeight);
            }

            GUI.backgroundColor = bufferColor;
        }

        protected virtual void DrawLabel(ValidationCommandData data)
        {
            using (var horizontalScore = new EditorGUILayout.HorizontalScope())
            {
                var reference = data.Target;

                var csIcon = EditorGUIUtility.IconContent(ScriptIconName);
                csIcon.text = reference.GetType().Name;

                var icon = EditorGUIUtility.GetIconForObject(reference);
                if (icon)
                {
                    csIcon.image = icon;
                }
                
                EditorGUILayout.LabelField(csIcon);
                EditorGUILayout.Space(ExtendedGUIUtility.SpaceHeight);

                if (GUILayout.Button("Show"))
                {
                    ValidationUtility.OpenReference(reference);
                    _currentItem = data;
                }
            }
        }

        public abstract void ClearResolved();

        public abstract ValidationCommandData GetNext();
        public abstract ValidationCommandData GetPrevious();
        public abstract void Revalidate();
        public abstract bool IsValid();

        protected abstract List<ValidationCommandData> GetRemaining();
    }
}