using System.IO;
using UnityEngine;

public class DataLogger : MonoBehaviour
{
   public static void Append(string fileName, string[] cols)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        bool newFile = !File.Exists(path);
        using (var sw = new StreamWriter(path, true))
        {
            if (newFile) sw.WriteLine("timestamp,path,samples");
            sw.WriteLine(string.Join(",", cols));
        }
        Debug.Log($"CSV logged: {path}");
    }

    public static string[] LoadAllPaths(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path)) return new string[0];
        var lines = File.ReadAllLines(path);
        var list = new System.Collections.Generic.List<string>();
        for (int i = 1; i < lines.Length; i++)
        {
            var parts = lines[i].Split(',');
            if (parts.Length >= 2) list.Add(parts[1]);
        }
        return list.ToArray();
    }
}
