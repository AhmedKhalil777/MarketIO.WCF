using System;
using System.Collections.Generic;
using System.Linq;

namespace WCFSettings
{
    public class WCFHostSettings
    {
        private const string DefaultHostName = "localhost";
        public bool UseHttps { get; set; } = true;
        public Uri basicHttpAddress { get; set; }
        public Uri basicHttpsAddress { get; set; }
        public Uri wsHttpAddress { get; set; }
        public Uri wsHttpAddressValidateUserPassword { get; set; }
        public Uri wsHttpsAddress { get; set; }
        public Uri netTcpAddress { get; set; }

        public int httpPort { get; set; } = 50088;
        public int httpsPort { get; set; } = 50443;
        public int nettcpPort { get; set; } = 50089;

        public IEnumerable<Uri> GetBaseAddresses(string hostname = DefaultHostName)
        {
            return new[] {
                $"net.tcp://{hostname}:{nettcpPort}/",
                $"http://{hostname}:{httpPort}/",
                $"https://{hostname}:{httpsPort}/" }
            .Select(a => new Uri(a)).ToArray();
        }

        private Uri AddPathPrefix(Uri source, string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                return source;
            var builder = new UriBuilder(source);
            builder.Path = prefix + builder.Path;
            return builder.Uri;
        }


        public WCFHostSettings SetDefaults(string hostname = DefaultHostName, string serviceprefix = default)
        {
            string baseHttpAddress = hostname + ":50088";
            string baseHttpsAddress = hostname + ":50443";
            string baseTcpAddress = hostname + ":50089";

            basicHttpAddress = AddPathPrefix(new Uri($"http://{baseHttpAddress}/basichttp"), serviceprefix);
            basicHttpsAddress = AddPathPrefix(new Uri($"https://{baseHttpsAddress}/basichttp"), serviceprefix);
            wsHttpAddress = AddPathPrefix(new Uri($"http://{baseHttpAddress}/wsHttp"), serviceprefix);
            wsHttpAddressValidateUserPassword = AddPathPrefix(new Uri($"https://{baseHttpsAddress}/wsHttpUserPassword"), serviceprefix);
            wsHttpsAddress = AddPathPrefix(new Uri($"https://{baseHttpsAddress}/wsHttp"), serviceprefix);
            netTcpAddress = AddPathPrefix(new Uri($"net.tcp://{baseTcpAddress }/nettcp"), serviceprefix);
            return this;
        }
    }
}
