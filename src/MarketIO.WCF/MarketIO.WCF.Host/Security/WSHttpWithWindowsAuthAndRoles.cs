using CoreWCF.Security;
using CoreWCF;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using CoreWCF.Configuration;
using Microsoft.AspNetCore.Hosting;
using MarketIO.WCF.Host.Services;
using MarketIO.WCF.Contracts;

namespace MarketIO.WCF.Host.Security
{
    public class WSHttpWithWindowsAuthAndRoles
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceModelServices();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            WSHttpBinding wSHttpBinding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);
            wSHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            app.UseServiceModel(builder =>
            {
                builder.AddService<ProductsService>();
                builder.AddServiceEndpoint<ProductsService, IProductsService>(wSHttpBinding, "/wsHttp");
                builder.AddServiceEndpoint<ProductsService, IProductsService>(new NetTcpBinding(), "/nettcp");
                Action<ServiceHostBase> serviceHost = host => ChangeHostBehavior(host);
                builder.ConfigureServiceHostBase<ProductsService>(serviceHost);
            });
        }

        public void ChangeHostBehavior(ServiceHostBase host)
        {
            var srvCredentials = new CoreWCF.Description.ServiceCredentials();
            LdapSettings _ldapSettings = new LdapSettings("yourownserver.mscore.local", "mscore.local", "yourowntoporg");
            srvCredentials.WindowsAuthentication.LdapSetting = _ldapSettings;
            host.Description.Behaviors.Add(srvCredentials);
        }
    }
}
