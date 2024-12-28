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

            // OAuth
            "/signin-google",
            "/google-response",
            "/signin-facebook",
            "/facebook-response",

            // UserController
            "/api/user/forgot-password",
            "/api/user/verify-code",
            "/api/user/reset-password",
            "/api/user/get-user-by-name",
            "/api/user/update-avatar-user",
            "/api/user/update-username",
            "/api/user/change-password",

            // ChatController
            "/api/chat/history",
            "/api/chat/send",
            "/api/chat/get-conversations",

            // FAQ
            "/api/faq/get-faqs",
            "/api/faq/get-faq-by-id",
            "/api/faq/get-faq-by-userId",
            "/api/faq/create-faq",
            "/api/faq/update-faq",
            "/api/faq/delete-faq",

            // API 
            "/api/api/documentation",
            "/api/api/documentation/{id}", // Để người dùng có thể xem docs
            "/api/api/version", // Để người dùng có thể xem versions
            "/api/api/version/{id}",

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

             // API 
            "/api/api",
            "/api/api/{id}",

            // Documentation
            "/api/api/documentation",
            "/api/api/documentation/{id}",

            // Version
            "/api/api/version",
            "/api/api/version/{id}",

            // Chat
            "/api/chat/get-conversations/{username}",

            // Notification
            "/api/notification/send",
            "/api/notification/send-many",
            "/api/notification/{username}",
            "/api/notification/detail/{id}",
            "/api/notification/{id}",
        };

        public static readonly string[] CustomerEndpoints =
        {
            // Chat
            "/api/chat/send",
            "/api/chat/history",

            // Others
            "/api/stripe/create-payment-intent",
            "/api/stripe/webhook",
            "/api/paypal/create-order",

            //Notification
            "/api/notification/{id}/read",
            "/api/notification/unread/{username}",
        };

        public static readonly string[] ProviderEndpoints =
        {
            // Chat
            "/api/chat/send",
            "/api/chat/history",

            "/api/stripe/create-payment-intent",
            "/api/stripe/webhook",
            "/api/paypal/create-order",

            // API Management endpoints
            "/api/api",
            "/api/api/{id}",

            // Documentation
            "/api/api/documentation",
            "/api/api/documentation/{id}",

            // Version  
            "/api/api/version",
            "/api/api/version/{id}",

             //Notification
            "/api/notification/{id}/read",
            "/api/notification/unread/{username}",
        };
    }
}
