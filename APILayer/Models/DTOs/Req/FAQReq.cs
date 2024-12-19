namespace APILayer.Models.DTOs.Req
{
    public class FAQReq
    {
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public string? Category { get; set; }
        public int UserId { get; set; }
    }
}
