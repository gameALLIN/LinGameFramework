using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class XLuaMgr : SingletonMono<XLuaMgr>
{
    public LuaEnv luaEnv = null;

    public string LuaScriptsFolder = "LuaScripts";

    protected override void Init()
    {
        base.Init();
        if (luaEnv == null)
        {
            luaEnv = new LuaEnv();
        }
        luaEnv.AddLoader(CustomLoader);
    }


    public void SafeDoString(string luaCode)
    {
        try
        {
            if (luaEnv == null)
            {
                Debug.LogError("LuaEnv is null!");
                return;
            }
            luaEnv.DoString(luaCode);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Lua error: " + ex.Message);
        }

    }


    public byte[] CustomLoader(ref string filepath)
    {
        Debug.Log("Loading Lua file: " + filepath);
        filepath = filepath.Replace('.', '/'); // Convert dots to slashes for folder structure
        string fullPath = System.IO.Path.Combine(Application.dataPath, LuaScriptsFolder, filepath + ".lua");
        if (System.IO.File.Exists(fullPath))
        {
            return System.IO.File.ReadAllBytes(fullPath);
        }
        else
        {
            Debug.LogError("Lua file not found: " + fullPath);
            return null;
        }
    }

    public override void Dispose()
    {
        if (luaEnv != null)
        {
            luaEnv.Dispose();
            luaEnv = null;
        }
    }
}
