namespace APILayer.Security
{
    public static class Endpoints
    {
        public static readonly string[] PublicEndpoints =
        {
            "/api/auth/login",
            "/api/auth/register",
            "/api/auth/confirm-email",
            "/api/img",
            "/api/auth/refresh-token",
            "/api/auth/signin-google",
            "/api/auth/google-response",
            "/graphql",
            "/api/chat/history",
            "/api/chat/send",
            "/chathub/negotiate",
            "/chathub",
            "/notificationhub",
            "/notificationhub/negotiate",
            "/api/chat/get-conversations",
            "/api/paypal",

        };

        public static readonly string[] AdminEndpoints =
        {
            "/api/user"
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
