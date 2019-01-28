using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ogaMadamProject.Models
{
    public class ResponseModel
    {
        public int ResponseCode { get; set; }
        public bool ResponseStatus { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public class UserReponseModel
    {
        public int ResponseCode { get; set; }
        public bool ResponseStatus { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
    }

    public class ErorrMessage
    {
        public int ResponseCode { get; set; }
        public bool ResponseStatus { get; set; }
        public string Message { get; set; }
    }

    public class UserData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string MiddleName { get; set; }
        public string PlaceOfBirth { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }
        public string StateOfOrigin { get; set; }
        public string UserType { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsUserVerified { get; set; }
        public string CreatedAt { get; set; }
    }

    public class EmployeeResponseModel
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }
        public string StateOfOrigin { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CategoryName { get; set; }
        public string Objective { get; set; }
    }
}