﻿using SCMModels.MPRMasterModels;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SCMModels;
using System.Data.SqlClient;

namespace BALayer.PurchaseAuthorization
{
   public interface IPurchaseAuthorizationBA
    {
        Task<statuscheckmodel> InsertPAAuthorizationLimits(PAAuthorizationLimitModel model);
        Task<PAAuthorizationLimitModel> GetPAAuthorizationLimitById(int deptid);
        Task<statuscheckmodel> CreatePAAuthirizationEmployeeMapping(PAAuthorizationEmployeeMappingModel model);
        Task<PAAuthorizationEmployeeMappingModel> GetMappingEmployee(PAAuthorizationLimitModel limit);
        Task<statuscheckmodel> CreatePACreditDaysmaster(PACreditDaysMasterModel model);
        Task<PACreditDaysMasterModel> GetCreditdaysMasterByID(int creditdaysid);
        Task<statuscheckmodel> AssignCreditdaysToEmployee(PACreditDaysApproverModel model);
        Task<statuscheckmodel> RemovePAAuthorizationLimitsByID(int authid);
        Task<statuscheckmodel> RemovePACreditDaysMaster(int creditid);
        Task<List<PAAuthorizationLimitModel>> GetPAAuthorizationLimitsByDeptId(int departmentid);
        Task<statuscheckmodel> RemovePACreditDaysApprover(EmployeemappingtocreditModel model);
        Task<statuscheckmodel> RemovePurchaseApprover(EmployeemappingtopurchaseModel model);
        Task<PACreditDaysApproverModel> GetPACreditDaysApproverById(int ApprovalId);
        Task<EmployeModel> GetEmployeeMappings(PAConfigurationModel model);
        DataSet GetEmployeeMappings1(PAConfigurationModel model);

