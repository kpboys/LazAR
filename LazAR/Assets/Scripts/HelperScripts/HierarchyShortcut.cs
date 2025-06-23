using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class HierarchyShortcut : MonoBehaviour
{
    [SerializeField]
    private GameObject shortcutTarget;
    [SerializeField, Button(nameof(OpenShortcut))]
    private bool btnOpenShortcutWindow;

    public void OpenShortcut()
    {
        if (shortcutTarget == null) return;
#if UNITY_EDITOR
        EditorUtility.OpenPropertyEditor(shortcutTarget);
#endif
    }
}
#region OLD_CODE
//public class ShortcutWindow : EditorWindow
//{
//    private Object asset;
//    private Editor assetEditor;
//    private Component[] comps;
//    private Editor[] compEditors;

//    private Vector2 scrollPosition;

//    public static ShortcutWindow Create(GameObject asset)
//    {
//        var window = CreateWindow<ShortcutWindow>($"{asset.name} | {asset.GetType().Name}");
//        window.asset = asset;
//        window.comps = asset.GetComponents<Component>();
//        Editor.CreateCachedEditor(asset, null, ref window.assetEditor);
//        window.compEditors = new Editor[window.comps.Length];
//        for (int i = 0; i < window.comps.Length; i++)
//        {
//            Editor.CreateCachedEditor(window.comps[i], null, ref window.compEditors[i]);
//        }
//        return window;
//    }

//    private void OnGUI()
//    {
//        GUI.enabled = false;
//        asset = EditorGUILayout.ObjectField("Asset", asset, asset.GetType(), false);
//        GUI.enabled = true;

//        scrollPosition = GUILayout.BeginScrollView(scrollPosition, true, true);
//        //GUILayout.BeginArea(new Rect(0, 0, 300, 300));    //Does not display correctly if this is not commented out!

//        EditorGUILayout.BeginVertical();
//        assetEditor.DrawHeader();
//        assetEditor.OnInspectorGUI();
//        for (int i = 0; i < compEditors.Length; i++)
//        {
//            compEditors[i].OnInspectorGUI();
//        }
//        EditorGUILayout.EndVertical();

//        //GUILayout.EndArea();
//        GUILayout.EndScrollView();
//    }
//    //private static GameObject target;
//    //private static Editor editor = null;
//    //private Editor firstComp = null;
//    //public static void OpenWindow(GameObject targetToShow)
//    //{
//    //    target = targetToShow;
//    //    Editor.CreateCachedEditor(target, null, ref editor);
//    //    EditorWindow.GetWindow(typeof(ShortcutWindow));
//    //}
//    //private void OnGUI()
//    //{
//    //    editor.OnInspectorGUI();

//    //}
//}
////[CustomEditor(typeof(HierarchyShortcut))]
////public class HierarchyShortcutEditor : Editor
////{
////    private GameObject prevTarget;
////    private Editor editor = null;
////    public override void OnInspectorGUI()
////    {
////        base.OnInspectorGUI();

////        HierarchyShortcut shortcut = (HierarchyShortcut)target;

////        if (shortcut.shortcutTarget != null)
////        {
////            if(shortcut.shortcutTarget != prevTarget)
////            {
////                Debug.Log("Made inspector");
////                Editor.CreateCachedEditor(shortcut.shortcutTarget.transform, null, ref editor);
////            }
////            editor.OnInspectorGUI();
////        }
////        prevTarget = shortcut.shortcutTarget;
////    }
////}
#endregion
