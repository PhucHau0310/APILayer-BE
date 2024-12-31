using APILayer.Data;
using APILayer.Models.DTOs.Req;
using APILayer.Models.DTOs.Res;
using APILayer.Models.Entities;
using APILayer.Services.Implementations;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMoMoService _momoService;
        private readonly IVnPayService _vnPayService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FirebaseService> _logger;

        public PaymentController(IMoMoService momoService, IVnPayService vnPayService, ILogger<FirebaseService> logger, ApplicationDbContext context)
        {
            _momoService = momoService;
            _vnPayService = vnPayService;
            _context = context;
            _logger = logger;
        }

        [HttpPost("create-momo-payment")]
        public async Task<IActionResult> CreateMoMoPayment([FromBody] MoMoPaymentReq request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            var api = await _context.APIs.FindAsync(request.ApiId);

            if (user == null || api == null)
                return NotFound("User or API not found");

            var payment = new Payment
            {
                UserId = request.UserId,
                ApiId = request.ApiId,
                Amount = request.Amount,
                PaymentMethod = "MoMo",
                PaymentStatus = "Pending",
                PaymentDate = DateTime.UtcNow,
                User = user,
                Api = api
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            var momoRequest = new MoMoPaymentReq
            {
                OrderId = payment.Id.ToString(),
                OrderInfo = $"Thanh toán API {payment.ApiId}",
                Amount = (long)(payment.Amount * 100), // Convert to VND cents
                ExtraData = ""
            };

            var paymentUrl = await _momoService.CreatePaymentRequestAsync(momoRequest);
            return Ok(new { PaymentUrl = paymentUrl, PaymentId = payment.Id });
        }

        [HttpGet("momo-callback")]
        public async Task<IActionResult> MoMoCallback([FromQuery] MoMoRes response)
        {
            var payment = await _context.Payments.FindAsync(int.Parse(response.OrderId));
            if (payment == null) return NotFound();

            payment.PaymentStatus = response.ResultCode == 0 ? "Completed" : "Failed";
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("create-vnpay-payment")]
        public async Task<IActionResult> CreateVnPayPayment([FromBody] VnPayPaymentReq request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            var api = await _context.APIs.FindAsync(request.ApiId);

            if (user == null || api == null)
                return NotFound("User or API not found");

            var payment = new Payment
            {
                UserId = request.UserId,
                ApiId = request.ApiId,
                Amount = request.Amount,
                PaymentMethod = "VNPay",
                PaymentStatus = "Pending",
                PaymentDate = DateTime.UtcNow,
                User = user,
                Api = api
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
            var paymentUrl = _vnPayService.CreatePaymentUrl(request, ipAddress);

            return Ok(new { PaymentUrl = paymentUrl, PaymentId = payment.Id });
        }

        //[HttpGet("vnpay-return")]
        //public async Task<IActionResult> VnPayReturn([FromQuery] IQueryCollection collections)
        //{
        //    var isValidSignature = _vnPayService.ValidateCallback(collections);
        //    if (!isValidSignature)
        //        return BadRequest("Invalid signature");

        //    var orderId = collections["vnp_TxnRef"].ToString();
        //    var vnPayResponseCode = collections["vnp_ResponseCode"].ToString();

        //    var payment = await _context.Payments.FindAsync(int.Parse(orderId));
        //    if (payment == null) return NotFound();

        //    payment.PaymentStatus = vnPayResponseCode == "00" ? "Completed" : "Failed";
        //    await _context.SaveChangesAsync();

        //    return Ok(new { Success = vnPayResponseCode == "00" });
        //}
        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnPayReturn()
        {
            var collections = HttpContext.Request.Query; // Truy cập trực tiếp query string từ HttpContext
            _logger.LogInformation("Data collections: {collections}", collections);

            var isValidSignature = _vnPayService.ValidateCallback(collections);
            if (!isValidSignature)
                return BadRequest("Invalid signature");

            var orderId = collections["vnp_TxnRef"].ToString();
            var vnPayResponseCode = collections["vnp_ResponseCode"].ToString();

            _logger.LogInformation("OrderId: {orderId}", orderId);

            // Convert orderId to int
            //if (!int.TryParse(orderId, out int paymentId))
            //    return BadRequest("Invalid order ID");

            var payment = await _context.Payments.FindAsync(long.Parse(orderId));
            if (payment == null) return NotFound();

            payment.PaymentStatus = vnPayResponseCode == "00" ? "Completed" : "Failed";
            await _context.SaveChangesAsync();

            return Ok(new { Success = vnPayResponseCode == "00" });
        }
    }
}
