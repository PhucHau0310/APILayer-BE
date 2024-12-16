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

            // ChatController
            "/api/chat/history",
            "/api/chat/send",
            "/api/chat/get-conversations",

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
