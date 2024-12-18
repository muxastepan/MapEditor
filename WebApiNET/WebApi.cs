﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using NavigationApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Nito.AsyncEx;


namespace WebApiNET
{
    /// <summary>
    /// Класс для взаимодействия с сервером.
    /// </summary>
    public class WebApi
    {
        public static string Host { get; set; } = File.Exists("host.txt") ? File.ReadAllText("host.txt") : "http://127.0.0.1:8000";

        private static readonly HttpClient HttpClient;
        public static bool UseApiSuffix;

        static WebApi()
        {
            HttpClient = new HttpClient();
            HttpClient.Timeout = TimeSpan.FromSeconds(10);

            if (File.Exists("Host.txt"))
            {
                Host = File.ReadAllText("Host.txt");
            }
        }


        /// <summary>
        /// Получает ответ с сервера в виде динамического JSON-объекта.
        /// </summary>
        public static async Task<JArray?> GetDynamicDataArray(string route)
        {
            try
            {
                var url = $"{Host}/{(UseApiSuffix ? "api" : string.Empty)}/{route}";
                var result = await HttpClient.GetStringAsync(url);
                var json = JArray.Parse(result);
                return json;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// GET-запрос.
        /// </summary>
        /// <param name="addedParams">Параметры запроса</param>
        /// <param name="mediaType">Content-Type заголовок</param>
        public static async Task<T?> GetData<T>(string addedParams = null, string mediaType = "application/json") where T : new()
        {
            if (!Directory.Exists("CacheData"))
                Directory.CreateDirectory("CacheData");
            string result;
            var path = "";

            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Add("Accept", mediaType);

            try
            {
                var route = GetRouteStr(new T());
                Debug.WriteLine($"{Host}/{(UseApiSuffix?"api":string.Empty)}/{route}/{(string.IsNullOrEmpty(addedParams) ? string.Empty : addedParams)}");
                var routePath = RemovePunctuations(route);
                var addedParamsPath = "";
                if (!string.IsNullOrEmpty(addedParams))
                    addedParamsPath = RemovePunctuations(addedParams);
                if (!Directory.Exists("CacheData/" + routePath))
                    Directory.CreateDirectory("CacheData/" + routePath);
                path = "CacheData/" + routePath + "/" + "data" + (string.IsNullOrEmpty(addedParams) ? string.Empty : "_" + addedParamsPath) + ".txt";

                result = await HttpClient.GetStringAsync($"{Host}/{(UseApiSuffix?"api":string.Empty)}/{route}{addedParams}");
                File.WriteAllText(path, result);
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception e)
            {
                if ((e.Message == "Произошла ошибка при отправке запроса.") || (e.Message == "Отменена задача."))
                {
                    try
                    {
                        if (!File.Exists(path)) throw;
                        result = File.ReadAllText(path);
                        return JsonConvert.DeserializeObject<T>(result);

                        //http://62.220.53.194/api/
                    }
                    catch (Exception)
                    {
                        //
                    }
                }
                return new T();
            }
        }


        /// <summary>
        /// POST-запрос.
        /// </summary>
        /// <param name="sendObject">Объект - тело запроса</param>
        public static async Task<(HttpResponseMessage?, TResult?)> SendData<TResult>(object sendObject)
        {
            var response = await SendData(sendObject);
            var result = await TryGetResult<TResult>(response);
            return (response, result);
        }

        /// <summary>
        /// POST-запрос.
        /// </summary>
        /// <param name="sendObject">Объект - тело запроса</param>
        public static async Task<HttpResponseMessage?> SendData(object sendObject)
        {
           
            try
            {
                var route = GetRouteStr(sendObject);
                var url = $"{Host}/{(UseApiSuffix?"api":string.Empty)}/{route}";
                var jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                var objectWithoutId = JObject.FromObject(sendObject);
                objectWithoutId.Remove("id");
                var content = JsonConvert.SerializeObject(objectWithoutId, jsonSettings);

                var data = new StringContent(content:content, encoding:null, mediaType:"application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(url, data);
                return response;
            }
            catch (Exception e)
            {
                return default;
            }
        }

        /// <summary>
        /// Пытается десериализовать ответ от сервера.
        /// </summary>
        public static async Task<T?> TryGetResult<T>(HttpResponseMessage? response)
        {
            try
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception e)
            {
                return default;
            }
        }

        /// <summary>
        /// PATCH-запрос.
        /// </summary>
        public static async Task<bool> UpdateData<T>(object updateObject, string id, string route) where T : new()
        {
            try
            {
                var url = $"{Host}/{(UseApiSuffix ? "api" : string.Empty)}/{route}/{id}";

                var objectWithoutId = JObject.FromObject(updateObject);
                objectWithoutId.Remove("id");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    Content = new StringContent(JsonConvert.SerializeObject(objectWithoutId), null, "application/merge-patch+json"),
                    RequestUri = new Uri(url)
                };
                using var client = new HttpClient();
                var response = await client.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// PATCH-запрос.
        /// </summary>
        public static async Task<bool> UpdateData<T>(object updateObject,string id) where T : new()
        {
            try
            {
                var route = GetRouteStr(new T());
                var url = $"{Host}/{(UseApiSuffix ? "api" : string.Empty)}/{route}/{id}";

                var objectWithoutId = JObject.FromObject(updateObject);
                objectWithoutId.Remove("id");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Patch,
                    Content = new StringContent(JsonConvert.SerializeObject(objectWithoutId),null, "application/merge-patch+json"),
                    RequestUri = new Uri(url)
                };
                using var client = new HttpClient();
                var response = await client.SendAsync(request);
                return response.StatusCode == HttpStatusCode.NotFound || response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// DELETE-запрос.
        /// </summary>
        public static async Task<bool> DeleteData<T>(string id) where T : new()
        {
            try
            {
                var route = GetRouteStr(new T());
                var url = $"{Host}/{(UseApiSuffix ? "api" : string.Empty)}/{route}/{id}";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(url)
                };
                using var client = new HttpClient();
                var response = await client.SendAsync(request);
                return response.StatusCode == HttpStatusCode.NotFound || response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }

        }


        private static string RemovePunctuations(string input)
        {
            return input == null ? "" : Regex.Replace(input, "[/\\:*? «=<>|~]", string.Empty);
        }

        private static string GetRouteStr(object type)
        {
            var folder = string.Empty;
            {
                folder = type switch
                {

                    ObservableCollection<Floor> _ => "floors",
                    List<Node> _=> "nodes",
                    Node _ => "nodes",
                    ObservableCollection<Area> _=>"areas",
                    Area _ => "areas",
                    ObservableCollection<NaviPoint> _=> "navigate",
                    _ => folder
                };
            }
            return folder;
        }

        private static readonly AsyncLock _mutex = new AsyncLock();
        /// <summary>
        /// Загружает картинку.
        /// </summary>
        public static async Task<string> DownloadIFile(string uri,string localPath = "AllImages",UriKind uriKind=UriKind.Relative)
        {
            try
            {
                using (await _mutex.LockAsync())
                {
                    var filename = Path.GetFileName(uri);
                    if (string.IsNullOrEmpty(filename))
                        return null;

                    var imageFile = Path.Combine(localPath, filename);


                    if (!Directory.Exists(localPath))
                        Directory.CreateDirectory(localPath);

                    if (!File.Exists(imageFile))
                    {
                        var url = uriKind==UriKind.Relative? $"{Host}{uri}": uri;
                        try
                        {
                            await DownloadIFileFromApi(url, Path.GetFullPath(imageFile));
                        }
                        catch (Exception)
                        {
                            //
                        }
                    }

                    return Path.GetFullPath(imageFile);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        private static async Task DownloadIFileFromApi(string url, string pathSaveImage)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                using (var fs = new FileStream(
                           pathSaveImage,
                           FileMode.CreateNew))
                {
                    await response.Content.CopyToAsync(fs);
                }
            }
        }
    }
}
