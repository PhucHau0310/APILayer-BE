namespace APILayer.Models.DTOs.Req
{
    public class NotificationBulkReq
    {
        public string Sender { get; set; } = string.Empty;
        public List<string> Recipients { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }
}
