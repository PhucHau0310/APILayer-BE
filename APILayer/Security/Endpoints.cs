namespace APILayer.Security
{
    public static class Endpoints
    {
        public static readonly string[] PublicEndpoints =
        {
            // AuthController
            "/api/auth/login",
            "/api/auth/register",
            "/api/auth/confirm-email",
            "/api/auth/refresh-token",
            "/api/auth/signin-google",
            "/api/auth/google-response",

            // UserController
            "/api/user/forgot-password",
            "/api/user/verify-code",
            "/api/user/reset-password",
            "/api/user/get-user-by-name",

            // ChatController
            "/api/chat/history",
            "/api/chat/send",
            "/api/chat/get-conversations",

            // FAQ
            "/api/faq/get-faq-by-id",
            "/api/faq/get-faq-by-userId",
            "/api/faq/create-faq",
            "/api/faq/update-faq",
            "/api/faq/delete-faq",

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
            "/api/user/delete-user",

            // ChatController
            "/api/chat/sendAll",

            // FAQ
            "/api/faq/get-faqs",
        };

        public static readonly string[] CustomerEndpoints =
        {
            "/api/chat/send",
            "/api/chat/history",
            "/api/stripe/create-payment-intent",
            "/api/stripe/webhook",
            "/api/paypal/create-order",
        };

        public static readonly string[] ProviderEndpoints =
        {
            
            "/api/chat/send",
            "/api/chat/history",
            "/api/stripe/create-payment-intent",
            "/api/stripe/webhook",
            "/api/paypal/create-order",
        };
    }
}
