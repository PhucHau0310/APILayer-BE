namespace APILayer.Models.DTOs.Req
{
    public class RegisterReq
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; } = "Customer";
    }
}
