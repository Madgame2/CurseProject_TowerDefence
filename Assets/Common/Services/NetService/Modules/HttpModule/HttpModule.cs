using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Common.Services.Net.Modules
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T Data { get; }
        public string Error { get; }
        public int StatusCode { get; }

        private Result(bool isSuccess, T data, string error, int statusCode)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
            StatusCode = statusCode;
        }

        public static Result<T> Success(T data, int statusCode = 200)
            => new Result<T>(true, data, null, statusCode);

        public static Result<T> Failure(string error, int statusCode)
            => new Result<T>(false, default, error, statusCode);
    }


    public class HttpModule
    {
        public async Task<HttpResponse> GetAsync(string url)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {

                request.timeout = 10;

                var operation = request.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

                var response = new HttpResponse
                {
                    StatusCode = (int)request.responseCode,
                    Body = request.downloadHandler.text,
                    Headers = new Dictionary<string, string>()
                };

                foreach (var key in request.GetResponseHeaders().Keys)
                    response.Headers[key] = request.GetResponseHeaders()[key];

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    throw new Exception($"Connection error: {request.error}");
                }

                // ❗ ВАЖНО: ProtocolError НЕ кидаем!
                // (400, 401, 500 — это нормальные ответы сервера)

                return response;
            }
        }
        public async Task<HttpResponse> PostJsonAsync(string url, string json)
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                request.timeout = 10;

                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                var operation = request.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

                var response = new HttpResponse
                {
                    StatusCode = (int)request.responseCode,
                    Body = request.downloadHandler.text,
                    Headers = new Dictionary<string, string>()
                };

                var headers = request.GetResponseHeaders();
                if (headers != null)
                {
                    foreach (var key in headers.Keys)
                        response.Headers[key] = headers[key];
                }

                // ❗ Только реальные проблемы сети — exception
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    throw new Exception($"Connection error: {request.error}");
                }

                // ❗ ВАЖНО: ProtocolError НЕ кидаем!
                // (400, 401, 500 — это нормальные ответы сервера)

                return response;
            }
        }

        public async Task<HttpResponse> DeleteJsonAsync(string url, string json = null)
        {
            using (UnityWebRequest request = new UnityWebRequest(url, "DELETE"))
            {
                request.timeout = 10;
                request.downloadHandler = new DownloadHandlerBuffer();

                if (!string.IsNullOrEmpty(json))
                {
                    byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
                    request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                    request.SetRequestHeader("Content-Type", "application/json");
                }

                var operation = request.SendWebRequest();
                while (!operation.isDone) await Task.Yield();

                var response = new HttpResponse
                {
                    StatusCode = (int)request.responseCode,
                    Body = request.downloadHandler.text,
                    Headers = new Dictionary<string, string>()
                };

                var headers = request.GetResponseHeaders();
                if (headers != null)
                {
                    foreach (var key in headers.Keys)
                        response.Headers[key] = headers[key];
                }

                if (request.result == UnityWebRequest.Result.ConnectionError)
                    throw new Exception($"Connection error: {request.error}");

                return response;
            }
        }

    }
}
