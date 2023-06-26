using System;
using System.Collections.Generic;
using Better.Validation.Runtime.Attributes;
using UnityEngine;

[Serializable]
public class TestManaged
{
    [SceneReference]
    [SerializeField] private List<GameObject> _gameObject;

    [Find(typeof(Test), ValidateIfFieldEmpty = true)]
    [SerializeField] private Test _test;

}

public class Test : MonoBehaviour
{
    [SerializeField] private TestManaged _testManaged;
    
    [PrefabField]
    [SerializeField] private List<GameObject> _gameObjects;
    
    [NotNullAttribute]
    [SerializeField] private GameObject _gameObject;
    
    [DataValidation(nameof(ValidateIntValue), ValidationType = ValidationType.Error)]
    [DataValidation(nameof(ValidateIntValueBool), ValidationType = ValidationType.Warning)]
    [DataValidation(nameof(ValidateIntValueString), ValidationType = ValidationType.Info)]
    [SerializeField] private int intValue;

    private void ValidateIntValue(int value)
    {
        Debug.Log(value);
    }
    
    private bool ValidateIntValueBool(int value)
    {
        return value == 10;
    }
    
    private string ValidateIntValueString(int value)
    {
        if (ValidateIntValueBool(value))
        {
            return string.Empty;
        }

        return "Wrong value";
    }
}
