using APILayer.Models.Entities;

namespace APILayer.Security
{
    public static class Endpoints
    {
        public static readonly string[] PublicEndpoints =
        {
            // Auth
            "/api/auth/login",
            "/api/auth/register",
            "/api/auth/confirm-email",
            "/api/auth/refresh-token",

            // OAuth
            "/signin-google",
            "/google-response",
            "/signin-facebook",
            "/facebook-response",

            // User
            "/api/user/forgot-password",
            "/api/user/verify-code",
            "/api/user/reset-password",

             // FAQ
            "/api/faq/get-faqs",
            "/api/faq/get-faq-by-id",
            "/api/faq/get-faq-by-userId",

            // API 
            "/api/api",
            "/api/api/documentation",
            "/api/api/version",
            "/api/api/featured",
            "/api/api/featured/current",
            "/api/api/featured/{id}",
            "/api/api/featured/user/{userId}",

            // Chat
            "/api/chat/history",
            "/api/chat/send",
            "/api/chat/get-conversations/{usename}",

            // Notification
            "/api/notification/send",
            "/api/notification/send-many",
            "/api/notification/{id}/read",
            "/api/notification/unread/{username}",
            "/api/notification/{username}",
            "/api/notification/detail/{id}",
            "/api/notification/{id}",

            // SignalR
            "/chathub",
            "/chathub/negotiate",
            "/notificationhub",
            "/notificationhub/negotiate",

            // Others
            "/api/img",
            "/graphql",
            "/api/paypal",
        };

        public static readonly string[] AdminEndpoints =
        {
            // UserController
            "/api/user/get-users",
            "/api/user/get-user-by-id",
            "/api/user/get-user-by-name",
            "/api/user/delete-user",
            "/api/user/update-avatar-user",
            "/api/user/update-username",
            "/api/user/change-password",

            // FAQ
            "/api/faq/create-faq",
            "/api/faq/update-faq",
            "/api/faq/delete-faq",

            // API 
            "/api/api/{id}",

            // API Docs
            "/api/api/documentation/{id}",

            // API Version
            "/api/api/version/{id}",
        };

        public static readonly string[] CustomerEndpoints =
        {
            // User
            "/api/user/get-user-by-name",
            "/api/user/update-avatar-user",
            "/api/user/update-username",
            "/api/user/change-password",

            // FAQ
            "/api/faq/create-faq",
            "/api/faq/update-faq",

            // API 
            "/api/api/{id}",

            // API Docs
            "/api/api/documentation/{id}",

            // API Version
            "/api/api/version/{id}",

            // Others
            "/api/stripe/create-payment-intent",
            "/api/stripe/webhook",
            "/api/paypal/create-order",
        };

        public static readonly string[] ProviderEndpoints =
        {
            // User
            "/api/user/get-user-by-name",
            "/api/user/update-avatar-user",
            "/api/user/update-username",
            "/api/user/change-password",

            // FAQ
            "/api/faq/create-faq",
            "/api/faq/update-faq",

            // API 
            "/api/api/{id}",

             // API Docs
            "/api/api/documentation/{id}",

            // API Version
            "/api/api/version/{id}",
        };
    }
}
