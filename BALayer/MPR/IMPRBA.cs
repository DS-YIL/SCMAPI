﻿using SCMModels;
using SCMModels.RemoteModel;
using SCMModels.RFQModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALayer.MPR
{
	public interface IMPRBA
	{
		DataTable getDBMastersList(DynamicSearchResult Result);
		bool addDataToDBMasters(DynamicSearchResult Result);
		bool updateDataToDBMasters(DynamicSearchResult Result);
		MPRRevision updateMPR(MPRRevision mpr);
		MPRRevision copyMprRevision(MPRRevision mpr, bool repeatOrder, bool revise);

		int addNewVendor(VendormasterModel vendor);
		DataTable GetListItems(DynamicSearchResult Result);
		bool deleteMPRDocument(MPRDocument mprDocument);
		bool deleteMPRItemInfo(MPRItemInfo mprItemInfo);
		string addMprItemInfo(MPRItemInfo mPRItemInfo);
		bool deleteMPRVendor(MPRVendorDetail mprVendor);
		bool deleteMPRDocumentation(MPRDocumentation MPRDocumentation);
		MPRRevision getMPRRevisionDetails(int RevisionId);
		DataTable getMPRList(mprFilterParams mprfilterparams);
		DataTable getSavingsReport(mprFilterParams mprfilterparams);
		int getMPRPendingListCnt(string preparedBy);
		List<Employee> getEmployeeList();
		List<MPRRevisionDetails_woItems> getMPRRevisionList(int RequisitionId);
		MPRRevision statusUpdate(MPRStatusUpdate mprStatus);
		List<SCMStatu> getStatusList();
		List<UserPermission> getAccessList(int RoleId);
		bool updateMPRVendor(List<MPRVendorDetail> MPRVendorDetails, int RevisionId);
		bool deleteMPR(DeleteMpr deleteMprInfo);
		string updateItemId(materialUpdate mPRItemInfo);
		List<loadloction> Loadstoragelocationsbydepartment();
		SaleorderDetail LoadJobCodesbysaleorder(string saleorder);

		VendorRegApprovalProcess updateVendorRegProcess(VendorRegApprovalProcessData model, string typeOfuser);
		List<VendorRegProcessView> getVendorReqList(vendorRegfilters getVendorReqList);
		List<RemoteStateMaster> StateNameList();
		List<VendorRegisterDocumenetMaster> DocumentMasterList();
		List<NatureOfBusinessMaster> natureOfBusinessesList();
		VendorRegistrationModel GetVendorDetails(int vendorId);
		bool SaveVendorDetails(VendorRegistrationModel model);
		bool DeletefileAttached(VendorRegisterDocumentDetail model);

		List<YILTermsGroup> GetYILTermGroups();
		bool UpdateYILTermsGroup(YILTermsGroup yilTermGroups);
		bool UpdateYILTermsAndConditions(YILTermsandCondition yilTermandconditions);
		bool DeleteTermGroup(int TermGroupId, string DeletedBy);
		bool DeleteTermsAndConditions(int TermId, string DeletedBy);
        DataTable Getoldrevisionitems(List<int> itemdetailsid);
		DataTable GetTokuchuinformation(int mprrevisionid);
		List<SCMModels.Models.Vendor> VendorList();

		SCMModels.Models.Vendor GetVendorInfo(int vendorid);

		List<VendorUserMaster> GetVendorUserInfo(int vendorId);

		bool DeactivateVendor(VendorUserMaster vendorUserMaster);

		bool AddVendorUser(VendorUserMaster vendorUserMaster);

		bool DeteteRfqItems(int rfqrevisionId, int mprrevisionid);

		List<SCMModels.Models.RFQ> CheckMprRevision(int mprrevisionId);
	}

}
