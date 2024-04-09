using UnityEditor;
using System.IO;
using UnityEngine;

namespace Editor.AssetBundleSystem
{
    public class AssetBundleBuilder
    {
#if UNITY_EDITOR
        public const string ASSET_BUNDLE_DIRECTORY = "Assets/AssetBundles/";

        [MenuItem("AssetBundleCreator/Build for Android")]
        private static void BuildAndroid()
        {
            BuildAllAssetBundlesBuild(BuildTarget.Android);
        }

        [MenuItem("AssetBundleCreator/Build for iOS")]
        private static void BuildIOS()
        {
            BuildAllAssetBundlesBuild(BuildTarget.iOS);
        }

        private static void BuildAllAssetBundlesBuild(BuildTarget buildTarget)
        {
            if (Directory.Exists(ASSET_BUNDLE_DIRECTORY))
            {
                Directory.Delete(ASSET_BUNDLE_DIRECTORY, true);
            }

            Directory.CreateDirectory(ASSET_BUNDLE_DIRECTORY);

            BuildPipeline.BuildAssetBundles(ASSET_BUNDLE_DIRECTORY, BuildAssetBundleOptions.None, buildTarget);
            AppendPlatformToFileName(buildTarget.ToString());

            Debug.Log($"{buildTarget} bundle created...");

            RemoveSpacesInFileNames();

            AssetDatabase.Refresh();
            Debug.Log("Process complete!");
        }

        private static void RemoveSpacesInFileNames()
        {
            string[] paths = Directory.GetFiles(ASSET_BUNDLE_DIRECTORY);

            for (int index = 0; index < paths.Length; index++)
            {
                string path = paths[index];
                string oldName = path;
                string newName = path.Replace(' ', '-');

                File.Move(oldName, newName);
            }
        }

        private static void AppendPlatformToFileName(string platform)
        {
            string[] paths = Directory.GetFiles(ASSET_BUNDLE_DIRECTORY);

            for (int index = 0; index < paths.Length; index++)
            {
                string path = paths[index];

                string[] files = path.Split('/');
                string fileName = files[files.Length - 1];

                if (fileName.Contains(".") || fileName.Contains("Bundle"))
                {
                    File.Delete(path);
                }
                else if (!fileName.Contains("-"))
                {
                    FileInfo info = new FileInfo(path);
                    info.MoveTo(path + "-" + platform);
                }
            }
        }
#endif
    }
}