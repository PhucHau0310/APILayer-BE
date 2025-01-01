namespace APILayer.Models.DTOs.Req
{
    public class ReviewReq
    {
        public int UserId { get; set; }
        public int ApiId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
