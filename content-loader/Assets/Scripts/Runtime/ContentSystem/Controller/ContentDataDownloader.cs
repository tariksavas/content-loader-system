using Newtonsoft.Json;
using Rumtime.Utility;
using Runtime.ContentSystem.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Runtime.ContentSystem.Controller
{
    public class ContentDataDownloader : MonoBehaviour
    {
        public const string URL = "https://raw.githubusercontent.com/tariksavas/content-loader-system/main/content/ContentList.txt";

        public static async Task<ContentDataDTO[]> GetContentDTOAsync()
        {
            ContentDataDTO[] contentDataDtos = Array.Empty<ContentDataDTO>();

            UnityWebRequest www = new UnityWebRequest(URL, UnityWebRequest.kHttpVerbGET);

            string path = Application.persistentDataPath + "/DownloadList.json";
            www.downloadHandler = new DownloadHandlerFile(path);

            await www.SendWebRequest().GetAwaiter();

            if (www.result == UnityWebRequest.Result.Success)
            {
                StreamReader reader = new StreamReader(path);

                string dataText = reader.ReadToEnd();
                contentDataDtos = JsonConvert.DeserializeObject<ContentDataDTO[]>(dataText);

                reader.Close();
            }
            else
            {
                contentDataDtos = null;
            }

            return contentDataDtos;
        }
    }
}