namespace RFID_.NET_MVC_PROJECT.Models
{
    public class User
    {
        public int user_id { get; set; }       // user_id from API response
        public string name { get; set; }      // name from API response
        public string email { get; set; }     // email from API response
        public string mobile { get; set; }    // mobile from API response
        public int status { get; set; }
    }
    public class Client1
    {
        public int client_id { get; set; }       // user_id from API response
        public string name { get; set; }      // name from API response
        public string email { get; set; }     // email from API response
        public string mobile { get; set; }    // mobile from API response
        public int status { get; set; }
    }
    public class PurchasedTokens
    {
        public int ct_id { get; set; }
        public int client_id { get; set; }
        public int token_id { get; set; }
        public string pass_key { get; set; }
        public int status { get; set; }
        public DateTime purchase_date { get; set; }
        public DateTime expire_date { get; set; }
        public string subject_name { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
    public class TokensDetails
    {
        public int token_id { get; set; }
        public string name { get; set; }
        public string price{ get; set; }
        public int duration_day { get; set; }
        public string description { get; set; }
        public int status{ get; set; }
    }
    

    public class getTokensDetailsApiResponse
    {
        public bool success { get; set; }
        public List<TokensDetails> data { get; set; }  
    }
    public class getUsersApiResponse
    {
        public bool success { get; set; }
        public List<User> data { get; set; } 
    }
    public class getClientsApiResponse
    {
        public bool success { get; set; }
        public List<Client1> data { get; set; }  // List of users
    }
    public class getPurchasedTokensApiResponse
    {
        public bool success { get; set; }
        public List<PurchasedTokens> data { get; set; }  // List of users
    }
    
    public class ApiResponse
    {
        public bool success { get; set; }
    }

}
