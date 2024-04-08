using Runtime.ContentSystem.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.ContentSystem.UI
{
    public class ContentButton : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _contentNameText = null;

        private ContentDataDTO _contentDataDto;

        [SerializeField]
        private Button _loadButton;

        private ContentManager _contentManager;

        private void Awake()
        {
            _contentManager = FindObjectOfType<ContentManager>();

            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _loadButton.onClick.AddListener(OnLoadButtonClicked);
        }

        public void SetContentData(ContentDataDTO contentDataDto)
        {
            _contentDataDto = contentDataDto;

            _contentNameText.text = _contentDataDto.assetName;
        }

        private void OnLoadButtonClicked()
        {
            _contentManager.LoadContentAsync(_contentDataDto.downloadUrl);
        }

        private void UnsubscribeEvents()
        {
            _loadButton.onClick.RemoveListener(OnLoadButtonClicked);
        }

        private void OnDestroy()
        {
            UnsubscribeEvents();
        }
    }
}