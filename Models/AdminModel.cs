namespace RFID_.NET_MVC_PROJECT.Models
{
    public class AdminModel
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

    // Define ApiResponse to capture full response
    public class getUsersApiResponse
    {
        public bool success { get; set; }
        public List<AdminModel> data { get; set; }  // List of users
    }
    public class getClientsApiResponse
    {
        public bool success { get; set; }
        public List<Client1> data { get; set; }  // List of users
    }
    public class ApiResponse
    {
        public bool success { get; set; }
    }

}
