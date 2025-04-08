using System.ComponentModel.DataAnnotations;

namespace RFID_.NET_MVC_PROJECT.Models
{
    public class UserSubjectDetailForUser
    {
        public string designation { get; set; }
        public string rfid { get; set; }

    }
    public class getUserSubjectApiResponse
    {
        public bool success { get; set; }
        public List<UserSubject> data { get; set; }
    }
    public class UserSubject
    {
        public int ct_id { get; set; }
        public string subject_name { get; set; }

    }
    public class getSubjectDetailForUserApiResponse
    {
        public bool success { get; set; }
        public List<UserSubjectDetailForUser> data { get; set; }
    }
    public class UserSubjectViewModel
    {
        public getCtIdAndSubjectName Info { get; set; }
        public List<UserSubjectDetailForUser> UserSubjectDetailForUser { get; set; }
    }
    public class AttendanceRecordforUser
    {
        public int punch_id { get; set; }
        public int ct_id { get; set; }
        public string timestamp { get; set; }
        public int user_id { get; set; }
    
    }

    public class AttendanceRecordforUserapiresponse
    {
        public bool success { get; set; }
        public List<AttendanceRecordforUser> data { get; set; }
    }
}