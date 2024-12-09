using Google.Apis.Auth;

namespace APILayer.Services
{
    public class GoogleService
    {
        private readonly string _googleClientId;

        public GoogleService(IConfiguration configuration)
        {
            _googleClientId = configuration["Google:ClientId"];
        }

        public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string token)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string> { _googleClientId }
            };
            return await GoogleJsonWebSignature.ValidateAsync(token, settings);
        }
    }
}
