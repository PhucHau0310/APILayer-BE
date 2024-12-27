namespace APILayer.Models.DTOs.Req
{
    public class UpdateUserReq
    {
        public int? Id { get; set; }
        public string? Username { get; set;}
        public string? Email { get; set;}
        public IFormFile? Avatar { get; set;}
        public string? Role { get; set;}
        public string? Provider { get; set;}
    }
}
