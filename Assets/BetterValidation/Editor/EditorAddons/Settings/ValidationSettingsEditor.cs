using Better.Commons.EditorAddons.CustomEditors.Attributes;
using Better.Commons.EditorAddons.CustomEditors.Base;
using Better.Commons.EditorAddons.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Better.Validation.EditorAddons.Settings
{
    //TODO: fix 'Better.Validation.EditorAddons.Settings.ValidationSettingsEditor' is missing the class attribute 'ExtensionOfNativeClass'!
    [MultiEditor(typeof(ValidationSettings), OverrideDefaultEditor = true)]
    public class ValidationSettingsEditor : ExtendedEditor
    {
        private SerializedProperty _disableBuildProperty;
        private SerializedProperty _logLevelProperty;
        private SerializedProperty _stepsProperty;
        
        private ValidationStepsList _list;

        public ValidationSettingsEditor(Object target, SerializedObject serializedObject) : base(target, serializedObject)
        {
            _disableBuildProperty = serializedObject.FindProperty("_disableBuildValidation");
            _logLevelProperty = serializedObject.FindProperty("_buildLoggingLevel");
            _stepsProperty = serializedObject.FindProperty("_validationSteps");
        }

        public override void OnEnable()
        {
        }

        public override VisualElement CreateInspectorGUI()
        {
            var container = VisualElementUtility.CreateVerticalGroup();
            var disableBuildProperty = new PropertyField(_disableBuildProperty);
            container.Add(disableBuildProperty);

            var logLevelProperty = new PropertyField(_logLevelProperty);
            container.Add(logLevelProperty);

            var listView = new ValidationStepsList(_stepsProperty);
            container.Add(listView);
            return container;
        }

        public override void OnDisable()
        {
        }

        public override void OnChanged(SerializedObject serializedObject)
        {
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
        }
    }
}