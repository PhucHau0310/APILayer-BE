using System.ComponentModel.DataAnnotations;

namespace APILayer.Models.DTOs.Req
{
    public class APIDocsReq
    {
        public int ApiId { get; set; }
        public string? DocumentUrl { get; set; }
        public string? LogoUrl { get; set; }
        public string? CodeExamples { get; set; }
        public string? Status { get; set; }
    }
}
