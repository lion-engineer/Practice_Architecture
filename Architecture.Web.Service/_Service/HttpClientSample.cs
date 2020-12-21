using System;
using System.Net.Http;
using System.Threading.Tasks;
using Architecture.Common.Model;
using Newtonsoft.Json;

namespace Architecture.Web.Service
{


    public class HttpClientSample
    {
        
        public HttpClientSample()
        {
            this._httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:44362/") };
        }

        private HttpClient _httpClient;

        public async Task<Adapter> testhttpclient()
        {
            var response = await this._httpClient.GetAsync("Demo/TestDemo").ConfigureAwait(false);
            var responsestream = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Adapter>(responsestream);
            return result;
        }
    }
}
