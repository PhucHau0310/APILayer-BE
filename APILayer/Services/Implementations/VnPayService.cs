using APILayer.Configurations;
using APILayer.Helper;
using APILayer.Models.DTOs.Req;
using APILayer.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace APILayer.Services.Implementations
{
    public class VnPayService : IVnPayService
    {
        private readonly VnPayConfig _config;

        public VnPayService(IOptions<VnPayConfig> config)
        {
            _config = config.Value;
        }

        public string CreatePaymentUrl(VnPayPaymentReq request, string ipAddress)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _config.TmnCode);
            vnpay.AddRequestData("vnp_Amount", ((long)request.Amount * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", ipAddress);
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", request.OrderDescription);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _config.ReturnUrl);
            vnpay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl = vnpay.CreateRequestUrl(_config.PaymentUrl, _config.HashSecret);
            return paymentUrl;
        }

        public bool ValidateCallback(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            var orderId = Convert.ToInt64(collections["vnp_TxnRef"]);
            var vnPayTranId = Convert.ToInt64(collections["vnp_TransactionNo"]);
            var vnpResponseCode = collections["vnp_ResponseCode"].ToString();
            var vnpSecureHash = collections["vnp_SecureHash"].ToString();

            var checkSignature = vnpay.ValidateSignature(vnpSecureHash, _config.HashSecret);

            return checkSignature;
        }
    }
}
