using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

//MADE BY PETER CHRISTENSEN
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EasyDropdownAttribute))]
public class EasyDropdownDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int index = 0;
        EasyDropdownAttribute attri = attribute as EasyDropdownAttribute;
        string optionsName = attri.optionsArrayName;

        Object target = property.serializedObject.targetObject;
        System.Type type = target.GetType();
        System.Reflection.FieldInfo options = type.GetField(optionsName);

        //Catch if something is done wrong
        if (options == null)
        {
            GUI.Label(position, "Field could not be found.");
            return;
        }
        if(options.GetValue(target) is string[] == false)
        {
            GUI.Label(position, "The given options is not a string array.");
            return;
        }
        if((options.GetValue(target) as string[]).Length == 0)
        {
            GUI.Label(position, "No options in collection.");
            return;
        }

        //Get the string array properly to make things easier
        string[] actualOptions = options.GetValue(target) as string[];

        //Figure out the index that was previously selected
        //If nothing was chosen before, set it to the first option
        if (property.stringValue != null)
        {
            for (int i = 0; i < actualOptions.Length; i++)
            {
                if (actualOptions[i] == property.stringValue)
                {
                    index = i; 
                    break;
                }
            }
        }
        else
        {
            property.stringValue = actualOptions[0];
            index = 0;
        }

        //Actual dropdown
        index = EditorGUI.Popup(position, property.displayName,
             index, actualOptions, EditorStyles.popup);

        //Update selected option
        property.stringValue = actualOptions[index];
    }
}
#endif
/// <summary>
/// Attribute for making a dropdown for strings <br/>
/// Setup:<br/>
/// - Put this attribute on a string field that is serialized.<br/>
/// - Use nameof to input the name of an array of strings that you have in the same script. It must also be serialized<br/>
/// </summary>
public class EasyDropdownAttribute : PropertyAttribute
{
    public readonly string optionsArrayName;

    public EasyDropdownAttribute(string optionsArrayName)
    {
        this.optionsArrayName = optionsArrayName;
    }
}