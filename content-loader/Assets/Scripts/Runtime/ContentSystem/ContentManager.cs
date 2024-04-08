using Runtime.ContentSystem.Controller;
using UnityEngine;
using Logger = Runtime.LogSystem.Logger;

namespace Runtime.ContentSystem
{
    public class ContentManager : MonoBehaviour
    {
        private Logger _logger;

        private string _activeContentUrl;

        private void Awake()
        {
            _logger = FindObjectOfType<Logger>();
        }

        public async void LoadContentAsync(string downloadUrl)
        {
            if (!string.IsNullOrEmpty(_activeContentUrl))
            {
                BundleLoader.UnloadBundleAsset(_activeContentUrl);
            }

            GameObject loadedContent = await BundleLoader.LoadBundleAssetAsync<GameObject>(downloadUrl);
            if (loadedContent == null)
            {
                _logger.Log($"There is no content to download in this url: {downloadUrl}", LogSystem.Model.LogType.Exception);
                return;
            }

            _activeContentUrl = downloadUrl;
            _logger.Log($"The content has been downloaded successfully.", LogSystem.Model.LogType.Information);

            SetPosAndRotContent(loadedContent.transform);

#if UNITY_EDITOR
            RefreshShaderModels(loadedContent);
#endif
        }

        private void SetPosAndRotContent(Transform _content)
        {
            Transform mainCam = Camera.main.transform;

            Vector3 tempPos = mainCam.position + mainCam.forward * 2f;
            tempPos.y = mainCam.position.y - 1.5f;
            _content.position = tempPos;
        }

        private void RefreshShaderModels(GameObject _content)
        {
            //Handling pink error for materials in only editor
            Renderer[] renderers = _content.GetComponentsInChildren<Renderer>();
            for (int index = 0; index < renderers.Length; index++)
            {
                Renderer renderer = renderers[index];
                Shader shader = Shader.Find(renderer.material.shader.name);
                renderer.material.shader = shader;
            }
        }
    }
}