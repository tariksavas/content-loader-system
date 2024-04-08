using System;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using LogType = Runtime.LogSystem.Model.LogType;

namespace Runtime.LogSystem
{
    public class Logger : MonoBehaviour
    {
        private const int CLOSE_TIME = 3000;

        [SerializeField]
        private TMP_Text _ownText;

        private CancellationTokenSource _cancellationTokenSource = new();

        public void Log(string logText, LogType logType)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            _ownText.text = logText;
            _ownText.color = logType switch
            {
                LogType.Information => Color.green,
                LogType.Warning => Color.yellow,
                LogType.Exception => Color.red
            };

            LogAsync(_cancellationTokenSource.Token);
        }

        private async void LogAsync(CancellationToken cancellationToken = default)
        {
            _ownText.enabled = true;
            
            try
            {
                await Task.Delay(CLOSE_TIME, cancellationToken);
            }
            catch (Exception)
            {
            }

            _ownText.enabled = false;
        }

        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}