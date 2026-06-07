using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class LuaTestCreateTool : EditorWindow
{
    private string author = "";
    private string description = "";
    private string fileName = "";
    private bool createSubFolder;

    [MenuItem("Tools/XLua/CreateTestLuaScript")]
    public static void ShowWindow()
    {
        var window = GetWindow<LuaTestCreateTool>("Create Test Lua Script");
        window.minSize = new Vector2(400, 250);
    }

    private void OnEnable()
    {
        author = EditorPrefs.GetString("LuaCreateTool_Author", "");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Test Lua Script", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);

        author = EditorGUILayout.TextField("Author", author);
        fileName = EditorGUILayout.TextField("Test Name", fileName);
        createSubFolder = EditorGUILayout.Toggle("Create Sub Folder", createSubFolder);
        description = EditorGUILayout.TextArea(description, GUILayout.Height(60));

        EditorGUILayout.Space(20);

        if (GUILayout.Button("Create", GUILayout.Height(30)))
        {
            CreateTestLuaScript();
        }
    }

    private void CreateTestLuaScript()
    {
        if (string.IsNullOrEmpty(fileName))
        {
            EditorUtility.DisplayDialog("Error", "Test name cannot be empty", "OK");
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
        template = template.Replace("local M = {}", string.Format("print('开启{0}测试')\n\nlocal M = {{}}", fileName));

        string fullDir = Path.Combine(Application.dataPath, "LuaScripts/Test");
        if (createSubFolder)
        {
            fullDir = Path.Combine(fullDir, fileName);
        }
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
        AddRequireToTestMain(fileName, createSubFolder);

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Success", "Test script created:\n" + fullPath, "OK");
    }

    private void AddRequireToTestMain(string testName, bool hasSubFolder)
    {
        string testMainPath = Path.Combine(Application.dataPath, "LuaScripts/Test/TestMain.lua");
        if (!File.Exists(testMainPath))
        {
            Debug.LogWarning("TestMain.lua not found: " + testMainPath);
            return;
        }

        string content = File.ReadAllText(testMainPath);

        // XLua loader 将 '.' 转为 '/' 来定位文件，路径相对于 LuaScripts/ 根目录
        // 有子文件夹: Test/XXX/XXX.lua → require 'Test.XXX.XXX'
        // 无子文件夹: Test/XXX.lua     → require 'Test.XXX'
        string requirePath = hasSubFolder
            ? string.Format("Test.{0}.{0}", testName)
            : string.Format("Test.{0}", testName);

        string requireLine = string.Format("require '{0}'", requirePath);

        if (content.Contains(requireLine))
        {
            return;
        }

        // 找到最后一个 require 行，在其后插入
        string pattern = @"require\s+'[^']+'\s*$";
        MatchCollection matches = Regex.Matches(content, pattern, RegexOptions.Multiline);

        if (matches.Count > 0)
        {
            Match lastMatch = matches[matches.Count - 1];
            int insertIndex = lastMatch.Index + lastMatch.Length;
            content = content.Insert(insertIndex, requireLine + "\n");
        }
        else
        {
            content = content.TrimEnd() + "\n" + requireLine + "\n";
        }

        File.WriteAllText(testMainPath, content);
    }
}
