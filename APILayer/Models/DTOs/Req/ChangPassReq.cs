namespace APILayer.Models.DTOs.Req
{
    public class ChangPassReq
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? NewPassword { get; set; }    
    }
}
