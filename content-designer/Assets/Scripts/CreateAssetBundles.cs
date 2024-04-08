using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateAssetBundles
{
#if UNITY_EDITOR 
    public static string assetBundleDirectory = "Assets/AssetBundles/";

    [MenuItem("Assets/Build AssetBundles")]
    private static void BuildAllAssetBundles()
    {
        if (Directory.Exists(assetBundleDirectory))
            Directory.Delete(assetBundleDirectory, true);

        Directory.CreateDirectory(assetBundleDirectory);

        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
        AppendPlatformToFileName("Android");
        Debug.Log("Android bundle created...");

        RemoveSpacesInFileNames();

        AssetDatabase.Refresh();
        Debug.Log("Process complete!");
    }

    private static void RemoveSpacesInFileNames()
    {
        //If there are spaces in some strings, remove the spaces because you may get an errors.
        foreach (string path in Directory.GetFiles(assetBundleDirectory))
        {
            string oldName = path;
            string newName = path.Replace(' ', '-');
            File.Move(oldName, newName);
        }
    }

    private static void AppendPlatformToFileName(string platform)
    {
        foreach (string path in Directory.GetFiles(assetBundleDirectory))
        {
            //Get filename
            string[] files = path.Split('/');
            string fileName = files[files.Length - 1];

            //Delete files we dont need -> .manifest and AssetBundle files
            if (fileName.Contains(".") || fileName.Contains("Bundle"))
                File.Delete(path);

            else if (!fileName.Contains("-"))
            {
                //Append platform to filename
                FileInfo info = new FileInfo(path);
                info.MoveTo(path + "-" + platform);
            }
        }
    }
# endif
}