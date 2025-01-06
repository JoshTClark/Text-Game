using System;
using System.IO;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;


public class InteractableCreator : EditorWindow
{
    private static string inputText = "";
    private static string log = "";
    private string pathToTemplate = "Assets/InteractableScriptTemplate.txt";

    [MenuItem("CustomMenu/Create Interactable")]
    static void CreateMenu()
    {
        //Type t = Type.GetType(typeof(Allie).FullName);
        //Debug.Log(t.FullName);
        //Debug.Log(typeof(Allie).FullName);

        inputText = "";
        log = "Log is here";
        InteractableCreator window = GetWindow<InteractableCreator>();
        window.ShowUtility();
    }

    void OnGUI()
    {
        inputText = EditorGUILayout.TextField("Object Name", inputText);

        if (GUILayout.Button("Create Class"))
        {
            StreamReader reader = new StreamReader(pathToTemplate);
            string templateText = reader.ReadToEnd();
            templateText = templateText.Replace("#FILE_NAME#", inputText);

            string path = "Assets/Scripts/Interactables/" + inputText + ".cs";
            System.IO.File.WriteAllText(path, templateText);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

            log = "Created new interactable with name " + inputText;
        }

        if (GUILayout.Button("Abort"))
            Close();

        EditorGUILayout.LabelField(log);
    }

}
