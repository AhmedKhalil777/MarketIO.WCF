using MarketIO.WCF.Client.Managers;
using System;
using WCFSettings;

namespace MarketIO.WCF.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string hostname = args.Length >= 1 ? args[0] : "localhost";
            Console.Title = "WCF .Net Core Client";
            WCFHostSettings settings = ProductsManager.BuildClientSettings(hostname);
            static void log(string value) => Console.WriteLine(value);
            ProductsManager.InvokeProductServiceUsingWcf(settings, log);
            Console.ReadLine();
        }
    }
}
