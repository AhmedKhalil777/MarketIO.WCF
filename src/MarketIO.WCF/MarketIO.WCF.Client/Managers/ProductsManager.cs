using MarketIO.WCF.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WCFSettings;
using WCFStandardClient;

namespace MarketIO.WCF.Client.Managers
{
    public class ProductsManager
    {
        public static WCFHostSettings BuildClientSettings(string hostname)
        {
            const string s_hostname = "localhost";

            string title = Console.Title;
            if (string.IsNullOrWhiteSpace(hostname)) hostname = s_hostname;
            Console.WriteLine(title + " - " + hostname);
            WCFHostSettings settings = new WCFHostSettings().SetDefaults(hostname, "ProductsService");
            return settings;
        }

        public static void InvokeProductServiceUsingWcf(WCFHostSettings settings, Action<string> action)
        {
            var getProducts = (Func<IProductsService, string>)
                (channel => JsonSerializer.Serialize(channel.GetProducts()));
            action($"BasicHttp:\n\tGetProducts() => "
                 + getProducts.WcfInvoke(new BasicHttpBinding(BasicHttpSecurityMode.None), settings.basicHttpAddress));

        }

        /// <summary>
        /// Creates a basic web request to the specified endpoint,
        /// sends the SOAP request and reads the response
        /// </summary>
        public static string InvokeProductServiceUsingWebRequest(Uri address)
        {
            string _soapEnvelopeContent =
            @"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>
                <soapenv:Body>
                    <AddProduct xmlns='http://tempuri.org/'>
                        <id>1</id>
                        <name>Product Name</name>
                        <price>123</price>
                    </AddProduct>
                </soapenv:Body>
            </soapenv:Envelope>";

            // Prepare the raw content
            var utf8Encoder = new UTF8Encoding();
            byte[] bodyContentBytes = utf8Encoder.GetBytes(_soapEnvelopeContent);

            // Create the web request
            var webRequest = System.Net.WebRequest.Create(address);
            webRequest.Headers.Add("SOAPAction", "http://tempuri.org/IProductsService/AddProduct");
            webRequest.ContentType = "text/xml";
            webRequest.Method = "POST";
            webRequest.ContentLength = bodyContentBytes.Length;

            // Append the content
            System.IO.Stream requestContentStream = webRequest.GetRequestStream();
            requestContentStream.Write(bodyContentBytes, 0, bodyContentBytes.Length);

            // Send the request and read the response
            using (System.IO.Stream responseStream = webRequest.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader responsereader = new System.IO.StreamReader(responseStream))
                {
                    string soapResponse = responsereader.ReadToEnd();
                    return soapResponse;
                }
            }
        }


    }
}
