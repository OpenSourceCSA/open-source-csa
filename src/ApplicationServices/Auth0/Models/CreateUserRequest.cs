namespace ApplicationServices.Auth0.Models
{
    public class CreateUserRequest
    {
        public string email { get; set; } // = "";
        //public string phone_number { get; set; } = "";
        // public UserMetaData user_metadata { get; set; }
        // public bool blocked { get; set; } = false;
        // public bool email_verified { get; set; } = false;
        // public bool phone_verified { get; set; } = false;
        public AppMetaData app_metadata { get; set; }
        // public string given_name { get; set; } = "";
        // public string family_name { get; set; } = "";
        // public string name { get; set; } = "";
        // public string nickname { get; set; } = "";
        // public string picture { get; set; } = "";
        // public string user_id { get; set; } = "";
        public string connection { get; set; }  // = "Username-Password-Authentication";
        public string password { get; set; } // = "";
        // public bool verify_email { get; set; } = true;
        //public string username { get; set; } = "";
        public CreateUserRequest(string email, string password, string connection, AppMetaData appMetadata)
        {
            this.email = email;
            this.password = password;
            this.connection = connection;
            this.app_metadata = appMetadata;
        }
    }

    public class AppMetaData
    {
        public string consumerId { get; set; } = "";
        public string userId { get; set; } = "";

        public AppMetaData(string userId, string consumerId)
        {
            this.userId = userId;
            this.consumerId = consumerId;
        }
    }
}