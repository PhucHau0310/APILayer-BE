using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace APILayer.Security
{
    public class IdentityProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // Lấy `NameIdentifier` từ các claim của người dùng
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
