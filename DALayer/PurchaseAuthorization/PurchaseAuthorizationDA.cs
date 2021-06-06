using DALayer.Emails;
using DALayer.MPR;
using DALayer.PAEmailDA;
using SCMModels;
using SCMModels.MPRMasterModels;
using SCMModels.RemoteModel;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALayer.Common;

namespace DALayer.PurchaseAuthorization
{
    /*
Name of Class : <<PurchaseAuthorization>>  Author :<<Akhil Kumar reddy>>  
Date of Creation <<1-11-2019>>
Purpose : <<to generate PA, get PA data>>
Review Date :<<>>   Reviewed By :<<>>

*/
    public class PurchaseAuthorizationDA : IPurchaseAuthorizationDA
    {
        private ErrorLog log = new ErrorLog();
        private IPAEmailDA emailDA = default(IPAEmailDA);
        private IEmailTemplateDA emailTemplateDA = default(IEmailTemplateDA);
        public PurchaseAuthorizationDA(IPAEmailDA EmailDA, IEmailTemplateDA emailTemplateDA)
        {
            this.emailDA = EmailDA;
            this.emailTemplateDA = emailTemplateDA;
        }
        VSCMEntities vscm = new VSCMEntities();
        YSCMEntities obj = new YSCMEntities();
        //inserting pa limits
        /*Name of Function : <<InsertPAAuthorizationLimits>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<To Insert the purchase authorization slabs to the department>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> InsertPAAuthorizationLimits(PAAuthorizationLimitModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new PAAuthorizationLimit();
                if (model != null)
                {
                    data.DeptId = model.DeptId;
                    data.AuthorizationType = model.AuthorizationType;
                    data.MinPAValue = model.MinPAValue;
                    data.MaxPAValue = model.MaxPAValue;
                    data.CreatedBy = model.CreatedBy;
                    data.CreatedDate = System.DateTime.Now;
                    data.DeletedBY = model.DeletedBY;
                    data.DeletedDate = model.DeletedDate;
                }
                obj.PAAuthorizationLimits.Add(data);
                obj.SaveChanges();
                int id = data.Authid;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<BulkInsertPAAuthorizationLimits>>  Author :<<Akhil>>  
    Date of Creation <<15-09-2019>>
    Purpose : <<Inserting multiple slabs at a timme>>
    Review Date :<<>>   Reviewed By :<<>>*/
        //bulk insert palimits
        public async Task<statuscheckmodel> BulkInsertPAAuthorizationLimits(List<PAAuthorizationLimitModel> model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = new PAAuthorizationLimit();
                foreach (var item in model)
                {
                    data.AuthorizationType = item.AuthorizationType;
                    data.DeptId = item.DeptId;
                    data.MinPAValue = item.MinPAValue;
                    data.MaxPAValue = item.MaxPAValue;
                    data.CreatedBy = item.CreatedBy;
                    data.CreatedDate = item.CreatedDate;
                    obj.PAAuthorizationLimits.Add(data);
                    obj.SaveChanges();
                }
                int id = data.Authid;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //get authorization limits by deptid
        /*Name of Function : <<GetPAAuthorizationLimitById>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<To get the pa slabs by department>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<PAAuthorizationLimitModel> GetPAAuthorizationLimitById(int deptid)
        {
            PAAuthorizationLimitModel model = new PAAuthorizationLimitModel();
            try
            {
                var data = obj.PAAuthorizationLimits.Where(x => x.Authid == deptid && x.DeleteFlag == false).FirstOrDefault();
                model.DeptId = data.DeptId;
                model.MaxPAValue = data.MaxPAValue;
                model.MinPAValue = data.MinPAValue;
                model.AuthorizationType = data.AuthorizationType;
                model.DeletedBY = data.DeletedBY;
                model.DeletedDate = data.DeletedDate;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //employee mapping to the palimits
        //get authorization limits by deptid
        /*Name of Function : <<CreatePAAuthirizationEmployeeMapping>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Assign the pa slabs and funcyional role id to the employee>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> CreatePAAuthirizationEmployeeMapping(PAAuthorizationEmployeeMappingModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var mapping = new PAAuthorizationEmployeeMapping();
                if (model != null)
                {
                    mapping.Authid = model.Authid;
                    mapping.FunctionalRoleId = model.FunctionalRoleId;
                    mapping.CreatedBY = model.CreatedBY;
                    mapping.CreatedDate = System.DateTime.Now;
                    //mapping.Employeeid = model.Employeeid;
                    foreach (var item in model.Employeeid)
                    {
                        mapping.Employeeid = item.EmployeeNo;
                    }
                    mapping.LessBudget = model.LessBudget;
                    mapping.MoreBudget = model.MoreBudget;
                    mapping.DeletedBy = model.DeletedBy;
                    mapping.DeletedDate = model.DeletedDate;
                }
                obj.PAAuthorizationEmployeeMappings.Add(mapping);
                obj.SaveChanges();
                int mapid = mapping.PAmapid;
                status.Sid = mapid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<PAAuthorizationEmployeeMappingModel> GetMappingEmployee(PAAuthorizationLimitModel limit)
        {
            PAAuthorizationEmployeeMappingModel model = new PAAuthorizationEmployeeMappingModel();
            try
            {
                var authdata = obj.PAAuthorizationLimits.Where(x => x.Authid == limit.Authid && x.MinPAValue >= limit.MinPAValue && x.MaxPAValue <= limit.MaxPAValue).FirstOrDefault();
                var mappingdata = obj.PAAuthorizationEmployeeMappings.Where(x => x.Authid == authdata.Authid && x.DeleteFlag == false).FirstOrDefault();
                var employeedata = obj.Employees.Where(x => x.EmployeeNo == mappingdata.Employeeid).FirstOrDefault();
                //model.Employeeid = mappingdata.Employeeid;
                model.Employeename = employeedata.Name;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*Name of Function : <<CreatePACreditDaysmaster>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Inserting the pa credit days to PACreditDaysMaster>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> CreatePACreditDaysmaster(PACreditDaysMasterModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            var credit = new PACreditDaysMaster();
            try
            {
                if (model != null)
                {
                    credit.MinDays = model.MinDays;
                    credit.MaxDays = model.MaxDays;
                    credit.DeletedBy = model.DeletedBy;
                    credit.DeletedDate = model.DeletedDate;
                    credit.CreatedBy = model.CreatedBy;
                    credit.CreatedDate = System.DateTime.Now;
                }
                obj.PACreditDaysMasters.Add(credit);
                obj.SaveChanges();
                int creditid = credit.CreditDaysid;
                status.Sid = creditid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //get credit days by credit id
        /*Name of Function : <<GetCreditdaysMasterByID>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting the assigned credit days by creditdaysid>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<PACreditDaysMasterModel> GetCreditdaysMasterByID(int creditdaysid)
        {
            PACreditDaysMasterModel model = new PACreditDaysMasterModel();
            try
            {
                var creditdata = obj.PACreditDaysMasters.Where(x => x.CreditDaysid == creditdaysid).FirstOrDefault();
                if (creditdata != null)
                {
                    model.CreditDaysid = creditdata.CreditDaysid;
                    model.MinDays = creditdata.MinDays;
                    model.MaxDays = creditdata.MaxDays;
                    model.CreatedBy = creditdata.CreatedBy;
                    model.CreatedDate = creditdata.CreatedDate;
                    model.DeletedBy = creditdata.DeletedBy;
                    model.DeletedDate = creditdata.DeletedDate;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //mapping credit days to employee
        /*Name of Function : <<AssignCreditdaysToEmployee>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Assign Creditdays ToEmployee>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> AssignCreditdaysToEmployee(PACreditDaysApproverModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                ////var data = obj.PAAuthorizationLimits.Where(x => x.Authid == model.AuthId).FirstOrDefault();
                var creditapprover = new PACreditDaysApprover();
                creditapprover.AuthId = model.AuthId;
                creditapprover.EmployeeNo = model.EmployeeNo;
                creditapprover.CreditdaysId = Convert.ToByte(model.CreditdaysId);
                creditapprover.Createdby = model.Createdby;
                creditapprover.CreatedDate = System.DateTime.Now;
                creditapprover.DeletedBy = model.DeletedBy;
                creditapprover.DeletedDate = model.DeletedDate;
                obj.PACreditDaysApprovers.Add(creditapprover);
                obj.SaveChanges();
                int approvalid = creditapprover.CRApprovalId;
                status.Sid = approvalid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //remove pa limits by Auth id
        /*Name of Function : <<RemovePAAuthorizationLimitsByID>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Removing the slabs to department by authid>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> RemovePAAuthorizationLimitsByID(int authid)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var removedata = obj.PAAuthorizationLimits.Where(x => x.Authid == authid && x.DeleteFlag == false).FirstOrDefault();
                if (removedata != null)
                {
                    removedata.DeleteFlag = true;
                    obj.SaveChanges();
                }
                var mappingdata = obj.PAAuthorizationEmployeeMappings.Where(x => x.Authid == removedata.Authid && x.DeleteFlag == false).ToList();
                if (mappingdata != null)
                {
                    foreach (var item in mappingdata)
                    {
                        item.DeleteFlag = true;
                        obj.SaveChanges();
                    }
                }
                int id = removedata.Authid;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //removing pa creditdays by creditid
        /*Name of Function : <<RemovePACreditDaysMaster>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Removing the PACreditDays>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> RemovePACreditDaysMaster(int creditid)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var creditdata = obj.PACreditDaysMasters.Where(x => x.CreditDaysid == creditid && x.DeleteFlag == false).FirstOrDefault();
                if (creditdata != null)
                {
                    creditdata.DeleteFlag = true;
                    obj.SaveChanges();
                }
                int id = creditdata.CreditDaysid;
                status.Sid = id;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<GetPAAuthorizationLimitsByDeptId>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Get PAAuthorizationLimitsByDeptId>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<PAAuthorizationLimitModel>> GetPAAuthorizationLimitsByDeptId(int departmentid)
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            try
            {
                model = obj.PAAuthorizationLimits.Where(x => x.DeptId == departmentid && x.DeleteFlag == false).Select(x => new PAAuthorizationLimitModel
                {
                    MinPAValue = x.MinPAValue,
                    MaxPAValue = x.MaxPAValue
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //removing employee assigned to pa credit days
        /*Name of Function : <<RemovePACreditDaysApprover>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Removing the assigned employee to credit days>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> RemovePACreditDaysApprover(EmployeemappingtocreditModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.PACreditDaysApprovers.Where(x => x.CRApprovalId == model.CRApprovalId).FirstOrDefault();
                if (data != null)
                {
                    data.DeleteFlag = true;
                    obj.SaveChanges();
                }
                status.Sid = data.AuthId;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> RemovePurchaseApprover(EmployeemappingtopurchaseModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.PAAuthorizationEmployeeMappings.Where(x => x.PAmapid == model.PAmapid).FirstOrDefault();
                if (data != null)
                {
                    data.DeleteFlag = true;
                    obj.SaveChanges();
                }
                status.Sid = data.Authid;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //getting credit days approver by approval id
        /*Name of Function : <<GetPACreditDaysApproverById>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<GetPACreditDaysApproverById>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<PACreditDaysApproverModel> GetPACreditDaysApproverById(int ApprovalId)
        {
            PACreditDaysApproverModel model = new PACreditDaysApproverModel();
            try
            {
                var data = obj.PACreditDaysApprovers.Where(x => x.CRApprovalId == ApprovalId).FirstOrDefault();
                if (data != null)
                {
                    model.AuthId = data.AuthId;
                    model.EmployeeNo = data.EmployeeNo;
                    model.CreditdaysId = data.CreditdaysId;
                    model.Createdby = data.Createdby;
                    model.CreatedDate = data.CreatedDate;
                    model.DeletedBy = data.DeletedBy;
                    model.DeletedDate = data.DeletedDate;
                    model.CRApprovalId = data.CRApprovalId;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public async Task<List<EmployeModel>> GetEmployeeMappings(PAConfigurationModel model)
        //{
        //    List<EmployeModel> employee = new List<EmployeModel>();
        //    model.PAValue = model.UnitPrice;
        //    if (model.PAValue > model.TargetSpend)
        //    {
        //        model.Budgetvalue = false;
        //    }
        //    else
        //    {
        //        model.Budgetvalue = true;
        //    }
        //    try
        //    {
        //        if (model != null)
        //        {
        //            //var padata = obj.PAAuthorizationLimits.Where(x => x.MinPAValue >= model.PAValue && x.MaxPAValue <= model.PAValue).FirstOrDefault();
        //            var padata = obj.PAAuthorizationLimits.Where(x => x.MinPAValue.CompareTo(model.PAValue) <= 0 && x.MaxPAValue.CompareTo(model.PAValue) >= 0 && x.DeptId==model.DeptId).FirstOrDefault();
        //            if (padata != null)
        //            {
        //                //string.Equals(padata.AuthorizationType,"pa", StringComparison.CurrentCultureIgnoreCase);
        //                //padata.AuthorizationType.Contains( StringComparison.CurrentCultureIgnoreCase);
        //                if (padata.AuthorizationType.ToLower().Equals("pa"))
        //                {
        //                    var mappingdata = obj.PAAuthorizationEmployeeMappings.Where(x => x.Authid == padata.Authid).FirstOrDefault();
        //                    if (mappingdata != null)
        //                    {
        //                        var employeedata = obj.Employees.Where(x => x.EmployeeNo == mappingdata.Employeeid).ToList();
        //                        employee = employeedata.Select(x => new EmployeModel()
        //                        {
        //                            EmployeeNo = x.EmployeeNo,
        //                            Name = x.Name
        //                        }).ToList();
        //                    }
        //                }
        //                else
        //                {
        //                    var creditdata = obj.PACreditDaysApprovers.Where(x => x.AuthId == padata.Authid).FirstOrDefault();
        //                    var employeedata = obj.Employees.Where(x => x.EmployeeNo == creditdata.EmployeeNo).ToList();
        //                    if (creditdata != null)
        //                    {
        //                        var creditmasterdata = obj.PACreditDaysMasters.Where(x => x.CreditDaysid == creditdata.CreditdaysId).FirstOrDefault();
        //                    }
        //                    employee = employeedata.Select(x => new EmployeModel()
        //                    {
        //                        EmployeeNo = x.EmployeeNo,
        //                        Name = x.Name
        //                    }).ToList();
        //                }
        //            }
        //            else
        //            {
        //                return employee;
        //            }

        //        }
        //        return employee;
        //    }
        //    catch (Exception ex)  
        //    {
        //        throw;
        //    }
        //}

        //getting approvers based on toatl unitprice and target spend
        /*Name of Function : <<GetEmployeeMappings>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting configured employee based on total pa value,target spend and credit days>>
Review Date :<<>>   Reviewed By :<<>>*/
        public DataTable GetEmployeeMappings12(PAConfigurationModel model)
        {
            string con = obj.Database.Connection.ConnectionString;
            SqlConnection Conn1 = new SqlConnection(con);
            EmployeModel employee = new EmployeModel();
            DataSet Ds = new DataSet();
            DataTable dt = new DataTable();
            string data = string.Join(",", model.MPRItemDetailsid);
            model.PAValue = model.UnitPrice;
            int Termscode = 0;
            if (model.PAValue > model.TargetSpend)
                model.LessBudget = false;
            else
                model.LessBudget = true;
            if (model.PaymentTermCode != null && model.Creditdays == 0)
                Termscode = Convert.ToInt32(model.PaymentTermCode.Substring(model.PaymentTermCode.Length - 3, 3));
            else if (model.Creditdays != 0)
                Termscode = Convert.ToInt32(model.Creditdays);
            else
                Termscode = 0;
            try
            {
                SqlParameter[] Param = new SqlParameter[5];
                Param[0] = new SqlParameter("@itemid", data);
                Param[1] = new SqlParameter("@PAvalue", model.PAValue);
                Param[2] = new SqlParameter("@TargetSpend", model.TargetSpend);
                Param[3] = new SqlParameter("@creditdays", Termscode);
                Param[4] = new SqlParameter("@departmentid", model.DeptId);
                string spname = "PAApprovers";
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter Adp = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.Connection = Conn1;
                cmd.CommandText = spname;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                if (Param != null)
                {
                    foreach (SqlParameter sqlParam in Param)
                    {
                        cmd.Parameters.Add(sqlParam);
                    }
                }

                Adp = new SqlDataAdapter(cmd);
                Ds = new DataSet();

                Adp.Fill(Ds);
                cmd.Parameters.Clear();
                //Ds.Clear();
                return dt;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public DataSet GetEmployeeMappings1(PAConfigurationModel model)
        {
            string con = obj.Database.Connection.ConnectionString;
            //SqlConnection Conn1 = new SqlConnection(@"Data Source=10.29.15.183;User ID=sa;Password=yil@1234;initial catalog=YSCM;Integrated Security=false;");
            SqlConnection Conn1 = new SqlConnection(con);
            EmployeModel employee = new EmployeModel();
            DataSet Ds = new DataSet();
            string data = string.Join(",", model.MPRItemDetailsid);
            model.PAValue = model.UnitPrice;
            int Termscode = 0;
            if (model.PAValue > model.TargetSpend)
                model.LessBudget = false;
            else
                model.LessBudget = true;
            if (model.PaymentTermCode != null && model.Creditdays == 0)
                Termscode = Convert.ToInt32(model.PaymentTermCode.Substring(model.PaymentTermCode.Length - 3, 3));
            else if (model.Creditdays != 0)
                Termscode = Convert.ToInt32(model.Creditdays);
            else
                Termscode = 0;
            try
            {
                SqlParameter[] Param = new SqlParameter[5];
                Param[0] = new SqlParameter("@itemid", data);
                Param[1] = new SqlParameter("@PAvalue", model.PAValue);
                Param[2] = new SqlParameter("@TargetSpend", model.TargetSpend);
                Param[3] = new SqlParameter("@creditdays", Termscode);
                Param[4] = new SqlParameter("@departmentid", model.DeptId);
                string spname = "PAApprovers";
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter Adp = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.Connection = Conn1;
                cmd.CommandText = spname;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;



                if (Param != null)
                {
                    foreach (SqlParameter sqlParam in Param)
                    {
                        cmd.Parameters.Add(sqlParam);
                    }
                }



                Adp = new SqlDataAdapter(cmd);
                Ds = new DataSet();



                Adp.Fill(Ds);
                cmd.Parameters.Clear();
                //Ds.Clear();
                return Ds;
            }
            catch (Exception ex)
            {



                throw;
            }
        }
        public DataTable GetEmployeeMappings11(PAConfigurationModel model)
        {

            EmployeModel employee = new EmployeModel();
            DataTable table = new DataTable();
            string data = string.Join(",", model.MPRItemDetailsid);
            model.PAValue = model.UnitPrice;
            int Termscode = 0;
            if (model.PAValue > model.TargetSpend)
                model.LessBudget = false;
            else
                model.LessBudget = true;
            if (model.PaymentTermCode != null && model.Creditdays == 0)
                Termscode = Convert.ToInt32(model.PaymentTermCode.Substring(model.PaymentTermCode.Length - 3, 3));
            else if (model.Creditdays != 0)
                Termscode = Convert.ToInt32(model.Creditdays);
            else
                Termscode = 0;
            try
            {
                string spname = "exec PAApprovers " + data + "," + model.PAValue + "," + model.TargetSpend + "," + Termscode + "," + model.DeptId + "";
                var con = obj.Database.Connection.CreateCommand();
                con.CommandText = spname;
                con.Connection.Open();
                table.Load(con.ExecuteReader());
                con.Connection.Close();
                return table;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<GetEmployeeMappings>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting configured employee based on total pa value,target spend and credit days>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<EmployeModel> GetEmployeeMappings(PAConfigurationModel model)
        {
            EmployeModel employee = new EmployeModel();
            model.PAValue = model.UnitPrice;
            int Termscode = 0;
            if (model.PAValue > model.TargetSpend)
            {
                model.LessBudget = false;
                //model.MoreBudget = true;
            }
            else
            {
                //model.MoreBudget = false;
                model.LessBudget = true;
            }
            if (model.PaymentTermCode != null)
            {
                Termscode = Convert.ToInt32(model.PaymentTermCode.Substring(model.PaymentTermCode.Length - 2, 2));
            }
            else
            {
                Termscode = 0;
            }

            try
            {
                var BuyerManagers = obj.LoadBuyerManagers.Where(x => model.MPRItemDetailsid.Contains(x.Itemdetailsid) && x.BoolValidRevision == true).FirstOrDefault();
                if (BuyerManagers != null)
                {
                    employee.BuyerGroupManager = BuyerManagers.EmployeeName;
                    employee.BuyerGroupNo = BuyerManagers.EmpNo;
                    employee.BGRole = BuyerManagers.Role;
                }
                var projectmanagers = obj.LoadProjectManagers.Where(x => model.MPRItemDetailsid.Contains(x.Itemdetailsid) && x.BoolValidRevision == true).FirstOrDefault();
                if (projectmanagers != null)
                {
                    employee.ProjectManager = projectmanagers.EmployeeName;
                    employee.ProjectMangerNo = projectmanagers.EmpNo;
                    employee.PMRole = projectmanagers.Role;
                }

                var sqlquery = "";
                sqlquery = "select distinct EmployeeNo,name,* from PAandCRMapping where (departmentid=' " + model.DeptId + "' and '" + model.PAValue + "' between minpavalue and maxpavalue and AuthorizationType='PA'and (LessBudget= case when (" + model.TargetSpend + " -" + model.PAValue + ")>=0 then 1 else 0 end or MOREbUDGET= CASE when (" + model.TargetSpend + " - " + model.PAValue + " <0) then 1 else 0 end)) or ('" + model.PAValue + "' between minpavalue and maxpavalue  and '" + Termscode + "' between MinDays and maxdays and AuthorizationType = 'CR')";

                //PAandCRmapping= obj.PAandCRMappings.Where(x => x.DepartmentId == model.DeptId && x.minpavalue.CompareTo(model.PAValue) <= 0 && x.maxpavalue.CompareTo(model.PAValue) >= 0 && x.lessbudget == model.LessBudget && x.MinDays.CompareTo(Termscode) <= 0 && x.MaxDays.CompareTo(Termscode) >= 0).OrderBy(x => x.roleorder).ToList();

                var PAandCRmapping = obj.Database.SqlQuery<PAandCRMapping>(sqlquery).ToList();
                //else
                //{
                //    PAandCRmapping = obj.PAandCRMappings.Where(x => x.DepartmentId == model.DeptId && x.minpavalue.CompareTo(model.PAValue) <= 0 && x.maxpavalue.CompareTo(model.PAValue) >= 0 && x.morebudget == model.MoreBudget && x.MinDays.CompareTo(Termscode) <= 0 && x.MaxDays.CompareTo(Termscode) >= 0).OrderBy(x => x.roleorder).ToList();
                //}
                employee.Approvers = PAandCRmapping.Select(x => new PurchaseCreditApproversModel()
                {
                    ApproverName = x.Name,
                    AuthorizationType = x.AuthorizationType,
                    RoleName = x.role,
                    EmployeeNo = x.EmployeeNo,
                    RoleId = x.FunctionalRoleId,
                    roleorder = x.roleorder
                }).ToList();

                return employee;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public async Task<EmployeeModel> GetEmployeeMappingsList(List<PAConfigurationModel> model)
        //{
        //    EmployeeModel employee = new EmployeeModel();
        //    model.PAValue = model.UnitPrice;
        //    try
        //    {
        //        if (model != null)
        //        {
        //            //var padata = obj.PAAuthorizationLimits.Where(x => x.MinPAValue >= model.PAValue && x.MaxPAValue <= model.PAValue).FirstOrDefault();
        //            var padata = obj.PAAuthorizationLimits.Where(x => x.MinPAValue.CompareTo(model.PAValue) <= 0 && x.MaxPAValue.CompareTo(model.PAValue) >= 0 && x.DeptId == model.DeptId).FirstOrDefault();
        //            if (padata != null)
        //            {
        //                var mappingdata = obj.PAAuthorizationEmployeeMappings.Where(x => x.Authid == padata.Authid && x.LessBudget == true).FirstOrDefault();
        //                if (mappingdata != null)
        //                {
        //                    var res = obj.PACreditDaysApprovers.ToList();
        //                    var creditapprovaldata = obj.PACreditDaysApprovers.Where(x => x.AuthId == padata.Authid && x.EmployeeNo == mappingdata.Employeeid).FirstOrDefault();
        //                    if (creditapprovaldata != null)
        //                    {
        //                        var creditmasterdata = obj.PACreditDaysMasters.Where(x => x.CreditDaysid == creditapprovaldata.CreditdaysId).FirstOrDefault();
        //                    }
        //                    var employeedata = obj.Employees.Where(x => x.EmployeeNo == creditapprovaldata.EmployeeNo).ToList();
        //                    foreach (var item in employeedata)
        //                    {
        //                        employee.EmployeeNo = item.EmployeeNo;
        //                        employee.Name = item.Name;
        //                    }
        //                }
        //            }
        //        }
        //        return employee;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //getting approved items and also go by filters to get approved items
        public List<loadtaxesbyitemwise> GetItemsByMasterIDs(PADetailsModel masters)
        {
            //List<LoadItemsByID> view = new List<LoadItemsByID>();
            try
            {
                using (YSCMEntities yscm = new YSCMEntities())
                {
                    var sqlquery = "";
                    sqlquery = "select * from loadtaxesbyitemwise where itemstatus='Approved' and Mprrfqsplititemid not in(select Mprrfqsplititemid  from PAItem inner join MPRPADetails on PAItem.PAID = MPRPADetails.PAId and(DeleteFlag = 0 or DeleteFlag is null)  and Mprrfqsplititemid is not null) ";
                    if (masters.venderid != 0)
                        sqlquery += " and VendorId='" + masters.venderid + "'";
                    if (masters.RevisionId != 0)
                        sqlquery += " and MPRRevisionId='" + masters.RevisionId + "'";
                    if (masters.RFQNo != null && masters.RFQNo != "")
                        sqlquery += " and RFQNo='" + masters.RFQNo + "'";
                    if (masters.DocumentNumber != null && masters.DocumentNumber != "")
                        sqlquery += " and DocumentNo='" + masters.DocumentNumber + "'";
                    if (masters.BuyerGroupId != 0)
                        sqlquery += " and BuyerGroupId='" + masters.BuyerGroupId + "'";
                    if (masters.SaleOrderNo != null && masters.SaleOrderNo != "")
                        sqlquery += " and SaleOrderNo='" + masters.SaleOrderNo + "'";
                    if (masters.DepartmentId != 0)
                        sqlquery += " and DepartmentId='" + masters.DepartmentId + "'";
                    //if (masters.EmployeeNo != null)
                    //    sqlquery += " and DepartmentId='" + masters.EmployeeNo + "'";
                    if (masters.vendorProjectManager != null && masters.vendorProjectManager != "")
                        sqlquery += " and ProjectManager='" + masters.vendorProjectManager + "'";

                    return yscm.Database.SqlQuery<loadtaxesbyitemwise>(sqlquery).ToList();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<RfqRevisionModel> GetLocalRfqDetailsById(int revisionId)
        {
            RfqRevisionModel revision = new RfqRevisionModel();
            try
            {
                var localrevision = obj.RFQRevisions_N.Where(x => x.rfqRevisionId == revisionId && x.DeleteFlag == false).Include(x => x.RFQMaster).Include(x => x.RFQItems_N).FirstOrDefault();
                if (localrevision != null)
                {
                    revision.RfqMasterId = localrevision.rfqMasterId;
                    revision.RfqRevisionNo = localrevision.RevisionNo;
                    revision.CreatedBy = localrevision.CreatedBy;
                    revision.CreatedDate = localrevision.CreatedDate;
                    revision.PackingForwading = localrevision.PackingForwarding;
                    revision.salesTax = localrevision.SalesTax;
                    revision.Insurance = localrevision.Insurance;
                    revision.CustomsDuty = localrevision.CustomsDuty;
                    revision.PaymentTermDays = localrevision.PaymentTermDays;
                    revision.PaymentTermRemarks = localrevision.PaymentTermRemarks;
                    revision.BankGuarantee = localrevision.BankGuarantee;
                    revision.DeliveryMaxWeeks = localrevision.DeliveryMaxWeeks;
                    revision.DeliveryMinWeeks = localrevision.DeliveryMinWeeks;


                    var rfqmasters = obj.RFQMasters.Where(x => x.RfqMasterId == localrevision.rfqMasterId).ToList();
                    var masters = new RFQMasterModel();
                    var vendors = new VendormasterModel();
                    if (rfqmasters != null)
                    {
                        foreach (var item in rfqmasters)
                        {
                            masters.RfqMasterId = item.RfqMasterId;
                            masters.RFQNo = item.RFQNo;
                            masters.RfqUniqueNo = item.RFQUniqueNo;
                            masters.VendorId = item.VendorId;
                            var vendormasters = obj.VendorMasters.Where(x => x.Vendorid == masters.VendorId).FirstOrDefault();
                            masters.Vendor = new VendormasterModel()
                            {
                                ContactNumber = vendormasters.ContactNo,
                                VendorCode = vendormasters.VendorCode,
                                VendorName = vendormasters.VendorName,
                                Emailid = vendormasters.Emailid,
                                Street = vendormasters.Street,
                            };
                            masters.MPRRevisionId = (int)item.MPRRevisionId;
                            masters.CreatedBy = item.CreatedBy;
                        }
                    }
                    revision.rfqmaster = masters;
                    var vendordata = obj.VendorMasters.Where(x => x.Vendorid == masters.VendorId).FirstOrDefault();
                    if (vendordata != null)
                    {
                        revision.VendorName = vendordata.VendorName;
                    }

                    var rfqitemss = obj.RFQItems.Where(x => x.RFQRevisionId == localrevision.rfqRevisionId).ToList();
                    if (rfqitemss != null)
                    {
                        foreach (var item in rfqitemss)
                        {
                            RfqItemModel rfqitems = new RfqItemModel();
                            rfqitems.HSNCode = item.HSNCode;
                            rfqitems.MRPItemsDetailsID = item.MPRItemDetailsid;
                            rfqitems.QuotationQty = item.QuotationQty;
                            rfqitems.RFQRevisionId = item.RFQRevisionId;
                            rfqitems.RFQItemID = item.RFQItemsId;
                            revision.rfqitem.Add(rfqitems);
                        }
                    }
                }
                var rfqterms = obj.RFQTerms.Where(x => x.RFQrevisionId == revisionId).ToList();

                RFQTermsModel terms = new RFQTermsModel();
                foreach (var item in rfqterms)
                {
                    terms.termsid = item.termsid;
                    terms.RfqTermsid = item.RfqTermsid;
                    terms.Remarks = item.Remarks;
                    terms.VendorResponse = item.VendorResponse;
                    revision.RFQTerms.Add(terms);
                }
                return revision;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting all departments
        public async Task<List<DepartmentModel>> GetAllDepartments()
        {
            List<DepartmentModel> model = new List<DepartmentModel>();
            try
            {
                var departments = obj.Departments.ToList();
                foreach (var item in departments)
                {
                    model.Add(new DepartmentModel()
                    {
                        DepartmentID = item.DepartmentId,
                        DepartmentName = item.DepartmentName
                    });
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting mapped slabs to department by department id
        public async Task<List<PAAuthorizationLimitModel>> GetSlabsByDepartmentID(int DeptID)
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            try
            {
                var data = obj.PAAuthorizationLimits.Where(x => x.DeptId == DeptID && x.DeleteFlag == false).ToList();
                if (data != null)
                {
                    model = data.Select(x => new PAAuthorizationLimitModel()
                    {
                        MaxPAValue = x.MaxPAValue,
                        MinPAValue = x.MinPAValue,
                        Authid = x.Authid
                    }).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting all the employees
        public async Task<List<EmployeModel>> GetAllEmployee()
        {
            List<EmployeModel> model = new List<EmployeModel>();
            try
            {
                var data = obj.Employees.ToList();
                if (data != null)
                {
                    model = data.Select(x => new EmployeModel()
                    {
                        EmployeeNo = x.EmployeeNo,
                        Name = x.Name
                    }).ToList();
                }
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //getting all the credit days mapped
        public async Task<List<PAAuthorizationLimitModel>> GetAllCredits()
        {
            List<PAAuthorizationLimitModel> model = new List<PAAuthorizationLimitModel>();
            try
            {
                //padata.AuthorizationType.ToLower().Equals("pa")
                var data = obj.PAAuthorizationLimits.Where(x => x.AuthorizationType.ToLower() == "cr").ToList();
                //var mappingdata = obj.PAAuthorizationEmployeeMappings.ToList();
                foreach (var item in data)
                {
                    model.Add(new PAAuthorizationLimitModel()
                    {
                        Authid = item.Authid,
                        MinPAValue = item.MinPAValue,
                        MaxPAValue = item.MaxPAValue,
                        //PAAuthorizationEmployeeMappings=mappingdata.Where(x=>x.Authid== item.Authid).Select(x=>new PAAuthorizationEmployeeMappingModel()
                        //{
                        //    Employeeid=x.Employeeid,
                        //    AuthLevel=x.AuthLevel
                        //}).ToList()
                    });
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<GetAllCreditDays>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting credt days limits>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<PACreditDaysMasterModel>> GetAllCreditDays()
        {
            List<PACreditDaysMasterModel> model = new List<PACreditDaysMasterModel>();
            try
            {
                var credit = obj.PACreditDaysMasters.ToList();
                if (credit != null)
                {
                    model = credit.Select(x => new PACreditDaysMasterModel()
                    {
                        CreditDaysid = x.CreditDaysid,
                        MinDays = x.MinDays,
                        MaxDays = x.MaxDays
                    }).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting the mpr purcahse modes
        /*Name of Function : <<GetAllMprPAPurchaseModes>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting the mpr purcahse modes>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<MPRPAPurchaseModesModel>> GetAllMprPAPurchaseModes()
        {
            List<MPRPAPurchaseModesModel> model = new List<MPRPAPurchaseModesModel>();
            try
            {
                var data = obj.MPRPAPurchaseModes.Where(x => x.BoolInUse == true).ToList();
                model = data.Select(x => new MPRPAPurchaseModesModel()
                {
                    PurchaseModeId = x.PurchaseModeId,
                    PurchaseMode = x.PurchaseMode,
                    XOrder = x.XOrder
                }).ToList();

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //getting mpr purchase types
        /*Name of Function : <<GetAllMprPAPurchaseTypes>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<GetAllMprPAPurchaseTypes>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<MPRPAPurchaseTypesModel>> GetAllMprPAPurchaseTypes()
        {
            List<MPRPAPurchaseTypesModel> model = new List<MPRPAPurchaseTypesModel>();
            try
            {
                var data = obj.MPRPAPurchaseTypes.Where(x => x.BoolInUse == true).ToList();
                model = data.Select(x => new MPRPAPurchaseTypesModel()
                {
                    PurchaseTypeId = x.PurchaseTypeId,
                    PurchaseType = x.PurchaseType,
                    XOrder = x.XOrder
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //generating the purchase authorization for approve ditems
        /*Name of Function : <<InsertPurchaseAuthorization>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<InsertPurchaseAuthorization>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> InsertPurchaseAuthorization(MPRPADetailsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var dateAndTime = DateTime.Now;
                var authorization = new MPRPADetail();
                if (model != null)
                {
                    authorization.PurchaseTypeId = model.PurchaseTypeId;
                    authorization.PurchaseModeId = model.PurchaseModeId;
                    authorization.RequestedOn = dateAndTime.Date;
                    authorization.RequestedBy = model.RequestedBy;
                    authorization.DepartmentID = model.DepartmentID;
                    authorization.BuyerGroupId = model.BuyerGroupId;
                    authorization.ProjectCode = model.ProjectCode;
                    authorization.ProjectName = model.ProjectName;
                    authorization.PackagingForwarding = model.PackagingForwarding;
                    authorization.Taxes = model.Taxes;
                    authorization.Freight = model.Freight;
                    authorization.Insurance = model.Insurance;
                    authorization.DeliveryCondition = model.DeliveryCondition;
                    authorization.ShipmentMode = model.ShipmentMode;
                    authorization.PaymentTerms = model.PaymentTerms;
                    authorization.CreditDays = model.CreditDays;
                    authorization.Warranty = model.Warranty;
                    authorization.BankGuarantee = model.BankGuarantee;
                    authorization.LDPenaltyTerms = model.LDPenaltyTerms;
                    authorization.SpecialInstructions = model.SpecialInstructions;
                    authorization.FactorsForImports = model.FactorsForImports;
                    authorization.SpecialRemarks = model.SpecialRemarks;
                    authorization.SuppliersReference = model.SuppliersReference;
                    authorization.VendorId = model.VendorId;
                    authorization.PAStatus = "Inprogress";
                    authorization.DeleteFlag = false;
                    authorization.POtype = model.potype;
                    authorization.Aribarequired = model.AribaRequired;
                    authorization.MSArequired = model.msarequired;
                    authorization.incoterms = model.incoterms;
                    obj.MPRPADetails.Add(authorization);
                    obj.SaveChanges();
                    status.Sid = authorization.PAId;

                    List<int> revisionid = model.Item.Select(x => x.MPRRevisionId).Distinct().ToList();
                    foreach (var item in revisionid)
                    {
                        var data = new MPRStatusTrack();
                        data.StatusId = 11;
                        int requisitionid = obj.MPRRevisions.Where(x => x.RevisionId == item).FirstOrDefault().RequisitionId;
                        data.RequisitionId = requisitionid;
                        data.RevisionId = item;
                        data.UpdatedDate = dateAndTime.Date;
                        data.UpdatedBy = model.LoginEmployee;
                        obj.MPRStatusTracks.Add(data);
                        obj.SaveChanges();

                        var data1 = new MPRRevision();
                        data1 = obj.MPRRevisions.Where(x => x.RevisionId == item).FirstOrDefault();
                        data1.StatusId = Convert.ToByte(data.StatusId);
                        obj.SaveChanges();
                    }
                    //model.Item.Select(x => x.MPRItemDetailsid).FirstOrDefault()

                    //foreach (var item in model.Item.GroupBy(n => n.RFQItemsId).Select(x => x.FirstOrDefault()))
                    //{
                    //    var itemdata = obj.RFQItemsInfo_N.Where(x => x.RFQItemsId == item.RFQItemsId).ToList();
                    //    foreach (var items in itemdata)
                    //    {
                    //        PAItem paitem = new PAItem()
                    //        {
                    //            PAID = status.Sid,
                    //            RfqSplitItemId = items.RFQSplitItemId,
                    //            Mprrfqsplititemid = item.Mprrfqsplititemid,
                    //            MPRItemDetailsId = item.MPRItemDetailsid
                    //        };
                    //        obj.PAItems.Add(paitem);
                    //        obj.SaveChanges();
                    //    }

                    //}


                    foreach (var items in model.Item)
                    {
                        var splitdata = obj.MPRRfqItemInfos.Where(x => x.Mprrfqsplititemid == items.Mprrfqsplititemid).FirstOrDefault().rfqsplititemid;
                        PAItem paitem = new PAItem()
                        {
                            PAID = status.Sid,
                            RfqSplitItemId = Convert.ToInt32(splitdata),
                            Mprrfqsplititemid = items.Mprrfqsplititemid,
                            MPRItemDetailsId = items.MPRItemDetailsid,
                            //PODescription = items.PODescription,
                            //POText = items.POText,
                            //POItemType = items.itemtypesupplier
                        };
                        obj.PAItems.Add(paitem);
                        obj.SaveChanges();
                    }


                }
                else
                {

                }

                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<finalpa>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Inserting approvers to the generated pa based on pa value,target spend and credit days>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> finalpa(MPRPADetailsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var dateAndTime = DateTime.Now;
                var authorization = new MPRPADetail();
                if (model != null)
                {
                    var Approveritem = new MPRPAApprover();
                    foreach (var item in model.ApproversList)
                    {

                        Approveritem.PAId = model.PAId;
                        Approveritem.ApproverLevel = 1;
                        Approveritem.RoleName = item.rolename;
                        Approveritem.Approver = item.Approver;
                        Approveritem.ApproversRemarks = item.ApproversRemarks;
                        Approveritem.ApprovalStatus = "submitted";
                        Approveritem.ApprovedOn = dateAndTime.Date;

                        obj.MPRPAApprovers.Add(Approveritem);
                        obj.SaveChanges();
                    }

                    this.emailDA.PAEmailRequest(model.PAId, model.LoginEmployee);

                    var data = obj.MPRPADetails.Where(x => x.PAId == model.PAId).FirstOrDefault();
                    data.PAStatus = "Submitted";
                    obj.SaveChanges();

                }
                else
                {

                }

                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //getting the generated purchase authorization by pa id
        /*Name of Function : <<GetMPRPADeatilsByPAID>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting pa details by paid>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<MPRPADetailsModel> GetMPRPADeatilsByPAID(int PID)
        {
            MPRPADetailsModel model = new MPRPADetailsModel();
            try
            {
                var data = obj.MPRPADetails.Where(x => x.PAId == PID).FirstOrDefault();

                if (data != null)
                {
                    model.PurchaseTypeId = data.PurchaseTypeId;
                    var purchasetype = obj.MPRPAPurchaseTypes.Where(x => x.PurchaseTypeId == model.PurchaseTypeId).FirstOrDefault();
                    model.purchasetypes.PurchaseType = purchasetype.PurchaseType;
                    model.PurchaseModeId = data.PurchaseModeId;
                    var purchasemode = obj.MPRPAPurchaseModes.Where(x => x.PurchaseModeId == model.PurchaseModeId).FirstOrDefault();
                    model.purchasemodes.PurchaseMode = purchasemode.PurchaseMode;
                    model.RequestedOn = data.RequestedOn;
                    model.RequestedBy = data.RequestedBy;
                    model.DepartmentID = data.DepartmentID;
                    var department = obj.MPRDepartments.Where(x => x.DepartmentId == model.DepartmentID).FirstOrDefault();
                    model.department.Department = department.Department;
                    model.BuyerGroupId = data.BuyerGroupId;
                    var buyergroup = obj.MPRBuyerGroups.Where(x => x.BuyerGroupId == model.BuyerGroupId).FirstOrDefault();
                    model.buyergroup.BuyerGroup = buyergroup.BuyerGroup;
                    model.ProjectCode = data.ProjectCode;
                    model.ProjectName = data.ProjectName;
                    model.PackagingForwarding = data.PackagingForwarding;
                    model.Taxes = data.Taxes;
                    model.Freight = data.Freight;
                    model.Insurance = data.Insurance;
                    model.PAStatus = data.PAStatus;
                    model.DeliveryCondition = data.DeliveryCondition;
                    model.ShipmentMode = data.ShipmentMode;
                    model.PaymentTerms = data.PaymentTerms;
                    model.CreditDays = data.CreditDays;
                    model.Warranty = data.Warranty;
                    model.BankGuarantee = data.BankGuarantee;
                    model.LDPenaltyTerms = data.LDPenaltyTerms;
                    model.SpecialInstructions = data.SpecialInstructions;
                    model.FactorsForImports = data.FactorsForImports;
                    model.SpecialRemarks = data.SpecialRemarks;
                    model.SuppliersReference = data.SuppliersReference;
                    model.potype = data.POtype;
                    model.AribaRequired = data.Aribarequired;
                    model.msarequired = data.MSArequired;
                    model.Deleteflag = data.DeleteFlag;

                    var statusdata = obj.ShowAdditionalcharges.Where(x => x.itemstatus == "Approved" && x.PAId == PID).ToList();
                    model.Item = statusdata.Select(x => new RfqItemModel()
                    {
                        ItemDescription = x.ItemDescription,
                        UnitPrice = Convert.ToDecimal(x.UnitPrice),
                        QuotationQty = Convert.ToDouble(x.QuotationQty),
                        DocumentNo = x.DocumentNo,
                        SaleOrderNo = x.SaleOrderNo,
                        Department = x.Department,
                        TargetSpend = Convert.ToDecimal(x.TargetSpend),
                        PaymentTermCode = x.PaymentTermCode,
                        VendorName = x.VendorName,
                        DepartmentId = x.DepartmentId,
                        MRPItemsDetailsID = Convert.ToInt32(x.MPRItemDetailsid),
                        RFQRevisionId = x.rfqRevisionId,
                        paid = x.PAId,
                        paitemid = x.PAItemID,
                        POItemNo = x.POItemNo,
                        PONO = x.PONO,
                        Remarks = x.Remarks,
                        POStatusDate = x.PODate,
                        RFQNo = x.RFQNo,
                        HSNCode = x.HSNCode,
                        TotalPFAmount = x.pfamounts,
                        FreightAmount = x.FreightAmount,
                        FreightPercentage = x.FreightPercentage,
                        PFAmount = x.PFAmount,
                        PFPercentage = x.PFPercentage,
                        TotalFreightAmount = x.freightamounts,
                        HandlingAmount = x.HandlingAmount,
                        ImportFreightAmount = x.ImportFreightAmount,
                        DutyAmount = x.DutyAmount,
                        InsuranceAmount = x.InsuranceAmount,
                        MPRRevisionId = Convert.ToInt32(x.MPRRevisionId),
                        POText = x.POText,
                        PODescription = x.PODescription,
                        itemtypesupplier = x.POItemType,
                        rawdiscount = x.rawdiscount
                    }).ToList();
                    //var taxes = obj.ShowAdditionalcharges.Where(x => x.PAId == PID).ToList();
                    //model.additionaltaxes = taxes.Select(x => new Additionaltaxes()
                    //{
                    //    HandlingAmount=x.HandlingAmount,
                    //    ImportFreightAmount=x.ImportFreightAmount,
                    //    DutyAmount=x.DutyAmount,
                    //    InsuranceAmount=x.InsuranceAmount
                    //}).ToList();
                    var approverdata = obj.GetmprApproverdeatils.Where(x => x.PAId == PID).ToList();
                    model.ApproversList = approverdata.Select(x => new MPRPAApproversModel()
                    {
                        ApproverName = x.Name,
                        RoleName = x.RoleName,
                        ApproversRemarks = x.ApproversRemarks,
                        ApprovalStatus = x.ApprovalStatus,
                        EmployeeNo = x.Approver,
                        ApprovedOn = x.ApprovedOn,
                        parequested = x.parequested,
                        PARequestedOn = x.RequestedOn
                    }).ToList();
                    var documentsdata = obj.MPRPADocuments.Where(x => x.paid == PID && x.deleteflag == false).ToList();
                    if (documentsdata.Count != 0)
                    {

                        model.documents = documentsdata.Select(x => new PADocumentsmodel()
                        {
                            filename = x.Filename,
                            path = x.Filepath,
                            uploadeddate = x.uploadeddate,
                            DocumentId = x.DocumentId
                        }).ToList();

                    }

                    var requested = obj.parequestedanddeletedemployees.Where(x => x.paid == PID).ToList();
                    model.request = requested.Select(x => new parequestedanddeletemodel()
                    {
                        parequested = x.parequested,
                        RequestedOn = x.RequestedOn,
                        PAStatus = x.PAStatus,
                        PAStatusUpdate = x.PAStatusUpdate,
                        DeleteFlag = x.DeleteFlag,
                        DeleteBy = x.DeleteBy,
                        DeleteOn = x.DeleteOn,
                        padeleted = x.padeleted,
                        Remarks = x.Remarks
                    }).ToList();

                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //retreving the generated pa total list
        public async Task<List<MPRPADetailsModel>> GetAllMPRPAList()
        {
            List<MPRPADetailsModel> model = new List<MPRPADetailsModel>();
            try
            {
                var data = obj.MPRPADetails.ToList();
                if (data != null)
                {
                    model = data.Select(c => new MPRPADetailsModel()
                    {
                        PAId = c.PAId,
                        RequestedBy = c.RequestedBy,
                        RequestedOn = c.RequestedOn
                    }).ToList();
                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting the functinal role of approvers 
        /*Name of Function : <<GetAllPAFunctionalRoles>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting functional roles like pm,UH,... >>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<PAFunctionalRolesModel>> GetAllPAFunctionalRoles()
        {
            List<PAFunctionalRolesModel> model = new List<PAFunctionalRolesModel>();
            try
            {
                var roles = obj.PAFunctionalRoles.OrderBy(x => x.XOrder).ToList();
                if (roles != null)
                {
                    model = roles.Select(x => new PAFunctionalRolesModel()
                    {
                        FunctionalRoleId = x.FunctionalRoleId,
                        FunctionalRole = x.FunctionalRole,
                        XOrder = x.XOrder
                    }).ToList();
                }
                else
                {
                    return model;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<GetCreditSlabsandemployees>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting employee based on credit days>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<EmployeemappingtocreditModel>> GetCreditSlabsandemployees()
        {
            List<EmployeemappingtocreditModel> model = new List<EmployeemappingtocreditModel>();
            try
            {
                var data = obj.Employeemappingtocredits.OrderByDescending(x => x.CreditdaysId).ToList();
                if (data != null)
                {
                    model = data.Select(x => new EmployeemappingtocreditModel()
                    {
                        Authid = x.Authid,
                        AuthorizationType = x.AuthorizationType,
                        CreditdaysId = x.CreditdaysId,
                        DeptId = x.DeptId,
                        EmployeeNo = x.EmployeeNo,
                        Name = x.Name,
                        MinDays = x.MinDays,
                        MaxDays = x.MaxDays,
                        MinPAValue = x.MinPAValue,
                        MaxPAValue = x.MaxPAValue,
                        CRApprovalId = x.CRApprovalId
                    }).ToList();
                }
                else
                {
                    return model;
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<EmployeemappingtopurchaseModel>> GetPurchaseSlabsandMappedemployees()
        {
            List<EmployeemappingtopurchaseModel> model = new List<EmployeemappingtopurchaseModel>();
            try
            {
                var data = obj.Employeemappingtopurchases.OrderBy(x => x.Authid).ToList();
                if (data != null)
                {
                    model = data.Select(x => new EmployeemappingtopurchaseModel()
                    {
                        Authid = x.Authid,
                        AuthorizationType = x.AuthorizationType,
                        MaxPAValue = x.MaxPAValue,
                        MinPAValue = x.MinPAValue,
                        Employeeid = x.Employeeid,
                        LessBudget = x.LessBudget,
                        MoreBudget = x.MoreBudget,
                        DepartmentName = x.Department,
                        Name = x.Name,
                        FunctionalRoleId = x.FunctionalRoleId,
                        PAmapid = x.PAmapid
                    }).ToList();
                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting allthe project managers based on employeeno
        /*Name of Function : <<LoadAllProjectManagers>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting project managers from mprrevsions table>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<ProjectManagerModel>> LoadAllProjectManagers()
        {
            List<ProjectManagerModel> model = new List<ProjectManagerModel>();
            try
            {
                var data = obj.MPRRevisions.Where(x => x.BoolValidRevision == true).Select(x => x.ProjectManager).Distinct().ToList();
                if (data != null)
                {
                    var vmodel = obj.Employees.Where(x => data.Contains(x.EmployeeNo)).ToList();
                    model = vmodel.Select(x => new ProjectManagerModel()
                    {
                        EmployeeNo = x.EmployeeNo,
                        Name = x.Name
                    }).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<LoadVendorByMprDetailsId>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting vendor by item wise>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<VendormasterModel>> LoadVendorByMprDetailsId(List<int?> MPRItemDetailsid)
        {
            List<VendormasterModel> model = new List<VendormasterModel>();
            try
            {
                //var data = obj.LoadItemsByIDs.Where(x => MPRItemDetailsid.Contains(x.MPRItemDetailsid)).ToList();
                var vendor = (from xx in obj.LoadItemsByIDs select xx).Where(y => MPRItemDetailsid.Contains(y.MPRItemDetailsid)).GroupBy(n => new { n.VendorId, n.VendorName }).Select(x => x.FirstOrDefault()).ToList();
                //var data = obj.LoadItemsByIDs.Distinct().Where(x => MPRItemDetailsid.Contains(x.MPRItemDetailsid)).Distinct().ToList();
                model = vendor.Select(x => new VendormasterModel()
                {
                    VendorName = x.VendorName,
                    Vendorid = x.VendorId
                }).ToList();
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<MPRPAApproversModel>> GetAllApproversList()
        {
            List<MPRPAApproversModel> model = new List<MPRPAApproversModel>();
            try
            {
                var data = obj.MPRPAApprovers.ToList();
                if (data != null)
                {
                    model = data.Select(x => new MPRPAApproversModel()
                    {
                        PAId = x.PAId,
                        ApproverLevel = x.ApproverLevel,
                        RoleName = x.RoleName,
                        ApproverName = x.Approver,
                        ApproversRemarks = x.ApproversRemarks,
                        ApprovalStatus = x.ApprovalStatus,
                        ApprovedOn = x.ApprovedOn
                    }).ToList();
                    return model;
                }
                else
                {
                    return model;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting pa approved,pending and submitted
        /*Name of Function : <<GetMprApproverDetailsBySearch>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting pa approved,pending and submitted>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<mprApproverdetailsview>> GetMprApproverDetailsBySearch(PAApproverDetailsInputModel model)
        {
            List<mprApproverdetailsview> details = new List<mprApproverdetailsview>();
            int mprno = 0;
            int rfqno = 0;
            if (model.DocumentNumber != null && model.DocumentNumber != "")
            {
                if (model.DocumentNumber.StartsWith("MPR", StringComparison.CurrentCultureIgnoreCase))
                {
                    model.DocumentNumber = model.DocumentNumber;
                }
                else
                {
                    mprno = Convert.ToInt32(model.DocumentNumber);
                    model.DocumentNumber = null;
                }
            }
            if (model.rfqnumber != null && model.rfqnumber != null)
            {
                if (model.rfqnumber.StartsWith("RFQ", StringComparison.CurrentCultureIgnoreCase))
                {
                    model.rfqnumber = model.rfqnumber;
                }
                else
                {
                    rfqno = Convert.ToInt32(model.rfqnumber);
                    model.rfqnumber = null;
                }
            }
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                var sqlquery = "";
                sqlquery = "select * from mprApproverdetailsview where Approver='" + model.CreatedBy + "' and PAStatus not in ('Rejected','InProgress') and DeleteFlag=0 ";
                if (model.Paid != 0)
                    sqlquery += " and  PAId='" + model.Paid + "'";
                if (model.Status != null)
                    sqlquery += " and ApprovalStatus='" + model.Status + "'";
                if (model.FromDate != null && model.ToDate != null)
                    sqlquery += " and RequestedOn between '" + model.FromDate + "' and '" + model.ToDate + "'";
                if (model.Status == null)
                    sqlquery += " and ApprovalStatus in('submitted', 'pending') ";
                if (model.rfqnumber != null && model.rfqnumber != "")
                    sqlquery += " and  RFQNo='" + model.rfqnumber + "'";
                if (model.DocumentNumber != null && model.DocumentNumber != "")
                    sqlquery += " and  DocumentNo='" + model.DocumentNumber + "'";
                if (model.VendorId != 0)
                    sqlquery += " and  Vendorid='" + model.VendorId + "'";
                if (model.DepartmentId != 0)
                    sqlquery += " and  DepartmentID='" + model.DepartmentId + "'";
                if (mprno != 0)
                    sqlquery += " and MPRSeqNo = '" + mprno + "'";
                if (rfqno != 0)
                    sqlquery += " and RFQUniqueNo ='" + rfqno + "'";

                sqlquery += "  order by PAId desc ";

                details = obj.Database.SqlQuery<mprApproverdetailsview>(sqlquery).ToList();
                return details;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //updating the pa status based on paid
        /*Name of Function : <<UpdateMprpaApproverStatus>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<updating assigned approver status after approving or rejection of pa>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> UpdateMprpaApproverStatus(MPRPAApproversModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var approverdata = obj.MPRPAApprovers.Where(x => x.PAId == model.PAId && x.Approver == model.EmployeeNo).ToList();
                if (approverdata != null)
                {
                    foreach (var data in approverdata)
                    {
                        data.ApproversRemarks = model.ApproversRemarks;
                        data.ApprovalStatus = model.ApprovalStatus;
                        data.ApprovedOn = System.DateTime.Now;
                        obj.SaveChanges();
                    }
                    //approverdata.ApproversRemarks = model.ApproversRemarks;
                    //approverdata.ApprovalStatus = model.ApprovalStatus;
                    //approverdata.ApprovedOn = System.DateTime.Now;
                    //obj.SaveChanges();
                    //status.Sid = approverdata.PAId;
                    UpdatePAStatus(model.PAId, model.mprrevisionid, model.EmployeeNo, model.ApprovalStatus);
                    return status;
                }
                else
                {
                    return status;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //continuation to above api
        /*Name of Function : <<UpdatePAStatus>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<updating the pastatus after approver approving or rejection>>
Review Date :<<>>   Reviewed By :<<>>*/
        public bool UpdatePAStatus(int paid, int mprrevisionid, string employeeno, string ApprovalStatus)
        {
            List<approverFinalview> approver = new List<approverFinalview>();
            int statusid = 0;
            var sqlquery = "";
            var padetails = obj.MPRPADetails.Where(x => x.PAId == paid).FirstOrDefault();
            var pastatus = obj.MPRRevisions.Where(x => x.RevisionId == mprrevisionid).FirstOrDefault();
            sqlquery = "select * from approverFinalview where PAId='" + paid + "' ";
            approver = obj.Database.SqlQuery<approverFinalview>(sqlquery).ToList();
            if (ApprovalStatus == "Approved")
                statusid = 18;
            else if (ApprovalStatus == "Rejected")
                statusid = 21;

            if (approver == null || approver.Count == 0)
            {
                MPRStatusTrack statustrack = new MPRStatusTrack();
                statustrack.StatusId = statusid;
                statustrack.RevisionId = mprrevisionid;
                int requisitionid = obj.MPRRevisions.Where(x => x.RevisionId == mprrevisionid).FirstOrDefault().RequisitionId;
                statustrack.RequisitionId = requisitionid;
                statustrack.UpdatedBy = employeeno;
                statustrack.UpdatedDate = System.DateTime.Now;
                //statustrack.Status = "PA Approved";
                obj.MPRStatusTracks.Add(statustrack);
                obj.SaveChanges();
                if (padetails != null)
                    padetails.PAStatus = ApprovalStatus;
                padetails.POStatus = "Pending";
                padetails.POStatusUpdate = System.DateTime.Now;
                padetails.PAStatusUpdate = System.DateTime.Now;
                obj.SaveChanges();
                pastatus.StatusId = 18;
                obj.SaveChanges();

                this.emailDA.paemailstatus(statusid, paid, mprrevisionid, ApprovalStatus, employeeno);
                int id = statustrack.StatusId;
                updatepaitems(mprrevisionid, paid);
            }
            else if (ApprovalStatus == "Rejected")
            {
                MPRStatusTrack statustrack = new MPRStatusTrack();
                statustrack.StatusId = statusid;
                statustrack.RevisionId = mprrevisionid;
                int requisitionid = obj.MPRRevisions.Where(x => x.RevisionId == mprrevisionid).FirstOrDefault().RequisitionId;
                statustrack.RequisitionId = requisitionid;
                statustrack.UpdatedBy = employeeno;
                statustrack.UpdatedDate = System.DateTime.Now;
                //statustrack.Status = "PA Approved";
                obj.MPRStatusTracks.Add(statustrack);
                obj.SaveChanges();

                if (padetails != null)
                    padetails.PAStatus = ApprovalStatus;
                obj.SaveChanges();

                pastatus.StatusId = 21;
                obj.SaveChanges();

                this.emailDA.paemailstatus(statusid, paid, mprrevisionid, ApprovalStatus, employeeno);

            }
            else
            {
                MPRStatusTrack statustrack = new MPRStatusTrack();
                statustrack.StatusId = statusid;
                statustrack.RevisionId = mprrevisionid;
                int requisitionid = obj.MPRRevisions.Where(x => x.RevisionId == mprrevisionid).FirstOrDefault().RequisitionId;
                statustrack.RequisitionId = requisitionid;
                statustrack.UpdatedBy = employeeno;
                statustrack.UpdatedDate = System.DateTime.Now;
                //statustrack.Status = "PA Approved";
                obj.MPRStatusTracks.Add(statustrack);
                obj.SaveChanges();
                padetails.PAStatus = "Pending";
                padetails.POStatus = "Pending";
                obj.SaveChanges();
            }
            //approver.approved;
            return true;
        }
        /*Name of Function : <<updatepaitems>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<updatepaitems>>
Review Date :<<>>   Reviewed By :<<>>*/
        public bool updatepaitems(int mprrevisionid, int paid)
        {
            List<Updatepaitem> items = new List<Updatepaitem>();
            try
            {
                var sqlquery = "";
                sqlquery = "select * from Updatepaitem where MPRRevisionId='" + mprrevisionid + "' and paid='" + paid + "' ";
                items = obj.Database.SqlQuery<Updatepaitem>(sqlquery).ToList();
                List<MPRRfqItemInfo> paitem = new List<MPRRfqItemInfo>();
                foreach (var item in items)
                {
                    var mprrfq = new MPRRfqItemInfo()
                    {
                        PAItemID = item.paitemid,
                    };
                    obj.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<DisplayRfqTermsByRevisionId>> getrfqtermsbyrevisionid(List<int> RevisionId)
        {
            List<DisplayRfqTermsByRevisionId> revision = new List<DisplayRfqTermsByRevisionId>();
            try
            {
                revision = obj.DisplayRfqTermsByRevisionIds.Where(x => RevisionId.Contains(x.RFQrevisionId)).ToList();
                return revision;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public async Task<List<EmployeemappingtopurchaseModel>> GetPurchaseSlabsandMappedemployeesByDeptId(int deptid)
        //{
        //    List<EmployeemappingtopurchaseModel> model = new List<EmployeemappingtopurchaseModel>();
        //    try
        //    {
        //        var data = obj.Employeemappingtopurchases.Where(x => x.DeptId == deptid).ToList();
        //        if (data != null)
        //        {
        //            model = data.Select(x => new EmployeemappingtopurchaseModel()
        //            {
        //                Authid = x.Authid,
        //                AuthorizationType = x.AuthorizationType,
        //                MaxPAValue = x.MaxPAValue,
        //                MinPAValue = x.MinPAValue,
        //                Employeeid = x.Employeeid,
        //                LessBudget = x.LessBudget,
        //                MoreBudget = x.MoreBudget,
        //                DepartmentName = x.Department,
        //                Name = x.Name,
        //                FunctionalRoleId = x.FunctionalRoleId,
        //                PAmapid = x.PAmapid
        //            }).ToList();
        //            return model;
        //        }
        //        else
        //        {
        //            return model;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}


        /*Name of Function : <<GetPurchaseSlabsandMappedemployeesByDeptId>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting mapped salbs and corresponding employee based on budget or deptid or employee id>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<Employeemappingtopurchase>> GetPurchaseSlabsandMappedemployeesByDeptId(EmployeeFilterModel model)
        {
            List<Employeemappingtopurchase> purchase = new List<Employeemappingtopurchase>();
            try
            {
                var sqlquery = "";
                sqlquery = "select * from Employeemappingtopurchase where AuthorizationType='PA' and DeleteFlag=0 ";
                if (model.DeptId != 0)
                    sqlquery += " and  DeptId='" + model.DeptId + "'";
                if (model.Employeeid != null && model.Employeeid != "0")
                    sqlquery += " and  Employeeid='" + model.Employeeid + "'";
                if (model.LessBudget != false)
                    sqlquery += " and  LessBudget='" + model.LessBudget + "'";
                if (model.MoreBudget != false)
                    sqlquery += " and  MoreBudget='" + model.MoreBudget + "'";
                if (model.Authid != 0)
                    sqlquery += " and  Authid='" + model.Authid + "'";
                purchase = obj.Database.SqlQuery<Employeemappingtopurchase>(sqlquery).ToList();

                return purchase;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public async Task<statuscheckmodel> InsertPaitems(ItemsViewModel paitem)
        //{
        //    statuscheckmodel status = new statuscheckmodel();
        //    try
        //    {
        //        var data = obj.PAItems.Where(x => x.PAItemID == paitem.paitemid).FirstOrDefault();
        //        data.PONO = paitem.PONO;
        //        data.POItemNo = paitem.POItemNo;
        //        data.PODate = System.DateTime.Now;
        //        data.Remarks = paitem.Remarks;
        //        data.MPRItemDetailsId = paitem.MRPItemsDetailsID;
        //        obj.SaveChanges();

        //        return status;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //inserting pa items after pa approval
        /*Name of Function : <<InsertPaitems>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<inserting pa items after pa approval>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> InsertPaitems(List<ItemsViewModel> paitem)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                foreach (var itemdata in paitem)
                {
                    var paitems = obj.PAItems.Where(x => x.PAItemID == itemdata.paitemid).FirstOrDefault();
                    if (paitems != null)
                    {
                        paitems.PONO = itemdata.PONO;
                        paitems.POItemNo = itemdata.POItemNo;
                        //paitems.PODate = DateTime.ParseExact(itemdata.PODate.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        //paitems.PODate = Convert.ToDateTime( DateTime.ParseExact(itemdata.PODate.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture));
                        //paitems.PODate = DateTime.ParseExact(itemdata.PODate.ToString(), "dd/MM/yyyy", null);
                        paitems.PODate = itemdata.POStatusDate;
                        paitems.Remarks = itemdata.Remarks;
                        paitems.UpdatedDate = System.DateTime.Now;
                        paitems.UpdatedBy = itemdata.EmployeeNo;
                        obj.SaveChanges();
                    }
                    status.Sid = paitems.PAID;
                }

                var sqlquery = "";
                sqlquery = "select * from  PAItem where  PONO  is null and paid= " + status.Sid + " ";
                List<PAItem> item = new List<PAItem>();
                item = obj.Database.SqlQuery<PAItem>(sqlquery).ToList();
                if (item.Count == 0)
                {
                    var data = obj.MPRPADetails.Where(x => x.PAId == status.Sid).FirstOrDefault();
                    data.POStatus = "PO Released";
                    data.POStatusUpdate = System.DateTime.Now;
                    obj.SaveChanges();
                    var mprstatus = obj.LoadItemsByPAIDs.Where(x => x.PAId == status.Sid).FirstOrDefault();
                    MPRStatusTrack status1 = new MPRStatusTrack();
                    status1.RevisionId = Convert.ToInt32(mprstatus.MPRRevisionId);
                    status1.RequisitionId = mprstatus.RequisitionId;
                    status1.StatusId = 12;
                    status1.UpdatedDate = System.DateTime.Now;
                    obj.MPRStatusTracks.Add(status1);
                    obj.SaveChanges();
                }
                else
                {

                }
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<GetAllMappedSlabs>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting all mapped slabs by department wise>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<GetMappedSlab>> GetAllMappedSlabs()
        {
            List<GetMappedSlab> slab = new List<GetMappedSlab>();
            try
            {
                slab = obj.GetMappedSlabs.Where(x => x.DeleteFlag == false).OrderBy(x => x.Authid).ToList();
                return slab;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //removing mapped slabs
        /*Name of Function : <<RemoveMappedSlab>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<removing mapped slabs>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> RemoveMappedSlab(PAAuthorizationLimitModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.PAAuthorizationLimits.Where(x => x.Authid == model.Authid).FirstOrDefault();
                if (data != null)
                {
                    data.DeleteFlag = true;
                    obj.SaveChanges();
                }
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting the all palist and also go by filetrs like mprno or vendor or buyergroup or rfqno or paid or postatus or pastatus or fromdate and todate
        /*Name of Function : <<getMprPaDetailsBySearch>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting the all palist and also go by filetrs like mprno or vendor or buyergroup or rfqno or paid or postatus or pastatus or fromdate and todate which will show pending,submitted and approve pa's>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<NewGetMprPaDetailsByFilter>> getMprPaDetailsBySearch(PADetailsModel model)
        {
            string data = "";
            if (model.OrgDepartmentId != 0)
            {
                List<int> departments = obj.MPRDepartments.Where(x => x.ORgDepartmentid == model.OrgDepartmentId).Select(x => (int)x.DepartmentId).ToList();
                data = string.Join(" ',' ", departments);
            }

            int mprno = 0;
            int rfqno = 0;
            if (model.DocumentNumber != null && model.DocumentNumber != "")
            {
                if (model.DocumentNumber.StartsWith("MPR", StringComparison.CurrentCultureIgnoreCase))
                {
                    model.DocumentNumber = model.DocumentNumber;
                }
                else
                {
                    mprno = Convert.ToInt32(model.DocumentNumber);
                    model.DocumentNumber = null;
                }
            }
            if (model.rfqnumber != null && model.rfqnumber != null)
            {
                if (model.rfqnumber.StartsWith("RFQ", StringComparison.CurrentCultureIgnoreCase))
                {
                    model.rfqnumber = model.rfqnumber;
                }
                else
                {
                    rfqno = Convert.ToInt32(model.rfqnumber);
                    model.rfqnumber = null;
                }
            }
            List<NewGetMprPaDetailsByFilter> filter = new List<NewGetMprPaDetailsByFilter>();
            try
            {
                var sqlquery = "";
                sqlquery = "select * from NewGetMprPaDetailsByFilters  where PAStatus in ('Pending','rejected','Approved','Submitted') and DeleteFlag=0 ";
                if (model.DocumentNumber != null && model.DocumentNumber != "")
                    sqlquery += " and  DocumentNo='" + model.DocumentNumber + "'";
                if (model.OrgDepartmentId != 0)
                    sqlquery += " and DepartmentId in ('" + data + "')";
                if (model.PONO != null)
                    sqlquery += " and PONO like '%" + model.PONO + "%'";
                if (model.Paid != 0)
                    sqlquery += " and PAId='" + model.Paid + "'";
                if (model.POdate != null)
                    sqlquery += " and PODate='" + model.POdate + "'";
                if (model.fromDate != null && model.toDate != null)
                    sqlquery += " and RequestedOn between '" + model.fromDate + "' and '" + model.toDate + "'";
                if (model.BuyerGroupId != 0)
                    sqlquery += " and BuyerGroupId='" + model.BuyerGroupId + "'";
                if (model.VendorId != 0)
                    sqlquery += " and VendorId='" + model.VendorId + "'";
                if (model.PAStatus != null)
                    sqlquery += " and PAStatus='" + model.PAStatus + "'";
                if (model.POStatus != null)
                    sqlquery += " and POStatus='" + model.POStatus + "'";
                if (mprno != 0)
                    sqlquery += " and MPRSeqNo = '" + mprno + "'";
                if (model.rfqnumber != null && model.rfqnumber != "")
                    sqlquery += " and RFQNo ='" + model.rfqnumber + "'";
                if (rfqno != 0)
                    sqlquery += " and RFQUniqueNo ='" + rfqno + "'";
                if (!string.IsNullOrEmpty(model.Approvername))
                    sqlquery += " and approvers1 like '% " + model.Approvername + "%'";
                if (!string.IsNullOrEmpty(model.Approverstatus))
                    sqlquery += " and PAStatus not in ('Rejected') and DeleteFlag=0 and ApprovalStatus like '% " + model.Approverstatus + "%'";

                sqlquery += " order by PAId desc ";
                //if (model.FromDate != null && model.ToDate != null)
                //    sqlquery += " and RequestedOn between '" + model.FromDate + "' and '" + model.ToDate + "'";

                filter = obj.Database.SqlQuery<NewGetMprPaDetailsByFilter>(sqlquery).ToList();
                return filter;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //getting ps status reports 
        /*Name of Function : <<GetPaStatusReports>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<GetPaStatusReports>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<PAReport>> GetPaStatusReports(PAReportInputModel model)
        {
            List<PAReport> report = new List<PAReport>();
            //DateTime? fromdate= model.FromDate;
            string fromdate = model.FromDate?.ToString("yyyy-MM-dd");
            string todate = model.ToDate?.ToString("yyyy-MM-dd");
            //DateTime?  todate = model.ToDate;
            try
            {
                var sqlquery = "";
                sqlquery = "select * from pareports where RevisionId!=0 ";
                //sqlquery = "select * from MPRDates where mprrevisionid!=0 ";
                if (fromdate != null && todate != null)
                    sqlquery += " and PreparedOn between '" + fromdate + "' and '" + todate + "' ";
                if (model.MPRRevisionId != 0)
                    sqlquery += " and RevisionId='" + model.MPRRevisionId + "'";
                //sqlquery = " select * from (select ms.Status, ms.StatusId, mr.RevisionId, mst.UpdatedDate, mr.ApprovalStatus, mr.ApprovedOn, mr.SecondApproversStatus, mr.ThirdApproverStatus, mr.SecondApprovedOn from MPRStatusTrack mst inner join MPRRevisions mr on mr.RevisionId = mst.RevisionId inner join MPRStatus ms on ms.StatusId = mst.StatusId) t pivot(count(statusid) for status in (submitted, Checked, approved, rejected, Acknowledged,[Clarification to End User],[RFQ Generated],[RFQ Responded],[Technical Spec Approved],[Quote Finalized],[PA Generated],[PO Released],[Raising PO Checked],[Raising PO Approved],[MPR Rejected],[MPR On Hold],[RFQ Finalized],[PA Approved],[MPR Closed])) as pivot_table ";
                report = obj.Database.SqlQuery<PAReport>(sqlquery).ToList();
                return report;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //sending email to approver that to approve pa
        /*Name of Function : <<UpdateApproverforRequest>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Sending remainder mail to the approver to approve pa>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> UpdateApproverforRequest(MPRPAApproversModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                string email = obj.Employees.Where(x => x.EmployeeNo == model.EmployeeNo).FirstOrDefault().EMail;
                this.emailDA.PAEmailRequestForApproval(model.PAId, email, model.EmployeeNo);
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<DeletePAByPAid>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Delete purchase puthorization ByPAid>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> DeletePAByPAid(padeletemodel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.MPRPADetails.Where(x => x.PAId == model.PAId).FirstOrDefault();
                if (data != null)
                {
                    data.Remarks = model.Remarks;
                    data.DeleteFlag = true;
                    data.DeleteBy = model.employeeno;
                    data.DeleteOn = DateTime.Now;
                    obj.SaveChanges();
                }
                status.Sid = data.PAId;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<GetIncompletedPAlist>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting the loist of inprogress pa list>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<List<IncompletedPAlist>> GetIncompletedPAlist(painputmodel model)
        {
            List<IncompletedPAlist> filter = new List<IncompletedPAlist>();
            try
            {
                var sqlquery = "";
                sqlquery = "select * from IncompletedPAlist where PAStatus in ('Inprogress')";
                if (model.PAId != 0)
                    sqlquery += " and PAId='" + model.PAId + "'";
                filter = obj.Database.SqlQuery<IncompletedPAlist>(sqlquery).ToList();
                return filter;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<UpdatePurchaseAuthorization>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Updating the inprogress purchase authorization>>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> UpdatePurchaseAuthorization(MPRPADetailsModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            var dateAndTime = DateTime.Now;
            try
            {
                var authorization = obj.MPRPADetails.Where(x => x.PAId == model.PAId).FirstOrDefault();
                authorization.PurchaseTypeId = model.PurchaseTypeId;
                authorization.PurchaseModeId = model.PurchaseModeId;
                authorization.RequestedOn = dateAndTime.Date;
                authorization.RequestedBy = model.RequestedBy;
                authorization.DepartmentID = model.DepartmentID;
                authorization.BuyerGroupId = model.BuyerGroupId;
                authorization.ProjectCode = model.ProjectCode;
                authorization.ProjectName = model.ProjectName;
                authorization.PackagingForwarding = model.PackagingForwarding;
                authorization.Taxes = model.Taxes;
                authorization.Freight = model.Freight;
                authorization.Insurance = model.Insurance;
                authorization.DeliveryCondition = model.DeliveryCondition;
                authorization.ShipmentMode = model.ShipmentMode;
                authorization.PaymentTerms = model.PaymentTerms;
                authorization.CreditDays = model.CreditDays;
                authorization.Warranty = model.Warranty;
                authorization.BankGuarantee = model.BankGuarantee;
                authorization.LDPenaltyTerms = model.LDPenaltyTerms;
                authorization.SpecialInstructions = model.SpecialInstructions;
                authorization.FactorsForImports = model.FactorsForImports;
                authorization.SpecialRemarks = model.SpecialRemarks;
                authorization.SuppliersReference = model.SuppliersReference;
                obj.SaveChanges();
                status.Sid = authorization.PAId;
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<getrfqtermsbyrevisionsid1>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<getting the assigned vendor rfq terms by revisionid>>
Review Date :<<>>   Reviewed By :<<>>*/
        public DataTable getrfqtermsbyrevisionsid1(List<int> revisionid)
        {
            SqlConnection Conn1 = new SqlConnection(@"Data Source=10.29.15.183;User ID=sa;Password=yil@1234;initial catalog=YSCM;Integrated Security=false;");
            EmployeModel employee = new EmployeModel();
            DataTable Ds = new DataTable();
            //string data = "'" + string.Join("','" , revisionid.Distinct())+ "'";
            string data = string.Join(",", revisionid.Distinct());
            try
            {
                SqlParameter[] Param = new SqlParameter[1];
                Param[0] = new SqlParameter("@revisionid", data);
                string spname = "Getrfqtermsbyrevision";
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter Adp = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.Connection = Conn1;
                cmd.CommandText = spname;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                if (Param != null)
                {
                    foreach (SqlParameter sqlParam in Param)
                    {
                        cmd.Parameters.Add(sqlParam);
                    }
                }

                Adp = new SqlDataAdapter(cmd);

                Adp.Fill(Ds);
                cmd.Parameters.Clear();
                //Ds.Clear();
                return Ds;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /*Name of Function : <<DeletePADocument>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<To deleted the attached document to pa >>
Review Date :<<>>   Reviewed By :<<>>*/
        public async Task<statuscheckmodel> DeletePADocument(PADocumentsmodel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var data = obj.MPRPADocuments.Where(x => x.DocumentId == model.DocumentId).FirstOrDefault();
                if (data != null)
                {
                    data.deleteflag = true;
                    obj.SaveChanges();
                    status.Sid = data.DocumentId;
                }
                return status;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public MPRPADetailsModel GetTokuchuDetailsByPAID(int? PID, int? TokuchRequestid)
        {
            if (TokuchRequestid != 0 && PID == 0)
            {
                var det = obj.TokuchuRequests.Where(li => li.TokuchRequestid == TokuchRequestid && li.DeleteFlag == false).FirstOrDefault<TokuchuRequest>();
                if (det != null)
                    PID = det.PAId;
                else
                    PID = 0;
            }
            MPRPADetailsModel model = new MPRPADetailsModel();

            int tokuchuRequestid = 0;
            try
            {
                var data = obj.MPRPADetails.Where(x => x.PAId == PID).FirstOrDefault();
                if (data != null)
                {
                    var tokuchudata = obj.TokuchuRequests.Where(li => li.PAId == PID && li.DeleteFlag == false).FirstOrDefault<TokuchuRequest>();
                    if (tokuchudata != null)
                    {
                        var tokuchureq = new TokuchuRequest();
                        tokuchureq.TokuchRequestid = tokuchudata.TokuchRequestid;
                        tokuchureq.PreparedBY = tokuchudata.PreparedBY;
                        tokuchureq.Preparedon = tokuchudata.Preparedon;
                        tokuchureq.PAId = tokuchudata.PAId;
                        tokuchureq.PreVerfiedBY = tokuchudata.PreVerfiedBY;
                        tokuchureq.PreVerifiedOn = tokuchudata.PreVerifiedOn;
                        tokuchureq.PreVerifiedStatus = tokuchudata.PreVerifiedStatus;
                        tokuchureq.PreVerifiedRemarks = tokuchudata.PreVerifiedRemarks;
                        tokuchureq.VerifiedBy = tokuchudata.VerifiedBy;
                        tokuchureq.VerifiedOn = tokuchudata.VerifiedOn;
                        tokuchureq.VerifiedStatus = tokuchudata.VerifiedStatus;
                        tokuchureq.VerifiedRemarks = tokuchudata.VerifiedRemarks;
                        //Added on 26082020 by Senthil - Start
                        tokuchureq.CompletedStatus = tokuchudata.CompletedStatus;
                        tokuchureq.CompletedOn = tokuchudata.CompletedOn;
                        tokuchureq.DownloadedBy = tokuchudata.DownloadedBy;
                        tokuchureq.DownloadedOn = tokuchudata.DownloadedOn;
                        //Added on 26082020 by Senthil - End 


                        tokuchureq.TokuchuProcessTracks = tokuchudata.TokuchuProcessTracks.Select(x => new TokuchuProcessTrack()
                        {
                            TokuchProcessTrackid = x.TokuchProcessTrackid,
                            TokuchRequestid = x.TokuchRequestid,
                            Statusby = obj.Employees.Where(li => li.EmployeeNo == x.Statusby).FirstOrDefault().Name,
                            Status = x.Status,
                            StatusDate = x.StatusDate,
                            Remarks = x.Remarks,
                        }).ToList();
                        model.TokuchuRequest = tokuchureq;
                        if (model.TokuchuRequest.TokuchRequestid != null)
                            tokuchuRequestid = model.TokuchuRequest.TokuchRequestid;
                    }
                    var statusdata = obj.LoadItemsByPAIDs.Where(x => x.itemstatus == "Approved" && x.PAId == PID).ToList();
                    model.Item = statusdata.Select(x => new RfqItemModel()
                    {
                        ItemDescription = x.ItemDescription,
                        materialid = x.MaterialId,
                        Materialdescription = obj.MaterialMasterYGS.Where(li => li.Material == x.MaterialId).FirstOrDefault().Materialdescription,
                        UnitPrice = Convert.ToDecimal(x.UnitPrice),
                        QuotationQty = Convert.ToDouble(x.QuotationQty),
                        DocumentNo = x.DocumentNo,
                        SaleOrderNo = x.SaleOrderNo,
                        Department = x.Department,
                        TargetSpend = Convert.ToDecimal(x.TargetSpend),
                        PaymentTermCode = x.PaymentTermCode,
                        VendorName = x.VendorName,
                        VendorCode = x.VendorCode,
                        DepartmentId = x.DepartmentId,
                        MRPItemsDetailsID = Convert.ToInt32(x.MPRItemDetailsid),
                        RFQRevisionId = x.rfqRevisionId,
                        paid = x.PAId,
                        paitemid = x.PAItemID,
                        POItemNo = x.POItemNo,
                        PONO = x.PONO,
                        Remarks = x.Remarks,
                        PODate = x.PODate.ToString(),
                        RFQNo = x.RFQNo,
                        HSNCode = x.HSNCode,
                        RFQItemsId = x.RFQItemsId,
                        VendorModelNo = x.VendorModelNo,
                        MfgModelNo = x.MfgModelNo,
                        MfgPartNo = x.MfgPartNo,
                        ManufacturerName = x.ManufacturerName,
                        MPRRevisionId = Convert.ToInt32(x.MPRRevisionId),
                        SoldToParty = x.soldtoparty + "-" + x.soldtopartyname,
                        ShipToParty = x.ShipToParty,
                        EndUser = x.Enduser + "-" + x.Endusername,
                        Tklineitemid = obj.TokuchuLIneItems.Where(li => li.PAItemID == x.PAItemID && li.TokuchRequestid == tokuchuRequestid).FirstOrDefault()?.Tklineitemid,
                        TokuchuNo = obj.TokuchuLIneItems.Where(li => li.PAItemID == x.PAItemID && li.TokuchRequestid == tokuchuRequestid).FirstOrDefault() != null ? obj.TokuchuLIneItems.Where(li => li.PAItemID == x.PAItemID && li.TokuchRequestid == tokuchuRequestid).FirstOrDefault().TokuchuNo : null,
                        StandardLeadtime = obj.TokuchuLIneItems.Where(li => li.PAItemID == x.PAItemID && li.TokuchRequestid == tokuchuRequestid).FirstOrDefault() != null ? obj.TokuchuLIneItems.Where(li => li.PAItemID == x.PAItemID && li.TokuchRequestid == tokuchuRequestid).FirstOrDefault().StandardLeadtime : null,
                        ProductCategorylevel2id = obj.TokuchuLIneItems.Where(li => li.PAItemID == x.PAItemID && li.TokuchRequestid == tokuchuRequestid).FirstOrDefault() != null ? obj.TokuchuLIneItems.Where(li => li.PAItemID == x.PAItemID && li.TokuchRequestid == tokuchuRequestid).FirstOrDefault().ProductCategorylevel2id : null,
                        //SoldToParty=obj.MPRRevisions.Where(y=>y.RevisionId==x.MPRRevisionId && y.BoolValidRevision==true).FirstOrDefault().soldtopartyname,
                        //ShipToParty = obj.MPRRevisions.Where(y => y.RevisionId == x.MPRRevisionId && y.BoolValidRevision == true).FirstOrDefault().shiptopartyname,
                        //EndUser = obj.MPRRevisions.Where(y => y.RevisionId == x.MPRRevisionId && y.BoolValidRevision == true).FirstOrDefault().Endusername,

                    }).ToList();
                    return model;
                }
                else
                {
                    return model;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int updateTokuchuRequest(TokuchuRequest tokuchuRequest, string typeOfuser, int revisionId)
        {
            int tokuchureqid = 0;
            using (YSCMEntities Context = new YSCMEntities())
            {
                TokuchuRequest request = Context.TokuchuRequests.Where(li => li.TokuchRequestid == tokuchuRequest.TokuchRequestid).FirstOrDefault();
                if (request == null)
                {
                    request = new TokuchuRequest();
                    request.PAId = tokuchuRequest.PAId;
                    request.PreparedBY = tokuchuRequest.PreparedBY;
                    request.VerifiedBy = request.PreVerfiedBY = tokuchuRequest.VerifiedBy;
                    request.Preparedon = DateTime.Now;
                    request.DeleteFlag = false;
                    Context.TokuchuRequests.Add(request);
                    Context.SaveChanges();
                }
                else
                {
                    request.VerifiedBy = request.PreVerfiedBY = tokuchuRequest.VerifiedBy;
                    if (typeOfuser == "PreVerifier")
                        request.PreVerifiedOn = DateTime.Now;
                    request.PreVerifiedStatus = tokuchuRequest.PreVerifiedStatus;
                    request.PreVerifiedRemarks = tokuchuRequest.PreVerifiedRemarks;
                    if (typeOfuser == "Verifier")
                        request.VerifiedOn = DateTime.Now;
                    request.VerifiedStatus = tokuchuRequest.VerifiedStatus;
                    request.VerifiedRemarks = tokuchuRequest.VerifiedRemarks;
                    Context.SaveChanges();
                }
                tokuchureqid = request.TokuchRequestid;

                foreach (TokuchuLIneItem item in tokuchuRequest.TokuchuLIneItems)
                {
                    TokuchuLIneItem TokuchuLIneItem = Context.TokuchuLIneItems.Where(li => li.Tklineitemid == item.Tklineitemid).FirstOrDefault();
                    if (TokuchuLIneItem == null)
                    {
                        TokuchuLIneItem = new TokuchuLIneItem();
                        TokuchuLIneItem.TokuchRequestid = request.TokuchRequestid;
                        TokuchuLIneItem.PAItemID = item.PAItemID;
                        TokuchuLIneItem.StandardLeadtime = item.StandardLeadtime;
                        TokuchuLIneItem.ProductCategorylevel2id = item.ProductCategorylevel2id;
                        Context.TokuchuLIneItems.Add(TokuchuLIneItem);
                        Context.SaveChanges();

                    }
                    else
                    {
                        TokuchuLIneItem.StandardLeadtime = item.StandardLeadtime;
                        TokuchuLIneItem.ProductCategorylevel2id = item.ProductCategorylevel2id;
                        Context.SaveChanges();

                    }

                }
                int tokuchureqId = Convert.ToInt32(request.TokuchRequestid);
                if (typeOfuser == "Requestor")
                {
                    //mail to verifier					
                    this.emailTemplateDA.prepareAribaTemplate(tokuchureqId, tokuchuRequest.PreparedBY, tokuchuRequest.VerifiedBy, typeOfuser, revisionId);
                }

                if (typeOfuser == "PreVerifier")
                {
                    //mail to requestor
                    //mail to ariba
                    if (tokuchuRequest.PreVerifiedStatus == "Approved")
                        this.emailTemplateDA.prepareAribaTemplate(tokuchureqId, tokuchuRequest.PreVerfiedBY, tokuchuRequest.PreparedBY, typeOfuser, revisionId);
                }
                if (typeOfuser == "Verifier")
                {
                    //mail to requestor
                    if (tokuchuRequest.VerifiedStatus == "Approved")
                    {
                        this.emailTemplateDA.prepareAribaTemplate(tokuchureqId, tokuchuRequest.VerifiedBy, tokuchuRequest.PreparedBY, typeOfuser, revisionId);
                        string mprpreparedby = obj.MPRRevisions.Where(li => li.RevisionId == revisionId).FirstOrDefault().CheckedBy;
                        this.emailTemplateDA.prepareAribaTemplate(tokuchureqId, tokuchuRequest.VerifiedBy, mprpreparedby, "MPRRequestor", revisionId);
                    }
                }
            }
            return tokuchureqid;
        }
        public List<GetTokuchuDetail> getTokuchuReqList(tokuchuFilterParams tokuchuFilterParams)
        {
            List<GetTokuchuDetail> tokuchuDetails;
            using (YSCMEntities Context = new YSCMEntities())
            {
                var query = default(string);
                query = "select * from GetTokuchuDetails where DeleteFlag!=1";
                if (!string.IsNullOrEmpty(tokuchuFilterParams.ToDate))
                    query += " and Preparedon <= '" + tokuchuFilterParams.ToDate + "'";
                if (!string.IsNullOrEmpty(tokuchuFilterParams.FromDate))
                    query += "  and Preparedon >= '" + tokuchuFilterParams.FromDate + "'";
                if (!string.IsNullOrEmpty(tokuchuFilterParams.Paid))
                    query += "  and PAID = '" + tokuchuFilterParams.Paid + "'";
                if (!string.IsNullOrEmpty(tokuchuFilterParams.PreparedBY))
                    query += "  and PreparedBY = '" + tokuchuFilterParams.PreparedBY + "'";
                if (!string.IsNullOrEmpty(tokuchuFilterParams.VerifiedBy))
                    query += "  and VerifiedBy = '" + tokuchuFilterParams.VerifiedBy + "'";
                query += " order by TokuchRequestid desc ";
                tokuchuDetails = Context.GetTokuchuDetails.SqlQuery(query).ToList<GetTokuchuDetail>();
            }
            return tokuchuDetails;
        }
        public List<mprstatuspivot> Getmprstatus()
        {
            List<mprstatuspivot> data = new List<mprstatuspivot>();
            try
            {
                var query = "";
                query = "select * from mprstatuspivot";
                data = obj.mprstatuspivots.SqlQuery(query).ToList<mprstatuspivot>();
                return data;
            }
            catch (Exception)
            {

                throw;
            }
        }
        /*Name of Function : <<GetMprstatuswisereport>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Getting mpr status report by buyer groupid and from and to dates of mpr >>
Review Date :<<>>   Reviewed By :<<>>*/
        public DataSet GetMprstatuswisereport(string spName, SqlParameter[] paramArr)
        {
            SqlConnection Conn1 = new SqlConnection(@"Data Source=10.29.15.183;User ID=sa;Password=yil@1234;initial catalog=YSCM;Integrated Security=false;");
            EmployeModel employee = new EmployeModel();
            DataSet Ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter Adp = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.Connection = Conn1;
                cmd.CommandText = spName;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                if (paramArr != null)
                {
                    foreach (SqlParameter sqlParam in paramArr)
                    {
                        cmd.Parameters.Add(sqlParam);
                    }
                }

                Adp = new SqlDataAdapter(cmd);
                Ds = new DataSet();

                Adp.Fill(Ds);
                cmd.Parameters.Clear();
                //Ds.Clear();
                return Ds;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /*Name of Function : <<GetmprstatusReport>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Getting count of pending ,submitted and approved mpr on department wise >>
Review Date :<<>>   Reviewed By :<<>>*/
        public DataSet GetmprstatusReport(string spName, SqlParameter[] paramArr)
        {
            SqlConnection Conn1 = new SqlConnection(@"Data Source=10.29.15.183;User ID=sa;Password=yil@1234;initial catalog=YSCM;Integrated Security=false;");
            EmployeModel employee = new EmployeModel();
            DataSet Ds = new DataSet();
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter Adp = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.Connection = Conn1;
                cmd.CommandText = spName;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                if (paramArr != null)
                {
                    foreach (SqlParameter sqlParam in paramArr)
                    {
                        cmd.Parameters.Add(sqlParam);
                    }
                }

                Adp = new SqlDataAdapter(cmd);
                Ds = new DataSet();

                Adp.Fill(Ds);
                cmd.Parameters.Clear();
                return Ds;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<GetmprRequisitionReport>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Getting data for mpr requisition report based on serach filter >>
Review Date :<<>>   Reviewed By :<<>>*/
        public List<RequisitionReport> GetmprRequisitionReport(ReportInputModel input)
        {
            List<RequisitionReport> data = new List<RequisitionReport>();
            string department = "";
            try
            {
                if (input.DepartmentId == 0)
                {
                    List<int> departments = obj.MPRDepartments.Where(x => x.ORgDepartmentid == input.OrgDepartmentId).Select(x => (int)x.DepartmentId).ToList();
                    department = string.Join(" ',' ", departments);
                }
                int mprno = 0;
                if (!string.IsNullOrEmpty(input.DocumentNo))
                {
                    if (input.DocumentNo.StartsWith("MPR", StringComparison.CurrentCultureIgnoreCase))
                    {
                        input.DocumentNo = input.DocumentNo;
                    }
                    else
                    {
                        mprno = Convert.ToInt32(input.DocumentNo);
                        input.DocumentNo = null;
                    }
                }
                var query = "";
                query = "select * from RequisitionReport where revisionid!=0 ";
                if (input.status == null)
                {
                    if (!string.IsNullOrEmpty(input.DocumentNo))
                        query += " and documentno='" + input.DocumentNo + "'";
                    if (!string.IsNullOrEmpty(input.jobcode))
                        query += " and jobcode='" + input.jobcode + "'";
                    if (!string.IsNullOrEmpty(input.preparedby))
                        query += " and preparedby='" + input.preparedby + "'";
                    if (!string.IsNullOrEmpty(input.Checked))
                        query += " and Checker='" + input.Checked + "'";
                    if (!string.IsNullOrEmpty(input.ApprovedBy))
                        query += " and Approver='" + input.ApprovedBy + "'";
                    if (!string.IsNullOrEmpty(input.checkerstatus))
                        query += " and checkstatus='" + input.checkerstatus + "'";
                    if (!string.IsNullOrEmpty(input.finalApproverstatus))
                        query += " and approvalstatus='" + input.finalApproverstatus + "'";
                    if (!string.IsNullOrEmpty(input.Fromdate))
                        query += " and preparedon>='" + input.Fromdate + "'";
                    if (!string.IsNullOrEmpty(input.Todate))
                        query += " and preparedon < DATEADD(day, 1, '" + input.Todate + "')";
                    if (input.RequisitionId != 0)
                        query += " and requisitionid='" + input.RequisitionId + "'";
                    if (input.revisionId != 0)
                        query += " and revisionid='" + input.revisionId + "'";
                    if (input.BuyerGroupId != 0)
                        query += " and Buyergroupid='" + input.BuyerGroupId + "'";
                    if (input.Issuepurposeid != 0)
                        query += " and IssuePurposeId='" + input.Issuepurposeid + "'";
                    if (!string.IsNullOrEmpty(department))
                        query += " and DepartmentId in ('" + department + "') ";
                    if (input.DepartmentId != 0)
                        query += " and DepartmentId = '" + input.DepartmentId + "'";
                    if (!string.IsNullOrEmpty(input.jobcode))
                        query += " and jobcode='" + input.jobcode + "'";
                    if (input.ShowAllrevisions == false)
                        query += " and BoolValidRevision='" + 1 + "'";
                    //query += " and BoolValidRevision='" + 1 + "' and ApprovalStatus not in ('Pending','Rejected','Sent for Modification','Submitted') and SecondApproversStatus not in ('Pending','Rejected','Sent for Modification','Submitted') and ThirdApproverStatus not in ('Pending','Rejected','Sent for Modification','Submitted') ";
                    if (input.ShowAllrevisions == true)
                        query += " and BoolValidRevision is not null";
                }
                else
                {
                    if (input.status == "Completed")
                        query += " and CheckStatus  in('Approved') and statusid in (12,15,19)  and departmentid='" + input.DepartmentId + "'";
                    if (input.status == "Pending")
                        query += " and CheckStatus  in('Approved') and statusid not in (12,15,19) and departmentid='" + input.DepartmentId + "'";
                    if (input.status == "submitted")
                        query += " and CheckStatus  in('Approved') and approvedate is not null and departmentid='" + input.DepartmentId + "'";
                    //if (input.DepartmentId !=0)
                    //    query += " and departmentid='" + input.DepartmentId + "'";
                    if (input.BuyerGroupId != 0)
                        query += " and Buyergroupid='" + input.BuyerGroupId + "'";
                    if (input.Issuepurposeid != 0)
                        query += " and IssuePurposeId='" + input.Issuepurposeid + "'";
                    //if (input.DepartmentId != 0)
                    //    query += " and departmentid='" + input.DepartmentId + "'";
                    if (!string.IsNullOrEmpty(input.Fromdate))
                        query += " and approvedate>='" + input.Fromdate + "'";
                    if (!string.IsNullOrEmpty(input.Todate))
                        query += " and approvedate < DATEADD(day, 1, '" + input.Todate + "') ";
                    if (input.ShowAllrevisions == false)
                        query += " and BoolValidRevision='" + 1 + "'";
                }
                data = obj.RequisitionReports.SqlQuery(query).ToList<RequisitionReport>();
                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<GetmprRequisitionfilters>>  Author :<<Akhil>>  
Date of Creation <<>>
Purpose : <<Getting the checker,mpr preparedby and approvers >>
Review Date :<<>>   Reviewed By :<<>>*/
        public ReportFilterModel GetmprRequisitionfilters()
        {
            ReportFilterModel filter = new ReportFilterModel();
            try
            {
                filter.jobcode = obj.MPRRevisions.Where(x => x.JobCode != null).Select(x => new jobcodes()
                {
                    Jobcode = x.JobCode
                }).Distinct().ToList();
                var data = obj.MPRIssuePurposes.Where(x => x.BoolInUse == true).ToList();
                filter.purposetype = data.Select(x => new IssuepurposeType()
                {
                    Issuepurpose = x.IssuePurpose,
                    Issuepurposeid = x.IssuePurposeId
                }).ToList();

                var checkers = obj.Loadmprcheckers.ToList();
                filter.mprcheckedby = checkers.Select(x => new MprCheckers()
                {
                    checker = x.CheckedBy,
                    checkername = x.Name
                }).ToList();
                var prepare = obj.Loadmprprepares.ToList();
                filter.mprprepares = prepare.Select(x => new Mprprepare()
                {
                    Preparedby = x.PreparedBy,
                    preparedname = x.Name
                }).ToList();
                var approvers = obj.Loadmprapprovers.ToList();
                filter.mprApprovedby = approvers.Select(x => new MprApprovers()
                {
                    approvedby = x.ApprovedBy,
                    approvername = x.Name
                }).ToList();
                return filter;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<loadprojectmangersforreport> Loadprojectmanagersforreport()
        {
            List<loadprojectmangersforreport> report = new List<loadprojectmangersforreport>();
            var sqlquery = "";
            sqlquery = "select * from loadprojectmangersforreport ";
            report = obj.loadprojectmangersforreports.SqlQuery(sqlquery).ToList<loadprojectmangersforreport>();
            return report;
        }
        public DataTable Loadprojectcodewisereport(ReportInputModel input)
        {
            DataTable table = new DataTable();
            string data = "";
            if (input.DepartmentId == 0)
            {
                List<int> departments = obj.MPRDepartments.Where(x => x.ORgDepartmentid == input.OrgDepartmentId).Select(x => (int)x.DepartmentId).ToList();
                data = string.Join(" ',' ", departments);
            }
            //List<Reportbyprojectcode> report = new List<Reportbyprojectcode>();
            var query = "";
            if (input.Itemwise == false)
                query = "select * from Reportbyprojectcodeitemwise where documentno is not null ";
            else
                query = "select * from Reportbyprojectcode where documentno is not null ";


            //query = "select * from Reportbyprojectcode where documentno is not null ";
            if (!string.IsNullOrEmpty(input.jobcode))
                query += " and JobCode='" + input.jobcode + "'";
            if (input.BuyerGroupId != 0)
                query += " and BuyerGroupId='" + input.BuyerGroupId + "'";
            if ((!string.IsNullOrEmpty(data)))
                query += " and DepartmentId in ('" + data + "')";
            if (input.DepartmentId != 0)
                query += " and DepartmentId = '" + input.DepartmentId + "'";
            if (!string.IsNullOrEmpty(input.ProjectManager))
                query += " and ProjectManager='" + input.ProjectManager + "'";
            if (!string.IsNullOrEmpty(input.SaleOrderNo))
                query += " and SaleOrderNo='" + input.SaleOrderNo + "'";
            //if (!string.IsNullOrEmpty(input.ApprovedBy))
            //    query += " and Approver='" + input.ApprovedBy + "'";
            if (!string.IsNullOrEmpty(input.Fromdate))
                query += " and approveddate>='" + input.Fromdate + "'";
            if (!string.IsNullOrEmpty(input.Todate))
                query += " and approveddate < DATEADD(day, 1, '" + input.Todate + "')";

            query += " order by RevisionId desc ";
            var cmd = obj.Database.Connection.CreateCommand();
            cmd.CommandText = query;

            cmd.Connection.Open();
            table.Load(cmd.ExecuteReader());
            cmd.Connection.Close();
            //report = obj.Reportbyprojectcodes.SqlQuery(query).ToList<Reportbyprojectcode>();
            return table;
        }
        public List<ReportbyprojectDuration> LoadprojectDurationwisereport(ReportInputModel input)
        {
            string data = "";
            if (input.DepartmentId == 0)
            {
                List<int> departments = obj.MPRDepartments.Where(x => x.ORgDepartmentid == input.OrgDepartmentId).Select(x => (int)x.DepartmentId).ToList();
                data = string.Join(" ',' ", departments);
            }
            List<ReportbyprojectDuration> report = new List<ReportbyprojectDuration>();
            var query = "";
            query = "select * from ReportbyprojectDuration where documentno is not null ";
            if (!string.IsNullOrEmpty(input.jobcode))
                query += " and JobCode='" + input.jobcode + "'";
            if (input.BuyerGroupId != 0)
                query += " and BuyerGroupId='" + input.BuyerGroupId + "'";
            if ((!string.IsNullOrEmpty(data)))
                query += " and DepartmentId in ('" + data + "')";
            if (input.DepartmentId != 0)
                query += " and DepartmentId ='" + input.DepartmentId + "'";
            if (!string.IsNullOrEmpty(input.ProjectManager))
                query += " and ProjectManager='" + input.ProjectManager + "'";
            if (!string.IsNullOrEmpty(input.SaleOrderNo))
                query += " and SaleOrderNo='" + input.SaleOrderNo + "'";
            //if (!string.IsNullOrEmpty(input.ApprovedBy))
            //    query += " and Approver='" + input.ApprovedBy + "'";
            if (!string.IsNullOrEmpty(input.Fromdate))
                query += " and approveddate>='" + input.Fromdate + "'";
            if (!string.IsNullOrEmpty(input.Todate))
                query += " and approveddate< DATEADD(day, 1, '" + input.Todate + "')";

            query += " order by RevisionId desc ";
            report = obj.ReportbyprojectDurations.SqlQuery(query).ToList<ReportbyprojectDuration>();
            return report;
        }
        public List<jobcodes> Loadjobcodes()
        {
            List<jobcodes> report = new List<jobcodes>();
            var data = obj.MPRRevisions.Where(x => x.JobCode != null).Distinct().ToList();
            report = data.Select(x => new jobcodes()
            {
                Jobcode = x.JobCode
            }).ToList();
            return report;
        }
        public List<Saleorderno> Loadsaleorder()
        {
            List<Saleorderno> report = new List<Saleorderno>();
            var data = obj.MPRRevisions.Where(x => x.SaleOrderNo != null).Distinct().ToList();
            report = data.Select(x => new Saleorderno()
            {
                SaleOrderNo = x.SaleOrderNo
            }).ToList();
            return report;
        }
        public DataTable GETApprovernamesbydepartmentid(int departmentid)
        {
            string con = obj.Database.Connection.ConnectionString;
            SqlConnection Conn1 = new SqlConnection(con);
            DataTable Ds = new DataTable();
            try
            {
                SqlParameter[] Param = new SqlParameter[1];
                Param[0] = new SqlParameter("@departmentid", departmentid);
                string spname = "loadDepartmentwiseapprovers";
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter Adp = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.Connection = Conn1;
                cmd.CommandText = spname;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                if (Param != null)
                {
                    foreach (SqlParameter sqlParam in Param)
                    {
                        cmd.Parameters.Add(sqlParam);
                    }
                }
                Adp = new SqlDataAdapter(cmd);
                Ds = new DataTable();
                Adp.Fill(Ds);
                cmd.Parameters.Clear();
                return Ds;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<loadpastatsreport>> getPaValueReport(PADetailsModel model)
        {
            string data = "";
            if (model.OrgDepartmentId != 0)
            {
                List<int> departments = obj.MPRDepartments.Where(x => x.ORgDepartmentid == model.OrgDepartmentId).Select(x => (int)x.DepartmentId).ToList();
                data = string.Join(" ',' ", departments);
            }

            int mprno = 0;
            int rfqno = 0;
            if (model.DocumentNumber != null && model.DocumentNumber != "")
            {
                if (model.DocumentNumber.StartsWith("MPR", StringComparison.CurrentCultureIgnoreCase))
                {
                    model.DocumentNumber = model.DocumentNumber;
                }
                else
                {
                    mprno = Convert.ToInt32(model.DocumentNumber);
                    model.DocumentNumber = null;
                }
            }
            if (model.rfqnumber != null && model.rfqnumber != null)
            {
                if (model.rfqnumber.StartsWith("RFQ", StringComparison.CurrentCultureIgnoreCase))
                {
                    model.rfqnumber = model.rfqnumber;
                }
                else
                {
                    rfqno = Convert.ToInt32(model.rfqnumber);
                    model.rfqnumber = null;
                }
            }
            List<loadpastatsreport> filter = new List<loadpastatsreport>();
            try
            {
                var sqlquery = "";
                sqlquery = "select * from loadpastatsreport  where PAStatus in ('Pending','rejected','Approved','Submitted') and DeleteFlag=0 ";
                if (model.DocumentNumber != null && model.DocumentNumber != "")
                    sqlquery += " and  DocumentNo='" + model.DocumentNumber + "'";
                if (model.OrgDepartmentId != 0)
                    sqlquery += " and DepartmentId in ('" + data + "')";
                if (model.PONO != null)
                    sqlquery += " and PONO like '%" + model.PONO + "%'";
                if (model.Paid != 0)
                    sqlquery += " and PAId='" + model.Paid + "'";
                if (model.POdate != null)
                    sqlquery += " and PODate='" + model.POdate + "'";
                if (model.fromDate != null && model.toDate != null)
                    sqlquery += " and RequestedOn between '" + model.fromDate + "' and '" + model.toDate + "'";
                if (model.BuyerGroupId != 0)
                    sqlquery += " and BuyerGroupId='" + model.BuyerGroupId + "'";
                if (model.VendorId != 0)
                    sqlquery += " and VendorId='" + model.VendorId + "'";
                if (model.PAStatus != null)
                    sqlquery += " and PAStatus='" + model.PAStatus + "'";
                if (model.POStatus != null)
                    sqlquery += " and POStatus='" + model.POStatus + "'";
                if (mprno != 0)
                    sqlquery += " and MPRSeqNo = '" + mprno + "'";
                if (model.rfqnumber != null && model.rfqnumber != "")
                    sqlquery += " and RFQNo ='" + model.rfqnumber + "'";
                if (rfqno != 0)
                    sqlquery += " and RFQUniqueNo ='" + rfqno + "'";
                if (!string.IsNullOrEmpty(model.Approvername))
                    sqlquery += " and approvers1 like '% " + model.Approvername + "%'";
                if (!string.IsNullOrEmpty(model.Approverstatus))
                    sqlquery += " and PAStatus not in ('Rejected') and DeleteFlag=0 and ApprovalStatus like '% " + model.Approverstatus + "%'";

                sqlquery += " order by PAId desc ";
                //if (model.FromDate != null && model.ToDate != null)
                //    sqlquery += " and RequestedOn between '" + model.FromDate + "' and '" + model.ToDate + "'";

                filter = obj.Database.SqlQuery<loadpastatsreport>(sqlquery).ToList();
                return filter;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<statuscheckmodel>> InsertMappingItems(List<MappingItemModel> model)
        {
            List<statuscheckmodel> status = new List<statuscheckmodel>();
            try
            {
                foreach (var item in model)
                {
                    var rfqdata = new MPRRfqItem()
                    {
                        MPRItemDetailsid = item.previousitemdetails,
                        MPRRevisionId = item.itemrevision,
                        RfqItemsid = item.RFQItemsId,
                        DeleteFlag = false
                    };
                    obj.MPRRfqItems.Add(rfqdata);
                    obj.SaveChanges();
                    statuscheckmodel status1 = new statuscheckmodel();
                    status1.Sid = rfqdata.MPRRFQitemId;
                    status1.rfqitemid = rfqdata.RfqItemsid;
                    status.Add(status1);

                    var iteminfo = new MPRRfqItemInfo();
                    iteminfo.rfqsplititemid = item.RFQSplitItemId;
                    iteminfo.MPRRFQitemId = status1.Sid;
                    obj.MPRRfqItemInfos.Add(iteminfo);
                    obj.SaveChanges();
                    var mprrfqid = iteminfo.MPRRFQitemId;
                }
                return status;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<MSAMasterConfirmation> updateMSAConfirmation(MSAMasterConfirmation model)
        {
            MSAMasterConfirmation status = new MSAMasterConfirmation();
            try
            {
                var data = obj.MSAMasterConfirmations.Where(x => x.PAID == model.PAID && x.Deleteflag == false && x.Confirmationflag == false).FirstOrDefault();
                if (data != null)
                {
                    data.Confirmationflag = model.Confirmationflag;
                    data.ConfirmedBy = model.UploadedBy;
                    data.ConfirmedDate = DateTime.Now;
                    obj.SaveChanges();
                    model.StatusRemarks = "1";
                    var msadata = obj.MSALineItems.Where(x => x.paid == model.PAID && x.deletionflag == null).ToList();
                    foreach (var item in msadata)
                    {
                        item.MSAMasterID = data.MSAMasterID;
                        obj.SaveChanges();
                    }

                    //item.MSAMasterID = data.MSAMasterID; 
                    //obj.MSALineItems.Add(item);
                    //obj.SaveChanges();
                    this.emailDA.msaconfirmationmail(Convert.ToInt32(model.PAID));
                }

                else
                {
                    var alreadyConfirmed = obj.MSAMasterConfirmations.Where(x => x.PAID == model.PAID && x.Deleteflag == false && x.Confirmationflag == true).FirstOrDefault();
                    if (alreadyConfirmed != null)
                    {
                        model.StatusRemarks = "2";
                    }
                    else
                    {       // If User are ok with record than he can directly click on confirm. 
                            //So Automatically based on PAID records will take from View and Insert MSALineItemTable,
                            //MSAMasterConfirmationTable and Update Confirmation flag is Yes.

                        int msamasterid = 0;
                        var MSAMasterConfirmation1 = obj.MSAMasterConfirmations.Where(li => li.PAID == model.PAID && li.Deleteflag == false).FirstOrDefault();


                        //need to take data from View and dump into MSALineItem and MSAMasterConfirmation
                        var RPAViewData = obj.RPALoadItemForMSAs.Where(li => li.paid == model.PAID).ToList();
                        if (IsPAVaild(model.PAID, RPAViewData))
                        {
                            if (MSAMasterConfirmation1 == null)
                            {
                                MSAMasterConfirmation mSAMasterConfirmation = new MSAMasterConfirmation();
                                mSAMasterConfirmation.Deleteflag = model.Deleteflag;
                                mSAMasterConfirmation.Confirmationflag = model.Confirmationflag;
                                mSAMasterConfirmation.PAID = model.PAID;
                                mSAMasterConfirmation.ConfirmedBy = model.UploadedBy;
                                mSAMasterConfirmation.ConfirmedDate = DateTime.Now;
                                mSAMasterConfirmation.UploadedBy = model.UploadedBy;
                                mSAMasterConfirmation.UplaodedDate = DateTime.Now;
                                obj.MSAMasterConfirmations.Add(mSAMasterConfirmation);
                                obj.SaveChanges();
                                msamasterid = mSAMasterConfirmation.MSAMasterID;
                                model.StatusRemarks = "4";
                            }

                            foreach (var item in RPAViewData)
                            {
                                if (item.PAItemID != null)
                                {
                                    MSALineItem mSALineItem1 = new MSALineItem();
                                    mSALineItem1.MSAMasterID = msamasterid;
                                    mSALineItem1.Item_No_ = item.Item_No_;
                                    //mSALineItem1.deletionflag = false;
                                    mSALineItem1.mscode = item.mscode;
                                    mSALineItem1.ItemDescription = item.ItemDescription;
                                    mSALineItem1.WBS = item.WBS;
                                    mSALineItem1.wbsdesc = item.wbsdesc;
                                    mSALineItem1.Quantity = Convert.ToInt32(item.Quantity);
                                    mSALineItem1.unit = item.unit;
                                    mSALineItem1.RequirementDate = item.RequirementDate;
                                    mSALineItem1.Project = item.Project;
                                    mSALineItem1.projectdesc = item.projectdesc;
                                    mSALineItem1.TokuchuNo = item.TokuchuNo;
                                    mSALineItem1.tokchuno = item.tokchuno;
                                    mSALineItem1.bupfl = item.bupfl;
                                    mSALineItem1.bupfldesc = item.bupfldesc;
                                    mSALineItem1.UnitPrice = item.UnitPrice.ToString();
                                    mSALineItem1.Currency = item.Currency;
                                    mSALineItem1.priceunit = item.priceunit;
                                    mSALineItem1.costelement = item.costelement;
                                    mSALineItem1.costelementdesc = item.costelementdesc;

                                    mSALineItem1.plant = item.plant;
                                    mSALineItem1.plantname = item.plantname;
                                    mSALineItem1.StorageLocation = item.StorageLocation;
                                    mSALineItem1.storagelocationname = item.storagelocationname;
                                    mSALineItem1.VendorCode = item.VendorCode;
                                    mSALineItem1.VendorName = item.VendorName;
                                    mSALineItem1.VendorModelNo = item.VendorModelNo.Substring(0, 35);
                                    mSALineItem1.sortstring1 = item.sortstring1;
                                    mSALineItem1.ProjectManager = item.ProjectManager;
                                    mSALineItem1.note1 = item.note1;
                                    mSALineItem1.note2 = item.note3.ToString();
                                    mSALineItem1.note3 = item.PAItemID.ToString();
                                    mSALineItem1.note4 = item.note2.ToString("yyyyMMdd");
                                    mSALineItem1.lt = item.lt;
                                    mSALineItem1.deadline = item.deadline;
                                    mSALineItem1.deliverydate = item.deliverydate;
                                    mSALineItem1.text = item.text;
                                    mSALineItem1.Direct_Shipping = item.Direct_Shipping;
                                    mSALineItem1.Ship_to_Party = item.Ship_to_Party;
                                    mSALineItem1.Ship_to_Party_seq__No_ = item.Ship_to_Party_seq__No_;

                                    mSALineItem1.Ship_to_Party_Name = item.Ship_to_Party_Name;
                                    mSALineItem1.Ship_to_Party_Address = item.Ship_to_Party_Address;
                                    mSALineItem1.Ship_to_Party_Phone = item.Ship_to_Party_Phone;
                                    mSALineItem1.Nuclear_Spec_Code = item.Nuclear_Spec_Code;
                                    mSALineItem1.QW_Box_No_ = item.QW_Box_No_;
                                    mSALineItem1.Safe_Proof_ID = item.Safe_Proof_ID;
                                    mSALineItem1.XJNo_ = item.XJNo_;
                                    mSALineItem1.Product_career_code = item.Product_career_code;
                                    mSALineItem1.QIC_Language = item.QIC_Language;
                                    mSALineItem1.QIC_Delivery_style = item.QIC_Delivery_style;
                                    mSALineItem1.Document_Quantity = item.Document_Quantity;
                                    mSALineItem1.Document_Item_No_ = item.Document_Item_No_;
                                    mSALineItem1.IM_Language = item.IM_Language;
                                    mSALineItem1.IM_Attach_Style = item.IM_Attach_Style;
                                    mSALineItem1.Tokuchu_IM_No_ = item.Tokuchu_IM_No_;
                                    mSALineItem1.Parts_Instrument_Model = item.Parts_Instrument_Model;
                                    mSALineItem1.Serial_Information_Flag = item.Serial_Information_Flag;
                                    mSALineItem1.System_Model = item.System_Model;
                                    mSALineItem1.Additional_work_code_1 = item.Additional_work_code_1;
                                    mSALineItem1.Additional_work_code_2 = item.Additional_work_code_2;

                                    mSALineItem1.Additional_work_code_3 = item.Additional_work_code_3;
                                    mSALineItem1.Additional_work_code_4 = item.Additional_work_code_4;
                                    mSALineItem1.Additional_work_code_5 = item.Additional_work_code_5;
                                    mSALineItem1.Work_sheet_flag = item.Work_sheet_flag;
                                    mSALineItem1.Work_sheet_Rev = item.Work_sheet_Rev;
                                    mSALineItem1.Work_sheet_No_ = item.Work_sheet_No_;
                                    mSALineItem1.Freight_RSP__JPY_ = item.Freight_RSP__JPY_;
                                    mSALineItem1.Freight_RSP__USD_ = item.Freight_RSP__USD_;
                                    mSALineItem1.Freight_RSP__EUR_ = item.Freight_RSP__EUR_;
                                    mSALineItem1.Combined_MS_Code_Indicator = item.Combined_MS_Code_Control_Number;
                                    mSALineItem1.Combined_MS_Code_Control_Number = item.Combined_MS_Code_Control_Number;
                                    mSALineItem1.Comp__No_ = item.Comp__No_;
                                    mSALineItem1.Order_Instruction_Title_code = item.Order_Instruction_Title_code;
                                    mSALineItem1.Order_Instruction_Title = item.Order_Instruction_Title;
                                    mSALineItem1.Input_Type = item.Input_Type;
                                    mSALineItem1.Min_ = item.Min_;
                                    mSALineItem1.Max_ = item.Min_;

                                    mSALineItem1.Unitno = item.Unitno;
                                    mSALineItem1.Sensor = item.Sensor;
                                    mSALineItem1.Factor = item.Factor;
                                    mSALineItem1.Feature = item.Feature;
                                    mSALineItem1.Free_Text = item.Free_Text;
                                    mSALineItem1.Inquiry_ID = item.Inquiry_ID;
                                    mSALineItem1.Process_flag_INT_for_internal = item.Process_flag_INT_for_internal;

                                    mSALineItem1.paid = item.paid;
                                    mSALineItem1.PAItemID = item.PAItemID;
                                    obj.MSALineItems.Add(mSALineItem1);
                                    obj.SaveChanges();
                                    model.StatusRemarks = "3";

                                }

                            }

                            //var MSAMasterConfirmation1 = obj.MSAMasterConfirmations.Where(li => li.PAID == model.PAID && li.Deleteflag == false).FirstOrDefault();
                            //if (MSAMasterConfirmation1 == null)
                            //{
                            //	MSAMasterConfirmation mSAMasterConfirmation = new MSAMasterConfirmation();
                            //	mSAMasterConfirmation.Deleteflag = model.Deleteflag;
                            //	mSAMasterConfirmation.Confirmationflag = model.Confirmationflag;
                            //	mSAMasterConfirmation.PAID = model.PAID;
                            //	mSAMasterConfirmation.ConfirmedBy = model.UploadedBy;
                            //	mSAMasterConfirmation.ConfirmedDate = DateTime.Now;
                            //	mSAMasterConfirmation.UploadedBy = model.UploadedBy;
                            //	mSAMasterConfirmation.UplaodedDate = DateTime.Now;
                            //	obj.MSAMasterConfirmations.Add(mSAMasterConfirmation);
                            //	obj.SaveChanges();
                            //	model.StatusRemarks = "4";
                            //}
                            this.emailDA.msaconfirmationmail(Convert.ToInt32(model.PAID));
                        }
                        else
                        {
                            model.StatusRemarks = "-2";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                model.StatusRemarks = "-1";
                throw;
            }
            status = model;
            return status;
        }
        private bool IsPAVaild(int? paid, List<RPALoadItemForMSA> RPAViewData)
        {
            bool result = false;
            try
            {
                foreach (var item in RPAViewData)
                {
                    if (paid != item.paid)
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(item.mscode) || string.IsNullOrEmpty(item.ItemDescription))
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(item.WBS) || string.IsNullOrEmpty(item.Quantity.ToString()) || string.IsNullOrEmpty(item.RequirementDate) || string.IsNullOrEmpty(item.Project))
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(item.UnitPrice.ToString()) || string.IsNullOrEmpty(item.Currency) || string.IsNullOrEmpty(item.StorageLocation) || string.IsNullOrEmpty(item.text))
                    {
                        return false;
                    }
                    if (string.IsNullOrEmpty(item.TokuchuNo) || string.IsNullOrEmpty(item.PAItemID.ToString()))
                    {
                        return false;
                    }

                    else
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        public async Task<MSAMasterConfirmation> clearMSAConfirmation(MSAMasterConfirmation model)
        {
            try
            {
                var data = obj.MSAMasterConfirmations.Where(x => x.PAID == model.PAID && x.Deleteflag == false && x.Confirmationflag == true).FirstOrDefault();
                if (data != null)
                {
                    data.Deleteflag = model.Deleteflag;
                    data.DeletedBy = model.DeletedBy;
                    data.DeletedRemarks = model.DeletedRemarks;
                    data.DeletedDate = DateTime.Now;
                    obj.SaveChanges();
                    model.StatusRemarks = "1";
                    var msalintItem = obj.MSALineItems.Where(x => x.paid == model.PAID && x.deletionflag == false).ToList();
                    if (msalintItem.Count > 0)
                    {
                        foreach (var item in msalintItem)
                        {
                            item.deletionflag = true;
                            obj.SaveChanges();
                        }
                        model.StatusRemarks = "2";
                    }
                }
                else
                {
                    model.StatusRemarks = "0";
                }
            }
            catch (Exception ex)
            {
                model.StatusRemarks = "-1";
                throw;
            }
            return model;
        }
        public async Task<statuscheckmodel> Unmappingitem(MappingItemModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var sqlquery = "update MPRRfqItems set DeleteFlag=1 where MPRItemDetailsid='" + model.previousitemdetails + "' and RfqItemsid='" + model.RFQItemsId + "' and MPRRevisionId='" + model.itemrevision + "'";
                var cmd = obj.Database.Connection.CreateCommand();
                cmd.CommandText = sqlquery;
                cmd.Connection.Open();
                cmd.ExecuteReader();
                cmd.Connection.Close();

                var id = obj.MPRRfqItems.Where(x => x.MPRItemDetailsid == model.previousitemdetails && x.RfqItemsid == model.RFQItemsId && x.MPRRevisionId == model.itemrevision).FirstOrDefault()?.MPRRFQitemId;

                var sqlquery1 = "update MPRRfqItemInfos set Deleteflag=1 where MPRRFQitemId='" + id + "'";
                var cmd1 = obj.Database.Connection.CreateCommand();
                cmd1.CommandText = sqlquery1;
                cmd1.Connection.Open();
                cmd1.ExecuteReader();
                cmd1.Connection.Close();

                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<IncoTermMaster>> getincotermmaster()
        {
            List<IncoTermMaster> master = new List<IncoTermMaster>();
            try
            {
                master = obj.IncoTermMasters.ToList();
                return master;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<statuscheckmodel> updatetokuchubyid(int tokuchuid)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var query = "update TokuchuRequest set DownloadedBy=NULL,DownloadedOn=NULL, CompletedStatus=NULL,CompletedOn=NULL where TokuchRequestid ='" + tokuchuid + "'";
                var cmd = obj.Database.Connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Connection.Open();
                cmd.ExecuteReader();
                cmd.Connection.Close();
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> UpdateMsaprconfirmation(List<ItemsViewModel> msa)
        {
            statuscheckmodel status = new statuscheckmodel();
            int paid = msa[0].paid;
            try
            {
                var podata = obj.PAItems.Where(x => x.PAID == paid).ToList();
                foreach (var item in msa)
                {
                    var data = obj.PAItems.Where(x => x.PAItemID == item.paitemid).FirstOrDefault();
                    data.PRno = item.PRno;
                    data.PRLineItemNo = item.PRLineItemNo;
                    data.PRcreatedOn = System.DateTime.Now;
                    //data.PRcreatedBy = item.PRcreatedBy;
                    //obj.MSALineItems.Add(item);
                    obj.SaveChanges();
                }
                //var data = obj.MSALineItems.Where(x => x.PAItemID == msa.PAItemID).FirstOrDefault();
                //MSALineItem item = new MSALineItem();

                return status;
            }
            catch (Exception ex)
            {
                throw ex;
                //log.ErrorMessage("PAController", "UpdateMsaprconfirmation", ex.Message + "; " + ex.StackTrace.ToString());
            }
            //try
            //{
            //	var data = obj.MSALineItems.Where(x => x.PAItemID == msa.PAItemID).FirstOrDefault();
            //	//MSALineItem item = new MSALineItem();
            //	data.PRno = msa.PRno;
            //	data.PRLineItemNo = msa.PRLineItemNo;
            //	data.PRcreatedOn = msa.PRcreatedOn;
            //	data.PRcreatedBy = msa.PRcreatedBy;
            //	//obj.MSALineItems.Add(item);
            //	obj.SaveChanges();
            //	return status;
            //}
            //catch (Exception ex)
            //{
            //	log.ErrorMessage("PAController", "UpdateMsaprconfirmation", ex.Message + "; " + ex.StackTrace.ToString());
            //}
        }
        public async Task<List<MSAProcessTrack>> getmsaprocesstrackbyId(int paid)
        {
            List<MSAProcessTrack> process = new List<MSAProcessTrack>();
            try
            {
                var data = obj.MSAMasterConfirmations.Where(x => x.PAID == paid).FirstOrDefault();
                if (data != null)
                {
                    process = obj.MSAProcessTracks.Where(x => x.MSAMasterID == data.MSAMasterID).ToList();
                }
                return process;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<statuscheckmodel> InsertScrapRregister(ScrapRegisterMasterModel msa)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                ScrapEntryMaster scrap = new ScrapEntryMaster();
                scrap.TruckNo = msa.TruckNo;
                scrap.Dateofentry = msa.DateOfEntry;
                scrap.PreparedDate = System.DateTime.Now;
                scrap.RequesterDepartmentID = msa.RequesterDepartmentID;
                scrap.RequestedBY = msa.RequestedBY;
                scrap.ApprovalBy = msa.ApprovalBy;
                scrap.PreparedBY = msa.PreparedBY;
                scrap.Vendor = msa.Vendor;
                scrap.Vendorid = msa.Vendorcode;
                obj.ScrapEntryMasters.Add(scrap);
                obj.SaveChanges();
                status.Sid = scrap.ScrapentryId;
                foreach (var item in msa.scrapitems)
                {
                    var item1 = new ScrapItem();
                    item1.UOM = item.UOM;
                    item1.Description = item.Description;
                    item1.Qty = item.Qty;
                    item1.UnitPrice = item.UnitPrice;
                    if (item.Qty > 0 && item.UnitPrice > 0)
                        item1.BasicPrice = item.Qty * item.UnitPrice;
                    // Note : Here item.cgstamount means cgst(in %),item.sgstamount (in %), item.igstamount (in %). 
                    //SInce in UI Model there is no use of amount so please consider as it percent and calculate amount.
                    item1.TCS = item.tcs;
                    item1.CGST = item.cgstamount;
                    item1.SGST = item.sgstamount;
                    item1.IGST = item.igstamount;

                    // Now we calculate Amount of all kind of tax.
                    if (item1.BasicPrice > 0)
                    {
                        item1.TCSAmount = (item1.BasicPrice * item1.TCS) / 100;
                        item1.CGSTAmount = (item1.BasicPrice * item1.CGST) / 100;
                        item1.SGSTAmount = (item1.BasicPrice * item1.SGST) / 100;
                        item1.IGSTAmount = (item1.BasicPrice * item1.IGST) / 100;
                    }
                    item1.TotalBasic = item1.BasicPrice + item1.TCSAmount;
                    item1.TotalPrice = item1.BasicPrice + item1.TCSAmount + item1.SGSTAmount + item1.CGSTAmount + item1.IGSTAmount;

                    item1.ItemId = item.ItemId;
                    item1.Scrapentryid = status.Sid;
                    item1.Scraptype = item.Scraptype;

                    item1.Itemcode = item.Itemcode;
                    obj.ScrapItems.Add(item1);
                    obj.SaveChanges();
                }
                //updatetotalprice(status.Sid); no need to call this already done above line no 3496 

                if (msa.ScrapStatusId == 0)
                {
                    ScrapStatusTrack scarpstatus = new ScrapStatusTrack();
                    scarpstatus.scrapid = status.Sid;
                    scarpstatus.stausid = 1;
                    scarpstatus.UpdatedBy = msa.PreparedBY;
                    scarpstatus.UpdatedDate = System.DateTime.Now;
                    obj.ScrapStatusTracks.Add(scarpstatus);
                    obj.SaveChanges();
                    status.Scrapstatusid = Convert.ToInt32(scarpstatus.stausid);
                }
                this.emailDA.ScrapApprovalRequest(status.Sid, msa.ApprovalBy, msa.PreparedBY, "Approverequest");
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool updatetotalprice(int scrapid)
        {
            var sqlquery = "update ScrapItems set TotalPrice=((UnitPrice * Qty) + (((UnitPrice * Qty)  *  ISNULL(sgstamount,0)/100 ) + ((UnitPrice * Qty)  * ISNULL(CGSTAmount,0)/100))) where Scrapentryid='" + scrapid + "'";
            //var sqlquery = "insert into scrapstatustrack(stausid,scrapid,UpdatedBy,UpdatedDate) values(6,'" + msa.scrapid + "','400108','" + dt.ToString("yyyy-MM-dd hh:mm:ss") + "')";
            var cmd = obj.Database.Connection.CreateCommand();
            cmd.CommandText = sqlquery;
            cmd.Connection.Open();
            cmd.ExecuteReader();
            cmd.Connection.Close();
            //SqlParameter[] paramArr = new SqlParameter[1];
            //paramArr[0] = new SqlParameter("@scrapid", scrapid);
            //string con = obj.Database.Connection.ConnectionString;
            //SqlConnection Conn1 = new SqlConnection(con);
            //SqlCommand Cmd = new SqlCommand();
            //Cmd.Connection = Conn1;
            //Cmd.CommandText = "UpdatescrapTotalprice";
            //Cmd.CommandTimeout = 0;
            //Cmd.CommandType = CommandType.StoredProcedure;

            //if (paramArr != null)
            //{
            //	foreach (SqlParameter sqlParam in paramArr)
            //	{
            //		Cmd.Parameters.Add(sqlParam);
            //	}
            //}
            //SqlDataAdapter adp = new SqlDataAdapter();
            //adp = new SqlDataAdapter(Cmd);
            //DataSet Ds = new DataSet();
            //Ds = new DataSet();
            //adp.Fill(Ds);

            //Cmd.Parameters.Clear();
            return true;
        }
        public async Task<statuscheckmodel> UpdateScrapRregister(ScrapRegisterMasterModel msa)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                var sql = "";
                DateTime dt = DateTime.Now;
                if (msa.scraptype == "socreated")
                {
                    var socreate = obj.ScrapEntryMasters.Where(x => x.ScrapentryId == msa.scrapid).FirstOrDefault();
                    if (!string.IsNullOrEmpty(socreate.SONo))
                    {
                        socreate.Soupdatedby = msa.employeeno;
                        socreate.Soupdateddate = System.DateTime.Now;
                    }
                    socreate.SoDate = msa.SoDate;
                    socreate.SONo = msa.SONO;

                    socreate.statusid = 2;
                    obj.SaveChanges();

                    var sovalue = msa.statustarck.FindIndex(x => x.stausid == 2);
                    if (sovalue == -1)
                    {
                        sql = "insert into scrapstatustrack(stausid,scrapid,UpdatedBy,UpdatedDate) values(2,'" + msa.scrapid + "','400108','" + dt.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        var cmd = obj.Database.Connection.CreateCommand();
                        cmd.CommandText = sql;
                        cmd.Connection.Open();
                        cmd.ExecuteReader();
                        cmd.Connection.Close();
                    }
                    //var sql = "insert into scrapstatustrack(stausid,scrapid,UpdatedBy,UpdatedDate) values(2,'" + msa.scrapid + "','400108','" + dt.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    //var sqlquery = "update scrapstatustrack set stausid=6,Remarks='" + msa.ApprovalRemarks + "' where scrapid='" + msa.scrapid + "'";

                    IEnumerable<string> vatrequstor = obj.Scrapflows.Where(x => x.Scrapflow1 == "VatInvoice").ToList().Select(y => y.incharge);
                    foreach (var item in vatrequstor)
                    {
                        this.emailDA.ScrapApprovalRequest(msa.scrapid, item, msa.employeeno, "vatinvoice");
                    }

                    //return status;
                }
                else if (msa.scraptype == "vatinvoice")
                {
                    var vatinvoice = obj.ScrapEntryMasters.Where(x => x.ScrapentryId == msa.scrapid).FirstOrDefault();
                    if (!string.IsNullOrEmpty(vatinvoice.VATInvoiceNo))
                    {
                        vatinvoice.VATInvoiceupdatedby = msa.employeeno;
                        vatinvoice.VatInvoiceUpdatedDate = System.DateTime.Now;
                    }
                    vatinvoice.VATInvoiceNo = msa.VATInvoiceno;
                    vatinvoice.VATInvoiceDate = msa.VATInvoiceDate;

                    vatinvoice.statusid = 3;
                    obj.SaveChanges();
                    var sovalue = msa.statustarck.FindIndex(x => x.stausid == 3);
                    if (sovalue == -1)
                    {
                        sql = "insert into scrapstatustrack(stausid,scrapid,UpdatedBy,UpdatedDate) values(3,'" + msa.scrapid + "','400108','" + dt.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        var cmd = obj.Database.Connection.CreateCommand();
                        cmd.CommandText = sql;
                        cmd.Connection.Open();
                        cmd.ExecuteReader();
                        cmd.Connection.Close();
                    }
                    //var sql = "insert into scrapstatustrack(stausid,scrapid,UpdatedBy,UpdatedDate) values(3,'" + msa.scrapid + "','400108','" + dt.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    //var sqlquery = "update scrapstatustrack set stausid=3,Remarks='" + msa.ApprovalRemarks + "' where scrapid='" + msa.scrapid + "'";

                    IEnumerable<string> taxrequstor = obj.Scrapflows.Where(x => x.Scrapflow1 == "FundVerification").ToList().Select(y => y.incharge);
                    foreach (var item in taxrequstor)
                    {
                        this.emailDA.ScrapApprovalRequest(msa.scrapid, item, msa.employeeno, "FundVerification");
                    }
                    IEnumerable<string> dispatchrequstor = obj.Scrapflows.Where(x => x.Scrapflow1 == "ReadyToDisPatch").ToList().Select(y => y.incharge);
                    foreach (var item in dispatchrequstor)
                    {
                        this.emailDA.ScrapApprovalRequest(msa.scrapid, item, msa.employeeno, "ReadyToDisPatch");
                    }
                }
                else if (msa.scraptype == "Taxinvoice")
                {
                    var taxinvoice = obj.ScrapEntryMasters.Where(x => x.ScrapentryId == msa.scrapid).FirstOrDefault();
                    taxinvoice.FundAvailableWithVendor = msa.fundavailablewithvendor;
                    taxinvoice.vendorVerificationremarks = msa.fundavendorremarks;
                    //taxinvoice.TaxInvoiceUpdatedY = msa.TaxInvoiceUpdatedY;
                    taxinvoice.VendorVerifierUpdateddate = System.DateTime.Now;
                    taxinvoice.statusid = 5;
                    obj.SaveChanges();
                    var sovalue = msa.statustarck.FindIndex(x => x.stausid == 5);
                    if (sovalue == -1)
                    {
                        sql = "insert into scrapstatustrack(stausid,scrapid,UpdatedBy,UpdatedDate) values(5,'" + msa.scrapid + "','400108','" + dt.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                        var cmd = obj.Database.Connection.CreateCommand();
                        cmd.CommandText = sql;
                        cmd.Connection.Open();
                        cmd.ExecuteReader();
                        cmd.Connection.Close();
                    }
                    //var sql = "insert into scrapstatustrack(stausid,scrapid,UpdatedBy,UpdatedDate) values(5,'" + msa.scrapid + "','400108','"+ dt.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    //var sqlquery = "update scrapstatustrack set stausid=5,Remarks='" + msa.ApprovalRemarks + "' where scrapid='" + msa.scrapid + "'";

                }
                else if (msa.scraptype == "readytodispatch")
                {
                    var readytodispatch = obj.ScrapEntryMasters.Where(x => x.ScrapentryId == msa.scrapid).FirstOrDefault();
                    readytodispatch.VerifierBy = msa.employeeno;
                    readytodispatch.VerifierRemarks = msa.VerifierRemarks;
                    obj.SaveChanges();
                    var sovalue = msa.statustarck.FindIndex(x => x.stausid == 7);
                    if (sovalue == -1)
                    {
                        sql = "insert into scrapstatustrack(stausid,scrapid,UpdatedBy,UpdatedDate) values(7,'" + msa.scrapid + "','400108','" + dt.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    }
                    //var sql = "insert into scrapstatustrack(stausid,scrapid,UpdatedBy,UpdatedDate) values(5,'" + msa.scrapid + "','400108','"+ dt.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    //var sqlquery = "update scrapstatustrack set stausid=5,Remarks='" + msa.ApprovalRemarks + "' where scrapid='" + msa.scrapid + "'";
                    var cmd = obj.Database.Connection.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                    //readytodispatch.ver
                    IEnumerable<string> dispatchrequstor = obj.ScrapEntryMasters.Where(x => x.ScrapentryId == msa.scrapid).ToList().Select(y => y.ApprovalBy);
                    foreach (var item in dispatchrequstor)
                    {
                        this.emailDA.ScrapApprovalRequest(msa.scrapid, item, msa.employeeno, "DisPatchDone");
                    }
                }
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<DataTable> getscraplist(scrapsearchmodel model)
        {
            DataTable table = new DataTable();
            try
            {
                var query = "";
                query = "select  distinct TruckNo,md.Department,md.DepartmentId ,sf.ScrapentryId," +
                "er.Name as requestedby,statusid,case when statusid = 2 then 'SoCreated' when statusid = 3 " +
                "then 'Vat Incoice Raised' when statusid = 5 then 'TaxInvoice Raised' when statusid = 7 then 'Disptached' when statusid = 6 then 'Scrap Request Approved' else 'Scrap Request Submitted' end as Scrapstatus, " +
                "ep.Name as preparedby,ea.Name as ApprovedBy,RequesterDepartmentID,RequestedBY,ApprovalRemarks,PreparedBY,ApprovalBy,ApprovalDate,ApprovalStatus," +
                "SONo,VATInvoiceNo,TaxInvoiceNo,VerifierRemarks from ScrapEntryMaster sf inner join MPRDepartments md on md.DepartmentId = sf.RequesterDepartmentID " +
                "inner join Employee er on er.EmployeeNo = sf.RequestedBY inner join Employee ep on ep.EmployeeNo = sf.PreparedBY " +
                "inner join Employee ea on ea.EmployeeNo = sf.ApprovalBy inner join scrapstatustrack st on sf.ScrapentryId = st.scrapid " +
                "inner join ScrapItems sii on sii.Scrapentryid=sf.ScrapentryId where sf.ScrapentryId is not null ";
                if (model.DepartmentId != 0)
                    query += " and md.DepartmentId='" + model.DepartmentId + "'";
                if (!string.IsNullOrEmpty(model.Vendorid))
                    query += " and Vendorid='" + model.Vendorid + "'";
                if (model.scrapfrom != null)
                    query += " and PreparedDate>='" + model.scrapfrom + "' and PreparedDate<='" + model.scrapto + "'";
                if (!string.IsNullOrEmpty(model.scraptype))
                    query += " and sii.Scraptype='" + model.scraptype + "'";
                //if (model.scraptype == "Ready For Dispatch")
                //	query += " and sf.VerifierRemarks is null";
                if (!string.IsNullOrEmpty(model.truckno))
                    query += " and sf.TruckNo='" + model.truckno + "' ";

                var cmd = obj.Database.Connection.CreateCommand();
                cmd.CommandText = query;

                cmd.Connection.Open();
                table.Load(cmd.ExecuteReader());
                cmd.Connection.Close();
                return table;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<ScrapRegisterMasterModel> getscrapitembyid(int scrapid)
        {
            ScrapRegisterMasterModel scrap = new ScrapRegisterMasterModel();
            try
            {
                var data = obj.ScrapEntryMasters.Where(x => x.ScrapentryId == scrapid).Include(x => x.ScapRegisterDocuments).FirstOrDefault();
                scrap.RequesterDepartmentID = data.RequesterDepartmentID;
                scrap.DepartmentName = obj.Departments.Where(x => x.DepartmentId == data.RequesterDepartmentID).FirstOrDefault().DepartmentName;
                scrap.TruckNo = data.TruckNo;
                scrap.DateOfEntry = data.Dateofentry;
                scrap.ApprovalBy = data.ApprovalBy;
                scrap.ApprovalName = obj.Employees.Where(x => x.EmployeeNo == data.ApprovalBy).FirstOrDefault().Name;
                scrap.RequestedBY = data.RequestedBY;
                scrap.RequestedName = obj.Employees.Where(x => x.EmployeeNo == data.RequestedBY).FirstOrDefault().Name;
                scrap.PreparedBY = data.PreparedBY;
                //scrap.PreparedDate = data.PreparedDate;
                scrap.scrapid = data.ScrapentryId;
                scrap.Approvalstatus = data.ApprovalStatus;
                scrap.SONO = data.SONo;
                scrap.SoDate = data.SoDate;
                if (!string.IsNullOrEmpty(data.Soupdatedby))
                {
                    scrap.Soupdatedby = obj.Employees.Where(x => x.EmployeeNo == data.Soupdatedby).FirstOrDefault().Name;
                    scrap.Soupdateddate = data.Soupdateddate;
                }
                scrap.VATInvoiceno = data.VATInvoiceNo;
                scrap.VATInvoiceDate = data.VATInvoiceDate;
                if (!string.IsNullOrEmpty(data.VATInvoiceupdatedby))
                {
                    scrap.VATInvoiceupdatedby = obj.Employees.Where(x => x.EmployeeNo == data.VATInvoiceupdatedby).FirstOrDefault().Name;
                    scrap.VatInvoiceUpdatedDate = data.VatInvoiceUpdatedDate;
                }
                scrap.GatePAssNo = data.GatePAssNo;
                scrap.GatePassDate = data.GatePAssDate;
                scrap.GatePassupdateby = data.GatePassupdateby;
                scrap.GatePassupdateddate = data.GatePassupdateddate;
                scrap.ScrapStatusId = Convert.ToInt32(data.statusid);
                scrap.fundavailablewithvendor = Convert.ToDecimal(data.FundAvailableWithVendor);
                scrap.fundavendorremarks = data.vendorVerificationremarks;
                scrap.TaxInvoiceNo = data.TaxInvoiceNo;
                scrap.TaxInvoiceDate = data.TaxInvoiceDate;
                scrap.TaxInvoiceUpdatedY = data.TaxInvoiceUpdatedY;
                scrap.TaxInvoiceUpdatedDate = data.TaxInvoiceUpdatedDate;
                scrap.Vendor = data.Vendor;
                if (data.VerifierBy != null)
                {
                    scrap.Verifier = obj.Employees.Where(x => x.EmployeeNo == data.VerifierBy).FirstOrDefault().Name;
                }
                scrap.VerifierRemarks = data.VerifierRemarks;

                var scrapitem = obj.ScrapItems.Where(x => x.Scrapentryid == scrapid).ToList();
                scrap.scrapitems = scrapitem.Select(x => new ScrapItems()
                {
                    ItemId = x.ItemId,
                    //PriceType=x.PriceType,
                    sgstamount = x.SGSTAmount,
                    cgstamount = x.CGSTAmount,
                    igstamount = x.IGSTAmount,
                    Qty = Convert.ToDecimal(x.Qty),
                    Description = x.Description,
                    UOM = x.UOM,
                    tcs = x.TCS,
                    UnitPrice = Convert.ToDecimal(x.UnitPrice),
                    TotalPrice = x.TotalPrice,
                    Scraptype = x.Scraptype,
                    Itemcode = x.Itemcode,
                    ScrapItemid = x.ScrapItemid
                    //Scratypeid=x.scs
                }).ToList();
                var documentsdata = obj.ScapRegisterDocuments.Where(x => x.Scrapentryid == scrapid).ToList();
                scrap.documents = documentsdata.Select(x => new ScapRegisterDocumentModel()
                {
                    DocumentNAme = x.DocumentNAme,
                    path = x.path,
                    Scrapentryid = x.Scrapentryid,
                    DocumentType = x.DocumentType,
                }).ToList();
                var queru1 = "select sc.Status,se.ScrapentryId,st.stausid,st.scrapid,st.UpdatedDate from ScrapEntryMaster se inner join scrapstatustrack st on se.ScrapentryId=st.scrapid inner join ScrapStatus sc on sc.Statusid = st.Stausid where ScrapentryId='" + scrapid + "'";
                scrap.statustarck = obj.Database.SqlQuery<ScrapStatustarckModel>(queru1).ToList();
                //var scrapstatustarck = obj.scrapstatustrackviews.Where(x => x.ScrapentryId == scrapid).ToList();
                //scrap.statustarck = scrapstatustarck.Select(x => new ScrapStatustarckModel()
                //{
                //	Status=x.Status,
                //	statusid=Convert.ToInt32(x.stausid),
                //	scrapid=Convert.ToInt32(x.scrapid)
                //}).ToList();
                return scrap;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<statuscheckmodel> UpdateScrapRequest(ScrapRegisterMasterModel msa)
        {
            statuscheckmodel status = new statuscheckmodel();
            DateTime dt = DateTime.Now;
            try
            {
                var approvedata = obj.ScrapEntryMasters.Where(x => x.ScrapentryId == msa.scrapid).FirstOrDefault();
                approvedata.ApprovalBy = msa.ApprovalBy;
                approvedata.ApprovalStatus = msa.Approvalstatus;
                approvedata.ApprovalDate = System.DateTime.Now;
                approvedata.ApprovalRemarks = msa.ApprovalRemarks;
                if (msa.Approvalstatus == "Approved")
                {
                    approvedata.statusid = 6;
                    status.Scrapstatusid = 6;
                    obj.SaveChanges();
                    var sqlquery = "insert into scrapstatustrack(stausid,scrapid,UpdatedBy,UpdatedDate) values(6,'" + msa.scrapid + "','400108','" + dt.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                    //var sqlquery = "update scrapstatustrack set stausid=6,Remarks='"+ msa.ApprovalRemarks +"' where scrapid='" + msa.scrapid + "'";
                    var cmd = obj.Database.Connection.CreateCommand();
                    cmd.CommandText = sqlquery;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                    IEnumerable<string> soincharge = obj.Scrapflows.Where(x => x.Scrapflow1 == "SoCreation").ToList().Select(y => y.incharge);
                    //string soinchargemail = obj.Employees.Where(x => x.EmployeeNo == soincharge).FirstOrDefault().EMail;
                    foreach (var item in soincharge)
                    {
                        this.emailDA.ScrapApprovalRequest(msa.scrapid, item, approvedata.ApprovalBy, "sorequest");
                    }
                }
                //obj.ScrapEntryMasters.Add(approvedata);				
                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertScarpflowIncharge(ScrapflowModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                Scrapflow flow = new Scrapflow();
                flow.incharge = model.Incharge;
                flow.Scrapflow1 = model.Scrapflow;
                flow.Createdby = model.createdby;
                flow.Createddate = System.DateTime.Now;
                flow.active = true;
                obj.Scrapflows.Add(flow);
                obj.SaveChanges();
                return status;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<ScrapflowModel>> Getincharelist()
        {
            List<ScrapflowModel> flow = new List<ScrapflowModel>();
            try
            {
                var sqlquery = "select sf.scrapflow,sf.incharge,e.name as inchargename,e1.name as createdby,sf.createddate from scrapflow sf inner join Employee e on e.EmployeeNo=sf.incharge inner join Employee e1 on e1.EmployeeNo = sf.createdby";
                flow = obj.Database.SqlQuery<ScrapflowModel>(sqlquery).ToList();
                return flow;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<ScrapflowModel>> Getincharepermissionlist(scrapsearchmodel search)
        {
            List<ScrapflowModel> flow = new List<ScrapflowModel>();
            try
            {
                var sqlquery = "select sf.scrapflow,e.name as inchargename,e1.name as createdby,sf.createddate from scrapflow sf inner join Employee e on e.EmployeeNo=sf.incharge inner join Employee e1 on e1.EmployeeNo = sf.createdby where incharge='" + search.employeeno + "'";
                flow = obj.Database.SqlQuery<ScrapflowModel>(sqlquery).ToList();
                return flow;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<ScrapitemRatecontract> Getscrapitemdetails(string itemcode)
        {
            ScrapitemRatecontract scrap = new ScrapitemRatecontract();
            try
            {
                scrap = obj.ScrapitemRatecontracts.Where(x => x.Material == itemcode).FirstOrDefault();
                return scrap;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<DataTable> getscraplistbysearch(scrapsearchmodel model)
        {
            DataTable table = new DataTable();
            try
            {
                var query = "";
                query = "select  distinct TruckNo,md.Department,md.DepartmentId,sf.PreparedDate,sf.ScrapentryId,er.Name as requestedby,statusid," +
                        "case when statusid = 2 then 'SoCreated' when statusid = 3 then 'Vat Incoice Raised' when statusid = 5 then 'TaxInvoice Raised' when statusid = 7 then 'Disptached' when statusid=6 then 'Scrap Request Approved' else 'Scrap Request Submitted' end as Scrapstatus," +
                        "ep.Name as preparedby,ea.Name as ApprovedBy,RequesterDepartmentID,RequestedBY,ApprovalRemarks,PreparedBY,ApprovalBy," +
                        "ApprovalDate,ApprovalStatus,SONo,VATInvoiceNo,TaxInvoiceNo,VerifierRemarks from ScrapEntryMaster sf " +
                        "inner join MPRDepartments md on md.DepartmentId = sf.RequesterDepartmentID inner join Employee er on er.EmployeeNo = sf.RequestedBY" +
                        " inner join Employee ep on ep.EmployeeNo = sf.PreparedBY inner join Employee ea on ea.EmployeeNo = sf.ApprovalBy " +
                        "inner join scrapstatustrack st on sf.ScrapentryId = st.scrapid inner join ScrapItems sii on sii.Scrapentryid = sf.ScrapentryId " +
                        "where sf.ScrapentryId is not null ";

                if (!string.IsNullOrEmpty(model.truckno))
                    query += " and sf.TruckNo='" + model.truckno + "' ";
                if (!string.IsNullOrEmpty(model.scraptype))
                    query += " and sii.Scraptype='" + model.scraptype + "'";
                if (model.DepartmentId != 0)
                    query += " and md.DepartmentId='" + model.DepartmentId + "' ";
                if (model.scrapfrom != null)
                    query += " and PreparedDate>='" + model.scrapfrom + "'";
                if (model.scrapto != null)
                    query += " and PreparedDate<='" + model.scrapto + "'";
                if (!string.IsNullOrEmpty(model.scraptypeapprove))
                {
                    if (model.scraptypeapprove == "SoCreation")
                    {
                        query += " and sf.SONo is not null ";
                    }
                    if (model.scraptypeapprove == "Vatinvoice")
                    {
                        query += " and sf.VATInvoiceNo is not null ";
                    }
                    if (model.scraptypeapprove == "taxinvoice")
                    {
                        query += " and sf.TaxInvoiceNo is not null ";
                    }
                }
                if (!string.IsNullOrEmpty(model.scraptypepending))
                {
                    if (model.scraptypepending == "SoCreation")
                    {
                        query += " and sf.SONo is  null ";
                    }
                    if (model.scraptypepending == "Vatinvoice")
                    {
                        query += " and sf.VATInvoiceNo is  null ";
                    }
                    if (model.scraptypepending == "taxinvoice")
                    {
                        query += " and sf.TaxInvoiceNo is null ";
                    }
                }

                if (!string.IsNullOrEmpty(model.Vendorid))
                    query += " and Vendorid='" + model.Vendorid + "'";
                //            if (model.scraptype == null)
                //            {
                //	query = "select  distinct TruckNo,md.Department,md.DepartmentId ,sf.ScrapentryId,er.Name as requestedby,statusid," +
                //		"case when statusid = 2 then 'SoCreated' when statusid = 3 then 'Vat Incoice Raised' when statusid = 5 then 'TaxInvoice Raised' when statusid = 7 then 'Disptached' else '' end as Scrapstatus," +
                //		"ep.Name as preparedby,ea.Name as ApprovedBy,RequesterDepartmentID,RequestedBY,ApprovalRemarks,PreparedBY,ApprovalBy," +
                //		"ApprovalDate,ApprovalStatus,SONo,VATInvoiceNo,TaxInvoiceNo,VerifierRemarks from ScrapEntryMaster sf " +
                //		"inner join MPRDepartments md on md.DepartmentId = sf.RequesterDepartmentID inner join Employee er on er.EmployeeNo = sf.RequestedBY" +
                //		" inner join Employee ep on ep.EmployeeNo = sf.PreparedBY inner join Employee ea on ea.EmployeeNo = sf.ApprovalBy " +
                //		"inner join scrapstatustrack st on sf.ScrapentryId = st.scrapid inner join ScrapItems sii on sii.Scrapentryid = sf.ScrapentryId " +
                //		"where sf.ScrapentryId is not null ";
                //	var sodata = model.scrapflow.Contains("SOCreated");
                //	if (sodata == true)
                //		query += " and sf.sono is null";
                //	var vatdata = model.scrapflow.Contains("Vatinvoice generated");
                //	if (vatdata == true)
                //		query += " and sf.VATInvoiceNo is null";
                //	var taxdata = model.scrapflow.Contains("Taxinvoice raised");
                //	if (taxdata == true)
                //		query += " and sf.TaxInvoiceNo is null";
                //	var dispatchdata = model.scrapflow.Contains("Ready For Dispatch");
                //	if (dispatchdata == true)
                //		query += " and sf.VerifierRemarks is null";
                //}
                //            else
                //            {

                //}
                var cmd = obj.Database.Connection.CreateCommand();
                cmd.CommandText = query;

                cmd.Connection.Open();
                table.Load(cmd.ExecuteReader());
                cmd.Connection.Close();
                return table;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<ScrapflowModel>> getauthorizescrapflowlist(string employeeno)
        {
            List<ScrapflowModel> flow = new List<ScrapflowModel>();
            try
            {
                var scrapflow = obj.Scrapflows.Where(x => x.incharge == employeeno && x.active == true).ToList();
                flow = scrapflow.Select(x => new ScrapflowModel()
                {
                    Scrapflow = x.Scrapflow1,

                }).ToList();
                return flow;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /*Name of Function : <<GetCMMMonthlyPerformance1>>  Author :<<Rahul>>  
Date of Creation <<>>
Purpose : <<Getting Monthly Performance report buyer group wise >>
Review Date :<<>>   Reviewed By :<<>>*/
        public DataSet GetCMMMonthlyPerformance1(string spName, SqlParameter[] paramArr)
        {
            string con = obj.Database.Connection.ConnectionString;
            SqlConnection Conn1 = new SqlConnection(con);
            EmployeModel employee = new EmployeModel();
            DataSet Ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter Adp = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.Connection = Conn1;
                cmd.CommandText = spName;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                if (paramArr != null)
                {
                    foreach (SqlParameter sqlParam in paramArr)
                    {
                        cmd.Parameters.Add(sqlParam);
                    }
                }

                Adp = new SqlDataAdapter(cmd);
                Ds = new DataSet();

                Adp.Fill(Ds);
                cmd.Parameters.Clear();
                //Ds.Clear();
                return Ds;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /*Name of Function : <<GetCMMMonthlyPerformance1>>  Author :<<Rahul>>  
Date of Creation <<>>
Purpose : <<Getting Monthly Performance report buyer group wise >>
Review Date :<<>>   Reviewed By :<<>>*/
        public DataSet GetCMMMonthlyPerformance2(string spName, SqlParameter[] paramArr)
        {
            string con = obj.Database.Connection.ConnectionString;
            SqlConnection Conn1 = new SqlConnection(con);
            EmployeModel employee = new EmployeModel();
            DataSet Ds = new DataSet();
            try
            {

                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter Adp = new SqlDataAdapter();
                cmd = new SqlCommand();
                cmd.Connection = Conn1;
                cmd.CommandText = spName;
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                if (paramArr != null)
                {
                    foreach (SqlParameter sqlParam in paramArr)
                    {
                        cmd.Parameters.Add(sqlParam);
                    }
                }

                Adp = new SqlDataAdapter(cmd);
                Ds = new DataSet();

                Adp.Fill(Ds);
                cmd.Parameters.Clear();
                //Ds.Clear();
                return Ds;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<List<Loadpaforpocration>> LoadItemsforpocreation(PADetailsModel masters)
        {
            List<Loadpaforpocration> pocreation = new List<Loadpaforpocration>();
            try
            {
                var query = "select * from Loadpaforpocration where paid is not null ";
                if (masters.Paid != 0)
                    query += " and paid=" + masters.Paid + "";
                if (!string.IsNullOrEmpty(masters.DocumentNumber))
                    query += " and DocumentNo='" + masters.DocumentNumber + "'";
                pocreation = obj.Database.SqlQuery<Loadpaforpocration>(query).ToList();
                return pocreation;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<LoadItemsforpo>> LoadItemsforpogeneration(List<int> paid)
        {
            List<LoadItemsforpo> pocreation = new List<LoadItemsforpo>();
            string padata = "";
            try
            {
                padata = string.Join(" ',' ", paid);
                var query = "select * from LoadItemsforpo where paid in ('" + padata + "') and PAStatus='Approved' ";
                pocreation = obj.Database.SqlQuery<LoadItemsforpo>(query).ToList();
                return pocreation;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<LoadItemsByPAID>> LoadItemsforpogenerationbasedonvendor(string VendorId, List<int> PAId)
        {
            List<LoadItemsByPAID> pocreation = new List<LoadItemsByPAID>();
            string padata = "";
            try
            {
                padata = string.Join(" ',' ", PAId);
                var query = "select * from LoadItemsforpo where VendorId ='" + VendorId + "' and PAStatus='Approved' and POStatus='Pending' and paid not in ('" + padata + "') ";
                pocreation = obj.Database.SqlQuery<LoadItemsByPAID>(query).ToList();
                return pocreation;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<statuscheckmodel> InsertPOItems(POMasterModel model)
        {
            statuscheckmodel status = new statuscheckmodel();
            try
            {
                POMaster master = new POMaster();
                int sequenceNo = Convert.ToInt32(obj.POMasters.Max(li => li.POID));
                if (sequenceNo == null || sequenceNo == 0)
                    sequenceNo = 1;
                else
                {
                    sequenceNo = sequenceNo + 1;
                }
                var value = obj.SP_sequenceNumber(sequenceNo).FirstOrDefault();
                master.pono = "PO/" + DateTime.Now.ToString("MMyy") + "/" + value;
                master.preparedby = model.preparedby;
                master.prepareddate = System.DateTime.Now;
                master.podate = System.DateTime.Now;
                master.poremarks = model.poremarks;
                master.poterms = model.poterms;
                master.potype = model.potype;
                master.collectiveno = model.collectiveno;
                master.priorvendor = 5000000;
                master.BuyerGroupID = model.BuyerGroupID;
                master.departmentid = model.departmentid;
                master.Reqdeliverydate = model.Reqdeliverydate;
                master.scmpoconfirmation = model.scmpoconfirmation;
                master.potype = model.potype;
                master.POVariant = model.itemtype;
                if (model.insurance == "ByYil")
                    master.insurance = "0.015";
                else
                    master.insurance = "0.00";
                obj.POMasters.Add(master);
                obj.SaveChanges();
                status.Sid = master.POID;
                foreach (var item in model.poitems)
                {
                    SCMModels.SCMModels.POItem items = new SCMModels.SCMModels.POItem();
                    //items.paitemid = item.paitemid;
                    items.poid = status.Sid;
                    items.date = System.DateTime.Now;
                    obj.POItems.Add(items);
                    obj.SaveChanges();
                }
                //padata = string.Join(" ',' ", paid);
                //var query = "select * from LoadItemsByPAID where paid in ('" + padata + "') and PAStatus='Approved' ";
                //pocreation = obj.Database.SqlQuery<LoadItemsByPAID>(query).ToList();

                return status;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DataTable> getscrapRegisterReport(scrapsearchmodel model)
        {
            DataTable table = new DataTable();
            try
            {
                var query = "";
                query = "select distinct VM.VendorName,PreparedDate,SEM.Dateofentry,SEM.TruckNo,md.Department,md.DepartmentId,Qty,UOM,UnitPrice,BasicPrice,TCS,TCSAmount,TotalPrice,TotalBasic,(CGST+SGST+IGST)as GSTper,(CGSTAmount+SGSTAmount+IGSTAmount)as GSTAmt, " +
                        " SI.Itemcode+' '+SI.Description as ItemDescription,SI.Remarks as Remarks2,SI.Scraptype as Remarks1 from ScrapEntryMaster SEM " +
                        " inner join Scrapitems SI on SEM.ScrapentryId=SI.Scrapentryid inner join MPRDepartments md on md.DepartmentId = SEM.RequesterDepartmentID " +
                        " inner join VendorMaster VM on VM.Vendorid=SEM.Vendorid " +
                        "where SEM.ScrapentryId is not null ";

                if (!string.IsNullOrEmpty(model.truckno))
                    query += " and SEM.TruckNo='" + model.truckno + "' ";
                if (!string.IsNullOrEmpty(model.scraptype))
                    query += " and SI.Scraptype='" + model.scraptype + "'";
                if (model.DepartmentId != 0)
                    query += " and md.DepartmentId='" + model.DepartmentId + "' ";
                if (!string.IsNullOrEmpty(model.Datetype))
                {
                    if (model.scrapfrom != null)
                        query += " and " + model.Datetype + ">='" + model.scrapfrom + "'";
                    if (model.scrapto != null)
                        query += " and " + model.Datetype + "<='" + model.scrapto + "'";
                }
                if (!string.IsNullOrEmpty(model.Vendorid))
                    query += " and SEM.Vendorid='" + model.Vendorid + "'";

                var cmd = obj.Database.Connection.CreateCommand();
                cmd.CommandText = query;

                cmd.Connection.Open();
                table.Load(cmd.ExecuteReader());
                cmd.Connection.Close();
                return table;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public async Task<PoLineItemstoExcel> GetpoitemsByPoId(int revisionid)
        //      {
        //          try
        //          {
        //		var query
        //          }
        //          catch (Exception ex)
        //          {

        //              throw;
        //          }
        //      }
    }
}
