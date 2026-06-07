using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LuaCreateTool : EditorWindow
{
    private string author = "";
    private string description = "";
    private string fileName = "";
    private string saveFolderPath = "Assets/LuaScripts";

    [MenuItem("Tools/XLua/CreateLuaScript")]
    public static void ShowWindow()
    {
        var window = GetWindow<LuaCreateTool>("Create Lua Script");
        window.minSize = new Vector2(400, 250);
    }

    private void OnEnable()
    {
        author = EditorPrefs.GetString("LuaCreateTool_Author", "");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Lua Script", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);

        author = EditorGUILayout.TextField("Author", author);
        fileName = EditorGUILayout.TextField("File Name", fileName);
        saveFolderPath = EditorGUILayout.TextField("Save Path", saveFolderPath);
        description = EditorGUILayout.TextArea(description, GUILayout.Height(60));

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Create", GUILayout.Height(30)))
        {
            CreateLuaScript();
        }
    }

    private void CreateLuaScript()
    {
        if (string.IsNullOrEmpty(fileName))
        {
            EditorUtility.DisplayDialog("Error", "File name cannot be empty", "OK");
            return;
        }

        EditorPrefs.SetString("LuaCreateTool_Author", author);

        string templatePath = Path.Combine(Application.dataPath, "Editor/XLuaTool/Common/LuaTemplate.txt");
        if (!File.Exists(templatePath))
        {
            EditorUtility.DisplayDialog("Error", "Template not found: " + templatePath, "OK");
            return;
        }

        string template = File.ReadAllText(templatePath);
        template = template.Replace("#AUTHOR#", author);
        template = template.Replace("#DATETIME#", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        template = template.Replace("#DESCRIPTION#", description);

        string fullDir = Path.Combine(Application.dataPath, saveFolderPath.Replace("Assets/", ""));
        if (!Directory.Exists(fullDir))
        {
            Directory.CreateDirectory(fullDir);
        }

        string fullPath = Path.Combine(fullDir, fileName + ".lua");
        if (File.Exists(fullPath))
        {
            if (!EditorUtility.DisplayDialog("Confirm", fileName + ".lua already exists, overwrite?", "Yes", "No"))
                return;
        }

        File.WriteAllText(fullPath, template);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", "Lua script created:\n" + fullPath, "OK");
    }
}
