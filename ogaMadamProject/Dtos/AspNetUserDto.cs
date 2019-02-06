using ogaMadamProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ogaMadamProject.Dtos
{
    public class AspNetUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

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
    }

    public class EmployeeDto
    {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string MiddleName { get; set; }
        public string PlaceOfBirth { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }
        public string StateOfOrigin { get; set; }

        public string BVN { get; set; }
        public string NIMC { get; set; }
        public bool IsAttachedApproved { get; set; }
        public DateTime? AttachedDate { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public decimal SalaryAmount { get; set; }
        public bool IsUserVerified { get; set; }
        public bool IsTrained { get; set; }
        public bool IsInterviewed { get; set; }
        public string QualificationType { get; set; }

    }

    public class EmployerDto
    {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string MiddleName { get; set; }
        public string PlaceOfBirth { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }
        public string StateOfOrigin { get; set; }
        public string PlaceOfWork { get; set; }
        public string EmploymentIdNumber { get; set; }
        public string NextOfKin { get; set; }
        public string NextOfKinPhoneNumber { get; set; }
        public string NextOfKinAddress { get; set; }
        public string Profession { get; set; }

    }

    public class CategoryDto
    {
        public string CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int numberOfWorkers { get; set; }
    }

    public class EmployeeRegDto
    {
        public string EmployeeId { get; set; }
        public string EmployerId { get; set; }
        public string CategoryId { get; set; }
        public string BVN { get; set; }
        public string NIMC { get; set; }
        public bool IsAttachedApproved { get; set; }
        public DateTime? AttachedDate { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public decimal SalaryAmount { get; set; }
        public bool IsUserVerified { get; set; }
        public bool IsTrained { get; set; }
        public bool IsInterviewed { get; set; }
        public string QualificationType { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class EmployeeLoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string PlaceOfBirth { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Address { get; set; }

        public string Sex { get; set; }

        public string StateOfOrigin { get; set; }

        public string BVN { get; set; }

        public string NIMC { get; set; }

        public string PlaceOfWork { get; set; }

        public string NextOfKin { get; set; }

        public string NextOfKinPhoneNumber { get; set; }

        public string NextOfKinAddress { get; set; }

        public string Profession { get; set; }

        public string Role { get; set; }

        public string Id { get; set; }

        public IList<UploadDto> Upload { get; set; }
    }

    public class EmployerLoginDto
    {
        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string PlaceOfBirth { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string Address { get; set; }

        public string Sex { get; set; }

        public string StateOfOrigin { get; set; }

        public IList<UploadDto> Upload { get; set; }
    }

    public class UploadDto
    {
        public string UploadId { get; set; }
    }

    public class TransactionDto
    {
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public string TransactionDate { get; set; }
        [Required]
        public string Amount { get; set; }
        public string PaymentCategory { get; set; }
        [Required]
        public string PaymentStatus { get; set; }
        public string PaymentChannel { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        [Required]
        public string EmployerId { get; set; }
        [Required]
        public string EndDate { get; set; }
        [Required]
        public string StartDate { get; set; }
    }

    public class SearchWorkerDto
    {
        [Required]
        public string Category { get; set; }
        public string Location { get; set; }
    }

    public class TransByEmployerDTO
    {
        [Required]
        public string EmployerId { get; set; }
    }

    public class SalaryDto
    {
        public string SalaryId { get; set; }
        public string Employee { get; set; }
        public string Employer { get; set; }
        public string Amount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class ConfirmMeBVNResponse
    {
        public string ResponseCode { get; set; }
        public string Parameter { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public string ResponseNIMC { get; set; }
        public ResponseBVNDetails ResponseBVNDetails { get; set; }
        public string ResponseBVNImage { get; set; }
    }

    public class ResponseBVNDetails
    {
        public string BVN { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string RegistrationDate { get; set; }
        public string DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
    }

    public class ConfirmMeNINResponse
    {
        public string ResponseCode { get; set; }
        public string Parameter { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public string ResponseNIMC { get; set; }
        public string ResponseBVNDetails { get; set; }
        public string ResponseBVNImage { get; set; }
    }
}