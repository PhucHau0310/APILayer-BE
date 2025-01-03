using APILayer.Data;
using APILayer.Models.DTOs.Req;
using APILayer.Models.DTOs.Res;
using APILayer.Models.Entities;
using APILayer.Services.Implementations;
using APILayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            {
                _logger.LogWarning("User or API not found");
                return NotFound("User or API not found");
            }

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
            //var paymentUrl = _vnPayService.CreatePaymentUrl(request, ipAddress);

            // Sử dụng payment.Id để làm OrderInfo cho VNPay
            var paymentUrl = _vnPayService.CreatePaymentUrl(new VnPayPaymentReq
            {
                UserId = request.UserId,
                ApiId = request.ApiId,
                Amount = request.Amount,
                OrderDescription = payment.Id.ToString() // Truyền payment.Id làm OrderInfo
            }, ipAddress);

            return Ok(new { PaymentUrl = paymentUrl, PaymentId = payment.Id });
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnPayReturn()
        {
            var collections = HttpContext.Request.Query; // Truy cập trực tiếp query string từ HttpContext
            _logger.LogInformation("VNPay return query: {0}", collections.ToString());

            var isValidSignature = _vnPayService.ValidateCallback(collections);
            if (!isValidSignature)
            {
                _logger.LogError("Invalid VNPay signature");
                return BadRequest("Invalid signature");
            }

            var orderInfo = collections["vnp_OrderInfo"].ToString();
            if (!long.TryParse(orderInfo, out long paymentId))
            {
                _logger.LogError($"Invalid OrderInfo format: {orderInfo}");
                return BadRequest("Invalid order info");
            }

            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
            {
                _logger.LogError("Payment not found: {0}", paymentId);
                return NotFound($"Payment {paymentId} not found");
            }

            var vnPayResponseCode = collections["vnp_ResponseCode"].ToString();
            payment.PaymentStatus = vnPayResponseCode == "00" ? "Completed" : "Failed";

            await _context.SaveChangesAsync();
            _logger.LogInformation("Payment {0} status updated to {1}", paymentId, payment.PaymentStatus);

            return Ok(new { Success = vnPayResponseCode == "00" });
        }

        [HttpGet("payments")]
        public async Task<ActionResult<Response<List<PaymentRes>>>> GetAllPayments()
        {
            try
            {
                var payments = await _context.Payments
                    .Include(p => p.User) 
                    .Include(p => p.Api)  
                    .ToListAsync();

                if (payments == null || !payments.Any())
                {
                    return Ok(new Response<List<PaymentRes>>
                    {
                        Success = false,
                        Message = "Không có giao dịch nào.",
                        Data = null
                    });
                }

                var paymentDTOs = payments.Select(p => new PaymentRes
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    ApiId = p.ApiId,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    PaymentStatus = p.PaymentStatus,
                    PaymentDate = p.PaymentDate,
                    UserName = p.User.Username,
                    ApiName = p.Api.Name
                }).ToList();

                return Ok(new Response<List<PaymentRes>>
                {
                    Success = true,
                    Message = "Lấy danh sách giao dịch thành công.",
                    Data = paymentDTOs
                });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi và trả về thông báo lỗi
                return StatusCode(500, new Response<List<PaymentRes>>
                {
                    Success = false,
                    Message = $"Lỗi khi lấy danh sách giao dịch: {ex.Message}",
                    Data = null
                });
            }
        }
    }
}
