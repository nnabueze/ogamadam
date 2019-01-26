using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using ogaMadamProject.Controllers;
using ogaMadamProject.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

namespace ogaMadamProject.Models
{
    public class ServiceUtility : IDisposable
    {
        bool disposed;
        private ApplicationDbContext _db;
        private OgaMadamAdo _db2;
        private Ado1 _db3;

        private string confirmMeUrl = "https://confirmme.com/CustomerAPI2";
        private static string confirmMeClientId = "471";
        private string confirmMeClientKey = "6a1f757b6f039ac0c46db53f6d29b4c72751be2b2c36a7dcd50fd495f8864e8e";

        private string emailUrl = "https://api.elasticemail.com/v2/email/send";
        private string emailApiKey = "ecd98fde-5d02-45f8-857e-c6cf1c09129f";

        private string smsUrl = "http://www.estoresms.com/smsapi.php";
        private string smsUsername = "CHAMS";
        private string smsPassword = "welcome40@";
      


        public ServiceUtility()
        {
            _db = new ApplicationDbContext();
            _db2 = new OgaMadamAdo();
            _db3 = new Ado1();
        }

        public Task<IEnumerable<AspNetUserDto>> ListUsers()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                return _db2.AspNetUsers.ToList().Select(Mapper.Map<AspNetUser, AspNetUserDto>);
            });
        }

        public Task<IEnumerable<CategoryDto>> ListCategory()
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                return _db2.Categories.ToList().Select(Mapper.Map<Category, CategoryDto>);
            });
        }

        public bool VerifyBVN(string bvn)
        {
            var hashString = GetHashString(bvn);
            var hashToken = GenerateSHA256String(hashString);
            var url = confirmMeUrl + "/BVNText?bvn=" + bvn;
            var verifyResponse = confirmMeWebCall(url, hashToken, "GET");
            ConfirmMeBVNResponse results = JsonConvert.DeserializeObject<ConfirmMeBVNResponse>(verifyResponse);
            if (results.ResponseCode.Equals("00"))
            {
                return true;
            }
            return false;
        }

        public bool VerifyNIMC(string nimc)
        {
            var hashString = GetHashString(nimc);
            var hashToken = GenerateSHA256String(hashString);
            var url = confirmMeUrl + "/verifyNin?regNo=" + nimc;
            var verifyResponse = confirmMeWebCall(url, hashToken, "GET");
            ConfirmMeNINResponse results = JsonConvert.DeserializeObject<ConfirmMeNINResponse>(verifyResponse);
            if (results.ResponseCode.Equals("00"))
            {
                return true;
            }
            return false;
        }

        public static string confirmMeWebCall(string url, string hashToken, string method, bool isParam = false, string param = null)
        {
            string strResponseValue = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.Headers.Add("CLIENTID", confirmMeClientId);
                request.Headers.Add("HASHTOKEN", hashToken);
                if (isParam)
                {
                    request.ContentType = "application/x-www-form-urlencoded";
                    byte[] postData = Encoding.UTF8.GetBytes(param);
                    request.ContentLength = postData.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(postData, 0, postData.Length);
                    dataStream.Close();
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return strResponseValue;
                    }

                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                strResponseValue = reader.ReadToEnd();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return strResponseValue;
            }

            return strResponseValue;
        }

        public Task<bool> RegisterEmployee(RegisterModel model, string id)
        {
            return Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                var employeeDetails = new Employee()
                {
                    EmployeeId = id,
                    BVN = model.ExtraData.BVN,
                    NIMC = model.ExtraData.NIMC,
                    CreatedAt = DateTime.Now
                    
                };

                switch (model.ExtraData.QualificationType)
                {
                    case "Bsc":
                        employeeDetails.QualificationType = QualificationType.Bsc;
                        break;
                    case "Hnd":
                        employeeDetails.QualificationType = QualificationType.Hnd;
                        break;
                    case "Msc":
                        employeeDetails.QualificationType = QualificationType.Msc;
                        break;
                    case "Ond":
                        employeeDetails.QualificationType = QualificationType.Ond;
                        break;
                    default:
                        employeeDetails.QualificationType = QualificationType.Ssce;
                        break;
                }

                _db2.Employees.Add(employeeDetails);
                if (_db2.SaveChanges() == 1)
                {
                    var content = new EmailSmsRequest()
                    {
                        From = "Oga Madam",
                        Message = "Email confirmation",
                        RecieptEmail = model.Email,
                        SenderEmail = model.Email,
                        Subject = "Email confirmation"
                    };

                    SendEmailSms(content);

                    //send phone verification

                    return true;
                }
                return false;
            });
        }

        public bool VerifyEmail(string id, string hashParam)
        {
            var user = _db2.AspNetUsers.FirstOrDefault(o => o.Id == id);
            var hashKey = Crypto.Hash(user.PhoneNumber + user.Email + user.FirstName);
            if (hashKey.Equals(hashParam))
            {
                user.IsEmailVerified = true;
                _db2.SaveChanges();
                return true;
            }
            return false;
        }

        public Task<string> SendEmailSms(EmailSmsRequest dataRequest)
        {
            try
            {
                if (! string.IsNullOrEmpty(dataRequest.RecieptEmail))
                {
                    //send email
                    string url = emailUrl+"?apikey="+ emailApiKey +"&subject="+dataRequest.Subject+"&from="
                        +dataRequest.SenderEmail+"&fromName=Oga Madam&sender="
                        +dataRequest.SenderEmail+"&senderName=Oga Madam Team&replyTo="
                        +dataRequest.SenderEmail+"&to="
                        +dataRequest.RecieptEmail+"&bodyHtml="+dataRequest.Message;

                    var emailResponse = SmsEmailWebCall(url);
                }

                if (!string.IsNullOrEmpty(dataRequest.Phone))
                {
                    //send sms
                    string url = smsUrl + "?username="+ smsUsername + "&password="+ smsPassword 
                        + "&sender=Oga Madam&recipient="
                        +dataRequest.Phone+"&message="+dataRequest.Message+"&dnd=true";

                    var smsResponse = SmsEmailWebCall(url);

                }
            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }

        public IList<EmployeeResponseModel> SearchWorkers(SearchWorkerDto requestParam)
        {
            IList<EmployeeResponseModel> employeeList = new List<EmployeeResponseModel>();
            var category = _db2.Categories.Where(o => o.Title == requestParam.Category).FirstOrDefault();
            if (category == null)
            {
                var workList = _db2.Employees.Where(o => o.IsUserVerified == true).ToList();
                foreach (var item in workList)
                {
                    employeeList.Add(getWorker(item));
                }

            }
            else
            {
                foreach (var item in category.Employee)
                {
                    employeeList.Add(getWorker(item));
                }
            }
            return employeeList;
        }

        private EmployeeResponseModel getWorker(Employee item)
        {
            var worker = new EmployeeResponseModel()
            {
                EmployeeId = item.EmployeeId,
                Address = item.AspNetUser.Address,
                Email = item.AspNetUser.Email,
                FirstName = item.AspNetUser.FirstName,
                LastName = item.AspNetUser.LastName,
                MiddleName = item.AspNetUser.MiddleName,
                Phone = item.AspNetUser.PhoneNumber,
                PlaceOfBirth = item.AspNetUser.PlaceOfBirth,
                StateOfOrigin = item.AspNetUser.StateOfOrigin

            };

            if (item.AspNetUser.Sex == SexType.Female)
            {
                worker.Sex = "Female";
            }
            else
            {
                worker.Sex = "Male";
            }

            return worker;
        }

        public static string SmsEmailWebCall(string url)
        {
            string strResponseValue = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        return strResponseValue;
                    }

                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                strResponseValue = reader.ReadToEnd();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return strResponseValue;
            }

            return strResponseValue;
        }

        public string RandomNumber()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            string rNum = DateTime.Now.Millisecond + rnd.Next(0, 900000000).ToString();

            return rNum;
        }

        public ResponseModel EmployeeLoginAsync(EmployeeLoginDto requestParam)
        {
            ResponseModel res = new ResponseModel();
            var userStore = new UserStore<ApplicationUser>(_db);
            var manager = new UserManager<ApplicationUser>(userStore);

            var result = manager.Find(requestParam.Email, requestParam.Password);


            //check if user login successfully
            if (result != null)
            {
                var employeeData = _db2.Employees.FirstOrDefault(o => o.EmployeeId == result.Id);
                var uploadInfo = _db2.Uploads.Where(o => o.UploadId == result.Id).ToList();

                var roleId = _db3.AspNetUserRoles.FirstOrDefault(o => o.UserId == result.Id);
                var role = _db2.AspNetRoles.FirstOrDefault(o => o.Id == roleId.RoleId);

                IList<UploadDto> uploadDtos = new List<UploadDto>();
                foreach (var item in uploadInfo)
                {
                    var uploadId = new UploadDto()
                    {
                        UploadId = item.UploadId
                    };
                    uploadDtos.Add(uploadId);
                }

                //check if account is activated
                if (result.AccountStatus == StatusType.Pending)
                {
                    res.Data = "pending";
                    return res;
                }

                var user = new EmployeeLoginDto()
                {
                    Address = result.Address,
                    DateOfBirth = result.DateOfBirth,
                    Email = result.Email,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    MiddleName = result.MiddleName,
                    Password = requestParam.Password,
                    PhoneNumber = result.PhoneNumber,
                    PlaceOfBirth = result.PlaceOfBirth,
                    StateOfOrigin = result.StateOfOrigin,

                };
                user.Upload = uploadDtos;
                if (employeeData != null)
                {
                    user.BVN = employeeData.BVN;
                    user.NIMC = employeeData.NIMC;
                }
                if (result.Sex == SexType.Male)
                {
                    user.Sex = "Male";
                }
                else
                {
                    user.Sex = "Female";
                }

                res.Data = user;
            }

            return res;
            
        }

        public bool PayTransaction(TransactionDto requestParam)
        {
            var trans = new Transaction()
            {
                Amount = Convert.ToDecimal(requestParam.Amount),
                EmployeeId = requestParam.EmployeeId,
                EmployerId = requestParam.EmployerId,
                PaymentCategory = requestParam.PaymentCategory,
                TransactionDate = Convert.ToDateTime(requestParam.TransactionDate),
                TransactionId = requestParam.TransactionId,
                CreatedAt = DateTime.Now
            };

            switch (requestParam.PaymentChannel)
            {
                case "Web":
                    trans.PaymentChannel = PaymentChannelType.Web;
                    break;
                case "Pos":
                    trans.PaymentChannel = PaymentChannelType.Pos;
                    break;
                case "Cash":
                    trans.PaymentChannel = PaymentChannelType.Cash;
                    break;
                default:
                    break;
            }

            switch (requestParam.PaymentStatus)
            {
                case "Failed":
                    trans.PaymentStatus = PaymentStatus.Failed;
                    break;
                case "Successful":
                    trans.PaymentStatus = PaymentStatus.Successful;
                    break;
                case "pending":
                    trans.PaymentStatus = PaymentStatus.pending;
                    break;
                default:
                    break;
            }

            var transSave = _db2.Transactions.Add(trans);
            if (_db2.SaveChanges() == 1)
            {
                var salaryParam = new Salary()
                {
                    EmployeeId = requestParam.EmployeeId,
                    EmployerId = requestParam.EmployerId,
                    StartDate = Convert.ToDateTime(requestParam.StartDate),
                    EndDate = Convert.ToDateTime(requestParam.EndDate),
                    TotalAmount = Convert.ToDecimal(requestParam.Amount),
                    SalaryId = requestParam.TransactionId,
                    CreatedAt = DateTime.Now
                };
                _db2.Salaries.Add(salaryParam);
                _db2.SaveChanges();

                return true;
            }
            return false;

        }

        public IList<EmployeeDto> AttachedEmployee(string employerId)
        {
            IList<EmployeeDto> employDtoList = new List<EmployeeDto>();
            var employeeList = _db2.Employees.Where(o => o.EmployerId == employerId).ToList();
            if (employDtoList == null)
            {
                return null;
            }
            foreach (var item in employeeList)
            {
                var employ = new EmployeeDto()
                {
                    AccountName = item.AccountName,
                    AccountNumber = item.AccountNumber,
                    Address = item.AspNetUser.Address,
                    AttachedDate = item.AttachedDate,
                    BankName = item.BankName,
                    BVN = item.BVN,
                    DateOfBirth = item.AspNetUser.DateOfBirth.ToString(),
                    Email = item.AspNetUser.Email,
                    FirstName = item.AspNetUser.FirstName,
                    IsAttachedApproved = item.IsAttachedApproved,
                    LastName = item.AspNetUser.LastName,
                    MiddleName = item.AspNetUser.MiddleName,
                    PhoneNumber = item.AspNetUser.PhoneNumber,
                    Id = item.EmployeeId,
                    NIMC = item.NIMC,
                    PlaceOfBirth = item.AspNetUser.PlaceOfBirth,
                    StateOfOrigin = item.AspNetUser.StateOfOrigin,
                    SalaryAmount = item.SalaryAmount,
                    IsInterviewed = item.IsInterviewed,
                    IsTrained = item.IsTrained,
                    IsUserVerified = item.IsUserVerified
                     
                };
                switch (item.QualificationType)
                {
                    case QualificationType.Ssce:
                        employ.QualificationType = "Ssce";
                        break;
                    case QualificationType.Ond:
                        employ.QualificationType = "Ond";
                        break;
                    case QualificationType.Hnd:
                        employ.QualificationType = "Hnd";
                        break;
                    case QualificationType.Bsc:
                        employ.QualificationType = "Bsc";
                        break;
                    case QualificationType.Msc:
                        employ.QualificationType = "Msc";
                        break;
                    default:
                        break;
                }

                employDtoList.Add(employ);
            }

            return employDtoList;
        }

        public IList<SalaryDto> ListSalary()
        {
            IList<SalaryDto> salaryDtoList = new List<SalaryDto>();
            var salaries = _db2.Salaries.ToList();
            if (salaries == null)
            {
                return null;
            }
            foreach (var item in salaries)
            {
                var salary = new SalaryDto()
                {
                    Amount = item.TotalAmount.ToString(),
                    Employee = item.Employee.AspNetUser.FirstName,
                    Employer = item.Employer.AspNetUser.FirstName,
                    EndDate = item.EndDate.ToString(),
                    SalaryId = item.SalaryId,
                    StartDate = item.StartDate.ToString()
                };

                salaryDtoList.Add(salary);
            }

            return salaryDtoList;
        }

        public IEnumerable<TransactionDto> ListTransaction()
        {
            IList<TransactionDto> transDtoList = new List<TransactionDto>();
            var trans = _db2.Transactions.ToList();
            foreach (var item in trans)
            {
                var transDto = new TransactionDto()
                {
                  Amount = item.Amount.ToString(),
                  EmployeeId = getUserName(item.Employee.EmployeeId),
                  EmployerId = getUserName(item.Employer.EmployerId),
                  EndDate = item.Salary.EndDate.ToString(),
                  PaymentCategory = item.PaymentCategory,
                  StartDate = item.Salary.StartDate.ToString(),
                  TransactionDate = item.TransactionDate.ToString(),
                  TransactionId = item.TransactionId
                };

                switch (item.PaymentChannel)
                {
                    case PaymentChannelType.Web:
                        transDto.PaymentChannel = "Web";
                        break;
                    case PaymentChannelType.Pos:
                        transDto.PaymentChannel = "Pos";
                        break;
                    case PaymentChannelType.Cash:
                        transDto.PaymentChannel = "Cash";
                        break;
                    default:
                        break;
                }

                switch (item.PaymentStatus)
                {
                    case PaymentStatus.Failed:
                        transDto.PaymentStatus = "Failed";
                        break;
                    case PaymentStatus.Successful:
                        transDto.PaymentStatus = "Successful";
                        break;
                    case PaymentStatus.pending:
                        transDto.PaymentStatus = "pending";
                        break;
                    default:
                        break;
                }

                transDtoList.Add(transDto);
            }
            return transDtoList;
        }

        private string getUserName(string id)
        {
            var user = _db2.AspNetUsers.FirstOrDefault(o=>o.Id == id);
            return user.FirstName + " " + user.LastName;
        }

        public IList<EmployeeDto> ListEmployee()
        {
            var users = _db2.AspNetUsers.ToList();
            IList<EmployeeDto> userList = new List<EmployeeDto>();
            foreach (var user in users)
            {
                if (user.Employee != null)
                {
                    var list = new EmployeeDto()
                    {
                        Id = user.Id,
                        Address = user.Address,
                        DateOfBirth = user.DateOfBirth.ToString(),
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        MiddleName = user.MiddleName,
                        PhoneNumber = user.PhoneNumber,
                        PlaceOfBirth = user.PlaceOfBirth,
                        StateOfOrigin = user.StateOfOrigin,
                        AccountName = user.Employee.AccountName,
                        AccountNumber = user.Employee.AccountNumber,
                        AttachedDate = user.Employee.AttachedDate,
                        BankName = user.Employee.BankName,
                        BVN = user.Employee.BVN,
                        IsAttachedApproved = user.Employee.IsAttachedApproved,
                        IsInterviewed = user.Employee.IsInterviewed,
                        IsTrained = user.Employee.IsTrained,
                        IsUserVerified = user.Employee.IsUserVerified,
                        NIMC= user.Employee.NIMC,
                        SalaryAmount = user.Employee.SalaryAmount 
                    };

                    switch (user.Employee.QualificationType)
                    {
                        case QualificationType.Ssce:
                            list.QualificationType = "Ssce";
                            break;
                        case QualificationType.Ond:
                            list.QualificationType = "Ond";
                            break;
                        case QualificationType.Hnd:
                            list.QualificationType = "Hnd";
                            break;
                        case QualificationType.Bsc:
                            list.QualificationType = "Bsc";
                            break;
                        case QualificationType.Msc:
                            list.QualificationType = "Msc";
                            break;
                        default:
                            break;
                    }

                    if (user.Sex == SexType.Male)
                    {
                        list.Sex = "Male";
                    }
                    else
                    {
                        list.Sex = "Female";
                    }
                    userList.Add(list);
                }
            }
            return userList;

        }

        public IList<EmployeeDto> ListVerifyEmployee()
        {
            var users = _db2.AspNetUsers.ToList();
            IList<EmployeeDto> userList = new List<EmployeeDto>();
            foreach (var user in users)
            {
                if (user.Employee != null && user.Employee.IsUserVerified == true)
                {
                    var list = new EmployeeDto()
                    {
                        Id = user.Id,
                        Address = user.Address,
                        DateOfBirth = user.DateOfBirth.ToString(),
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        MiddleName = user.MiddleName,
                        PhoneNumber = user.PhoneNumber,
                        PlaceOfBirth = user.PlaceOfBirth,
                        StateOfOrigin = user.StateOfOrigin,

                        AccountName = user.Employee.AccountName,
                        AccountNumber = user.Employee.AccountNumber,
                        AttachedDate = user.Employee.AttachedDate,
                        BankName = user.Employee.BankName,
                        BVN = user.Employee.BVN,
                        IsAttachedApproved = user.Employee.IsAttachedApproved,
                        IsInterviewed = user.Employee.IsInterviewed,
                        IsTrained = user.Employee.IsTrained,
                        IsUserVerified = user.Employee.IsUserVerified,
                        NIMC = user.Employee.NIMC,
                        SalaryAmount = user.Employee.SalaryAmount
                    };

                    switch (user.Employee.QualificationType)
                    {
                        case QualificationType.Ssce:
                            list.QualificationType = "Ssce";
                            break;
                        case QualificationType.Ond:
                            list.QualificationType = "Ond";
                            break;
                        case QualificationType.Hnd:
                            list.QualificationType = "Hnd";
                            break;
                        case QualificationType.Bsc:
                            list.QualificationType = "Bsc";
                            break;
                        case QualificationType.Msc:
                            list.QualificationType = "Msc";
                            break;
                        default:
                            break;
                    }

                    if (user.Sex == SexType.Male)
                    {
                        list.Sex = "Male";
                    }
                    else
                    {
                        list.Sex = "Female";
                    }
                    userList.Add(list);
                }
            }
            return userList;

        }

        public IList<EmployerDto> ListEmployer()
        {
            var users = _db2.AspNetUsers.ToList();
            IList<EmployerDto> userList = new List<EmployerDto>();
            foreach (var user in users)
            {
                if (user.Employer != null)
                {
                    var list = new EmployerDto()
                    {
                        Id = user.Id,
                        Address = user.Address,
                        DateOfBirth = user.DateOfBirth.ToString(),
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        MiddleName = user.MiddleName,
                        PhoneNumber = user.PhoneNumber,
                        PlaceOfBirth = user.PlaceOfBirth,
                        StateOfOrigin = user.StateOfOrigin,

                        EmploymentIdNumber = user.Employer.EmploymentIdNumber,
                        NextOfKin = user.Employer.NextOfKin,
                        NextOfKinAddress = user.Employer.NextOfKinAddress,
                        NextOfKinPhoneNumber = user.Employer.NextOfKinPhoneNumber,
                        PlaceOfWork = user.Employer.PlaceOfWork,
                        Profession = user.Employer.Profession
                    };

                    if (user.Sex == SexType.Male)
                    {
                        list.Sex = "Male";
                    }
                    else
                    {
                        list.Sex = "Female";
                    }
                    userList.Add(list);
                }
            }
            return userList;

        }

        public static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private string GetHashString(string hashString)
        {
            var hashTokenString = confirmMeClientId + confirmMeClientKey + hashString;

            return hashTokenString;
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    _db.Dispose();
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}