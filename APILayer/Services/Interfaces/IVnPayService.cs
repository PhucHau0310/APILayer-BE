using APILayer.Models.DTOs.Req;

namespace APILayer.Services.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(VnPayPaymentReq request, string ipAddress);
        bool ValidateCallback(IQueryCollection collections);
    }
}
