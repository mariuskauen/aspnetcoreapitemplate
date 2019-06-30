using Microsoft.IdentityModel.Tokens;

namespace soapApi.ViewModels
{
    public class LoginResponseViewModel
    {
        public string StatusCode {get;set;}

        public string Token { get; set; }
    }
}