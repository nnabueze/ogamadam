using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ogaMadamProject.Models
{
    public class EntityModelTable
    {
    }

    [Table("igr_collections.aspnetuserroles")]
    public partial class aspnetuserrole
    {
        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }
        [Key]
        [Column(Order = 1)]
        public string RoleId { get; set; }
    }

    public class Employee
    {
        [Key]
        [ForeignKey("AspNetUser")]
        public string EmployeeId { get; set; }
        public string EmployerId { get; set; }
        public string CategoryId { get; set; }
        public string Objective { get; set; }
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
        public QualificationType QualificationType { get; set; }
        public DateTime CreatedAt { get; set; }

        
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Interview Interview { get; set; }
        [ForeignKey(nameof(EmployerId))]
        public virtual Employer Employer { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
        public ICollection<Review> Review { get; set; }
        public ICollection<Training> Training { get; set; }
        public ICollection<Report> Report { get; set; }
        public ICollection<Verification> Verification { get; set; }
        public ICollection<Transaction> Transaction { get; set; }
        public ICollection<Salary> Salary { get; set; }
        public ICollection<Guarantor> Guarantor { get; set; }
    }

    public class Employer
    {
        [Key]
        [ForeignKey("AspNetUser")]
        public string EmployerId { get; set; }
        public string PlaceOfWork { get; set; }
        public string EmploymentIdNumber { get; set; }
        public string NextOfKin { get; set; }
        public string NextOfKinPhoneNumber { get; set; }
        public string NextOfKinAddress { get; set; }
        public string Profession { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
        public ICollection<Employee> Employee { get; set; }
        public ICollection<Transaction> Transaction { get; set; }
        public ICollection<Review> Review { get; set; }
        public ICollection<Salary> Salary { get; set; }
    }

    public class Transaction
    {
        [Key]
        public string TransactionId { get; set; }
        public string EmployerId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentCategory { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentChannelType PaymentChannel { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(EmployerId))]
        public virtual Employer Employer { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        public virtual Salary Salary { get; set; }
    }

    public class Review
    {
        [Key]
        public string ReviewId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployerId { get; set; }
        public string Description { get; set; }
        public int? Star { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        [ForeignKey(nameof(EmployerId))]
        public virtual Employer Employer { get; set; }
    }

    public class Training
    {
        [Key]
        public string TrainingId { get; set; }
        public string EmployeeId { get; set; }
        public string TrainingType { get; set; }
        public string Description { get; set; }
        public DateTime? TrainingStartDate { get; set; }
        public DateTime? TrainingEndDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
    }

    public class Report
    {
        [Key]
        public string ReportId { get; set; }
        public ReportType ReportType { get; set; }
        public string Description { get; set; }
        public string EmployeeId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
    }

    public class Verification
    {
        [Key]
        public string VerificationId { get; set; }
        public string EmployeeId { get; set; }
        public VerificationType VerificationType { get; set; }
        public bool IsVerify { get; set; }
        public DateTime? VerifyDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
    }

    public class Notification
    {
        [Key]
        public string NotificationId { get; set; }
        public string AspNetUserId { get; set; }
        public string Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? NotificationDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(AspNetUserId))]
        public virtual AspNetUser AspNetUser { get; set; }
    }

    public class Upload
    {
        [Key]
        public string UploadId { get; set; }
        public string AspNetUserId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(AspNetUserId))]
        public virtual AspNetUser AspNetUser { get; set; }
    }

    public class Salary
    {
        [Key]
        [ForeignKey("Transaction")]
        public string SalaryId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployerId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        [ForeignKey(nameof(EmployerId))]
        public virtual Employer Employer { get; set; }

        public virtual Transaction Transaction { get; set; }

    }

    public class Category
    {
        [Key]
        [ForeignKey("Employee")]
        public string CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Employee> Employee { get; set; }
    }

    public class Interview
    {
        [Key]
        [ForeignKey("Employee")]
        public string InterviewId { get; set; }
        public int Score { get; set; }
        public bool IsPassed { get; set; }
        public string Description { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Employee Employee { get; set; }
    }

    public class Guarantor
    {
        [Key]
        public string GuarantorId { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PlaceOfWork { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
    }

    public class Ticket
    {
        [Key]
        public string TicketId { get; set; }
        public string AspNetUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketStatus status { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(AspNetUserId))]
        public virtual AspNetUser AspNetUser { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }
    }

    public class Comment
    {
        [Key]
        public string CommentId { get; set; }
        public string TicketId { get; set; }
        public string AspNetUserId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(AspNetUserId))]
        public virtual AspNetUser AspNetUser { get; set; }

        [ForeignKey(nameof(TicketId))]
        public virtual Ticket Ticket { get; set; }
    }

    public class BillerSetting
    {
        [Key]
        public string BillerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedAt { get; set; }


        public virtual ICollection<ApiSetting> ApiSetting { get; set; }
    }

    public class ApiSetting
    {
        [Key]
        public string ApiSettingId { get; set; }
        public string BillerSettingId { get; set; }
        public string Uri { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ApiMode Mode { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey(nameof(BillerSettingId))]
        public virtual BillerSetting BillerSetting { get; set; }

    }
}