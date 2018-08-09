using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace horus.fw.FwUtil
{
    public class HttpRequest
    {
        public static string JsonRequest { get; set; }

        public static string JsonResponse { get; set; }

        public static HttpResponseMessage HttpResponse { get; set; }

        static HttpRequest()
        {
            JsonRequest = string.Empty;
            JsonResponse = string.Empty;
            HttpResponse = new HttpResponseMessage();
        }

        public static string Get(string apiUrl)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponse = httpClient.GetAsync(new Uri(apiUrl)).Result;
            HttpResponse.EnsureSuccessStatusCode();
            JsonResponse = HttpResponse.Content.ReadAsStringAsync().Result;

            return JsonResponse;
        }

        public static string Post(string apiUrl, string json)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent httpContent = new StringContent(json);

            HttpResponse = httpClient.PostAsync(new Uri(apiUrl), httpContent).Result;
            HttpResponse.EnsureSuccessStatusCode();
            JsonResponse = HttpResponse.Content.ReadAsStringAsync().Result;

            return JsonResponse;
        }

        public static string Put(string apiUrl, string json)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpContent httpContent = new StringContent(json);

            HttpResponse = httpClient.PutAsync(new Uri(apiUrl), httpContent).Result;
            HttpResponse.EnsureSuccessStatusCode();
            JsonResponse = HttpResponse.Content.ReadAsStringAsync().Result;

            return JsonResponse;
        }

        public static string Delete(string apiUrl)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponse = httpClient.DeleteAsync(new Uri(apiUrl)).Result;
            HttpResponse.EnsureSuccessStatusCode();
            JsonResponse = HttpResponse.Content.ReadAsStringAsync().Result;

            return JsonResponse;
        }

        public static string Combine(string uri1, string uri2)
        {
            uri1 = uri1.TrimEnd('/');
            uri2 = uri2.TrimStart('/');

            return string.Format("{0}/{1}", uri1, uri2);
        }
    }
}
