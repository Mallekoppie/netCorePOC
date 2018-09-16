using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RnD.Http
{
    public class HttpClientHandlerForAzure : HttpClientHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            WebRequest newRequest = HttpWebRequest.Create(String.Concat("http://localhost:86/Azure", request.RequestUri.LocalPath));
            newRequest.Headers.Add("Host:login.microsoft.com");
            newRequest.Method = request.Method.Method;

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            using (var responseApi = (HttpWebResponse)newRequest.GetResponse())
            {
                using (var reader = new StreamReader(responseApi.GetResponseStream()))
                {
                    var objText = reader.ReadToEnd();
                    response.Content = new StringContent(objText, Encoding.UTF8, "application/json");
                }
            }
            var tcs = new TaskCompletionSource<HttpResponseMessage>();
            tcs.SetResult(response);
            return tcs.Task;
        }

    }
}
