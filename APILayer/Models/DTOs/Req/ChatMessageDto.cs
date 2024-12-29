namespace APILayer.Models.DTOs.Req
{
    public class ChatMessageDto
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Message { get; set; }
    }
}
