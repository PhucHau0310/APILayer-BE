using APILayer.Models.DTOs.Req;

namespace APILayer.Services.Interfaces
{
    public interface IMoMoService
    {
        Task<string> CreatePaymentRequestAsync(MoMoPaymentReq request);
    }
}
