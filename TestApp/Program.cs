using System;
using System.IO;
using System.Net;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var address = "http://localhost:86/Azure/8ca249c4-1128-4adb-82b7-31c72d015e09/v2.0/.well-known/openid-configuration";

            //WebClient client = new WebClient();            
            //UriBuilder builder = new UriBuilder(address);
            //builder.Host = "login.microsoftonline.com";            
            //var result = client.DownloadString(builder.Uri);

            WebRequest request = HttpWebRequest.Create(address);
            request.Headers.Add("Host:login.microsoft.com");
            var response = (HttpWebResponse)request.GetResponse();

            Console.WriteLine(response.StatusCode);

            var stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            var content = reader.ReadToEndAsync().Result;
            
            Console.WriteLine(content);
            Console.ReadLine();
        }
    }
}
