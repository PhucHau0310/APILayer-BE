namespace APILayer.Models.DTOs.Res
{
    public class Response <T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
