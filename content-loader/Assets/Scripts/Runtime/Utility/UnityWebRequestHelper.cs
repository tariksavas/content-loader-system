using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Rumtime.Utility
{
    public static class UnityWebRequestHelper
    {
        public static async Task GetAwaiter(this UnityWebRequestAsyncOperation @this)
        {
            while (!@this.isDone)
            {
                await Task.Delay(10);
            }
        }
    }
}