namespace APILayer.Models.DTOs.Res
{
    public class PaymentRes
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public int ApiId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }

        // Thông tin bổ sung từ quan hệ
        public string UserName { get; set; }
        public string ApiName { get; set; }
    }
}
