using Rumtime.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Runtime.ContentSystem.Controller
{
    public class BundleLoader
    {
        private static Dictionary<string, Object> _downloadedBundles = new Dictionary<string, Object>();

        private static Dictionary<string, Task> _downloadingQueue = new Dictionary<string, Task>();

        public static event Action<UnityWebRequest> DownloadStarted;

        public static async Task<T> LoadBundleAssetAsync<T>(string bundleUrl) where T : Object
        {
            if (_downloadingQueue.ContainsKey(bundleUrl))
            {
                await _downloadingQueue[bundleUrl];
            }

            if (_downloadedBundles.ContainsKey(bundleUrl))
            {
                return _downloadedBundles[bundleUrl] as T;
            }

            T loadedObject = null;

            UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(bundleUrl);

            DownloadStarted?.Invoke(webRequest);
            
            Task task = webRequest.SendWebRequest().GetAwaiter();
            _downloadingQueue.Add(bundleUrl, task);

            await task;

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(webRequest);

                if (assetBundle != null)
                {
                    string rootAssetPath = assetBundle.GetAllAssetNames()[0];

                    T downloadedObject = assetBundle.LoadAsset(rootAssetPath) as T;
                    assetBundle.Unload(false);

                    loadedObject = Object.Instantiate(downloadedObject);
                    _downloadedBundles.Add(bundleUrl, loadedObject);
                }
            }

            _downloadingQueue.Remove(bundleUrl);
            return loadedObject;
        }

        public static void UnloadBundleAsset(string bundleUrl)
        {
            Object.Destroy(_downloadedBundles[bundleUrl]);

            _downloadedBundles.Remove(bundleUrl);
        }
    }
}