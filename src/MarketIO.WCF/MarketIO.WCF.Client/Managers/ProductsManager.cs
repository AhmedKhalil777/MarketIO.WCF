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


    }
}
