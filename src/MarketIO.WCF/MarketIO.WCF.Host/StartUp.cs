using CoreWCF;
using CoreWCF.Configuration;
using MarketIO.WCF.Contracts;
using MarketIO.WCF.Host.Security;
using MarketIO.WCF.Host.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using WCFSettings;

namespace MarketIO.WCF.Host
{
    public class Startup
    {
        /// <summary>
        /// the debug continue just for 30 minutes
        /// </summary>
        private static readonly TimeSpan _debugTimeout = TimeSpan.FromMinutes(30);
        private static CoreWCF.Channels.Binding ApplyDebugTimeouts(CoreWCF.Channels.Binding binding)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                binding.OpenTimeout =
                    binding.CloseTimeout =
                    binding.SendTimeout =
                    binding.ReceiveTimeout = _debugTimeout;
            }
            return binding;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceModelServices();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseServiceModel(builder => { 
                WSHttpBinding GetTransportWithMessageCredentialBinding()
                {
                    var serverBindingHttpsUserPassword = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);
                    serverBindingHttpsUserPassword.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
                    return serverBindingHttpsUserPassword;
                }
                builder.ConfigureServiceHostBase<ProductsService>(CustomUserNamePasswordValidatorCore.AddToHost);

                void ConfigureSoapService<TService, TContract>(string serviceprefix) where TService : class
                {
                    WCFHostSettings settings = new WCFHostSettings().SetDefaults("localhost", serviceprefix);
                    builder.AddService<TService>()
                        .AddServiceEndpoint<TService, TContract>(
                            GetTransportWithMessageCredentialBinding(), settings.wsHttpAddressValidateUserPassword.LocalPath)
                        .AddServiceEndpoint<TService, TContract>(new BasicHttpBinding(),
                            settings.basicHttpAddress.LocalPath)
                        .AddServiceEndpoint<TService, TContract>(new WSHttpBinding(SecurityMode.None),
                            settings.wsHttpAddress.LocalPath)
                        .AddServiceEndpoint<TService, TContract>(new WSHttpBinding(SecurityMode.Transport),
                            settings.wsHttpsAddress.LocalPath)
                        .AddServiceEndpoint<TService, TContract>(new NetTcpBinding(),
                            settings.netTcpAddress.LocalPath);
                }

                ConfigureSoapService<ProductsService, IProductsService>(nameof(ProductsService));
            });
        }

    }
}
