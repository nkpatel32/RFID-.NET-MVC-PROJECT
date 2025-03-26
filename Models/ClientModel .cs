namespace RFID_.NET_MVC_PROJECT.Models
{
    public class getSUbjectApiResponse
    {
        public bool success { get; set; }
        public List<Subject> data { get; set; }
    }
    public class Subject
    {
        public int ct_id { get; set; }       // user_id from API response
        public string subject_name { get; set; }      // name from API response
      
    }
    public class getSubjectDetailApiResponse
    {
        public bool success { get; set; }
        public List<SubjectDetail> data { get; set; }
    }
    public class SubjectDetail
    {
        public string pass_key { get; set; }       
        public string purchase_date { get; set; }
        public string expire_date { get; set; }
        public int token_id { get; set; }
        public string price { get; set; }
        public int duration_day { get; set; }
        public string description { get; set; }
        public string name { get; set; }

    }
}
