using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using NavigationApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nito.AsyncEx;


namespace WebApiNET
{
    public class WebApi
    {
        public static string Host { get; set; } = File.Exists("host.txt") ? File.ReadAllText("host.txt") : "http://127.0.0.1:8000";

        private static readonly HttpClient HttpClient;

        static WebApi()
        {
            HttpClient = new HttpClient();
            HttpClient.Timeout = TimeSpan.FromSeconds(10);

            if (File.Exists("Host.txt"))
            {
                Host = File.ReadAllText("Host.txt");
            }
        }


        public static async Task<ObservableCollection<Dictionary<string, string>>> GetBusinessObjectsData(string route, List<string> fields)
        {
            var url = $"{Host}/{route}";
            var resultCollection = new ObservableCollection<Dictionary<string, string>>();
            var result = await HttpClient.GetStringAsync(url);
            var json = JsonConvert.DeserializeObject<ObservableCollection<Dictionary<string, string>>>(result);
            foreach (var obj in json)
            {
                foreach (var keyValue in obj.Where(keyValue => !fields.Contains(keyValue.Key)))
                {
                    obj.Remove(keyValue.Key);
                }
                resultCollection.Add(obj);
            }

            return resultCollection;
        }

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
                Debug.WriteLine($"{Host}/{route}/{(string.IsNullOrEmpty(addedParams) ? string.Empty : addedParams)}");
                var routePath = RemovePunctuations(route);
                var addedParamsPath = "";
                if (!string.IsNullOrEmpty(addedParams))
                    addedParamsPath = RemovePunctuations(addedParams);
                if (!Directory.Exists("CacheData/" + routePath))
                    Directory.CreateDirectory("CacheData/" + routePath);
                path = "CacheData/" + routePath + "/" + "data" + (string.IsNullOrEmpty(addedParams) ? string.Empty : "_" + addedParamsPath) + ".txt";

                result = await HttpClient.GetStringAsync($"{Host}/{route}{addedParams}");
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


        public static async Task<bool> LoadFile<T>(string filePath, string requestField = "file", List<Tuple<string, string>>? stringContents = null, string addedParams = "") where T : new()
        {
            var route = GetRouteStr(new T());
            var url = $"{Host}/{route}/{addedParams}";

            using var multipartFormContent = new MultipartFormDataContent();
            var fileStreamContent = new StreamContent(File.OpenRead(filePath));
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            multipartFormContent.Add(fileStreamContent, name: requestField, fileName: Path.GetFileName(filePath));
            if (stringContents != null)
                foreach (var content in stringContents)
                {
                    multipartFormContent.Add(new StringContent(content.Item2), content.Item1);
                }

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var response = await httpClient.PostAsync(url, multipartFormContent);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public static async Task<(HttpResponseMessage?, TResult?)> SendData<TResult>(object sendObject)
        {
            var response = await SendData(sendObject);
            var result = await TryGetResult<TResult>(response);
            return (response, result);
        }

        public static async Task<HttpResponseMessage?> SendData(object sendObject)
        {
           
            try
            {
                var route = GetRouteStr(sendObject);
                var url = $"{Host}/{route}/";
                var jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                var content = JsonConvert.SerializeObject(sendObject, jsonSettings);
                var data = new StringContent(content, Encoding.UTF8, "application/json");
                using var client = new HttpClient();
                var response = await client.PostAsync(url, data);
                return response;
            }
            catch (Exception e)
            {
                return default;
            }
        }

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

        public static async Task<bool> ReplaceData<T>(Dictionary<string, dynamic> sendObject, string addedParams = "") where T : new()
        {
            var content = new StringContent(JsonConvert.SerializeObject(sendObject), Encoding.UTF8, "application/merge-patch+json");
            try
            {
                var route = GetRouteStr(new T());
                var url = $"{Host}/{route}/{addedParams}";
                using var client = new HttpClient();
                var response = await client.PatchAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<bool> UpdateData<T>(object updateObject,string id) where T : new()
        {
            try
            {
                var route = GetRouteStr(new T());
                var url = $"{Host}/{route}/{id}/";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    Content = new StringContent(JsonConvert.SerializeObject(updateObject),null, "application/json"),
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

        public static async Task<bool> DeleteData<T>(string id) where T : new()
        {
            try
            {
                var route = GetRouteStr(new T());
                var url = $"{Host}/{route}/{id}/";
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
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

        //private static TaskController _lasTaskController;
        //private static readonly AsyncLock Mutex = new AsyncLock();
        public static async Task<bool> Ping()
        {
            try
            {
                await HttpClient.GetStringAsync(Host + "/api");
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
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
