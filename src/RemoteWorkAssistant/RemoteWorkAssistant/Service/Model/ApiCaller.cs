using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RemoteWorkAssistant.Service.Model
{
    abstract class ApiCaller
    {
        private string _baseUri;

        public ApiCaller(string baseUri)
        {
            this._baseUri = baseUri;
        }

        private HttpClient initHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(this._baseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        protected async Task<HttpResponseMessage> Put(string path, Object reqBodyObj)
        {
            HttpClient client = this.initHttpClient();
            StringContent reqBody = new StringContent(JsonSerializer.Serialize(reqBodyObj), Encoding.UTF8, @"application/json");
            return await client.PutAsync(path, reqBody);
        }

        protected async Task<HttpResponseMessage> Post(string path, Object reqBodyObj)
        {
            HttpClient client = this.initHttpClient();
            StringContent reqBody = new StringContent(JsonSerializer.Serialize(reqBodyObj), Encoding.UTF8, @"application/json");
            return await client.PostAsync(path, reqBody);
        }
    }
}
