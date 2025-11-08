using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Net;

namespace TreasureHunter.Network
{
    public static class ApiHelper
    {
        public static ApiResponse<BaseResponse<T>> HandleResponse<T>(UnityWebRequest request)
        {
            var response = new ApiResponse<BaseResponse<T>>
            {
                StatusCode = (HttpStatusCode)request.responseCode,
                RawJson = request.downloadHandler.text
            };

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    response.Data = JsonConvert.DeserializeObject<BaseResponse<T>>(response.RawJson);
                }
                catch (System.Exception e)
                {
                    response.Error = "JSON Parse Error: " + e.Message;
                }
            }
            else
            {
                response.Error = request.error;
            }

            return response;
        }
    }
}