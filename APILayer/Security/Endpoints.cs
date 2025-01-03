using APILayer.Models.Entities;

namespace APILayer.Security
{
    public static class Endpoints
    {
        public static readonly string[] PublicEndpoints =
        {
           // Auth Controller
            "/api/auth/login",
            "/api/auth/register",
            "/api/auth/confirm-email",
            "/api/auth/refresh-token",

            // OAuth Controller
            "/signin-google",
            "/google-response",
            "/signin-facebook",
            "/facebook-response",

           // User Controller - Public Operations
            "/api/user/forgot-password",
            "/api/user/verify-code",
            "/api/user/reset-password",

            // FAQ Controller - Public Operations
            "/api/faq/get-faqs",
            "/api/faq/get-faq-by-id",
            "/api/faq/get-faq-by-userId",

            // API Controller - Public Operations
            "/api/api",
            "/api/api/documentation",
            "/api/api/version",
            "/api/api/featured",
            "/api/api/featured/current",
            "/api/api/featured/{id}",
            "/api/api/featured/user/{userId}",

            // Chat Controller - All endpoints require authentication
            "/api/chat/history",
            "/api/chat/send",
            "/api/chat/get-conversations/{username}",

            // Notification Controller - All endpoints require authentication
            "/api/notification/send",
            "/api/notification/send-many",
            "/api/notification/{id}/read",
            "/api/notification/unread/{username}",
            "/api/notification/{username}",
            "/api/notification/detail/{id}",
            "/api/notification/{id}",

            // SignalR Endpoints
            "/chathub",
            "/chathub/negotiate",
            "/notificationhub",
            "/notificationhub/negotiate",

            // Additional Public Endpoints
            "/api/img",
            "/graphql",
            "/api/paypal",

            "/api/user/get-users",
            "/api/user/get-user-by-name",
            "/api/user/update-avatar-user",
            "/api/user/update-username",
            "/api/user/change-password",

             "/api/faq/get-faqs",
            "/api/faq/get-faq-by-id",
            "/api/faq/get-faq-by-userId",
            "/api/faq/create-faq",
            "/api/faq/update-faq",
            "/api/faq/delete-faq",

            "/api/subscription/user",
            "/api/subscription/user/{id}",

             "/api/review",
             "/api/review/{id}",
             "/api/review/user/{userId}",
             "/api/review/api/{apiId}",

             "/api/payment/create-momo-payment",
             "/api/payment/momo-callback",
             "/api/payment/create-vnpay-payment",
             "/api/payment/vnpay-return",
             "/api/payment/payments",
        };

        public static readonly string[] AdminEndpoints =
        {
           // User Controller - Admin Operations
            "/api/user/get-users",
            "/api/user/get-user-by-id",
            "/api/user/get-user-by-name",
            "/api/user/delete-user",

            // FAQ Controller - Admin Operations
            "/api/faq/create-faq",
            "/api/faq/update-faq",
            "/api/faq/delete-faq",

            // API Controller - Admin Operations
            "/api/api/{id}",
            "/api/api/documentation/{id}",
            "/api/api/version/{id}",

            // Subscription Controller - Admin Operations
            "/api/subscription/newsletter",
            "/api/subscription/newsletter/{id}",
            "/api/subscription/user",
            "/api/subscription/user/{id}"
        };

        public static readonly string[] CustomerEndpoints =
        {
            // User Controller - Customer Operations
            "/api/user/get-user-by-name",
            "/api/user/update-avatar-user",
            "/api/user/update-username",
            "/api/user/change-password",

            // FAQ Controller - Customer Operations
            "/api/faq/create-faq",
            "/api/faq/update-faq",

            // API Controller - Customer Operations
            "/api/api/{id}",
            "/api/api/documentation/{id}",
            "/api/api/version/{id}",

            // Payment Related Endpoints
            "/api/stripe/create-payment-intent",
            "/api/stripe/webhook",
            "/api/paypal/create-order"
        };

        public static readonly string[] ProviderEndpoints =
        {
            // User Controller - Customer Operations
            "/api/user/get-user-by-name",
            "/api/user/update-avatar-user",
            "/api/user/update-username",
            "/api/user/change-password",

            // FAQ Controller - Customer Operations
            "/api/faq/create-faq",
            "/api/faq/update-faq",

            // API Controller - Customer Operations
            "/api/api/{id}",
            "/api/api/documentation/{id}",
            "/api/api/version/{id}",

            // Payment Related Endpoints
            "/api/stripe/create-payment-intent",
            "/api/stripe/webhook",
            "/api/paypal/create-order"
        };
    }
}
