using APILayer.Configurations;
using APILayer.Models.DTOs.Req;
using APILayer.Models.DTOs.Res;
using APILayer.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace APILayer.Services.Implementations
{
    public class MoMoService : IMoMoService
    {
        private readonly MoMoConfig _config;
        private readonly HttpClient _httpClient;

        public MoMoService(IOptions<MoMoConfig> config, HttpClient httpClient)
        {
            _config = config.Value;
            _httpClient = httpClient;
        }

        public async Task<string> CreatePaymentRequestAsync(MoMoPaymentReq request)
        {
            var rawData = $"partnerCode={_config.PartnerCode}" +
                          $"&accessKey={_config.AccessKey}" +
                          $"&requestId={request.OrderId}" +
                          $"&amount={request.Amount}" +
                          $"&orderId={request.OrderId}" +
                          $"&orderInfo={request.OrderInfo}" +
                          $"&returnUrl={_config.ReturnUrl}" +
                          $"&notifyUrl={_config.NotifyUrl}" +
                          $"&extraData={request.ExtraData}";

            var signature = ComputeHmacSha256(rawData, _config.SecretKey);

            var requestBody = new
            {
                partnerCode = _config.PartnerCode,
                accessKey = _config.AccessKey,
                requestId = request.OrderId,
                amount = request.Amount,
                orderId = request.OrderId,
                orderInfo = request.OrderInfo,
                returnUrl = _config.ReturnUrl,
                notifyUrl = _config.NotifyUrl,
                extraData = request.ExtraData,
                signature = signature
            };

            var response = await _httpClient.PostAsJsonAsync(_config.ApiEndpoint, requestBody);
            var result = await response.Content.ReadFromJsonAsync<MoMoRes>();

            return result.PayUrl;
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                var hashBytes = hmac.ComputeHash(messageBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
