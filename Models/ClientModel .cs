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
    public class getTokensForClientApiResponse
    {
        public bool success { get; set; }
        public List<TokensForClient> data { get; set; }
    }
    public class TokensForClient
    {
        public int token_id { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public int duration_day { get; set; }
        public string description { get; set; }
       

    }
    public class getTokenDetailsApiResponse
    {
        public bool success { get; set; }
        public TokenDetailsForAddSubject data { get; set; }
    }
    public class TokenDetailsForAddSubject
    {
        public int token_id { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public int duration_day { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }
    public class getUsersWhichInSubjectApiResponse
    {
        public bool success { get; set; }
        public List<UsersWhichInSubject> data { get; set; }
    }
    public class UsersWhichInSubject {
    
        public string rfid {  get; set; }
        public string name { get; set; }
        public int user_id { get; set; }
        public string designation {  get; set; }
        public string email { get; set; }
        public string mobile {  get; set; }


    }

    public class getCtIdAndSubjectName
    {
        public int ct_id{ get; set; }
        public string subject_name { get; set; }
    }

    public class ClientSubjectViewModel
    {
        public getCtIdAndSubjectName Info { get; set; }
        public List<SubjectDetail> SubjectDetails { get; set; }
    }
    public class UsersWhichInSubjectModel
    {
        public getCtIdAndSubjectName Info { get; set; }

        public List<UsersWhichInSubject> UsersWhichInSubject { get; set; }

    }

}

