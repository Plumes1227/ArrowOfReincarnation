using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(string saveFileName, object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        try
        {
            File.WriteAllText(path, json);

            #if UNITY_EDITOR
            Debug.Log($"成功儲檔 至 {path}.");
            #endif
        }
        catch(System.Exception exception)
        {
            #if UNITY_EDITOR
            Debug.LogError($"存檔失敗 至 {path}. \n{exception}");
            #endif
        }
    }

    public static T Load<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);

            return data;
        }
        catch (System.Exception exception)
        {
            #if UNITY_EDITOR
            Debug.LogError($"讀取存檔失敗 從 {path}. \n{exception}");
            #endif

            return default;
        }
    }

    public static void DeleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
             File.Delete(path);
        }
        catch (System.Exception exception)
        {
            #if UNITY_EDITOR
            Debug.LogError($"刪除檔案失敗 {path}. \n{exception}");
            #endif
        }
    }

    public static bool SaveFileExists(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        
        return File.Exists(path);
    }
}