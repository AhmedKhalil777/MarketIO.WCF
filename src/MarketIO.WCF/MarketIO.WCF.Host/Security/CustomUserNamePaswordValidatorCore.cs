using CoreWCF;
using System.Threading.Tasks;

namespace MarketIO.WCF.Host.Security
{
    public class CustomUserNamePasswordValidatorCore : CoreWCF.IdentityModel.Selectors.UserNamePasswordValidator
    {
        public override async ValueTask ValidateAsync(string userName, string password)
        {
            await Task.Run(() =>
            {
                bool valid = userName.ToLowerInvariant().EndsWith("valid")
                            && password.ToLowerInvariant().EndsWith("valid");
                if (!valid)
                {
                    throw new FaultException("Unknown Username or Incorrect Password");
                }
            });

        }

        public static void AddToHost(ServiceHostBase host)
        {
            var srvCredentials = new CoreWCF.Description.ServiceCredentials();
            srvCredentials.UserNameAuthentication.UserNamePasswordValidationMode
                = CoreWCF.Security.UserNamePasswordValidationMode.Custom;
            srvCredentials.UserNameAuthentication.CustomUserNamePasswordValidator
                = new CustomUserNamePasswordValidatorCore();
            host.Description.Behaviors.Add(srvCredentials);
        }
    }
}
