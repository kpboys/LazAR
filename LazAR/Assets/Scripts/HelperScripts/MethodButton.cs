using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//MADE BY PETER CHRISTENSEN
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ButtonAttribute))]
public class ButtonPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ButtonAttribute attri = attribute as ButtonAttribute;
        string methodName = attri.methodName;
        string buttonName = methodName;
        int buttonWidth = attri.buttonWidth;
        if (attri.buttonName != "")
            buttonName = attri.buttonName;

        Object target = property.serializedObject.targetObject;
        System.Type type = target.GetType();
        System.Reflection.MethodInfo method = type.GetMethod(methodName);

        Color baseColor = GUI.color;

        //If the method could not be found
        if (method == null)
        {
            GUI.color = Color.red;
            GUI.Label(position, "Method could not be found. Is it public?");
            GUI.color = baseColor;
            return;
        }

        System.Reflection.ParameterInfo[] paraInfos = method.GetParameters();
		  //If the method has parameters
		  if (paraInfos.Length > 0)
        {
            //If the parameter array in the attribute are null
            if(attri.parameters == null)
            {
					 GUI.color = Color.red;
					 GUI.Label(position, "Method has parameters, but no fields were defined in attribute.");
					 GUI.color = baseColor;
					 return;
				}
            //If the parameter array in the attribute does not contain as many entries as there are parameters in the method
            if(attri.parameters.Length != paraInfos.Length)
            {
					 GUI.color = Color.red;
					 GUI.Label(position, "Incorrect number of fields given as parameters.");
                GUI.color = baseColor;
                return;
				}
            //Loop through the list of parameters strings from attribute
				object[] parameters = new object[attri.parameters.Length];
				for (int i = 0; i < attri.parameters.Length; i++)
				{
                //Get the field's value as an object type value.
					 parameters[i] = type.GetField(attri.parameters[i]).GetValue(target);

                //If the types do no match, stop this process and display error message
                if (parameters[i].GetType() != paraInfos[i].ParameterType)
                {
						  GUI.color = Color.red;
						  GUI.Label(position, "Fields given are not same type as coresponding parameters");
						  GUI.color = baseColor;
						  return;
					 }
				}

				if (GUI.Button(new Rect(position.x + (position.width / 2 - buttonWidth / 2), position.y, buttonWidth, position.height), buttonName))
				{
					 method.Invoke(target, parameters);
				}
		  }
		  else if (GUI.Button(new Rect(position.x + (position.width / 2 - buttonWidth / 2), position.y, buttonWidth, position.height), buttonName))
		  {
				method.Invoke(target, null);
		  }

    }
}
#endif
/// <summary>
/// Attribute for quickly making a button in the inspector to call a public method.<br/>
/// Has overload for methods with parameters.<br/>
/// Proper usage: <br/>
/// - Make a "dummy" field. The type doesn't matter but bool is recommended. <br/>
/// - Make sure it is serialized (public access modifier or SerializeField attribute. <br/>
/// - Add this attribute to it and fill out constructor.
/// </summary>
public class ButtonAttribute : PropertyAttribute
{
    public readonly string methodName;
    public readonly string buttonName;
    public readonly int buttonWidth;
    public readonly string[] parameters;

    /// <summary>
    /// The constructor for the attribute.
    /// </summary>
    /// <param name="methodName">The name of the method to call. Use nameof to get this.</param>
    /// <param name="buttonName">Optional override of the text written on the button.</param>
    /// <param name="buttonWidth">The width of the button. Default is 200.</param>
    public ButtonAttribute(string methodName, string buttonName = "", int buttonWidth = 200)
    {
        this.methodName = methodName;
        this.buttonName = "";
        this.buttonWidth = buttonWidth;
    }
	 /// <summary>
	 /// The constructor for the attribute.
	 /// </summary>
	 /// <param name="methodName">The name of the method to call. Use nameof to get this.</param>
	 /// <param name="parameters">The names of fields you wish to use as parameters. Use nameof to get the names of the fields.<br/>
    ///                          Make sure this array lines up with the method's parameters in both number of entries and types.</param>
	 /// <param name="buttonName">Optional override of the text written on the button.</param>
	 /// <param name="buttonWidth">The width of the button. Default is 200.</param>
	 public ButtonAttribute(string methodName, string[] parameters, string buttonName = "", int buttonWidth = 200)
	 {
		  this.methodName = methodName;
		  this.buttonName = "";
		  this.buttonWidth = buttonWidth;
        this.parameters = parameters;
	 }
}
