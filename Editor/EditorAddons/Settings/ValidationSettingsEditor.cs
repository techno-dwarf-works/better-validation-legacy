using System;
using Better.EditorTools.Helpers;
using UnityEditor;
using UnityEditorInternal;
using Object = UnityEngine.Object;

namespace Better.Validation.EditorAddons.Settings
{
    [CustomEditor(typeof(ValidationSettings))]
    public class ValidationSettingsEditor : Editor
    {
        private SerializedProperty _disableBuildProperty;
        private SerializedProperty _logLevelProperty;
        private BuildValidationStepsDrawer _drawer;
        private SerializedProperty _stepsProperty;
        private Object _target;

        private void OnEnable()
        {
            _disableBuildProperty = serializedObject.FindProperty("disableBuildValidation");
            _logLevelProperty = serializedObject.FindProperty("buildLoggingLevel");
            _stepsProperty = serializedObject.FindProperty("validationSteps");
            _drawer = new BuildValidationStepsDrawer(serializedObject, _stepsProperty);
            _target = serializedObject.targetObject;
        }

        public override bool RequiresConstantRepaint()
        {
            return EditorUtility.IsDirty(_target);
        }

        public override void OnInspectorGUI()
        {
            using(var scope = new EditorGUI.ChangeCheckScope())
            {
                if(EditorUtility.IsDirty(_target))
                {
                    serializedObject.Update();
                    serializedObject.ApplyModifiedProperties();
                }
                EditorGUILayout.PropertyField(_disableBuildProperty);
                EditorGUILayout.PropertyField(_logLevelProperty);
                _drawer.DoLayoutList();
                if (scope.changed)
                {
                    EditorUtility.SetDirty(_target);
                }
            }
        }
    }
}