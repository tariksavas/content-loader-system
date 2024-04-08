using Runtime.ContentSystem.Controller;
using Runtime.ContentSystem.Model;
using UnityEngine;
using Logger = Runtime.LogSystem.Logger;

namespace Runtime.ContentSystem.UI
{
    public class ContentPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject _contentButtonPrefab = null;

        [SerializeField]
        private Transform _parent = null;

        private Logger _logger;

        private void Awake()
        {
            _logger = FindObjectOfType<Logger>();
            
            CreateContentButtonsAsync();
        }

        private async void CreateContentButtonsAsync()
        {
            ContentDataDTO[] contentDataDto = await ContentDataDownloader.GetContentDTOAsync();
            if (contentDataDto == null)
            {
                _logger.Log($"There is no content data to download in this url: {ContentDataDownloader.URL}", LogSystem.Model.LogType.Exception);
                return;
            }

            _logger.Log($"The Content Data was successfully downloaded from: {ContentDataDownloader.URL}", LogSystem.Model.LogType.Information);

            for (int index = 0; index < contentDataDto.Length; index++)
            {
                ContentButton modelButton = Instantiate(_contentButtonPrefab, _parent).GetComponent<ContentButton>();
                modelButton.SetContentData(contentDataDto[index]);
            }
        }
    }
}