namespace APILayer.Models.DTOs.Req
{
    public class NotificationReq
    {
        public string Sender { get; set; } = string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
