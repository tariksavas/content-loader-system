using Runtime.ContentSystem.Controller;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Runtime.ContentSystem.UI
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField]
        private Image _barImage;

        [SerializeField]
        private GameObject _loadingPanelObject;

        private void Awake()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            BundleLoader.DownloadStarted += OnDownloadStarted;
        }

        private void OnDownloadStarted(UnityWebRequest unityWebRequest)
        {
            _loadingPanelObject.SetActive(true);
            _barImage.fillAmount = 0;

            DownloadProgressingAsync(unityWebRequest);
        }

        private async void DownloadProgressingAsync(UnityWebRequest uwr)
        {
            while (uwr.downloadProgress < 1)
            {
                _barImage.fillAmount = uwr.downloadProgress;
                await Task.Delay(10);
            }

            _loadingPanelObject.SetActive(false);
        }

        private void UnsubscribeEvents()
        {
            BundleLoader.DownloadStarted -= OnDownloadStarted;
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }
    }
}