        //Task<List<LoadItemsByID>> GetItemsByMasterIDs(PADetailsModel masters);
        List<loadtaxesbyitemwise> GetItemsByMasterIDs(PADetailsModel masters);
        Task<List<DepartmentModel>> GetAllDepartments();
        Task<List<PAAuthorizationLimitModel>> GetSlabsByDepartmentID(int DeptID);
        Task<List<EmployeModel>> GetAllEmployee();
        Task<List<PAAuthorizationLimitModel>> GetAllCredits();
        Task<List<PACreditDaysMasterModel>> GetAllCreditDays();
        Task<List<MPRPAPurchaseModesModel>> GetAllMprPAPurchaseModes();
        Task<List<MPRPAPurchaseTypesModel>> GetAllMprPAPurchaseTypes();
        Task<statuscheckmodel> InsertPurchaseAuthorization(MPRPADetailsModel model);
        Task<statuscheckmodel> UpdatePurchaseAuthorization(MPRPADetailsModel model);
        Task<statuscheckmodel> finalpa(MPRPADetailsModel model);
        Task<MPRPADetailsModel> GetMPRPADeatilsByPAID(int PID);
        Task<List<MPRPADetailsModel>> GetAllMPRPAList();
        Task<List<PAFunctionalRolesModel>> GetAllPAFunctionalRoles();
        Task<List<EmployeemappingtocreditModel>> GetCreditSlabsandemployees();
        Task<List<EmployeemappingtopurchaseModel>> GetPurchaseSlabsandMappedemployees();
        Task<List<ProjectManagerModel>> LoadAllProjectManagers();
        Task<List<VendormasterModel>> LoadVendorByMprDetailsId(List<int?> MPRItemDetailsid);
        Task<List<MPRPAApproversModel>> GetAllApproversList();
        Task<List<mprApproverdetailsview>> GetMprApproverDetailsBySearch(PAApproverDetailsInputModel model);
        Task<statuscheckmodel> UpdateMprpaApproverStatus(MPRPAApproversModel model);
        Task<List<DisplayRfqTermsByRevisionId>> getrfqtermsbyrevisionid(List<int> RevisionId);
        Task<List<Employeemappingtopurchase>> GetPurchaseSlabsandMappedemployeesByDeptId(EmployeeFilterModel model);
        Task<statuscheckmodel> InsertPaitems(List<ItemsViewModel> paitem);
        Task<List<GetMappedSlab>> GetAllMappedSlabs();
        Task<statuscheckmodel> RemoveMappedSlab(PAAuthorizationLimitModel model);
        Task<List<NewGetMprPaDetailsByFilter>> getMprPaDetailsBySearch(PADetailsModel model);
        Task<List<PAReport>> GetPaStatusReports(PAReportInputModel model);
        Task<statuscheckmodel> UpdateApproverforRequest(MPRPAApproversModel model);
        Task<statuscheckmodel> DeletePAByPAid(padeletemodel model);
        Task<List<IncompletedPAlist>> GetIncompletedPAlist(painputmodel model);
        DataTable getrfqtermsbyrevisionsid1(List<int> revisionid);
        Task<statuscheckmodel> DeletePADocument(PADocumentsmodel model);
		MPRPADetailsModel GetTokuchuDetailsByPAID(int? PID, int ?TokuchRequestid);
		int updateTokuchuRequest(TokuchuRequest tokuchuRequest, string typeOfuser, int revisionId);
		List<GetTokuchuDetail> getTokuchuReqList(tokuchuFilterParams tkparameters);
        List<mprstatuspivot> Getmprstatus();
        DataSet GetMprstatuswisereport(string spName, SqlParameter[] paramArr);
        DataSet GetmprstatusReport(string spName, SqlParameter[] paramArr);
        List<RequisitionReport> GetmprRequisitionReport(ReportInputModel input);
        ReportFilterModel GetmprRequisitionfilters();
        List<loadprojectmangersforreport> Loadprojectmanagersforreport();
        DataTable Loadprojectcodewisereport(ReportInputModel model);
        List<ReportbyprojectDuration> LoadprojectDurationwisereport(ReportInputModel model);
        List<jobcodes> Loadjobcodes();
        DataTable GETApprovernamesbydepartmentid(int departmentid);
        List<Saleorderno> Loadsaleorder();
        Task<List<loadpastatsreport>> getPaValueReport(PADetailsModel model);
        Task<List<statuscheckmodel>> InsertMappingItems(List<MappingItemModel> model);
        Task<MSAMasterConfirmation> updateMSAConfirmation(MSAMasterConfirmation model);
        Task<MSAMasterConfirmation> ClearMSAConfirmation(MSAMasterConfirmation model);
        Task<statuscheckmodel> Unmappingitem(MappingItemModel model);
        Task<List<IncoTermMaster>> getincotermmaster();
        Task<statuscheckmodel> updatetokuchubyid(int tokuchuid);
        Task<statuscheckmodel> UpdateMsaprconfirmation(List<ItemsViewModel> msa);
        Task<List<MSAProcessTrack>> getmsaprocesstrackbyId(int paid);
        Task<statuscheckmodel> InsertScrapRregister(ScrapRegisterMasterModel msa);
        Task<DataTable> getscraplist(scrapsearchmodel model);
        Task<ScrapRegisterMasterModel> getscrapitembyid(int scrapid);
        Task<statuscheckmodel> UpdateScrapRequest(ScrapRegisterMasterModel msa);
        Task<statuscheckmodel> UpdateScrapRregister(ScrapRegisterMasterModel msa);
        Task<statuscheckmodel> InsertScarpflowIncharge(ScrapflowModel model);
        Task<List<ScrapflowModel>> Getincharelist();
        Task<ScrapitemRatecontract> Getscrapitemdetails(string itemcode);
        Task<List<ScrapflowModel>> Getincharepermissionlist(scrapsearchmodel search);
        Task<DataTable> getscraplistbysearch(scrapsearchmodel model); 
         Task<List<ScrapflowModel>> getauthorizescrapflowlist(string employeeno);
        DataSet GetCMMMonthlyPerformance1(string spName, SqlParameter[] paramArr);
        DataSet GetCMMMonthlyPerformance2(string spName, SqlParameter[] paramArr);
        Task<List<Loadpaforpocration>> LoadItemsforpocreation(PADetailsModel masters);
        Task<List<LoadItemsforpo>> LoadItemsforpogeneration(List<int> paid);
        Task<List<LoadItemsforpo>> LoadItemsforpogenerationbasedonvendor(string VendorId, List<int> PAId);
        Task<statuscheckmodel> InsertPOItems(POMasterModel model);
        Task<DataTable> getscrapRegisterReport(scrapsearchmodel model);
        Task<List<PoLineItemstoExcel>> GetpoitemsByPoId(int revisionid);
        Task<List<Loadpolist>> LoadPolist(posearchmodel posearch);
        Task<statuscheckmodel> ApprovePRNos(List<ItemsViewModel> msa);
        Task<List<POCreationProcessTrack>> Getpostatus(int poid);
    }
}
