namespace ApplicationServices.Auth0.Models
{
    public class TokenRequest
    {
        public string grant_type { get; }
        public string client_id { get; }
        public string client_secret { get; }
        public string audience { get; }
        
        public TokenRequest(string grantType, string clientId, string clientSecret, string audience)
        {
            grant_type = grantType;
            client_id = clientId;
            client_secret = clientSecret;
            this.audience = audience;
        }
    }
}