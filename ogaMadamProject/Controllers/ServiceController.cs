using AutoMapper;
using Newtonsoft.Json;
using ogaMadamProject.Dtos;
using ogaMadamProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ogaMadamProject.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Authorize]
    [RoutePrefix("api/Service")]
    public class ServiceController : ApiController
    {
        ServiceUtility util = new ServiceUtility();

        [HttpGet]
        public async Task<IHttpActionResult> ListUsers()
        {
            try
            {
                var userList = await util.ListUsers();
                if (userList.Count() == 0)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "No user found")));
                }

                return Ok(SuccessResponse(200, "successful", userList));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }

        [HttpGet]
        public IHttpActionResult ListCategory()
        {
            try
            {
                var CategoryList = util.ListCategory();
                if (CategoryList.Count() == 0)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "No Category found")));
                }

                return Ok(SuccessResponse(200, "successful", CategoryList));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> SendEmailSms(EmailSmsRequest dataRequest)
        {
            try
            {
                var json = JsonConvert.SerializeObject(dataRequest);
                log(json);

                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));

                    var error = new ErorrMessage()
                    {
                        ResponseCode = 403,
                        ResponseStatus = false,
                        Message = message
                    };

                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden, error));
                }

                var SmsResponse = await util.SendEmailSms(dataRequest);
                if (SmsResponse == null)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "Unable to send")));
                }

                return Ok(SuccessResponse(200, "successful", SmsResponse));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult VerifyEmail(string id, string hashParam)
        {
            try
            {

                var verifyResponse = util.VerifyEmail(id, hashParam);
                if (! verifyResponse)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "Unable to send")));
                }

                return Ok(SuccessResponse(200, "successful", verifyResponse));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }


        [HttpPost]
        public IHttpActionResult EmployeeLogin(EmployeeLoginDto requestParam)
        {
            try
            {
                var json = JsonConvert.SerializeObject(requestParam);
                log(json);

                var employeeResponse = util.EmployeeLoginAsync(requestParam);
                if (employeeResponse.Data == null)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "Invalid userame and password")));
                }

                if (employeeResponse.Data.Equals("pending"))
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "Account is pending")));
                }

                return Ok(SuccessResponse(200, "successful", employeeResponse.Data));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }

        [HttpPost]
        public IHttpActionResult PayTransaction(TransactionDto requestParam)
        {
            try
            {
                var json = JsonConvert.SerializeObject(requestParam);
                log(json);

                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));

                    var error = new ErorrMessage()
                    {
                        ResponseCode = 403,
                        ResponseStatus = false,
                        Message = message
                    };

                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden, error));
                }

                var transactionResponse = util.PayTransaction(requestParam);
                if (! transactionResponse)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "Unable to capture record")));
                }

                return Ok(SuccessResponse(200, "successful", transactionResponse));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }

        [HttpPost]
        public IHttpActionResult SearchWorkers(SearchWorkerDto requestParam)
        {
            try
            {
                var json = JsonConvert.SerializeObject(requestParam);
                log(json);

                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));

                    var error = new ErorrMessage()
                    {
                        ResponseCode = 403,
                        ResponseStatus = false,
                        Message = message
                    };

                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden, error));
                }

                var transactionResponse = util.SearchWorkers(requestParam);
                if (transactionResponse.Count() == 0)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "Unable to capture record")));
                }

                return Ok(SuccessResponse(200, "successful", transactionResponse));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }



        [HttpGet]
        public IHttpActionResult ListSixWorkers()
        {
            try
            {

                var transactionResponse = util.ListSixWorkers();
                if (transactionResponse.Count() == 0)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "Unable to capture record")));
                }

                return Ok(SuccessResponse(200, "successful", transactionResponse));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }


        [HttpGet]
        public IHttpActionResult ListVerifyEmployee()
        {
            try
            {

                var verifyEmployees = util.ListVerifyEmployee();
                if (verifyEmployees.Count() == 0)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "No Employee found")));
                }

                return Ok(SuccessResponse(200, "successful", verifyEmployees));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }

        [HttpGet]
        public IHttpActionResult ListEmployee()
        {
            try
            {

                var verifyEmployees = util.ListEmployee();
                if (verifyEmployees.Count() == 0)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "No Employee found")));
                }

                return Ok(SuccessResponse(200, "successful", verifyEmployees));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }

        [HttpGet]
        public IHttpActionResult ListTransaction()
        {
            try
            {

                var listTrans = util.ListTransaction();
                if (listTrans.Count() == 0)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "No transaction found")));
                }

                return Ok(SuccessResponse(200, "successful", listTrans));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }

        [HttpPost]
        public IHttpActionResult TransactionByEmployer(TransByEmployerDTO requestParam)
        {
            try
            {
                var json = JsonConvert.SerializeObject(requestParam);
                log(json);

                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));

                    var error = new ErorrMessage()
                    {
                        ResponseCode = 403,
                        ResponseStatus = false,
                        Message = message
                    };

                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden, error));
                }

                var transactionResponse = util.TransactionByEmployer(requestParam);
                if (transactionResponse.Count() == 0)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "Unable to capture record")));
                }

                return Ok(SuccessResponse(200, "successful", transactionResponse));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }

        [HttpPost]
        public IHttpActionResult EmployeeByEmployer(TransByEmployerDTO requestParam)
        {
            try
            {
                var json = JsonConvert.SerializeObject(requestParam);
                log(json);

                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));

                    var error = new ErorrMessage()
                    {
                        ResponseCode = 403,
                        ResponseStatus = false,
                        Message = message
                    };

                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.Forbidden, error));
                }

                var transactionResponse = util.EmployeeByEmployer(requestParam);
                if (transactionResponse.Count() == 0)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "Unable to capture record")));
                }

                return Ok(SuccessResponse(200, "successful", transactionResponse));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }


        [HttpGet]
        public IHttpActionResult AttachedEmployee(string EmployerId)
        {
            try
            {

                var listEmployee = util.AttachedEmployee(EmployerId);
                if (listEmployee.Count() == 0)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "No transaction found")));
                }

                return Ok(SuccessResponse(200, "successful", listEmployee));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }

        [HttpGet]
        public IHttpActionResult ListEmployer()
        {
            try
            {

                var verifyEmployees = util.ListEmployer();
                if (verifyEmployees.Count() == 0)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "No Employer found")));
                }

                return Ok(SuccessResponse(200, "successful", verifyEmployees));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }

        [HttpGet]
        public IHttpActionResult ListSalary()
        {
            try
            {

                var listSalary = util.ListSalary();
                if (listSalary.Count() == 0)
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, ErrorResponse(404, "No Salary found")));
                }

                return Ok(SuccessResponse(200, "successful", listSalary));
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, ErrorResponse(500, ex.Message.ToString())));
            }
        }

        private ErorrMessage ErrorResponse(int num, string msg)
        {
            var error = new ErorrMessage()
            {
                ResponseCode = num,
                ResponseStatus = false,
                Message = msg
            };

            return error;
        }

        private ResponseModel SuccessResponse(int num, string msg, object obj)
        {
            var response = new ResponseModel()
            {
                ResponseCode = num,
                ResponseStatus = true,
                Message = msg,
                Data = obj
            };

            return response;
        }

        public static void log(string obj)
        {

            string sPathName = HttpContext.Current.Server.MapPath("/log.txt");

            using (StreamWriter w = new StreamWriter(sPathName, true))
            {
                w.WriteLine(Environment.NewLine + "New Log Entry: ");
                w.WriteLine(DateTime.Now.ToString());
                w.WriteLine(obj);
                w.WriteLine("__________________________");
                w.WriteLine(" ");
                w.Flush();
            }
        }
    }
}
