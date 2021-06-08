using System;
using SCMModels;
using SCMModels.MPRMasterModels;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BALayer.PurchaseAuthorization;
using System.Data;
using System.Web;
using System.IO;
using System.Data.OleDb;
using System.Globalization;
using System.Net.Http;
using System.Data.SqlClient;
using System.Linq;
using System.Configuration;
using System.Net;
using SCMModels.RemoteModel;
using System.Net.Http.Headers;
using System.Reflection;
using System.Collections;
using DALayer.Common;

namespace SCMAPI.Controllers
{
    [RoutePrefix("Api/PA")]
    public class PAController : ApiController
    {
        private readonly IPurchaseAuthorizationBA _paBusenessAcess;
        private ErrorLog log = new ErrorLog();

        public PAController(IPurchaseAuthorizationBA purchase)
        {
            this._paBusenessAcess = purchase;
        }
        [Route("InsertPAAuthorizationLimits")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> InsertPAAuthorizationLimits(PAAuthorizationLimitModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.InsertPAAuthorizationLimits(model);
            return Ok(status);
        }
        [Route("GetPAAuthorizationLimitById")]
        [ResponseType(typeof(PAAuthorizationLimitModel))]
        public async Task<IHttpActionResult> GetPAAuthorizationLimitById(int deptid)
        {
            PAAuthorizationLimitModel status = new PAAuthorizationLimitModel();
            status = await _paBusenessAcess.GetPAAuthorizationLimitById(deptid);
            return Ok(status);
        }
        [Route("CreatePAAuthirizationEmployeeMapping")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> CreatePAAuthirizationEmployeeMapping(PAAuthorizationEmployeeMappingModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.CreatePAAuthirizationEmployeeMapping(model);
            return Ok(status);
        }
        [Route("GetMappingEmployee")]
        [ResponseType(typeof(PAAuthorizationEmployeeMappingModel))]
        public async Task<IHttpActionResult> GetMappingEmployee(PAAuthorizationLimitModel model)
        {
            PAAuthorizationEmployeeMappingModel status = new PAAuthorizationEmployeeMappingModel();
            status = await _paBusenessAcess.GetMappingEmployee(model);
            return Ok(status);
        }
        [Route("CreatePACreditDaysmaster")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> CreatePACreditDaysmaster(PACreditDaysMasterModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.CreatePACreditDaysmaster(model);
            return Ok(status);
        }
        [Route("GetCreditdaysMasterByID")]
        [ResponseType(typeof(PACreditDaysMasterModel))]
        public async Task<IHttpActionResult> GetCreditdaysMasterByID(int creditdaysid)
        {
            PACreditDaysMasterModel status = new PACreditDaysMasterModel();
            status = await _paBusenessAcess.GetCreditdaysMasterByID(creditdaysid);
            return Ok(status);
        }
        [Route("AssignCreditdaysToEmployee")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> AssignCreditdaysToEmployee(PACreditDaysApproverModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.AssignCreditdaysToEmployee(model);
            return Ok(status);
        }
        [Route("RemovePAAuthorizationLimitsByID")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemovePAAuthorizationLimitsByID(int authid)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.RemovePAAuthorizationLimitsByID(authid);
            return Ok(status);
        }
        [Route("RemovePACreditDaysMaster")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemovePACreditDaysMaster(int creditid)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.RemovePACreditDaysMaster(creditid);
            return Ok(status);
        }
        [Route("GetPAAuthorizationLimitsByDeptId")]
        [ResponseType(typeof(List<PAAuthorizationLimitModel>))]
        public async Task<IHttpActionResult> GetPAAuthorizationLimitsByDeptId(int departmentid)
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            model = await _paBusenessAcess.GetPAAuthorizationLimitsByDeptId(departmentid);
            return Ok(model);
        }
        [HttpPost]
        [Route("RemovePACreditDaysApprover")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemovePACreditDaysApprover(EmployeemappingtocreditModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.RemovePACreditDaysApprover(model);
            return Ok(model);
        }
        [Route("GetPACreditDaysApproverById")]
        [ResponseType(typeof(PACreditDaysApproverModel))]
        public async Task<IHttpActionResult> GetPACreditDaysApproverById(int ApprovalId)
        {
            PACreditDaysApproverModel status = new PACreditDaysApproverModel();
            status = await _paBusenessAcess.GetPACreditDaysApproverById(ApprovalId);
            return Ok(status);
        }

        [HttpPost]
        [Route("GetEmployeeMappings")]
        [ResponseType(typeof(EmployeModel))]
        public async Task<IHttpActionResult> GetEmployeeMappings(PAConfigurationModel model)
        {
            EmployeModel employee = new EmployeModel();
            employee = await _paBusenessAcess.GetEmployeeMappings(model);
            return Ok(employee);
        }
        [HttpPost]
        [Route("GetEmployeeMappings1")]
        [ResponseType(typeof(DataSet))]
        public DataSet GetEmployeeMappings1(PAConfigurationModel model)
        {
            DataSet ds = new DataSet();
            ds = _paBusenessAcess.GetEmployeeMappings1(model);
            return ds;
        }

        //[HttpPost]
        //[Route("GetItemsByMasterIDs")]
        //[ResponseType(typeof(List<LoadItemsByID>))]
        //public async Task<IHttpActionResult> GetItemsByMasterIDs(PADetailsModel masters)
        //{

        //    List<LoadItemsByID> model = new List<LoadItemsByID>();
        //    model = await _rfqBusenessAcess.GetItemsByMasterIDs(masters);
        //    return Ok(model);
        //}
        [HttpPost]
        [Route("GetItemsByMasterIDs")]
        [ResponseType(typeof(List<loadtaxesbyitemwise>))]
        public IHttpActionResult GetItemsByMasterIDs(PADetailsModel masters)
        {
            return Ok(this._paBusenessAcess.GetItemsByMasterIDs(masters));
        }
        [HttpGet]
        [Route("GetAllDepartments")]
        [ResponseType(typeof(List<DepartmentModel>))]
        public async Task<IHttpActionResult> GetAllDepartments()
        {
            List<DepartmentModel> model = new List<DepartmentModel>();
            model = await _paBusenessAcess.GetAllDepartments();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetSlabsByDepartmentID/{DeptID}")]
        [ResponseType(typeof(List<PAAuthorizationLimitModel>))]
        public async Task<IHttpActionResult> GetSlabsByDepartmentID(int DeptID)
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            model = await _paBusenessAcess.GetSlabsByDepartmentID(DeptID);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllEmployee")]
        [ResponseType(typeof(List<EmployeeModel>))]
        public async Task<IHttpActionResult> GetAllEmployee()
        {
            List<EmployeModel> model = new List<EmployeModel>();
            model = await _paBusenessAcess.GetAllEmployee();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllCredits")]
        [ResponseType(typeof(List<PAAuthorizationLimitModel>))]
        public async Task<IHttpActionResult> GetAllCredits()
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            model = await _paBusenessAcess.GetAllCredits();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllCreditDays")]
        [ResponseType(typeof(List<PACreditDaysMasterModel>))]
        public async Task<IHttpActionResult> GetAllCreditDays()
        {
            List<PACreditDaysMasterModel> model = new List<PACreditDaysMasterModel>();
            model = await _paBusenessAcess.GetAllCreditDays();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllMprPAPurchaseModes")]
        [ResponseType(typeof(List<MPRPAPurchaseModesModel>))]
        public async Task<IHttpActionResult> GetAllMprPAPurchaseModes()
        {
            List<MPRPAPurchaseModesModel> model = new List<MPRPAPurchaseModesModel>();
            model = await _paBusenessAcess.GetAllMprPAPurchaseModes();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllMprPAPurchaseTypes")]
        [ResponseType(typeof(List<MPRPAPurchaseTypesModel>))]
        public async Task<IHttpActionResult> GetAllMprPAPurchaseTypes()
        {
            List<MPRPAPurchaseTypesModel> model = new List<MPRPAPurchaseTypesModel>();
            model = await _paBusenessAcess.GetAllMprPAPurchaseTypes();
            return Ok(model);
        }
        [HttpPost]
        [Route("InsertPurchaseAuthorization")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> InsertPurchaseAuthorization(MPRPADetailsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.InsertPurchaseAuthorization(model);
            return Ok(status);
        }
        [HttpPost]
        [Route("UpdatePurchaseAuthorization")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> UpdatePurchaseAuthorization(MPRPADetailsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.UpdatePurchaseAuthorization(model);
            return Ok(status);
        }
        [HttpPost]
        [Route("finalpa")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> finalpa(MPRPADetailsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.finalpa(model);
            return Ok(status);
        }
        [HttpGet]
        [Route("GetMPRPADeatilsByPAID/{PID}")]
        [ResponseType(typeof(MPRPADetailsModel))]
        public async Task<IHttpActionResult> GetMPRPADeatilsByPAID(int PID)
        {
            MPRPADetailsModel model = new MPRPADetailsModel();
            model = await _paBusenessAcess.GetMPRPADeatilsByPAID(PID);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllMPRPAList")]
        [ResponseType(typeof(List<MPRPADetailsModel>))]
        public async Task<IHttpActionResult> GetAllMPRPAList()
        {
            List<MPRPADetailsModel> model = new List<MPRPADetailsModel>();
            model = await _paBusenessAcess.GetAllMPRPAList();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllPAFunctionalRoles")]
        [ResponseType(typeof(List<PAFunctionalRolesModel>))]
        public async Task<IHttpActionResult> GetAllPAFunctionalRoles()
        {
            List<PAFunctionalRolesModel> model = new List<PAFunctionalRolesModel>();
            model = await _paBusenessAcess.GetAllPAFunctionalRoles();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetCreditSlabsandemployees")]
        [ResponseType(typeof(List<EmployeemappingtocreditModel>))]
        public async Task<IHttpActionResult> GetCreditSlabsandemployees()
        {
            List<EmployeemappingtocreditModel> model = new List<EmployeemappingtocreditModel>();
            model = await _paBusenessAcess.GetCreditSlabsandemployees();
            return Ok(model);
        }
        [HttpGet]
        [Route("GetPurchaseSlabsandMappedemployees")]
        [ResponseType(typeof(List<EmployeemappingtopurchaseModel>))]
        public async Task<IHttpActionResult> GetPurchaseSlabsandMappedemployees()
        {
            List<EmployeemappingtopurchaseModel> model = new List<EmployeemappingtopurchaseModel>();
            model = await _paBusenessAcess.GetPurchaseSlabsandMappedemployees();
            return Ok(model);
        }
        [HttpPost]
        [Route("RemovePurchaseApprover")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemovePurchaseApprover(EmployeemappingtopurchaseModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.RemovePurchaseApprover(model);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllProjectManagers")]
        [ResponseType(typeof(List<ProjectManagerModel>))]
        public async Task<IHttpActionResult> GetAllProjectManagers()
        {
            List<ProjectManagerModel> model = new List<ProjectManagerModel>();
            model = await _paBusenessAcess.LoadAllProjectManagers();
            return Ok(model);
        }
        [HttpPost]
        [Route("LoadVendorByMprDetailsId")]
        [ResponseType(typeof(List<VendormasterModel>))]
        public async Task<IHttpActionResult> LoadVendorByMprDetailsId(List<int?> MPRItemDetailsid)
        {
            List<VendormasterModel> model = new List<VendormasterModel>();
            model = await _paBusenessAcess.LoadVendorByMprDetailsId(MPRItemDetailsid);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllApproversList")]
        [ResponseType(typeof(List<MPRPAApproversModel>))]
        public async Task<IHttpActionResult> GetAllApproversList()
        {
            List<MPRPAApproversModel> model = new List<MPRPAApproversModel>();
            model = await _paBusenessAcess.GetAllApproversList();
            return Ok(model);
        }
        [HttpPost]
        [Route("GetMprApproverDetailsBySearch")]
        [ResponseType(typeof(List<GetmprApproverdeatil>))]
        public async Task<IHttpActionResult> GetMprApproverDetailsBySearch(PAApproverDetailsInputModel model)
        {
            List<mprApproverdetailsview> details = new List<mprApproverdetailsview>();
            details = await _paBusenessAcess.GetMprApproverDetailsBySearch(model);
            return Ok(details);
        }
        [HttpPost]
        [Route("UpdateMprpaApproverStatus")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> UpdateMprpaApproverStatus(MPRPAApproversModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.UpdateMprpaApproverStatus(model);
            return Ok(model);
        }
        [HttpPost]
        [Route("getrfqtermsbyrevisionid")]
        [ResponseType(typeof(List<DisplayRfqTermsByRevisionId>))]
        public async Task<IHttpActionResult> getrfqtermsbyrevisionid(List<int> RevisionId)
        {
            List<DisplayRfqTermsByRevisionId> details = new List<DisplayRfqTermsByRevisionId>();
            details = await _paBusenessAcess.getrfqtermsbyrevisionid(RevisionId);
            return Ok(details);
        }
        [HttpPost]
        [Route("GetPurchaseSlabsandMappedemployeesByDeptId")]
        [ResponseType(typeof(List<Employeemappingtopurchase>))]
        public async Task<IHttpActionResult> GetPurchaseSlabsandMappedemployeesByDeptId(EmployeeFilterModel model)
        {
            List<Employeemappingtopurchase> purchase = new List<Employeemappingtopurchase>();
            purchase = await _paBusenessAcess.GetPurchaseSlabsandMappedemployeesByDeptId(model);
            return Ok(purchase);
        }
        [HttpPost]
        [Route("InsertPaitems")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> InsertPaitems(List<ItemsViewModel> paitem)
        {
            statuscheckmodel model = new statuscheckmodel();
            model = await _paBusenessAcess.InsertPaitems(paitem);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetAllMappedSlabs")]
        [ResponseType(typeof(List<GetMappedSlab>))]
        public async Task<IHttpActionResult> GetAllMappedSlabs()
        {
            List<GetMappedSlab> model = new List<GetMappedSlab>();
            model = await _paBusenessAcess.GetAllMappedSlabs();
            return Ok(model);
        }
        [HttpPost]
        [Route("RemoveMappedSlab")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> RemoveMappedSlab(PAAuthorizationLimitModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.RemoveMappedSlab(model);
            return Ok(status);
        }
        [HttpPost]
        [Route("getMprPaDetailsBySearch")]
        [ResponseType(typeof(List<NewGetMprPaDetailsByFilter>))]
        public async Task<IHttpActionResult> getMprPaDetailsBySearch(PADetailsModel model)
        {
            List<NewGetMprPaDetailsByFilter> filter = new List<NewGetMprPaDetailsByFilter>();
            filter = await _paBusenessAcess.getMprPaDetailsBySearch(model);
            return Ok(filter);
        }
        [HttpPost]
        [Route("GetPaStatusReports")]
        [ResponseType(typeof(List<PAReport>))]
        public async Task<IHttpActionResult> GetPaStatusReports(PAReportInputModel model)
        {
            List<PAReport> filter = new List<PAReport>();
            filter = await _paBusenessAcess.GetPaStatusReports(model);
            return Ok(filter);
        }
        [HttpPost]
        [Route("UpdateApproverforRequest")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> UpdateApproverforRequest(MPRPAApproversModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.UpdateApproverforRequest(model);
            return Ok(status);
        }

        [HttpPost]
        [Route("UploadFile")]
        public IHttpActionResult UploadPADocuments()
        {
            var httpRequest = HttpContext.Current.Request;
            var serverPath = HttpContext.Current.Server.MapPath("~/PADocuments");
            string parsedFileName = "";
            var revisionId = httpRequest.Files.AllKeys[0];
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(postedFile.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(postedFile.ContentLength);
                    }

                    GC.Collect();
                    parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + revisionId + "\\" + ToValidFileName(postedFile.FileName));
                    serverPath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + revisionId;
                    var path = Path.Combine(serverPath, ToValidFileName(postedFile.FileName));
                    if (!Directory.Exists(serverPath))
                        Directory.CreateDirectory(serverPath);
                    var memory = new MemoryStream();
                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                    var updatedStream = new MemoryStream(fileData);
                    updatedStream.Seek(0, SeekOrigin.Begin);
                    updatedStream.CopyToAsync(fs).Wait();
                    fs.Flush();
                    GC.Collect();
                }
            }
            return Ok(parsedFileName);

        }

        [HttpPost]
        [Route("uploadExcel")]
        public IHttpActionResult uploadExcel()
        {
            var paid = "";
            int documentid = 0;
            var httpRequest = HttpContext.Current.Request;
            var serverPath = HttpContext.Current.Server.MapPath("~/PADocuments");
            string parsedFileName = "";
            string filename = "";

            if (httpRequest.Files.Count > 0)
            {
                paid = httpRequest.Files.AllKeys[0];
                //string employeeno = httpRequest.Files.AllKeys[1];
                var postedFile = httpRequest.Files[0];
                parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + paid + "\\" + ToValidFileName(postedFile.FileName));
                serverPath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + paid;
                var filePath = Path.Combine(serverPath, ToValidFileName(postedFile.FileName));
                if (!Directory.Exists(serverPath))
                    Directory.CreateDirectory(serverPath);
                postedFile.SaveAs(filePath);
                try
                {
                    YSCMEntities entities = new YSCMEntities();

                    var data = new MPRPADocument();
                    data.Filename = postedFile.FileName;
                    data.Filepath = parsedFileName;
                    data.uploadeddate = System.DateTime.Now;
                    data.paid = Convert.ToInt32(paid);
                    data.deleteflag = false;
                    entities.MPRPADocuments.Add(data);
                    entities.SaveChanges();
                    documentid = data.DocumentId;
                    filename = data.Filename;

                    //entities.MPRPADocuments.Add(new MPRPADocument
                    //{
                    //    Filename = postedFile.FileName,
                    //    Filepath = parsedFileName,
                    //    uploadeddate = System.DateTime.Now,
                    //    paid = Convert.ToInt32(paid)
                    //});
                    //entities.SaveChanges();

                    // int succRecs = iSucceRows;
                }
                catch (Exception e)
                {
                    throw e;
                }
                //for (int i = 0; i < httpRequest.Files.Count; i++)
                //{


                //}


            }
            return Ok(filename);

        }
        private static string ToValidFileName(string fileName)
        {
            fileName = fileName.ToLower().Replace(" ", "_").Replace("(", "_").Replace(")", "_").Replace("&", "_").Replace("*", "_").Replace("-", "_").Replace("+", "_");
            return string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
        }
        [HttpPost]
        [Route("DeletePAByPAid")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> DeletePAByPAid(padeletemodel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.DeletePAByPAid(model);
            return Ok(status);
        }
        [HttpPost]
        [Route("LoadIncompletedPAlist")]
        [ResponseType(typeof(List<IncompletedPAlist>))]
        public async Task<IHttpActionResult> LoadIncompletedPAlist(painputmodel model)
        {
            List<IncompletedPAlist> status = new List<IncompletedPAlist>();
            status = await _paBusenessAcess.GetIncompletedPAlist(model);
            return Ok(status);
        }
        //[HttpPost]
        //[Route("FileUploading1")]
        //public async Task<string> FileUploading1()
        //{
        //    var ctx = HttpContext.Current.Request;
        //    var root = HttpContext.Current.Server.MapPath("~/PADocuments");
        //    string path = "";
        //    string parsedFileName = "";
        //    DateTime dateUpload;
        //    string FileName = "";
        //    int paid = 61;
        //    var provider = new MultipartFormDataStreamProvider(root);
        //    try
        //    {
        //        await Request.Content.ReadAsMultipartAsync(provider);
        //        string FileType = ctx.Files.AllKeys[0];
        //        if (ctx.Files.Count > 0)
        //        {
        //            foreach (var file in provider.FileData)
        //            {
        //                var postedfile = ctx.Files[0];
        //                var name = file.Headers.ContentDisposition.FileName;
        //                name = name.Trim('"');
        //                //string extension = System.IO.Path.GetExtension(name);
        //                //string result = name.Substring(0, name.Length - extension.Length);
        //                //FileName = result;
        //                dateUpload = DateTime.Now;
        //                parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + paid + "\\" + ToValidFileName(postedfile.FileName));
        //                root = root + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + paid;
        //                var filePath = Path.Combine(root, ToValidFileName(postedfile.FileName));
        //                if (!Directory.Exists(root))
        //                    Directory.CreateDirectory(root);
        //                postedfile.SaveAs(filePath);
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        return $"Error:{e.Message}";
        //    }
        //    return "File Uploaded!";
        //}
        [HttpPost]
        [Route("getrfqtermsbyrevisionsid1")]
        [ResponseType(typeof(DataTable))]
        public DataTable getrfqtermsbyrevisionsid1(List<int> revisionid)
        {
            DataTable ds = new DataTable();
            ds = _paBusenessAcess.getrfqtermsbyrevisionsid1(revisionid);
            return ds;
        }
        [HttpPost]
        [Route("DeletePADocument")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> DeletePADocument(PADocumentsmodel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.DeletePADocument(model);
            return Ok(status);
        }
        [HttpPost]
        [Route("getTokuchuReqList")]
        public IHttpActionResult getTokuchuReqList(tokuchuFilterParams tokuchufilterparams)
        {
            return Ok(this._paBusenessAcess.getTokuchuReqList(tokuchufilterparams));
        }

        [HttpGet]
        [Route("GetTokuchuDetailsByPAID/{PID}/{TokuchRequestid}")]
        public IHttpActionResult GetTokuchuDetailsByPAID(int? PID, int? TokuchRequestid)
        {
            var result = _paBusenessAcess.GetTokuchuDetailsByPAID(PID, TokuchRequestid);
            return Ok(result);
        }

        [HttpPost]
        [Route("updateTokuchuRequest/{typeOfuser}/{revisionId}")]
        public IHttpActionResult getDBMastersList([FromBody] TokuchuRequest Result, string typeOfuser, int revisionId)
        {
            return Ok(this._paBusenessAcess.updateTokuchuRequest(Result, typeOfuser, revisionId));
        }

        [HttpPost]
        [Route("Getmprstatus")]
        public IHttpActionResult Getmprstatus()
        {
            return Ok(this._paBusenessAcess.Getmprstatus());
        }

        [HttpPost]
        [Route("GetmprstatusReport")]
        [ResponseType(typeof(DataSet))]
        public DataSet GetmprstatusReport(ReportInputModel model)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Param = new SqlParameter[5];
            string data = "";
            YSCMEntities obj = new YSCMEntities();
            if (model.OrgDepartmentId != 0)
            {
                List<int> departments = obj.MPRDepartments.Where(x => x.ORgDepartmentid == model.OrgDepartmentId).Select(x => (int)x.DepartmentId).ToList();
                data = string.Join(" , ", departments);
            }
            if (model.BuyerGroupId == 0)
            {
                Param[0] = new SqlParameter("buyergroupid", SqlDbType.VarChar);
                Param[0].Value = DBNull.Value;
                Param[1] = new SqlParameter("@fromdate", model.Fromdate);
                Param[2] = new SqlParameter("@todate", model.Todate);
                Param[3] = new SqlParameter("@DepartmentId", data);
                Param[4] = new SqlParameter("@issuepurpose", model.Issuepurposeid);
            }
            else
            {
                //string region = (string.Join(",", model.multiregion.Select(x => x.Region.ToString()).ToArray()));
                Param[0] = new SqlParameter("@BuyerGroupId", model.BuyerGroupId);
                Param[1] = new SqlParameter("@fromdate", model.Fromdate);
                Param[2] = new SqlParameter("@todate", model.Todate);
                Param[3] = new SqlParameter("@DepartmentId", data);
                Param[4] = new SqlParameter("@issuepurpose", model.Issuepurposeid);
            }
            ds = _paBusenessAcess.GetmprstatusReport("newmprstatuareport", Param);
            return ds;
        }
        [HttpPost]
        [Route("GetMprstatuswisereport")]
        [ResponseType(typeof(DataSet))]
        public DataSet GetMprstatuswisereport(ReportInputModel model)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Param = new SqlParameter[7];
            string data = "";
            YSCMEntities obj = new YSCMEntities();
            if (model.DepartmentId == 0)
            {
                List<int> departments = obj.MPRDepartments.Where(x => x.ORgDepartmentid == model.OrgDepartmentId).Select(x => (int)x.DepartmentId).ToList();
                data = string.Join(" , ", departments);
            }
            else
            {
                data = string.Join(",", model.DepartmentId);
            }
            if (model.BuyerGroupId == 0)
            {
                Param[0] = new SqlParameter("buyergroupid", SqlDbType.VarChar);
                Param[0].Value = DBNull.Value;
                Param[1] = new SqlParameter("@fromdate", model.Fromdate);
                Param[2] = new SqlParameter("@todate", model.Todate);
                Param[3] = new SqlParameter("@ProjectManager", model.ProjectManager);
                Param[4] = new SqlParameter("@SaleOrderNo", model.SaleOrderNo);
                Param[5] = new SqlParameter("@Departmentid", data);
                Param[6] = new SqlParameter("@JobCode", model.jobcode);
            }
            else
            {
                //string region = (string.Join(",", model.multiregion.Select(x => x.Region.ToString()).ToArray()));
                Param[0] = new SqlParameter("@BuyerGroupId", model.BuyerGroupId);
                Param[1] = new SqlParameter("@fromdate", model.Fromdate);
                Param[2] = new SqlParameter("@todate", model.Todate);
                Param[3] = new SqlParameter("@ProjectManager", model.ProjectManager);
                Param[4] = new SqlParameter("@SaleOrderNo", model.SaleOrderNo);
                Param[5] = new SqlParameter("@Departmentid", data);
                Param[6] = new SqlParameter("@JobCode", model.jobcode);
            }
            ds = _paBusenessAcess.GetMprstatuswisereport("Mprwisereport", Param);
            return ds;
        }
        [HttpPost]
        [Route("GetmprRequisitionReport")]
        public IHttpActionResult GetmprRequisitionReport(ReportInputModel input)
        {
            return Ok(this._paBusenessAcess.GetmprRequisitionReport(input));
        }
        [HttpGet]
        [Route("GetmprRequisitionfilters")]
        [ResponseType(typeof(ReportFilterModel))]
        public IHttpActionResult GetmprRequisitionfilters()
        {
            ReportFilterModel status = new ReportFilterModel();
            status = _paBusenessAcess.GetmprRequisitionfilters();
            return Ok(status);
        }
        [HttpGet]
        [Route("Loadprojectmanagersforreport")]
        public IHttpActionResult Loadprojectmanagersforreport()
        {
            return Ok(this._paBusenessAcess.Loadprojectmanagersforreport());
        }
        [HttpPost]
        [Route("Loadprojectcodewisereport")]

        public IHttpActionResult Loadprojectcodewisereport(ReportInputModel model)
        {
            //List<Reportbyprojectcode> status = new List<Reportbyprojectcode>();
            // status = _paBusenessAcess.Loadprojectcodewisereport(model);
            return Ok(_paBusenessAcess.Loadprojectcodewisereport(model));
        }
        [HttpPost]
        [Route("LoadprojectDurationwisereport")]
        [ResponseType(typeof(ReportbyprojectDuration))]
        public IHttpActionResult LoadprojectDurationwisereport(ReportInputModel model)
        {
            List<ReportbyprojectDuration> status = new List<ReportbyprojectDuration>();
            status = _paBusenessAcess.LoadprojectDurationwisereport(model);
            return Ok(status);
        }
        [HttpGet]
        [Route("Loadjobcodes")]
        public IHttpActionResult Loadjobcodes()
        {
            return Ok(this._paBusenessAcess.Loadjobcodes());
        }
        [HttpGet]
        [Route("GETApprovernamesbydepartmentid/{departmentid}")]
        [ResponseType(typeof(DataTable))]
        public DataTable GETApprovernamesbydepartmentid(int departmentid)
        {
            DataTable ds = new DataTable();
            ds = _paBusenessAcess.GETApprovernamesbydepartmentid(departmentid);
            return ds;
        }
        [HttpGet]
        [Route("Loadsaleorder")]
        public IHttpActionResult Loadsaleorder()
        {
            return Ok(this._paBusenessAcess.Loadsaleorder());
        }
        //[HttpGet]
        //[Route("Downloadexcel")]
        //public string Downloadexcel(int revisionid)
        //{
        //    try
        //    {
        //        string sourcePath = "C:\\Users\\developer4\\Desktop\\";
        //        string targetpath = ConfigurationManager.AppSettings["DownloadVexcel"];
        //        string srcfilename = "Book1.xlsx";
        //        string targetfilename = "Akil" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
        //        string sourceFile = System.IO.Path.Combine(sourcePath, srcfilename);
        //        string destFile = System.IO.Path.Combine(targetpath, targetfilename);
        //        if (!System.IO.Directory.Exists(targetpath))
        //        {
        //            System.IO.Directory.CreateDirectory(targetpath);
        //        }
        //        System.IO.File.Copy(sourceFile, destFile, false);
        //        Microsoft.Office.Interop.Excel._Application docExcel = new Microsoft.Office.Interop.Excel.Application();
        //        docExcel.Visible = false;
        //        docExcel.DisplayAlerts = false;
        //        Microsoft.Office.Interop.Excel._Workbook workbooksExcel = docExcel.Workbooks.Open(destFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //        Microsoft.Office.Interop.Excel._Worksheet worksheetExcel = (Microsoft.Office.Interop.Excel._Worksheet)workbooksExcel.ActiveSheet;

        //        YSCMEntities obj = new YSCMEntities();
        //        var data = obj.RfqForVendorDownloads.Where(x => x.rfqRevisionId == revisionid).ToList();
        //        Microsoft.Office.Interop.Excel.Range range = worksheetExcel.UsedRange;
        //        //for Headers
        //        foreach (var item in data)
        //        {
        //            (range.Worksheet.Cells["1", "F"]).Value2 = item.RFQNo;
        //            (range.Worksheet.Cells["1", "L"]).Value2 = item.rfqRevisionId;
        //            (range.Worksheet.Cells["2", "F"]).Value2 = item.RFQValidDate;
        //            (range.Worksheet.Cells["2", "L"]).Value2 = item.ReqRemarks;
        //            (range.Worksheet.Cells["3", "J"]).Value2 = item.RFQValidDate;
        //        }
        //        //line items

        //        int i = 6;
        //        foreach (var item1 in data)
        //        {
        //            (range.Worksheet.Cells[i, "A"]).Value2 = item1.RFQItemsId;
        //            (range.Worksheet.Cells[i, "B"]).Value2 = item1.ItemId;
        //            (range.Worksheet.Cells[i, "C"]).Value2 = item1.ItemDescription;
        //            (range.Worksheet.Cells[i, "D"]).Value2 = item1.QuotationQty;
        //            (range.Worksheet.Cells[i, "E"]).Value2 = item1.UOM;
        //            (range.Worksheet.Cells[i, "F"]).Value2 = item1.CurrencyValue;
        //            (range.Worksheet.Cells[i, "G"]).Value2 = item1.UnitPrice;
        //            (range.Worksheet.Cells[i, "H"]).Value2 = item1.UnitPrice;
        //            (range.Worksheet.Cells[i, "I"]).Value2 = item1.DiscountPercentage;
        //            (range.Worksheet.Cells[i, "J"]).Value2 = item1.Discount;
        //            (range.Worksheet.Cells[i, "K"]).Value2 = item1.VendorModelNo; 
        //            (range.Worksheet.Cells[i, "L"]).Value2 = item1.MfgPartNo;
        //            (range.Worksheet.Cells[i, "M"]).Value2 = item1.MfgModelNo;
        //            (range.Worksheet.Cells[i, "N"]).Value2 = item1.ManufacturerName;
        //            (range.Worksheet.Cells[i, "O"]).Value2 = item1.CGSTPercentage;
        //            (range.Worksheet.Cells[i, "P"]).Value2 = item1.IGSTPercentage;
        //            (range.Worksheet.Cells[i, "Q"]).Value2 = item1.SGSTPercentage;
        //            (range.Worksheet.Cells[i, "R"]).Value2 = item1.PFAmount;
        //            (range.Worksheet.Cells[i, "S"]).Value2 = item1.PFPercentage;
        //            (range.Worksheet.Cells[i, "T"]).Value2 = item1.FreightAmount;
        //            (range.Worksheet.Cells[i, "U"]).Value2 = item1.FreightPercentage;
        //            (range.Worksheet.Cells[i, "V"]).Value2 = item1.DeliveryDate;
        //            (range.Worksheet.Cells[i, "W"]).Value2 = item1.Remarks;
        //            i++;
        //        }
        //        workbooksExcel.Save();
        //        workbooksExcel.Close(false, Type.Missing, Type.Missing);
        //        docExcel.Application.DisplayAlerts = true;
        //        docExcel.Application.Quit();
        //        return "/";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        public string UploadingvendorExcel()
        {

            return "/";
        }
        [HttpPost]
        [Route("getPaValueReport")]
        [ResponseType(typeof(List<loadpastatsreport>))]
        public async Task<IHttpActionResult> getPaValueReport(PADetailsModel model)
        {
            List<loadpastatsreport> details = new List<loadpastatsreport>();
            details = await _paBusenessAcess.getPaValueReport(model);
            return Ok(details);
        }
        [HttpPost]
        [Route("uploadItemData")]
        public IHttpActionResult uploadItemData()
        {
            var Revisionid = "";
            int documentid = 0;
            var httpRequest = HttpContext.Current.Request;
            var serverPath = HttpContext.Current.Server.MapPath("~/VendorDocuments");
            string parsedFileName = "";
            string filename = "";

            if (httpRequest.Files.Count > 0)
            {
                Revisionid = httpRequest.Files.AllKeys[0];
                //string employeeno = httpRequest.Files.AllKeys[1];
                var postedFile = httpRequest.Files[0];
                parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + Revisionid + "\\" + ToValidFileName(postedFile.FileName));
                serverPath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + Revisionid;
                var filePath = Path.Combine(serverPath, ToValidFileName(postedFile.FileName));
                if (!Directory.Exists(serverPath))
                    Directory.CreateDirectory(serverPath);
                postedFile.SaveAs(filePath);
                try
                {
                    YSCMEntities entities = new YSCMEntities();

                    var data = new RFQDocument();
                    data.DocumentName = postedFile.FileName;
                    data.Path = parsedFileName;
                    data.UploadedDate = System.DateTime.Now;
                    data.rfqRevisionId = Convert.ToInt32(Revisionid);
                    data.DeleteFlag = false;
                    entities.RFQDocuments.Add(data);
                    entities.SaveChanges();
                    documentid = data.RfqDocId;
                    filename = data.DocumentName;

                    DataTable dtexcel = new DataTable();
                    bool hasHeaders = false;
                    string HDR = hasHeaders ? "Yes" : "No";
                    string strConn;
                    if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
                    else
                        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path.GetDirectoryName(filePath) + ";Extended Properties='Text;HDR=YES;FMT=Delimited;'";

                    OleDbConnection conn = new OleDbConnection(strConn);
                    conn.Open();
                    DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                    DataRow schemaRow = schemaTable.Rows[0];
                    string sheet = schemaRow["TABLE_NAME"].ToString();
                    if (!sheet.EndsWith("_"))
                    {
                        string query = " SELECT  * FROM [" + sheet + "]";
                        OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                        dtexcel.Locale = CultureInfo.CurrentCulture;
                        daexcel.Fill(dtexcel);
                    }

                    conn.Close();
                    int iSucceRows = 0;
                    System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    foreach (DataRow row in dtexcel.Rows)
                    {
                        entities.RFQItems_N.Add(new RFQItems_N
                        {
                            FreightAmount = Convert.ToDecimal(row["FreightAmount"].ToString()),
                            FreightPercentage = Convert.ToDecimal(row["FreightPercentage"].ToString()),
                            PFAmount = Convert.ToDecimal(row["P & F Amount"].ToString()),
                            PFPercentage = Convert.ToDecimal(row["P & F Percentage"].ToString()),
                            CGSTPercentage = Convert.ToDecimal(row["CGST  Percentage"].ToString()),
                            SGSTPercentage = Convert.ToDecimal(row["SGST  Percentage"].ToString()),
                            IGSTPercentage = Convert.ToDecimal(row["IGST  Percentage"].ToString()),
                        });
                    }

                    foreach (DataRow row in dtexcel.Rows)
                    {
                        entities.RFQItemsInfo_N.Add(new RFQItemsInfo_N
                        {
                            UnitPrice = Convert.ToDecimal(row["UnitPrice"].ToString()),
                            Discount = Convert.ToDecimal(row["Discount"].ToString()),
                            DiscountPercentage = Convert.ToDecimal(row["DiscountPercentage"].ToString()),
                        });
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            return Ok(filename);

        }
        [HttpPost]
        [Route("InsertMappingItems")]
        [ResponseType(typeof(List<List<statuscheckmodel>>))]
        public async Task<IHttpActionResult> InsertMappingItems(List<MappingItemModel> model)
        {
            List<statuscheckmodel> status = new List<statuscheckmodel>();
            status = await _paBusenessAcess.InsertMappingItems(model);
            return Ok(status);
        }

        [HttpPost]
        [Route("uploadMSA")]
        public IHttpActionResult uploadMSALineItem()
        {
            var paid = ""; var Uploadby = "";
            int documentid = 0;
            var httpRequest = HttpContext.Current.Request;
            var serverPath = HttpContext.Current.Server.MapPath("~/PADocuments");
            string parsedFileName = "";


            statuscheckmodel status = new statuscheckmodel();
            if (httpRequest.Files.Count > 0)
            {
                paid = httpRequest.Files.AllKeys[0];
                Uploadby = httpRequest.Files.AllKeys[1];
                //string employeeno = httpRequest.Files.AllKeys[1];
                var postedFile = httpRequest.Files[0];
                parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + paid + "\\" + ToValidFileName(postedFile.FileName));
                serverPath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + paid;
                var filePath = Path.Combine(serverPath, ToValidFileName(postedFile.FileName));
                if (!Directory.Exists(serverPath))
                    Directory.CreateDirectory(serverPath);
                postedFile.SaveAs(filePath);
                try
                {
                    DataTable dtexcel = new DataTable();
                    bool hasHeaders = false;
                    string HDR = hasHeaders ? "Yes" : "No";
                    string strConn;
                    if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
                    else
                        strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";

                    OleDbConnection conn = new OleDbConnection(strConn);
                    conn.Open();
                    DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                    DataRow schemaRow = schemaTable.Rows[0];
                    string sheet = schemaRow["TABLE_NAME"].ToString();
                    if (!sheet.EndsWith("_"))
                    {
                        string query = "SELECT  * FROM [data$]";
                        OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                        dtexcel.Locale = CultureInfo.CurrentCulture;
                        daexcel.Fill(dtexcel);
                    }

                    conn.Close();
                    int iSucceRows = 0;
                    int paidCn = Convert.ToInt32(paid);

                    if (IsPaIdMatch(paidCn, dtexcel, out string outputMsg))
                    {
                        YSCMEntities obj = new YSCMEntities();
                        var ConfirmationFlag = obj.MSAMasterConfirmations.Where(li => li.PAID == paidCn && li.Deleteflag == false && li.Confirmationflag == true).FirstOrDefault();

                        if (ConfirmationFlag == null)
                        {
                            foreach (DataRow row in dtexcel.Rows)
                            {
                                int paids = Convert.ToInt32(row["paid"]);
                                int paitemid = Convert.ToInt32(row["PAItemID"]);

                                MSALineItem mSALineItem1 = new MSALineItem();
                                mSALineItem1.Item_No_ = string.IsNullOrWhiteSpace(row["Item No#"].ToString()) ? DBNull.Value.ToString() : row["Item No#"].ToString();
                                //mSALineItem1.deletionflag = false;
                                mSALineItem1.mscode = row["mscode"].ToString();
                                mSALineItem1.ItemDescription = row["ItemDescription"].ToString();
                                mSALineItem1.WBS = row["WBS"].ToString();
                                mSALineItem1.wbsdesc = string.IsNullOrWhiteSpace(row["wbsdesc"].ToString()) ? null : row["wbsdesc"].ToString();
                                mSALineItem1.Quantity = Convert.ToInt32(row["Quantity"].ToString());
                                mSALineItem1.unit = string.IsNullOrWhiteSpace(row["unit"].ToString()) ? null : row["unit"].ToString();
                                mSALineItem1.RequirementDate = row["RequirementDate"].ToString();
                                mSALineItem1.Project = row["Project"].ToString();
                                mSALineItem1.projectdesc = string.IsNullOrWhiteSpace(row["projectdesc"].ToString()) ? null : row["projectdesc"].ToString();
                                mSALineItem1.TokuchuNo = row["TokuchuNo"].ToString();
                                mSALineItem1.tokchuno = string.IsNullOrWhiteSpace(row["tokchuno"].ToString()) ? null : row["tokchuno"].ToString();
                                mSALineItem1.bupfl = string.IsNullOrWhiteSpace(row["bupfl"].ToString()) ? null : row["bupfl"].ToString();
                                mSALineItem1.bupfldesc = string.IsNullOrWhiteSpace(row["bupfldesc"].ToString()) ? null : row["bupfldesc"].ToString();
                                mSALineItem1.UnitPrice = string.IsNullOrWhiteSpace(row["UnitPrice"].ToString()) ? null : row["UnitPrice"].ToString();
                                mSALineItem1.Currency = string.IsNullOrWhiteSpace(row["Currency"].ToString()) ? null : row["Currency"].ToString();
                                mSALineItem1.priceunit = string.IsNullOrWhiteSpace(row["priceunit"].ToString()) ? null : row["priceunit"].ToString();
                                mSALineItem1.costelement = string.IsNullOrWhiteSpace(row["costelement"].ToString()) ? null : row["costelement"].ToString();
                                mSALineItem1.costelementdesc = string.IsNullOrWhiteSpace(row["costelementdesc"].ToString()) ? null : row["costelementdesc"].ToString();

                                mSALineItem1.plant = string.IsNullOrWhiteSpace(row["plant"].ToString()) ? null : row["plant"].ToString();
                                mSALineItem1.plantname = string.IsNullOrWhiteSpace(row["plantname"].ToString()) ? null : row["plantname"].ToString();
                                mSALineItem1.StorageLocation = string.IsNullOrWhiteSpace(row["StorageLocation"].ToString()) ? null : row["StorageLocation"].ToString();
                                mSALineItem1.storagelocationname = string.IsNullOrWhiteSpace(row["storagelocationname"].ToString()) ? null : row["storagelocationname"].ToString();
                                mSALineItem1.VendorCode = string.IsNullOrWhiteSpace(row["VendorCode"].ToString()) ? null : row["VendorCode"].ToString();
                                mSALineItem1.VendorName = string.IsNullOrWhiteSpace(row["VendorName"].ToString()) ? null : row["VendorName"].ToString();
                                mSALineItem1.VendorModelNo = string.IsNullOrWhiteSpace(row["VendorModelNo"].ToString()) ? null : row["VendorModelNo"].ToString();
                                mSALineItem1.sortstring1 = string.IsNullOrWhiteSpace(row["sortstring1"].ToString()) ? null : row["sortstring1"].ToString();
                                mSALineItem1.ProjectManager = string.IsNullOrWhiteSpace(row["ProjectManager"].ToString()) ? null : row["ProjectManager"].ToString();
                                mSALineItem1.note1 = string.IsNullOrWhiteSpace(row["note1"].ToString()) ? null : row["note1"].ToString();
                                mSALineItem1.note2 = string.IsNullOrWhiteSpace(row["note3"].ToString()) ? null : row["note3"].ToString();
                                mSALineItem1.note3 = paitemid.ToString();
                                mSALineItem1.note4 = string.IsNullOrWhiteSpace(row["note2"].ToString()) ? null : Convert.ToDateTime(row["note2"].ToString()).ToString("yyyyMMdd");
                                mSALineItem1.lt = string.IsNullOrWhiteSpace(row["lt"].ToString()) ? null : row["lt"].ToString();
                                mSALineItem1.deadline = string.IsNullOrWhiteSpace(row["deadline"].ToString()) ? null : row["deadline"].ToString();
                                mSALineItem1.deliverydate = string.IsNullOrWhiteSpace(row["deliverydate"].ToString()) ? null : row["deliverydate"].ToString();
                                mSALineItem1.text = string.IsNullOrWhiteSpace(row["text"].ToString()) ? null : row["text"].ToString();
                                mSALineItem1.Direct_Shipping = string.IsNullOrWhiteSpace(row["Direct Shipping"].ToString()) ? null : row["Direct Shipping"].ToString();
                                mSALineItem1.Ship_to_Party = string.IsNullOrWhiteSpace(row["Ship to Party"].ToString()) ? null : row["Ship to Party"].ToString();
                                mSALineItem1.Ship_to_Party_seq__No_ = string.IsNullOrWhiteSpace(row["Ship to Party seq# No#"].ToString()) ? null : row["Ship to Party seq# No#"].ToString();

                                mSALineItem1.Ship_to_Party_Name = string.IsNullOrWhiteSpace(row["Ship to Party Name"].ToString()) ? null : row["Ship to Party Name"].ToString();
                                mSALineItem1.Ship_to_Party_Address = string.IsNullOrWhiteSpace(row["Ship to Party Address"].ToString()) ? null : row["Ship to Party Address"].ToString();
                                mSALineItem1.Ship_to_Party_Phone = string.IsNullOrWhiteSpace(row["Ship to Party Phone"].ToString()) ? null : row["Ship to Party Phone"].ToString();
                                mSALineItem1.Nuclear_Spec_Code = string.IsNullOrWhiteSpace(row["Nuclear Spec Code"].ToString()) ? null : row["Nuclear Spec Code"].ToString();
                                mSALineItem1.QW_Box_No_ = string.IsNullOrWhiteSpace(row["QW Box No#"].ToString()) ? null : row["QW Box No#"].ToString();
                                mSALineItem1.Safe_Proof_ID = string.IsNullOrWhiteSpace(row["Safe Proof ID"].ToString()) ? null : row["Safe Proof ID"].ToString();
                                mSALineItem1.XJNo_ = string.IsNullOrWhiteSpace(row["XJNo#"].ToString()) ? null : row["XJNo#"].ToString();
                                mSALineItem1.Product_career_code = string.IsNullOrWhiteSpace(row["Product career code"].ToString()) ? null : row["Product career code"].ToString();
                                mSALineItem1.QIC_Language = string.IsNullOrWhiteSpace(row["QIC Language"].ToString()) ? null : row["QIC Language"].ToString();
                                mSALineItem1.QIC_Delivery_style = string.IsNullOrWhiteSpace(row["QIC Delivery style"].ToString()) ? null : row["QIC Delivery style"].ToString();
                                mSALineItem1.Document_Quantity = string.IsNullOrWhiteSpace(row["Document Quantity"].ToString()) ? null : row["Document Quantity"].ToString();
                                mSALineItem1.Document_Item_No_ = string.IsNullOrWhiteSpace(row["Document Item No#"].ToString()) ? null : row["Document Item No#"].ToString();
                                mSALineItem1.IM_Language = string.IsNullOrWhiteSpace(row["IM Language"].ToString()) ? null : row["IM Language"].ToString(); row["IM Language"].ToString();
                                mSALineItem1.IM_Attach_Style = string.IsNullOrWhiteSpace(row["IM Attach Style"].ToString()) ? null : row["IM Attach Style"].ToString();
                                mSALineItem1.Tokuchu_IM_No_ = string.IsNullOrWhiteSpace(row["Tokuchu IM No#"].ToString()) ? null : row["Tokuchu IM No#"].ToString();
                                mSALineItem1.Parts_Instrument_Model = string.IsNullOrWhiteSpace(row["Parts Instrument Model"].ToString()) ? null : row["Parts Instrument Model"].ToString();
                                mSALineItem1.Serial_Information_Flag = string.IsNullOrWhiteSpace(row["Serial Information Flag"].ToString()) ? null : row["Serial Information Flag"].ToString();
                                mSALineItem1.System_Model = string.IsNullOrWhiteSpace(row["System Model"].ToString()) ? null : row["System Model"].ToString(); row["System Model"].ToString();
                                mSALineItem1.Additional_work_code_1 = string.IsNullOrWhiteSpace(row["Additional work code 1"].ToString()) ? null : row["Additional work code 1"].ToString();
                                mSALineItem1.Additional_work_code_2 = string.IsNullOrWhiteSpace(row["Additional work code 2"].ToString()) ? null : row["Additional work code 2"].ToString();

                                mSALineItem1.Additional_work_code_3 = string.IsNullOrWhiteSpace(row["Additional work code 3"].ToString()) ? null : row["Additional work code 3"].ToString();
                                mSALineItem1.Additional_work_code_4 = string.IsNullOrWhiteSpace(row["Additional work code 4"].ToString()) ? null : row["Additional work code 4"].ToString();
                                mSALineItem1.Additional_work_code_5 = string.IsNullOrWhiteSpace(row["Additional work code 5"].ToString()) ? null : row["Additional work code 5"].ToString();
                                mSALineItem1.Work_sheet_flag = string.IsNullOrWhiteSpace(row["Work sheet flag"].ToString()) ? null : row["Work sheet flag"].ToString();
                                mSALineItem1.Work_sheet_Rev = string.IsNullOrWhiteSpace(row["Work sheet Rev"].ToString()) ? null : row["Work sheet Rev"].ToString();
                                mSALineItem1.Work_sheet_No_ = string.IsNullOrWhiteSpace(row["Work sheet No#"].ToString()) ? null : row["Work sheet No#"].ToString();
                                mSALineItem1.Freight_RSP__JPY_ = string.IsNullOrWhiteSpace(row["Freight RSP (JPY)"].ToString()) ? null : row["Freight RSP (JPY)"].ToString();
                                mSALineItem1.Freight_RSP__USD_ = string.IsNullOrWhiteSpace(row["Freight RSP (USD)"].ToString()) ? null : row["Freight RSP (USD)"].ToString();
                                mSALineItem1.Freight_RSP__EUR_ = string.IsNullOrWhiteSpace(row["Freight RSP (EUR)"].ToString()) ? null : row["Freight RSP (EUR)"].ToString();
                                mSALineItem1.Combined_MS_Code_Indicator = string.IsNullOrWhiteSpace(row["Combined MS-Code Indicator"].ToString()) ? null : row["Combined MS-Code Indicator"].ToString();
                                mSALineItem1.Combined_MS_Code_Control_Number = string.IsNullOrWhiteSpace(row["Combined MS-Code Control Number"].ToString()) ? null : row["Combined MS-Code Control Number"].ToString();
                                mSALineItem1.Comp__No_ = string.IsNullOrWhiteSpace(row["Comp# No#"].ToString()) ? null : row["Comp# No#"].ToString();
                                mSALineItem1.Order_Instruction_Title_code = string.IsNullOrWhiteSpace(row["Order Instruction Title code"].ToString()) ? null : row["Order Instruction Title code"].ToString();
                                mSALineItem1.Order_Instruction_Title = string.IsNullOrWhiteSpace(row["Order Instruction Title"].ToString()) ? null : row["Order Instruction Title"].ToString();
                                mSALineItem1.Input_Type = string.IsNullOrWhiteSpace(row["Input Type"].ToString()) ? null : row["Input Type"].ToString();
                                mSALineItem1.Min_ = string.IsNullOrWhiteSpace(row["Min#"].ToString()) ? null : row["Min#"].ToString();
                                mSALineItem1.Max_ = string.IsNullOrWhiteSpace(row["Max#"].ToString()) ? null : row["Max#"].ToString();

                                mSALineItem1.Unitno = string.IsNullOrWhiteSpace(row["Unitno"].ToString()) ? null : row["Unitno"].ToString();
                                mSALineItem1.Sensor = string.IsNullOrWhiteSpace(row["Sensor"].ToString()) ? null : row["Sensor"].ToString();
                                mSALineItem1.Factor = string.IsNullOrWhiteSpace(row["Factor"].ToString()) ? null : row["Factor"].ToString();
                                mSALineItem1.Feature = string.IsNullOrWhiteSpace(row["Feature"].ToString()) ? null : row["Feature"].ToString();
                                mSALineItem1.Free_Text = string.IsNullOrWhiteSpace(row["Free Text"].ToString()) ? null : row["Free Text"].ToString();
                                mSALineItem1.Inquiry_ID = string.IsNullOrWhiteSpace(row["Inquiry ID"].ToString()) ? null : row["Inquiry ID"].ToString();
                                mSALineItem1.Process_flag_INT_for_internal = string.IsNullOrWhiteSpace(row["Process flag INT for internal"].ToString()) ? null : row["Process flag INT for internal"].ToString();
                                mSALineItem1.paid = paids;
                                mSALineItem1.PAItemID = paitemid;


                                MSALineItem mSALineItem = obj.MSALineItems.Where(li => li.paid == paids && li.PAItemID == paitemid && li.deletionflag == false).FirstOrDefault();
                                if (mSALineItem == null)
                                {
                                    obj.MSALineItems.Add(mSALineItem1);
                                    obj.SaveChanges();

                                }
                                else
                                {
                                    //Delete than new reocrd insterd

                                    mSALineItem.deletionflag = true;
                                    obj.SaveChanges();
                                    obj.MSALineItems.Add(mSALineItem1);
                                    obj.SaveChanges();
                                }

                            }
                            status.StatusMesssage = "Successfull Inserted";
                            status.Sid = 1;
                            //output = "Successfull Inserted";
                            //result = 1;
                        }
                        else
                        {
                            status.StatusMesssage = "It is already confirmed. Please click on reset and process it again";
                            status.Sid = 2;

                        }
                        var MSAMasterConfirmation1 = obj.MSAMasterConfirmations.Where(li => li.PAID == paidCn && li.Deleteflag == false).FirstOrDefault();
                        if (MSAMasterConfirmation1 == null)
                        {
                            MSAMasterConfirmation mSAMasterConfirmation = new MSAMasterConfirmation();
                            mSAMasterConfirmation.Deleteflag = false;
                            mSAMasterConfirmation.Confirmationflag = false;
                            mSAMasterConfirmation.PAID = paidCn;
                            mSAMasterConfirmation.UploadedBy = Uploadby.ToString();
                            mSAMasterConfirmation.UplaodedDate = DateTime.Now;
                            obj.MSAMasterConfirmations.Add(mSAMasterConfirmation);
                            obj.SaveChanges();
                        }
                    }
                    else
                    {
                        status.StatusMesssage = outputMsg;
                        status.Sid = -2;

                        //output = outputMsg;
                        //result = -2;
                    }
                }
                catch (Exception e)
                {
                    status.StatusMesssage = "Error while upload.Please check with developer";
                    status.Sid = -1;
                    throw e;
                }
            }
            return Ok(status);
        }

        [HttpPost]
        [Route("UpdateMSAConfirmation")]
        public async Task<IHttpActionResult> UpdateMSAMasterConfirmation(MSAMasterConfirmation model)
        {
            //this.msalineitems(model);
            return Ok(await _paBusenessAcess.updateMSAConfirmation(model));
        }
        [HttpPost]
        [Route("msalineitems")]
        public bool msalineitems(MSAMasterConfirmation model)
        {
            string sourcePath = @"C:\Users\developer4\Desktop";
            string targetpath = ConfigurationManager.AppSettings["DownloadVexcel"];
            string srcfilename = "finalmsa.xls";
            string targetfilename = "MSA" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls";
            string sourceFile = System.IO.Path.Combine(sourcePath, srcfilename);
            string destFile = System.IO.Path.Combine(targetpath, targetfilename);
            if (!System.IO.Directory.Exists(targetpath))
            {
                System.IO.Directory.CreateDirectory(targetpath);
            }
            System.IO.File.Copy(sourceFile, destFile, false);
            Microsoft.Office.Interop.Excel._Application docExcel = new Microsoft.Office.Interop.Excel.Application();
            //docExcel.ActiveWorkbook.Sheets[1].Activate();
            //docExcel.Sheets[1].Activate();
            // docExcel.ActiveSheet;
            docExcel.Visible = false;
            docExcel.DisplayAlerts = false;
            //xlWorkSheet = (Worksheet)xlWorkBook.Sheets["SheetName"];
            //Microsoft.Office.Interop.Excel._Workbook workbooksExcel = docExcel.Workbooks.Open(destFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlCorruptLoad.xlExtractData);
            Microsoft.Office.Interop.Excel._Workbook workbooksExcel = docExcel.Workbooks.Open(destFile, ReadOnly: true, Password: "Feb@1234Feb@1234");

            Microsoft.Office.Interop.Excel._Worksheet worksheetExcel = (Microsoft.Office.Interop.Excel._Worksheet)workbooksExcel.ActiveSheet;
            //Microsoft.Office.Interop.Excel._Worksheet worksheetExcel = workbooksExcel.Worksheets[3] as Microsoft.Office.Interop.Excel.Worksheet;
            Microsoft.Office.Interop.Excel.Range range = worksheetExcel.UsedRange;
            YSCMEntities obj = new YSCMEntities();
            var msadata = obj.RPALoadItemForMSAs.Where(x => x.paid == model.PAID).ToList();
            int i = 2;
            foreach (var item in msadata)
            {
                if (item.PAItemID != null)
                {
                    if (!string.IsNullOrEmpty(item.Item_No_))
                        (range.Worksheet.Cells["2", "A"]).Value2 = item.Item_No_;
                    else
                        (range.Worksheet.Cells["2", "A"]).Value2 = 0;

                    if (!string.IsNullOrEmpty(item.deletionflag))
                        (range.Worksheet.Cells["2", "B"]).Value2 = item.deletionflag;
                    else
                        (range.Worksheet.Cells["2", "B"]).Value2 = 0;

                    if (!string.IsNullOrEmpty(item.mscode))
                        (range.Worksheet.Cells["2", "C"]).Value2 = item.mscode;
                    else
                        (range.Worksheet.Cells["2", "C"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.ItemDescription))
                    //    (range.Worksheet.Cells[i, "D"]).Value2 = item.ItemDescription;
                    //else
                    //    (range.Worksheet.Cells[i, "D"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.WBS))
                    //    (range.Worksheet.Cells[i, "E"]).Value2 = item.WBS;
                    //else
                    //    (range.Worksheet.Cells[i, "E"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.wbsdesc))
                    //    (range.Worksheet.Cells[i, "F"]).Value2 = item.wbsdesc;
                    //else
                    //    (range.Worksheet.Cells[i, "F"]).Value2 = 0;

                    //if (item.Quantity!=0)
                    //    (range.Worksheet.Cells[i, "G"]).Value2 = item.Quantity;
                    //else
                    //    (range.Worksheet.Cells[i, "G"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.unit))
                    //    (range.Worksheet.Cells[i, "H"]).Value2 = item.unit;
                    //else
                    //    (range.Worksheet.Cells[i, "H"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.RequirementDate))
                    //    (range.Worksheet.Cells[i, "I"]).Value2 = item.RequirementDate;
                    //else
                    //    (range.Worksheet.Cells[i, "I"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.Project))
                    //    (range.Worksheet.Cells[i, "J"]).Value2 = item.Project;
                    //else
                    //    (range.Worksheet.Cells[i, "J"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.projectdesc))
                    //    (range.Worksheet.Cells[i, "K"]).Value2 = item.projectdesc;
                    //else
                    //    (range.Worksheet.Cells[i, "K"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.TokuchuNo))
                    //    (range.Worksheet.Cells[i, "L"]).Value2 = item.TokuchuNo;
                    //else
                    //    (range.Worksheet.Cells[i, "L"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.bupfl))
                    //    (range.Worksheet.Cells[i, "N"]).Value2 = item.bupfl;
                    //else
                    //    (range.Worksheet.Cells[i, "N"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.bupfldesc))
                    //    (range.Worksheet.Cells[i, "O"]).Value2 = item.bupfldesc;
                    //else
                    //    (range.Worksheet.Cells[i, "O"]).Value2 = 0;

                    //if (item.UnitPrice!=0)
                    //    (range.Worksheet.Cells[i, "P"]).Value2 = item.UnitPrice;
                    //else
                    //    (range.Worksheet.Cells[i, "P"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.Currency))
                    //    (range.Worksheet.Cells[i, "Q"]).Value2 = item.Currency;
                    //else
                    //    (range.Worksheet.Cells[i, "Q"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.priceunit))
                    //    (range.Worksheet.Cells[i, "R"]).Value2 = item.priceunit;
                    //else
                    //    (range.Worksheet.Cells[i, "R"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.costelement))
                    //    (range.Worksheet.Cells[i, "S"]).Value2 = item.costelement;
                    //else
                    //    (range.Worksheet.Cells[i, "S"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.costelementdesc))
                    //    (range.Worksheet.Cells[i, "T"]).Value2 = item.costelementdesc;
                    //else
                    //    (range.Worksheet.Cells[i, "T"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.plant))
                    //    (range.Worksheet.Cells[i, "U"]).Value2 = item.plant;
                    //else
                    //    (range.Worksheet.Cells[i, "U"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.plantname))
                    //    (range.Worksheet.Cells[i, "V"]).Value2 = item.plantname;
                    //else
                    //    (range.Worksheet.Cells[i, "V"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.StorageLocation))
                    //    (range.Worksheet.Cells[i, "W"]).Value2 = item.StorageLocation;
                    //else
                    //    (range.Worksheet.Cells[i, "W"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.storagelocationname))
                    //    (range.Worksheet.Cells[i, "X"]).Value2 = item.storagelocationname;
                    //else
                    //    (range.Worksheet.Cells[i, "X"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.VendorCode))
                    //    (range.Worksheet.Cells[i, "Y"]).Value2 = item.VendorCode;
                    //else
                    //    (range.Worksheet.Cells[i, "Y"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.VendorName))
                    //    (range.Worksheet.Cells[i, "Z"]).Value2 = item.VendorName;
                    //else
                    //    (range.Worksheet.Cells[i, "Z"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.VendorModelNo))
                    //    (range.Worksheet.Cells[i, "AA"]).Value2 = item.VendorModelNo;
                    //else
                    //    (range.Worksheet.Cells[i, "AA"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.sortstring1))
                    //    (range.Worksheet.Cells[i, "AB"]).Value2 = item.sortstring1;
                    //else
                    //    (range.Worksheet.Cells[i, "AB"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.note1))
                    //    (range.Worksheet.Cells[i, "AD"]).Value2 = item.note1;
                    //else
                    //    (range.Worksheet.Cells[i, "AD"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.note2.ToString()))
                    //    (range.Worksheet.Cells[i, "AE"]).Value2 = item.note2;
                    //else
                    //    (range.Worksheet.Cells[i, "AE"]).Value2 = 0;

                    //if (item.note3!=0)
                    //    (range.Worksheet.Cells[i, "AF"]).Value2 = item.note3;
                    //else
                    //    (range.Worksheet.Cells[i, "AF"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.note4))
                    //    (range.Worksheet.Cells[i, "AG"]).Value2 = item.note4;
                    //else
                    //    (range.Worksheet.Cells[i, "AG"]).Value2 = 0;

                    //if (!string.IsNullOrEmpty(item.lt))
                    //    (range.Worksheet.Cells[i, "AH"]).Value2 = item.lt;
                    //else
                    //    (range.Worksheet.Cells[i, "AH"]).Value2 = 0;
                }
                workbooksExcel.SaveAs("msaupdate.xls", "D:\\msalineitem");
                //workbooksExcel.Close(false, Type.Missing, Type.Missing);
                //docExcel.Application.DisplayAlerts = true;
                //docExcel.Application.Quit();
            }
            return true;
        }
        [HttpPost]
        [Route("ClearMSAConfirmation")]
        public async Task<IHttpActionResult> ClearMSAMasterConfirmation(MSAMasterConfirmation model)
        {
            return Ok(await _paBusenessAcess.ClearMSAConfirmation(model));
        }

        private bool IsPaIdMatch(int paid, DataTable dtexcel, out string errorMsg)
        {
            bool result = false;
            try
            {
                foreach (DataRow row in dtexcel.Rows)
                {
                    if (paid != Convert.ToInt32(row["paid"]))
                    {
                        errorMsg = "PAID mismatched with excel data";
                        return false;
                    }
                    if (string.IsNullOrEmpty(row["Item No#"].ToString()))
                    {
                        errorMsg = "Item No should not be Empty in excel sheet";
                        return false;
                    }
                    if (string.IsNullOrEmpty(row["mscode"].ToString()))
                    {
                        errorMsg = "MS Code should not be Empty in excel sheet"; ;
                        return false;
                    }
                    if (string.IsNullOrEmpty(row["ItemDescription"].ToString()))
                    {
                        errorMsg = "Item Description should not be Empty in excel sheet";
                        return false;
                    }
                    if (string.IsNullOrEmpty(row["WBS"].ToString()))
                    {
                        errorMsg = "WBS should not be Empty in excel sheet"; ;
                        return false;
                    }
                    if (string.IsNullOrEmpty(row["Quantity"].ToString()))
                    {
                        errorMsg = "Quantity should not be Empty in excel sheet"; ;
                        return false;
                    }
                    if (string.IsNullOrEmpty(row["RequirementDate"].ToString()))
                    {
                        errorMsg = "Requirement Date should not be Empty in excel sheet"; ;
                        return false;
                    }

                    if (string.IsNullOrEmpty(row["Project"].ToString()))
                    {
                        errorMsg = "Project should not be Empty in excel sheet"; ;
                        return false;
                    }
                    if (string.IsNullOrEmpty(row["UnitPrice"].ToString()))
                    {
                        errorMsg = "Unit Price should not be Empty in excel sheet"; ;
                        return false;
                    }
                    if (string.IsNullOrEmpty(row["Currency"].ToString()))
                    {
                        errorMsg = "Currency should not be Empty in excel sheet"; ;
                        return false;
                    }
                    if (string.IsNullOrEmpty(row["StorageLocation"].ToString()))
                    {
                        errorMsg = "Storage Location should not be Empty in excel sheet"; ;
                        return false;
                    }
                    if (string.IsNullOrEmpty(row["text"].ToString()))
                    {
                        errorMsg = "Text should not be Empty in excel sheet"; ;
                        return false;
                    }
                    if (string.IsNullOrEmpty(row["TokuchuNo"].ToString()))
                    {
                        errorMsg = "Tokuchu No should not be Empty in excel sheet"; ;
                        return false;
                    }
                    if (string.IsNullOrEmpty(row["PAItemID"].ToString()))
                    {
                        errorMsg = "PA Item ID should not be Empty in excel sheet"; ;
                        return false;
                    }
                    else
                    {
                        errorMsg = "No validation error";
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            errorMsg = null;
            return result;
        }
        [HttpPost]
        [Route("Unmappingitem")]
        [ResponseType(typeof(statuscheckmodel))]
        public async Task<IHttpActionResult> Unmappingitem(MappingItemModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            status = await _paBusenessAcess.Unmappingitem(model);
            return Ok(status);
        }
        //[HttpGet]
        //[Route("Downloadexcel/{revisionid}")]
        //public HttpResponseMessage Downloadexcel(int revisionid)
        //{
        //    try
        //    {
        //        string sourcePath = "C:\\Users\\developer4\\Desktop\\test\\";
        //        string targetpath = ConfigurationManager.AppSettings["DownloadVexcel"];
        //        string srcfilename = "Book1.xlsx";
        //        string targetfilename = "Akil" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
        //        string sourceFile = System.IO.Path.Combine(sourcePath, srcfilename);
        //        string destFile = System.IO.Path.Combine(targetpath, targetfilename);
        //        if (!System.IO.Directory.Exists(targetpath))
        //        {
        //            System.IO.Directory.CreateDirectory(targetpath);
        //        }
        //        System.IO.File.Copy(sourceFile, destFile, false);
        //        Microsoft.Office.Interop.Excel._Application docExcel = new Microsoft.Office.Interop.Excel.Application();
        //        docExcel.Visible = false;
        //        docExcel.DisplayAlerts = false;
        //        Microsoft.Office.Interop.Excel._Workbook workbooksExcel = docExcel.Workbooks.Open(destFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //        Microsoft.Office.Interop.Excel._Worksheet worksheetExcel = (Microsoft.Office.Interop.Excel._Worksheet)workbooksExcel.ActiveSheet;

        //        YSCMEntities obj = new YSCMEntities();
        //        var data = obj.RfqForVendorDownloads.Where(x => x.rfqRevisionId == revisionid).ToList();
        //        Microsoft.Office.Interop.Excel.Range range = worksheetExcel.UsedRange;
        //        foreach (var item in data)
        //        {
        //            (range.Worksheet.Cells["1", "F"]).Value2 = item.RFQNo;
        //            (range.Worksheet.Cells["1", "L"]).Value2 = item.rfqRevisionId;
        //            (range.Worksheet.Cells["2", "F"]).Value2 = item.RFQValidDate;
        //            (range.Worksheet.Cells["2", "L"]).Value2 = item.ReqRemarks;
        //            (range.Worksheet.Cells["3", "J"]).Value2 = item.RFQValidDate;
        //        }
        //        //line items

        //        int i = 6;
        //        foreach (var item1 in data)
        //        {
        //            (range.Worksheet.Cells[i, "A"]).Value2 = item1.RFQItemsId;
        //            (range.Worksheet.Cells[i, "B"]).Value2 = item1.ItemId;
        //            (range.Worksheet.Cells[i, "C"]).Value2 = item1.ItemDescription;
        //            (range.Worksheet.Cells[i, "D"]).Value2 = item1.QuotationQty;
        //            if (item1.UOM != null)
        //                (range.Worksheet.Cells[i, "E"]).Value2 = item1.UOM;
        //            else
        //                (range.Worksheet.Cells[i, "E"]).Value2 = "";
        //            (range.Worksheet.Cells[i, "F"]).Value2 = item1.CurrencyValue;
        //            (range.Worksheet.Cells[i, "G"]).Value2 = item1.UnitPrice;
        //            (range.Worksheet.Cells[i, "H"]).Value2 = item1.UnitPrice;
        //            (range.Worksheet.Cells[i, "I"]).Value2 = item1.DiscountPercentage;
        //            (range.Worksheet.Cells[i, "J"]).Value2 = item1.Discount;
        //            (range.Worksheet.Cells[i, "K"]).Value2 = item1.VendorModelNo;
        //            (range.Worksheet.Cells[i, "L"]).Value2 = item1.MfgPartNo;
        //            (range.Worksheet.Cells[i, "M"]).Value2 = item1.MfgModelNo;
        //            (range.Worksheet.Cells[i, "N"]).Value2 = item1.ManufacturerName;
        //            (range.Worksheet.Cells[i, "O"]).Value2 = item1.CGSTPercentage;
        //            (range.Worksheet.Cells[i, "P"]).Value2 = item1.IGSTPercentage;
        //            (range.Worksheet.Cells[i, "Q"]).Value2 = item1.SGSTPercentage;
        //            (range.Worksheet.Cells[i, "R"]).Value2 = item1.PFAmount;
        //            (range.Worksheet.Cells[i, "S"]).Value2 = item1.PFPercentage;
        //            (range.Worksheet.Cells[i, "T"]).Value2 = item1.FreightAmount;
        //            (range.Worksheet.Cells[i, "U"]).Value2 = item1.FreightPercentage;
        //            if (!string.IsNullOrEmpty(item1.DeliveryDate.ToString()))
        //                (range.Worksheet.Cells[i, "V"]).Value2 = Convert.ToDateTime(item1.DeliveryDate).ToString("dd/mm/yyyy");
        //            (range.Worksheet.Cells[i, "W"]).Value2 = item1.Remarks;
        //            (range.Worksheet.Cells[i, "X"]).Value2 = item1.RFQSplitItemId;
        //            i++;
        //        }
        //        workbooksExcel.Save();
        //        workbooksExcel.Close(false, Type.Missing, Type.Missing);
        //        docExcel.Application.DisplayAlerts = true;
        //        docExcel.Application.Quit();
        //        return getGenetatedExcel(destFile);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        [HttpGet]
        [Route("Downloadexcel/{revisionid}")]
        public HttpResponseMessage Downloadexcel(int revisionid)
        {

            try
            {
                //Microsoft.Office.Interop.Excel.Application xlApp = new
                //string sourcePath = "C:\\Users\\developer4\\Desktop\\";
                string sourcePath = ConfigurationManager.AppSettings["RFQTemplate"];
                string targetpath = ConfigurationManager.AppSettings["DownloadVexcel"];
                string srcfilename = "RFQDownloadTemplate.xlsx";
                string targetfilename = "RFQ" + DateTime.Now.ToString("ddMMyyyyhhmmss") + revisionid + ".xlsx";
                string sourceFile = System.IO.Path.Combine(sourcePath, srcfilename);
                string destFile = System.IO.Path.Combine(targetpath, targetfilename);
                if (!System.IO.Directory.Exists(targetpath))
                {
                    System.IO.Directory.CreateDirectory(targetpath);
                }
                System.IO.File.Copy(sourceFile, destFile, false);
                try
                {
                    Microsoft.Office.Interop.Excel.Application docExcel = new Microsoft.Office.Interop.Excel.Application();
                    docExcel.Visible = false;
                    docExcel.DisplayAlerts = false;
                    //Microsoft.Office.Interop.Excel._Workbook workbooksExcel = docExcel.Workbooks.Open(destFile);
                    Microsoft.Office.Interop.Excel.Workbook workbooksExcel = docExcel.Workbooks.Open(destFile, ReadOnly: false);
                    //Microsoft.Office.Interop.Excel._Workbook workbooksExcel = docExcel.Workbooks.Open(destFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //Microsoft.Office.Interop.Excel._Workbook workbooksExcel = docExcel.Workbooks.Open(destFile, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "",true, false, 0, true, false, false);
                    Microsoft.Office.Interop.Excel.Worksheet worksheetExcel = (Microsoft.Office.Interop.Excel.Worksheet)workbooksExcel.ActiveSheet;
                    Microsoft.Office.Interop.Excel.Range range = worksheetExcel.UsedRange;
                    YSCMEntities obj = new YSCMEntities();
                    var data = obj.RfqForVendorDownloads.Where(x => x.rfqRevisionId == revisionid).ToList();
                    foreach (var item in data)
                    {
                        if (!string.IsNullOrEmpty(item.RFQNo))
                            (range.Worksheet.Cells["2", "G"]).Value2 = item.RFQNo;
                        if (!string.IsNullOrEmpty(item.rfqRevisionId.ToString()))
                            (range.Worksheet.Cells["2", "O"]).Value2 = item.rfqRevisionId;
                        if (!string.IsNullOrEmpty(item.RFQValidDate.ToString()))
                            (range.Worksheet.Cells["3", "G"]).Value2 = item.RFQValidDate;
                        if (!string.IsNullOrEmpty(item.ReqRemarks))
                            (range.Worksheet.Cells["3", "L"]).Value2 = item.ReqRemarks;
                        if (!string.IsNullOrEmpty(item.RFQValidDate.ToString()))
                            (range.Worksheet.Cells["2", "L"]).Value2 = item.RFQValidDate;
                    }

                    int i = 6;
                    foreach (var item1 in data)
                    {
                        if (!string.IsNullOrEmpty(item1.RFQItemsId.ToString()))
                            (range.Worksheet.Cells[i, "A"]).Value2 = item1.RFQItemsId;
                        else
                            (range.Worksheet.Cells[i, "A"]).Value2 = 0;

                        (range.Worksheet.Cells[i, "B"]).Value2 = i - 5;

                        if (!string.IsNullOrEmpty(item1.ItemId.ToString()))
                            (range.Worksheet.Cells[i, "C"]).Value2 = item1.ItemId;
                        else
                            (range.Worksheet.Cells[i, "C"]).Value2 = "";

                        if (!string.IsNullOrEmpty(item1.ItemDescription))
                            (range.Worksheet.Cells[i, "D"]).Value2 = item1.ItemDescription;
                        else
                            (range.Worksheet.Cells[i, "D"]).Value2 = "";

                        if (!string.IsNullOrEmpty(item1.QuotationQty.ToString()))
                            (range.Worksheet.Cells[i, "E"]).Value2 = item1.QuotationQty;
                        else
                            (range.Worksheet.Cells[i, "E"]).Value2 = 0;

                        if (item1.UOM != null)
                            (range.Worksheet.Cells[i, "F"]).Value2 = item1.UOM;
                        else
                            (range.Worksheet.Cells[i, "F"]).Value2 = 0;

                        if (!string.IsNullOrEmpty(item1.CurrencyValue.ToString()))
                            (range.Worksheet.Cells[i, "G"]).Value2 = item1.CurrencyValue;
                        else
                            (range.Worksheet.Cells[i, "G"]).Value2 = 0;

                        if (!string.IsNullOrEmpty(item1.UnitPrice.ToString()))
                            (range.Worksheet.Cells[i, "H"]).Value2 = item1.UnitPrice;
                        else
                            (range.Worksheet.Cells[i, "H"]).Value2 = 0;

                        //if (item1.HSNCode != null)
                        //    (range.Worksheet.Cells[i, "I"]).Value2 = item1.HSNCode;
                        //else
                        //    (range.Worksheet.Cells[i, "I"]).Value2 = 0;

                        if (!string.IsNullOrEmpty(item1.DiscountPercentage.ToString()))
                            (range.Worksheet.Cells[i, "J"]).Value2 = item1.DiscountPercentage;
                        else
                            (range.Worksheet.Cells[i, "J"]).Value2 = 0;

                        if (!string.IsNullOrEmpty(item1.Discount.ToString()))
                            (range.Worksheet.Cells[i, "K"]).Value2 = item1.Discount;
                        else
                            (range.Worksheet.Cells[i, "K"]).Value2 = 0;

                        if (!string.IsNullOrEmpty(item1.VendorModelNo))
                            (range.Worksheet.Cells[i, "L"]).Value2 = item1.VendorModelNo;
                        else
                            (range.Worksheet.Cells[i, "L"]).Value2 = "";

                        if (!string.IsNullOrEmpty(item1.MfgPartNo))
                            (range.Worksheet.Cells[i, "M"]).Value2 = item1.MfgPartNo;
                        else
                            (range.Worksheet.Cells[i, "M"]).Value2 = "";

                        if (!string.IsNullOrEmpty(item1.MfgModelNo))
                            (range.Worksheet.Cells[i, "N"]).Value2 = item1.MfgModelNo;
                        else
                            (range.Worksheet.Cells[i, "N"]).Value2 = "";

                        if (!string.IsNullOrEmpty(item1.ManufacturerName))
                            (range.Worksheet.Cells[i, "O"]).Value2 = item1.ManufacturerName;
                        else
                            (range.Worksheet.Cells[i, "O"]).Value2 = "";

                        if (!string.IsNullOrEmpty(item1.CGSTPercentage.ToString()))
                            (range.Worksheet.Cells[i, "P"]).Value2 = item1.CGSTPercentage;
                        else
                            (range.Worksheet.Cells[i, "P"]).Value2 = 0;

                        if (!string.IsNullOrEmpty(item1.IGSTPercentage.ToString()))
                            (range.Worksheet.Cells[i, "Q"]).Value2 = item1.IGSTPercentage;
                        else
                            (range.Worksheet.Cells[i, "Q"]).Value2 = 0;

                        if (!string.IsNullOrEmpty(item1.SGSTPercentage.ToString()))
                            (range.Worksheet.Cells[i, "R"]).Value2 = item1.SGSTPercentage;
                        else
                            (range.Worksheet.Cells[i, "R"]).Value2 = 0;

                        if (!string.IsNullOrEmpty(item1.PFAmount.ToString()))
                            (range.Worksheet.Cells[i, "S"]).Value2 = item1.PFAmount;
                        else
                            (range.Worksheet.Cells[i, "S"]).Value2 = 0;

                        if (!string.IsNullOrEmpty(item1.PFPercentage.ToString()))
                            (range.Worksheet.Cells[i, "T"]).Value2 = item1.PFPercentage;
                        else
                            (range.Worksheet.Cells[i, "T"]).Value2 = 0;

                        if (!string.IsNullOrEmpty(item1.FreightAmount.ToString()))
                            (range.Worksheet.Cells[i, "U"]).Value2 = item1.FreightAmount;
                        else
                            (range.Worksheet.Cells[i, "U"]).Value2 = 0;

                        if (!string.IsNullOrEmpty(item1.FreightPercentage.ToString()))
                            (range.Worksheet.Cells[i, "V"]).Value2 = item1.FreightPercentage;
                        else
                            (range.Worksheet.Cells[i, "V"]).Value2 = 0;

                        if (!string.IsNullOrEmpty(item1.DeliveryDate.ToString()))
                        {
                            string date = Convert.ToDateTime(item1.DeliveryDate).Day.ToString() + "/" + Convert.ToDateTime(item1.DeliveryDate).Month.ToString() + "/" + Convert.ToDateTime(item1.DeliveryDate).Year.ToString();
                            (range.Worksheet.Cells[i, "W"]).Value2 = date;
                        }
                        else
                            (range.Worksheet.Cells[i, "W"]).Value2 = "";

                        if (item1.Remarks != null && !string.IsNullOrEmpty(item1.Remarks))
                            (range.Worksheet.Cells[i, "X"]).Value2 = item1.Remarks;
                        else
                            (range.Worksheet.Cells[i, "X"]).Value2 = "";
                        //if (!string.IsNullOrEmpty(item1.RFQSplitItemId.ToString()))
                        //    (range.Worksheet.Cells[i, "Y"]).Value2 = item1.RFQSplitItemId;
                        //else
                        //    (range.Worksheet.Cells[i, "Y"]).Value2 = 0;
                        i++;
                    }
                    workbooksExcel.Save();
                    workbooksExcel.Close(false, Type.Missing, Type.Missing);
                    docExcel.Application.DisplayAlerts = true;
                    docExcel.Application.Quit();
                }
                catch (Exception ex)
                {

                    log.ErrorMessage("PAController", "uploadexcel", ex.Message + "; " + ex.StackTrace.ToString());
                }




                //range.Worksheet.Protect("password", Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);


                //return webResponse.StatusCode.ToString();
                return getGenetatedExcel(destFile);
                //return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetPolineItemsToExcel/{revisionid}")]
        public HttpResponseMessage GetPolineItemsToExcel(int revisionid)
        {
            try
            {
                string destFile = "";
                //string sourcePath = "C:\\Users\\developer4\\Desktop\\";
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                try
                {
                    Microsoft.Office.Interop.Excel.Workbook excelBook = xlApp.Workbooks.Open("C:\\POLineItemTemplate\\POItemTemp.xlsx");

                    String[] excelSheets = new String[excelBook.Worksheets.Count];
                    int j = 0;

                    string sourcePath = ConfigurationManager.AppSettings["POLineItemTemplate"];
                    string targetpath = ConfigurationManager.AppSettings["DownloadPoitems"];
                    string srcfilename = "POItemTemp.xlsx";
                    string targetfilename = "PONO" + DateTime.Now.ToString("ddMMyyyyhhmmss") + revisionid + ".xlsx";
                    string sourceFile = System.IO.Path.Combine(sourcePath, srcfilename);
                    destFile = System.IO.Path.Combine(targetpath, targetfilename);
                    if (!System.IO.Directory.Exists(targetpath))
                    {
                        System.IO.Directory.CreateDirectory(targetpath);
                    }
                    System.IO.File.Copy(sourceFile, destFile, false);

                    Microsoft.Office.Interop.Excel._Application docExcel = new Microsoft.Office.Interop.Excel.Application();
                    docExcel.Visible = false;
                    docExcel.DisplayAlerts = false;

                    Microsoft.Office.Interop.Excel._Workbook workbooksExcel = docExcel.Workbooks.Open(destFile, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    Microsoft.Office.Interop.Excel._Worksheet worksheetExcel = (Microsoft.Office.Interop.Excel._Worksheet)workbooksExcel.ActiveSheet;

                    string varienttype = "";
                    YSCMEntities obj = new YSCMEntities();
                    var data = obj.PoLineItemstoExcels.Where(x => x.POID == revisionid).ToList();
                    Microsoft.Office.Interop.Excel.Range range = worksheetExcel.UsedRange;
                    foreach (Microsoft.Office.Interop.Excel.Worksheet wSheet in excelBook.Worksheets)
                    {
                        excelSheets[j] = wSheet.Name;
                        j++;
                        if (excelSheets[0] == "POHeader")
                        {
                            var ws = workbooksExcel.Worksheets;
                            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)ws.get_Item("POHeader");
                            range = worksheet.UsedRange;
                            foreach (var item in data)
                            {
                                if (!string.IsNullOrEmpty(item.DocumentNo))
                                    (range.Worksheet.Cells["2", "A"]).Value2 = item.DocumentNo;
                                if (!string.IsNullOrEmpty(item.PAId.ToString()))
                                    (range.Worksheet.Cells["2", "B"]).Value2 = item.PAId;
                                if (!string.IsNullOrEmpty(item.potype))
                                    (range.Worksheet.Cells["2", "C"]).Value2 = item.potype;
                                if (item.collectiveno != 0)
                                    (range.Worksheet.Cells["2", "D"]).Value2 = item.collectiveno;
                                if (!string.IsNullOrEmpty(item.PaymentTermCode))
                                    (range.Worksheet.Cells["2", "E"]).Value2 = item.PaymentTermCode;
                                if (!string.IsNullOrEmpty(item.incoterms))
                                    (range.Worksheet.Cells["2", "F"]).Value2 = item.incoterms;
                                //if (!string.IsNullOrEmpty(item.pono))
                                //    (range.Worksheet.Cells["2", "G"]).Value2 = item.pono;
                                if (item.poterms != null)
                                    (range.Worksheet.Cells["2", "I"]).Value2 = item.poterms;
                                if (!string.IsNullOrEmpty(item.poinsurance))
                                    (range.Worksheet.Cells["2", "K"]).Value2 = item.poinsurance;
                                if (!string.IsNullOrEmpty(item.Remarks))
                                    (range.Worksheet.Cells["2", "J"]).Value2 = item.Remarks;

                                varienttype = item.Remarks;
                            }
                        }
                        int i = 2;
                        int pono = 10;
                        if (excelSheets[1] == "POLineItems")
                        {
                            var ws = workbooksExcel.Worksheets;
                            var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)ws.get_Item("POLineItems");
                            range = worksheet.UsedRange;
                            foreach (var item in data)
                            {
                                if (item.PAItemID != 0)
                                    (range.Worksheet.Cells[i, "B"]).Value2 = item.PAItemID;
                                if (!string.IsNullOrEmpty(item.POItemType))
                                    (range.Worksheet.Cells[i, "C"]).Value2 = item.POItemType;
                                if (!string.IsNullOrEmpty(item.PRno))
                                    (range.Worksheet.Cells[i, "D"]).Value2 = item.PRno;
                                if (!string.IsNullOrEmpty(item.PRLineItemNo))
                                    (range.Worksheet.Cells[i, "E"]).Value2 = item.PRLineItemNo;
                                if (item.UnitPrice >= 0)
                                    (range.Worksheet.Cells[i, "F"]).Value2 = item.UnitPrice;
                                if (!string.IsNullOrEmpty(item.purchasetype))
                                    (range.Worksheet.Cells[i, "G"]).Value2 = item.purchasetype;
                                if (!string.IsNullOrEmpty(item.incoterms))
                                    (range.Worksheet.Cells[i, "H"]).Value2 = "Y001";
                                if (!string.IsNullOrEmpty(item.PriorVendor))
                                    (range.Worksheet.Cells[i, "I"]).Value2 = item.PriorVendor;
                                if (!string.IsNullOrEmpty(item.ABConfirmation))
                                    (range.Worksheet.Cells[i, "J"]).Value2 = item.ABConfirmation;

                                (range.Worksheet.Cells[i, "K"]).Value2 = pono;

                                if (!string.IsNullOrEmpty(item.ProjectDefinition))
                                    (range.Worksheet.Cells[i, "L"]).Value2 = item.ProjectDefinition;
                                if (!string.IsNullOrEmpty(item.WBS))
                                    (range.Worksheet.Cells[i, "M"]).Value2 = item.WBS;
                                //if (DateTime.(item.Reqdeliverydate))
                                //    (range.Worksheet.Cells[i, "M"]).Value2 = item.Reqdeliverydate;
                                if (item.Quantity != 0)
                                    (range.Worksheet.Cells[i, "O"]).Value2 = item.Quantity;

                                if (!string.IsNullOrEmpty(item.ItemDescription))
                                    (range.Worksheet.Cells[i, "Q"]).Value2 = item.ItemDescription;

                                (range.Worksheet.Cells[i, "N"]).Value2 = item.Reqdeliverydate.ToString();
                                if (varienttype == "Material")
                                    (range.Worksheet.Cells[i, "H"]).Value2 = "Y001";
                                else
                                    (range.Worksheet.Cells[i, "H"]).Value2 = "Y002";
                                //if (!string.IsNullOrEmpty(item.tex))
                                //    (range.Worksheet.Cells["2", "G"]).Value2 = item.pono;
                                i++;

                                pono = pono + 10;
                            }
                        }
                    }

                    workbooksExcel.Save();
                    workbooksExcel.Close(false, Type.Missing, Type.Missing);
                    docExcel.Application.DisplayAlerts = true;
                    docExcel.Application.Quit();
                    //return getGenetatedExcel(destFile);
                }
                catch (Exception ex)
                {
                    log.ErrorMessage("PAController", "GetPolineItemsToExcel", ex.Message + "; " + ex.StackTrace.ToString());
                }

                return getGenetatedExcel(destFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public HttpResponseMessage getGenetatedExcel(string filepath)
        {
            var path = filepath;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            result.Content = new StreamContent(stream);
            //result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            return (result);
        }


        /*
            Name of Function : <<UploadRfqData>>  Author :<<Fayaz>>  
            Date of Creation <<7-2-2021>>
            Purpose : <<Loading and Updating RfqData through excel>>
            Review Date :<<>>   Reviewed By :<<>>
            Version : 0.1 <change version only if there is major change - new release etc>
            Sourcecode Copyright : Yokogawa India Limited
       */
        [HttpPost]
        [Route("UploadRfqData/{revisionId}")]
        public IHttpActionResult UploadRfqData(int revisionId)
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var serverPath = HttpContext.Current.Server.MapPath("~/SCMDocs");
                string parsedFileName = "";
                if (httpRequest.Files.Count > 0)
                {
                    var Id = httpRequest.Files.AllKeys[0];
                    var postedFile = httpRequest.Files[0];
                    parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + Id + "\\" + ToValidFileName(postedFile.FileName));
                    serverPath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + Id;
                    var filePath = Path.Combine(serverPath, ToValidFileName(postedFile.FileName));
                    if (!Directory.Exists(serverPath))
                        Directory.CreateDirectory(serverPath);
                    postedFile.SaveAs(filePath);


                    DataTable dtexcel = new DataTable();
                    bool hasHeaders = false;
                    string HDR = hasHeaders ? "Yes" : "No";
                    string strConn;
                    if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
                    else
                        strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";

                    OleDbConnection conn = new OleDbConnection(strConn);
                    conn.Open();
                    DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                    DataRow schemaRow = schemaTable.Rows[0];
                    string sheet = schemaRow["TABLE_NAME"].ToString();
                    if (!sheet.EndsWith("_"))
                    {
                        string query = "SELECT  * FROM [Sheet1$]";
                        OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                        dtexcel.Locale = CultureInfo.CurrentCulture;
                        daexcel.Fill(dtexcel);
                    }

                    conn.Close();
                    int iSucceRows = 0;
                    YSCMEntities obj = new YSCMEntities();
                    VSCMEntities remoteObj = new VSCMEntities();

                    int i = 0;
                    string errorMessages = "";
                    foreach (DataRow row in dtexcel.Rows)
                    {

                        if (i > 3 && row[0] != null && !string.IsNullOrEmpty((row[0]).ToString()))
                        {
                            if (string.IsNullOrEmpty(row[7].ToString()))
                            {
                                return Ok(new SCMModels.Models.ServerSideValidation { ErrorMessage = "Unit Price Cannot be Empty At Line -" + i + " Cell No 7", ValidData = false });
                            }

                            if (string.IsNullOrEmpty(row[8].ToString()))
                            {
                                return Ok(new SCMModels.Models.ServerSideValidation { ErrorMessage = "HSN Code Cannot be Empty At Line - " + i + "Cell No 8", ValidData = false });
                            }
                            if (string.IsNullOrEmpty(row[16].ToString()))
                            {
                                if (string.IsNullOrEmpty(row[15].ToString()) && string.IsNullOrEmpty(row[17].ToString()))
                                    return Ok(new SCMModels.Models.ServerSideValidation { ErrorMessage = "CGST or SGST Percentage Cannot be Empty At Line - " + i + "Cell No 15 or 17", ValidData = false });

                            }


                            int itemsid = Convert.ToInt32(row[0]);
                            RemoteRFQItems_N remoteRFQItems_N = remoteObj.RemoteRFQItems_N.Where(x => x.RFQItemsId == itemsid).FirstOrDefault();
                            remoteRFQItems_N.ItemId = row[2].ToString();
                            if (!string.IsNullOrEmpty(row[4].ToString()))
                                remoteRFQItems_N.QuotationQty = Convert.ToDouble(row[4]);
                            if (!string.IsNullOrEmpty(row[11].ToString()))
                                remoteRFQItems_N.VendorModelNo = (row[11].ToString());
                            if (!string.IsNullOrEmpty(row[12].ToString()))
                                remoteRFQItems_N.MfgPartNo = row[12].ToString();
                            if (!string.IsNullOrEmpty(row[13].ToString()))
                                remoteRFQItems_N.MfgModelNo = row[13].ToString();

                            if (!string.IsNullOrEmpty(row[14].ToString()))
                                remoteRFQItems_N.ManufacturerName = row[14].ToString();

                            if (!string.IsNullOrEmpty(row[15].ToString()))
                                remoteRFQItems_N.CGSTPercentage = Convert.ToDecimal(row[15]);

                            if (!string.IsNullOrEmpty(row[16].ToString()))
                                remoteRFQItems_N.IGSTPercentage = Convert.ToDecimal(row[16]);

                            if (!string.IsNullOrEmpty(row[17].ToString()))
                                remoteRFQItems_N.SGSTPercentage = Convert.ToDecimal(row[17]);

                            if (!string.IsNullOrEmpty(row[18].ToString()))
                                remoteRFQItems_N.PFAmount = Convert.ToDecimal(row[18]);

                            if (!string.IsNullOrEmpty(row[19].ToString()))
                                remoteRFQItems_N.PFPercentage = Convert.ToDecimal(row[19]);

                            if (!string.IsNullOrEmpty(row[20].ToString()))
                                remoteRFQItems_N.FreightAmount = Convert.ToDecimal(row[20]);

                            if (!string.IsNullOrEmpty(row[21].ToString()))
                                remoteRFQItems_N.FreightPercentage = Convert.ToDecimal(row[21]);

                            if (!string.IsNullOrEmpty(row[8].ToString()))
                                remoteRFQItems_N.HSNCode = row[8].ToString();
                            remoteObj.SaveChanges();

                            RemoteRFQItemsInfo_N remoteitemsInfo_N = remoteObj.RemoteRFQItemsInfo_N.Where(x => x.RFQItemsId == itemsid && x.DeleteFlag == false).FirstOrDefault();
                            if (remoteitemsInfo_N != null)
                            {

                                if (!string.IsNullOrEmpty(row[5].ToString()))
                                    remoteitemsInfo_N.UOM = Convert.ToInt32(row[5]);
                                if (!string.IsNullOrEmpty(row[6].ToString()))
                                    remoteitemsInfo_N.CurrencyValue = Convert.ToDecimal(row[6]);
                                if (!string.IsNullOrEmpty(row[7].ToString()))
                                    remoteitemsInfo_N.UnitPrice = Convert.ToDecimal(row[7]);
                                if (!string.IsNullOrEmpty(row[9].ToString()))
                                    remoteitemsInfo_N.DiscountPercentage = Convert.ToDecimal(row[9]);
                                if (!string.IsNullOrEmpty(row[10].ToString()))
                                    remoteitemsInfo_N.Discount = Convert.ToDecimal(row[10]);
                                if (!string.IsNullOrEmpty(row[22].ToString()))
                                {
                                    DateTime dt = DateTime.ParseExact(row[22].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    remoteitemsInfo_N.DeliveryDate = dt;
                                }
                                if (!string.IsNullOrEmpty(row[23].ToString()))
                                    remoteitemsInfo_N.Remarks = row[23].ToString();
                                remoteObj.SaveChanges();
                            }
                            else
                            {
                                RemoteRFQItemsInfo_N newremoteRfqItemsInfon = new RemoteRFQItemsInfo_N();
                                int rfqsplititemid = obj.RFQItemsInfo_N.Max(li => li.RFQSplitItemId);
                                newremoteRfqItemsInfon.RFQSplitItemId = rfqsplititemid + 1;
                                if (!string.IsNullOrEmpty(row[5].ToString()))
                                    newremoteRfqItemsInfon.UOM = Convert.ToInt32(row[5]);
                                newremoteRfqItemsInfon.RFQItemsId = itemsid;
                                if (!string.IsNullOrEmpty(row[6].ToString()))
                                    newremoteRfqItemsInfon.CurrencyValue = Convert.ToDecimal(row[6]);

                                if (!string.IsNullOrEmpty(row[7].ToString()))
                                    newremoteRfqItemsInfon.UnitPrice = Convert.ToDecimal(row[7]);
                                if (!string.IsNullOrEmpty(row[9].ToString()))
                                    newremoteRfqItemsInfon.DiscountPercentage = Convert.ToDecimal(row[9]);

                                if (!string.IsNullOrEmpty(row[10].ToString()))
                                    newremoteRfqItemsInfon.Discount = Convert.ToDecimal(row[10]);

                                if (!string.IsNullOrEmpty(row[22].ToString()))
                                    newremoteRfqItemsInfon.DeliveryDate = Convert.ToDateTime(row[22]);
                                if (!string.IsNullOrEmpty(row[23].ToString()))
                                    newremoteRfqItemsInfon.Remarks = row[23].ToString();
                                remoteObj.RemoteRFQItemsInfo_N.Add(newremoteRfqItemsInfon);
                                remoteObj.SaveChanges();

                            }

                            RFQItems_N rFQItems_N = obj.RFQItems_N.Where(x => x.RFQItemsId == itemsid).FirstOrDefault();

                            rFQItems_N.ItemId = row[2].ToString();
                            if (!string.IsNullOrEmpty(row[4].ToString()))
                                rFQItems_N.QuotationQty = Convert.ToDouble(row[4]);
                            if (!string.IsNullOrEmpty(row[11].ToString()))
                                rFQItems_N.VendorModelNo = (row[11].ToString());
                            if (!string.IsNullOrEmpty(row[12].ToString()))
                                rFQItems_N.MfgPartNo = row[12].ToString();
                            if (!string.IsNullOrEmpty(row[13].ToString()))
                                rFQItems_N.MfgModelNo = row[13].ToString();
                            if (!string.IsNullOrEmpty(row[14].ToString()))
                                rFQItems_N.ManufacturerName = row[14].ToString();
                            if (!string.IsNullOrEmpty(row[15].ToString()))
                                rFQItems_N.CGSTPercentage = Convert.ToDecimal(row[15]);
                            if (!string.IsNullOrEmpty(row[16].ToString()))
                                rFQItems_N.IGSTPercentage = Convert.ToDecimal(row[16]);
                            if (!string.IsNullOrEmpty(row[17].ToString()))
                                rFQItems_N.SGSTPercentage = Convert.ToDecimal(row[17]);
                            if (!string.IsNullOrEmpty(row[18].ToString()))
                                rFQItems_N.PFAmount = Convert.ToDecimal(row[18]);
                            if (!string.IsNullOrEmpty(row[19].ToString()))
                                rFQItems_N.PFPercentage = Convert.ToDecimal(row[19]);
                            if (!string.IsNullOrEmpty(row[20].ToString()))
                                rFQItems_N.FreightAmount = Convert.ToDecimal(row[20]);
                            if (!string.IsNullOrEmpty(row[21].ToString()))
                                rFQItems_N.FreightPercentage = Convert.ToDecimal(row[21]);
                            if (!string.IsNullOrEmpty(row[8].ToString()))
                                rFQItems_N.HSNCode = row[8].ToString();
                            obj.SaveChanges();

                            RFQItemsInfo_N itemsInfo_N = obj.RFQItemsInfo_N.Where(x => x.RFQItemsId == itemsid && x.DeleteFlag == false).FirstOrDefault();
                            if (itemsInfo_N != null)
                            {
                                if (!string.IsNullOrEmpty(row[5].ToString()))
                                    itemsInfo_N.UOM = Convert.ToInt32(row[5]);
                                if (!string.IsNullOrEmpty(row[6].ToString()))
                                    itemsInfo_N.CurrencyValue = Convert.ToDecimal(row[6]);
                                if (!string.IsNullOrEmpty(row[7].ToString()))
                                    itemsInfo_N.UnitPrice = Convert.ToDecimal(row[7]);
                                if (!string.IsNullOrEmpty(row[9].ToString()))
                                    itemsInfo_N.DiscountPercentage = Convert.ToDecimal(row[9]);
                                if (!string.IsNullOrEmpty(row[10].ToString()))
                                    itemsInfo_N.Discount = Convert.ToDecimal(row[10]);
                                if (!string.IsNullOrEmpty(row[22].ToString()))
                                {
                                    DateTime dt = DateTime.ParseExact(row[22].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    itemsInfo_N.DeliveryDate = dt;
                                }
                                if (!string.IsNullOrEmpty(row[23].ToString()))
                                    itemsInfo_N.Remarks = row[23].ToString();
                                obj.SaveChanges();
                            }
                            else
                            {
                                RFQItemsInfo_N newRfqItemsInfon = new RFQItemsInfo_N();
                                int rfqsplititemid = obj.RFQItemsInfo_N.Max(li => li.RFQSplitItemId);
                                newRfqItemsInfon.RFQSplitItemId = rfqsplititemid + 1;
                                if (!string.IsNullOrEmpty(row[5].ToString()))
                                    newRfqItemsInfon.UOM = Convert.ToInt32(row[5]);
                                newRfqItemsInfon.RFQItemsId = itemsid;
                                if (!string.IsNullOrEmpty(row[6].ToString()))
                                    newRfqItemsInfon.CurrencyValue = Convert.ToDecimal(row[6]);
                                if (!string.IsNullOrEmpty(row[7].ToString()))
                                    newRfqItemsInfon.UnitPrice = Convert.ToDecimal(row[7]);
                                if (!string.IsNullOrEmpty(row[9].ToString()))
                                    newRfqItemsInfon.DiscountPercentage = Convert.ToDecimal(row[9]);
                                if (!string.IsNullOrEmpty(row[10].ToString()))
                                    newRfqItemsInfon.Discount = Convert.ToDecimal(row[10]);
                                if (!string.IsNullOrEmpty(row[22].ToString()))
                                {
                                    DateTime dt = DateTime.ParseExact(row[22].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    itemsInfo_N.DeliveryDate = dt;
                                }
                                if (!string.IsNullOrEmpty(row[23].ToString()))
                                    newRfqItemsInfon.Remarks = row[23].ToString();
                                obj.RFQItemsInfo_N.Add(newRfqItemsInfon);
                                obj.SaveChanges();

                            }
                            var mPRItemInfo = obj.MPRItemInfoes.Join(obj.RFQItems_N, mpr => mpr.Itemdetailsid, rfq => rfq.MPRItemDetailsid, (mpr, rfq) => new { mpr, rfq }).Where(mpritem => mpritem.rfq.RFQItemsId == itemsid).FirstOrDefault();
                            MPRItemInfo mpritemsinfor = obj.MPRItemInfoes.Where(x => x.Itemdetailsid == mPRItemInfo.mpr.Itemdetailsid).FirstOrDefault();
                            if (!string.IsNullOrEmpty(row[3].ToString()))
                                mpritemsinfor.ItemDescription = row[3].ToString();
                            obj.SaveChanges();
                        }
                        i++;
                    }


                    int succRecs = iSucceRows;
                }
                return Ok(parsedFileName);

            }
            catch (Exception e)
            {
                return Ok(e);
            }


        }
        [HttpPost]
        [Route("exceldata")]
        public bool exceldata()
        {
            try
            {
                string Actualpath = ConfigurationManager.AppSettings["SODocPath"];
                //var directory = new DirectoryInfo(@"\\10.29.15.68\\D$\\SODetails\\ActualPath");
                var directory = new DirectoryInfo(Actualpath);
                string myFile = (from f in directory.GetFiles()
                                 orderby f.LastWriteTime descending
                                 select f).First().ToString();
                //string filePath =Actualpath + "'\'" + myFile;
                string filePath = @"C:\\SODetails\\ActualPath\\" + myFile;
                string filePath1 = @"C:\\SODetails\\CopyPath\\" + myFile;
                System.IO.File.Move(filePath, filePath1);
                if (Directory.Exists(filePath))
                {
                    if (Directory.Exists(filePath1))
                    {
                        Directory.Move(filePath, filePath1);
                    }
                    else
                    {
                        Directory.Move(filePath, filePath1);
                    }
                }
                string parsedFileName = "";

                YSCMEntities obj = new YSCMEntities();


                DataTable dtexcel = new DataTable();
                bool hasHeaders = false;
                string HDR = hasHeaders ? "Yes" : "No";
                string strConn;
                if (filePath1.Substring(filePath1.LastIndexOf('.')).ToLower() == ".xlsx")
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath1 + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
                else
                    strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path.GetDirectoryName(filePath1) + ";Extended Properties='Text;HDR=YES;FMT=Delimited;'";

                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                DataRow schemaRow = schemaTable.Rows[0];
                string sheet = schemaRow["TABLE_NAME"].ToString();
                if (!sheet.EndsWith("_"))
                {
                    string query = " SELECT  * FROM [" + sheet + "]";
                    OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                    dtexcel.Locale = CultureInfo.CurrentCulture;
                    daexcel.Fill(dtexcel);
                }

                conn.Close();
                int iSucceRows = 0;
                //System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                //var data = obj.SaleorderDetails.Where(x => x.Deleteflag == false || x.Deleteflag==null).ToList();

                foreach (DataRow row in dtexcel.Rows)
                {
                    obj.SaleorderDetails.Add(new SaleorderDetail
                    {
                        Deleteflag = false,
                        UpdatedDate = System.DateTime.Now,
                        SalesDocumentNo = row["Sales Document No#"].ToString(),
                        PONumber = row["PO number"].ToString(),
                        //POdate = row["PO date"] != DBNull.Value ? Convert.ToDateTime(string.IsNullOrWhiteSpace( row["PO date"].ToString())) : Convert.ToDateTime(""),
                        //POdate = Convert.ToDateTime(row["PO date"].ToString()),
                        soldtoparty = row["Sold-to party"].ToString(),
                        soldtopartyname = row["Name: Sold-to party"].ToString(),
                        payer = row["Payer"].ToString(),
                        payername = row["Name: Payer"].ToString(),
                        Billtoparty = row["Bill-to party"].ToString(),
                        Billtopartyname = row["Name: Billing-to party"].ToString(),
                        shiptoparty = row["Ship-to party"].ToString(),
                        shiptopartyname = row["Name: Ship-to party"].ToString(),
                        Consignee = row["Consignee"].ToString(),
                        Consigneename = row["Name: Consignee"].ToString(),
                        Enduser = row["End User"].ToString(),
                        Endusername = row["Name: End User"].ToString(),
                        soldtoparty2 = row["Sold-to party2"].ToString(),
                        soldtoparty2name = row["Name: Sold-to party2"].ToString(),
                        soldtoparty3 = row["Sold-to party3"].ToString(),
                        soldtoparty3name = row["Name: Sold-to party3"].ToString(),
                        IndustrySegmentE_U = row["Text: E/U Industry Segment"].ToString(),
                        shiptopartyponumber = row["Ship-to party's PO Number"].ToString(),
                        ProjectDefinition = row["Project definition(level 0)"].ToString(),
                        ProjectText = row["Project text (Level 0)"].ToString(),
                    });

                }

                obj.SaveChanges();
                var data = "exec Updatesaleorderdetails";
                var cmd = obj.Database.Connection.CreateCommand();
                cmd.CommandText = data;
                cmd.Connection.Open();
                cmd.ExecuteReader();
                cmd.Connection.Close();
                //}
                //return Ok(parsedFileName);
                return true;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("getincotermmaster")]
        public async Task<IHttpActionResult> getincotermmaster()
        {
            return Ok(await _paBusenessAcess.getincotermmaster());
        }
        [HttpGet]
        [Route("updatetokuchubyid/{tokuchuid}")]
        public async Task<IHttpActionResult> updatetokuchubyid(int tokuchuid)
        {
            return Ok(await _paBusenessAcess.updatetokuchubyid(tokuchuid));
        }
        [HttpPost]
        [Route("UpdateMsaprconfirmation")]
        public async Task<IHttpActionResult> UpdateMsaprconfirmation(List<ItemsViewModel> msa)
        {
            return Ok(await _paBusenessAcess.UpdateMsaprconfirmation(msa));
        }
        [HttpGet]
        [Route("getmsaprocesstrackbyId/{paid}")]
        public async Task<IHttpActionResult> getmsaprocesstrackbyId(int paid)
        {
            return Ok(await _paBusenessAcess.getmsaprocesstrackbyId(paid));
        }
        [HttpPost]
        [Route("InsertScrapRregister")]
        public async Task<IHttpActionResult> InsertScrapRregister(ScrapRegisterMasterModel msa)
        {
            return Ok(await _paBusenessAcess.InsertScrapRregister(msa));
        }
        [Route("uploadscrapExcel")]
        public IHttpActionResult uploadscrapExcel()
        {
            var scrapid = "";
            int documentid = 0;
            var httpRequest = HttpContext.Current.Request;
            var serverPath = HttpContext.Current.Server.MapPath("~/ScrapDocuments");
            string parsedFileName = "";
            string filename = "";

            if (httpRequest.Files.Count > 0)
            {
                scrapid = httpRequest.Files.AllKeys[0];
                //var type = httpRequest.Files.AllKeys[1];
                //string employeeno = httpRequest.Files.AllKeys[1];
                var postedFile = httpRequest.Files[0];
                parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + scrapid + "\\" + ToValidFileName(postedFile.FileName));
                serverPath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + scrapid;
                var filePath = Path.Combine(serverPath, ToValidFileName(postedFile.FileName));
                if (!Directory.Exists(serverPath))
                    Directory.CreateDirectory(serverPath);
                postedFile.SaveAs(filePath);
                try
                {
                    YSCMEntities entities = new YSCMEntities();

                    var data = new ScrapRegisterDocumentTypeMaster();
                    data.DocumentName = postedFile.FileName;
                    data.DeleteFlag = false;
                    data.CreatedDate = System.DateTime.Now;
                    //data.CreatedBY = Convert.ToInt32(paid);

                    entities.ScrapRegisterDocumentTypeMasters.Add(data);
                    entities.SaveChanges();
                    documentid = data.DocumentTypeId;
                    filename = data.DocumentName;

                    var data1 = new ScapRegisterDocument();
                    data1.Scrapentryid = Convert.ToInt32(scrapid);
                    data1.DocumentTypeId = documentid;
                    data1.path = parsedFileName;
                    data1.DocumentNAme = filename;
                    entities.ScapRegisterDocuments.Add(data1);
                    entities.SaveChanges();

                    //entities.MPRPADocuments.Add(new MPRPADocument
                    //{
                    //    Filename = postedFile.FileName,
                    //    Filepath = parsedFileName,
                    //    uploadeddate = System.DateTime.Now,
                    //    paid = Convert.ToInt32(paid)
                    //});
                    //entities.SaveChanges();

                    // int succRecs = iSucceRows;
                }
                catch (Exception e)
                {
                    throw e;
                }
                //for (int i = 0; i < httpRequest.Files.Count; i++)
                //{


                //}


            }
            return Ok(filename);

        }
        [Route("uploadscrapfileExcel")]
        public IHttpActionResult uploadscrapfileExcel()
        {
            var scrapid = "";
            int documentid = 0;
            var httpRequest = HttpContext.Current.Request;
            var serverPath = HttpContext.Current.Server.MapPath("~/scrapDocuments");
            string parsedFileName = "";
            string filename = "";

            if (httpRequest.Files.Count > 0)
            {
                scrapid = httpRequest.Files.AllKeys[0];
                var postedFile = httpRequest.Files[0];
                var filetype = postedFile.FileName.Substring(postedFile.FileName.IndexOf('-') + 1);
                var scrapfilename = postedFile.FileName.Substring(0, postedFile.FileName.IndexOf('-'));
                parsedFileName = string.Format(DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM") + "\\" + scrapid + "\\" + ToValidFileName(scrapfilename));
                serverPath = serverPath + string.Format("\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.ToString("MMM")) + "\\" + scrapid;
                var filePath = Path.Combine(serverPath, ToValidFileName(scrapfilename));
                if (!Directory.Exists(serverPath))
                    Directory.CreateDirectory(serverPath);
                postedFile.SaveAs(filePath);
                try
                {
                    YSCMEntities entities = new YSCMEntities();

                    var data = new ScrapRegisterDocumentTypeMaster();
                    data.DocumentName = scrapfilename;
                    data.DeleteFlag = false;
                    data.CreatedDate = System.DateTime.Now;
                    entities.ScrapRegisterDocumentTypeMasters.Add(data);
                    entities.SaveChanges();
                    documentid = data.DocumentTypeId;
                    filename = data.DocumentName;

                    var data1 = new ScapRegisterDocument();
                    data1.Scrapentryid = Convert.ToInt32(scrapid);
                    data1.DocumentTypeId = documentid;
                    data1.path = filePath;
                    data1.DocumentNAme = filename;
                    data1.DocumentType = filetype;
                    entities.ScapRegisterDocuments.Add(data1);
                    entities.SaveChanges();

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return Ok(filename);

        }
        [HttpPost]
        [Route("getscraplist")]
        public async Task<IHttpActionResult> getscraplist(scrapsearchmodel model)
        {
            return Ok(await _paBusenessAcess.getscraplist(model));
        }
        [HttpGet]
        [Route("getscrapitembyid/{scrapid}")]
        public async Task<IHttpActionResult> getscrapitembyid(int scrapid)
        {
            return Ok(await _paBusenessAcess.getscrapitembyid(scrapid));
        }
        [HttpPost]
        [Route("UpdateScrapRequest")]
        public async Task<IHttpActionResult> UpdateScrapRequest(ScrapRegisterMasterModel msa)
        {
            return Ok(await _paBusenessAcess.UpdateScrapRequest(msa));
        }
        [HttpPost]
        [Route("UpdateScrapRregister")]
        public async Task<IHttpActionResult> UpdateScrapRregister(ScrapRegisterMasterModel msa)
        {
            return Ok(await _paBusenessAcess.UpdateScrapRregister(msa));
        }
        [HttpPost]
        [Route("InsertScarpflowIncharge")]
        public async Task<IHttpActionResult> InsertScarpflowIncharge(ScrapflowModel msa)
        {
            return Ok(await _paBusenessAcess.InsertScarpflowIncharge(msa));
        }
        [HttpGet]
        [Route("Getincharelist")]
        public async Task<IHttpActionResult> Getincharelist()
        {
            return Ok(await _paBusenessAcess.Getincharelist());
        }
        [HttpGet]
        [Route("Getscrapitemdetails/{itemcode}")]
        public async Task<IHttpActionResult> Getscrapitemdetails(string itemcode)
        {
            return Ok(await _paBusenessAcess.Getscrapitemdetails(itemcode));
        }
        [HttpPost]
        [Route("Getincharepermissionlist")]
        public async Task<IHttpActionResult> Getincharepermissionlist(scrapsearchmodel search)
        {
            return Ok(await _paBusenessAcess.Getincharepermissionlist(search));
        }
        [HttpPost]
        [Route("getscraplistbysearch")]
        public async Task<IHttpActionResult> getscraplistbysearch(scrapsearchmodel search)
        {
            return Ok(await _paBusenessAcess.getscraplistbysearch(search));
        }
        [HttpGet]
        [Route("getauthorizescrapflowlist/{employeeno}")]
        public async Task<IHttpActionResult> getauthorizescrapflowlist(string employeeno)
        {
            return Ok(await _paBusenessAcess.getauthorizescrapflowlist(employeeno));
        }
        [HttpPost]
        [Route("GetCMMMonthlyPerformancereport1")]
        [ResponseType(typeof(DataSet))]
        public DataSet GetCMMMonthlyPerformancereport1(ReportInputModel model)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Param = new SqlParameter[4];
            string data = "";
            YSCMEntities obj = new YSCMEntities();
            //if (model.DepartmentId == 0)
            //{
            //    List<int> departments = obj.MPRDepartments.Where(x => x.ORgDepartmentid == model.OrgDepartmentId).Select(x => (int)x.DepartmentId).ToList();
            //    data = string.Join(" , ", departments);
            //}
            //else
            //{
            //    data = string.Join(",", model.DepartmentId);
            //}
            if (model.BuyerGroupId == 0)
            {
                Param[0] = new SqlParameter("buyergroupid", SqlDbType.VarChar);
                Param[0].Value = DBNull.Value;
                Param[1] = new SqlParameter("@fromdate", model.Fromdate);
                Param[2] = new SqlParameter("@todate", model.Todate);
                if (model.DepartmentId == 0)
                {
                    Param[3] = new SqlParameter("Departmentid", SqlDbType.VarChar);
                    Param[3].Value = DBNull.Value;
                }
                else
                    Param[3] = new SqlParameter("@Departmentid", data);
            }
            else
            {
                Param[0] = new SqlParameter("@BuyerGroupId", model.BuyerGroupId);
                Param[1] = new SqlParameter("@fromdate", model.Fromdate);
                Param[2] = new SqlParameter("@todate", model.Todate);
                if (model.DepartmentId == 0)
                {
                    Param[3] = new SqlParameter("Departmentid", SqlDbType.VarChar);
                    Param[3].Value = DBNull.Value;
                }
                else
                    Param[3] = new SqlParameter("@Departmentid", data);
            }
            ds = _paBusenessAcess.GetCMMMonthlyPerformance1("SP_GetMonthlyPerformanceReport1", Param);
            return ds;
        }
        [HttpPost]
        [Route("GetCMMMonthlyreport2")]
        [ResponseType(typeof(DataSet))]
        public DataSet GetCMMMonthlyreport2(ReportInputModel model)
        {
            DataSet ds = new DataSet();
            SqlParameter[] Param = new SqlParameter[4];
            string data = "";
            YSCMEntities obj = new YSCMEntities();
            //if (model.DepartmentId == 0)
            //{
            //    List<int> departments = obj.MPRDepartments.Where(x => x.ORgDepartmentid == model.OrgDepartmentId).Select(x => (int)x.DepartmentId).ToList();
            //    data = string.Join(" , ", departments);
            //}
            //else
            //{
            //    data = string.Join(",", model.DepartmentId);
            //}
            if (model.BuyerGroupId == 0)
            {
                Param[0] = new SqlParameter("buyergroupid", SqlDbType.VarChar);
                Param[0].Value = DBNull.Value;
                Param[1] = new SqlParameter("@fromdate", model.Fromdate);
                Param[2] = new SqlParameter("@todate", model.Todate);
                if (model.DepartmentId == 0)
                {
                    Param[3] = new SqlParameter("Departmentid", SqlDbType.VarChar);
                    Param[3].Value = DBNull.Value;
                }
                else
                    Param[3] = new SqlParameter("@Departmentid", model.DepartmentId);

            }
            else
            {
                Param[0] = new SqlParameter("@BuyerGroupId", model.BuyerGroupId);
                Param[1] = new SqlParameter("@fromdate", model.Fromdate);
                Param[2] = new SqlParameter("@todate", model.Todate);
                if (model.DepartmentId == 0)
                {
                    Param[3] = new SqlParameter("Departmentid", SqlDbType.VarChar);
                    Param[3].Value = DBNull.Value;
                }
                else
                    Param[3] = new SqlParameter("@Departmentid", model.DepartmentId);
            }
            ds = _paBusenessAcess.GetCMMMonthlyPerformance1("SP_GetMonthlyReport2", Param);
            return ds;
        }
        [HttpPost]
        [Route("LoadItemsforpocreation")]
        public async Task<IHttpActionResult> LoadItemsforpocreation(PADetailsModel masters)
        {
            return Ok(await _paBusenessAcess.LoadItemsforpocreation(masters));
        }
        [HttpPost]
        [Route("LoadItemsforpogeneration")]
        public async Task<IHttpActionResult> LoadItemsforpogeneration(List<int> PAId)
        {
            return Ok(await _paBusenessAcess.LoadItemsforpogeneration(PAId));
        }
        [HttpPost]
        [Route("LoadItemsforpogenerationbasedonvendor/{VendorId}")]
        public async Task<IHttpActionResult> LoadItemsforpogenerationbasedonvendor(List<int> PAId, string VendorId)
        {
            return Ok(await _paBusenessAcess.LoadItemsforpogenerationbasedonvendor(VendorId, PAId));
        }
        [HttpPost]
        [Route("InsertPOItems")]
        public async Task<IHttpActionResult> InsertPOItems(POMasterModel model)
        {
            return Ok(await _paBusenessAcess.InsertPOItems(model));
        }
        [HttpPost]
        [Route("getscrapRegisterReport")]
        public async Task<IHttpActionResult> getscrapRegisterReport(scrapsearchmodel search)
        {
            return Ok(await _paBusenessAcess.getscrapRegisterReport(search));
        }
        [HttpGet]
        [Route("GetpoitemsByPoId/{revisionid}")]
        public async Task<IHttpActionResult> GetpoitemsByPoId(int revisionid)
        {
            return Ok(await _paBusenessAcess.GetpoitemsByPoId(revisionid));
        }
        [HttpPost]
        [Route("LoadPolist")]
        public async Task<IHttpActionResult> LoadPolist(posearchmodel posearch)
        {
            return Ok(await _paBusenessAcess.LoadPolist(posearch));
        }
        [HttpPost]
        [Route("Updateprnoapproval")]
        public async Task<IHttpActionResult> Updateprnoapproval(List<ItemsViewModel> msa)
        {
            return Ok(await _paBusenessAcess.ApprovePRNos(msa));
        }
    }
}
