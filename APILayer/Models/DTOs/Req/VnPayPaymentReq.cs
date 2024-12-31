namespace APILayer.Models.DTOs.Req
{
    public class VnPayPaymentReq
    {
        public int UserId { get; set; }
        public int ApiId { get; set; }
        public decimal Amount { get; set; }
        public string? OrderDescription { get; set; }
    }
}
