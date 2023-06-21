using System;
using System.Collections.Generic;
using Better.EditorTools.Helpers;
using Better.Validation.EditorAddons.Utilities;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Better.Validation.EditorAddons.WindowModule
{
    public abstract class CollectionDrawer
    {
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

                EditorGUILayout.Space(DrawersHelper.SpaceHeight);

                DrawLabel(data);

                EditorGUILayout.Space(DrawersHelper.SpaceHeight);
                var iconType = data.Type.GetIconType();
                if (data.IsValid)
                {
                    GUI.backgroundColor = Color.green;
                    iconType = IconType.Checkmark;
                }

                DrawersHelper.HelpBox(data.Result, iconType, false);
                EditorGUILayout.Space(DrawersHelper.SpaceHeight);
            }

            GUI.backgroundColor = bufferColor;
        }

        protected virtual void DrawLabel(ValidationCommandData data)
        {
            using (var horizontalScore = new EditorGUILayout.HorizontalScope())
            {
                var reference = data.Target;

                var icon = EditorGUIUtility.GetIconForObject(reference);
                var csIcon = EditorGUIUtility.IconContent("cs Script Icon");
                csIcon.text = reference.GetType().Name;
                if (icon)
                {
                    csIcon.image = icon;
                }

                EditorGUILayout.LabelField(csIcon);
                EditorGUILayout.Space(DrawersHelper.SpaceHeight);

                if (GUILayout.Button("Show"))
                {
                    ValidationWindow.OpenReference(reference);
                    _currentItem = data;
                }
            }
        }

        public abstract void ClearResolved();

        public abstract ValidationCommandData GetNext();
        public abstract void Revalidate();
        public abstract bool IsValid();

        protected abstract List<ValidationCommandData> GetRemaining();
    }
}