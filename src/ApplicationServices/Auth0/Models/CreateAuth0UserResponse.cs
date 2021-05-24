namespace ApplicationServices.Auth0.Models
{
    public class Identities
    {
        public string connection { get; set; }
        public string user_id { get; set; }
        public string provider { get; set; }
        public bool isSocial { get; set; } 
    }

    public class AppData
    {
        public string userId { get; set; }
    }
    
    public class CreateAuth0UserResponse
    {
        public string created_at { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; } 
        //public Identities identities { get; set; }
        public string name { get; set; }
        public string nickname { get; set; }
        public string picture { get; set; }
        public string updated_at { get; set; }
        public string user_id { get; set; } // Auth0 UserId like:auth0|2abc2222a2fa2c222f22d22b
        public AppData app_metadata { get; set; }
        
        // {"created_at":"2020-12-31T03:41:13.761Z",
        //     "email":"marctalcottzabcabcf@aol.com",
        //     "email_verified":false,
        //     "identities":[{"connection":"Username-Password-Authentication",
        //         "user_id":"5fed4859a4fa6c006f14d57b",
        //         "provider":"auth0",
        //         "isSocial":false}],
        //     "name":"marctalcottzabcabcf@aol.com",
        //     "nickname":"marctalcottzabcabcf",
        //     "picture":"https://s.gravatar.com/avatar/50a33ae5fe5e8f5584cc700e9f8f7f0f?s=480&r=pg&d=https%3A%2F%2Fcdn.auth0.com%2Favatars%2Fma.png",
        //     "updated_at":"2020-12-31T03:41:13.761Z",
        //     "user_id":"auth0|5fed4859a4fa6c006f14d57b",
        //     "app_metadata":
        //     {
        //         "tenantId":"00000000-0000-0000-0000-000000000000",
        //         "userId":"2a9ee377-8c16-4602-940c-37947121f3f9"}
        //     
        // }
    }
}