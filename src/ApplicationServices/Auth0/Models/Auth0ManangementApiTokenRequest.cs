namespace ApplicationServices.Auth0.Models
{
    public class Auth0ManangementApiTokenRequest
    {
        public string grant_type { get; private set; } = "client_credentials";
        public string client_id { get; private set; }
        public string client_secret { get; private set; }
        public string audience { get; private set; }

        public Auth0ManangementApiTokenRequest(string clientId, string clientSecret, string audience)
        {
            client_id = clientId;
            client_secret = clientSecret;
            this.audience = audience;
        }
    }
}