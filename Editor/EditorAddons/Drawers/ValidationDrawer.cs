using Better.Commons.EditorAddons.Drawers;
using Better.Commons.EditorAddons.Drawers.Container;
using Better.Commons.EditorAddons.Extensions;
using Better.Commons.EditorAddons.Utility;
using Better.Commons.Runtime.Extensions;
using Better.Validation.EditorAddons.Handlers;
using Better.Validation.EditorAddons.Utility;
using Better.Validation.Runtime.Attributes;
using UnityEditor;
using UnityEngine.UIElements;

namespace Better.Validation.EditorAddons.Drawers
{
    [CustomPropertyDrawer(typeof(ValidationAttribute))]
    public class ValidationDrawer : BasePropertyDrawer<PropertyValidationHandler, ValidationAttribute>
    {
        protected override void PopulateContainer(ElementsContainer container)
        {
            UpdateDrawer(container);
            container.SerializedPropertyChanged += UpdateDrawer;
        }

        private void UpdateDrawer(ElementsContainer container)
        {
            var handler = GetHandler(container.SerializedProperty);
            handler.Setup(container.SerializedProperty, FieldInfo, Attribute);

            if (handler.IsSupported())
            {
                var validation = handler.Validate();

                HelpBox helpBox;
                if (!container.TryGetByTag(container.SerializedProperty, out var element))
                {
                    helpBox = new HelpBox();
                    element = container.CreateElementFrom(helpBox);
                    element.AddTag(container.SerializedProperty);
                }
                else
                {
                    helpBox = element.Q<HelpBox>();
                }

                helpBox.text = validation.Result;
                helpBox.messageType = handler.Type.GetMessageType();

                helpBox.style.SetVisible(!validation.State);
            }
        }
    }
}