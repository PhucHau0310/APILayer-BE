namespace APILayer.Models.DTOs.Req
{
    public class MoMoPaymentReq
    {
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public long Amount { get; set; }
        public string ExtraData { get; set; }
        public int UserId { get; set; }
        public int ApiId { get; set; }
    }
}
