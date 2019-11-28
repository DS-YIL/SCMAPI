﻿using DALayer.MPR;
using SCMModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BALayer.MPR
{

	public class MPRBA : IMPRBA
	{
		public readonly IMPRDA _mprDataAcess;
		public MPRBA(IMPRDA MPRDA)
		{
			this._mprDataAcess = MPRDA;
		}
		public DataTable getDBMastersList(DynamicSearchResult Result)
		{
			return this._mprDataAcess.getDBMastersList(Result);

		}
		public bool addDataToDBMasters(DynamicSearchResult Result)
		{
			return this._mprDataAcess.addDataToDBMasters(Result);

		}
		public bool updateDataToDBMasters(DynamicSearchResult Result)
		{
			return this._mprDataAcess.updateDataToDBMasters(Result);

		}
		public MPRRevision updateMPR(MPRRevision mpr)
		{
			return this._mprDataAcess.updateMPR(mpr);

		}
		public DataTable GetListItems(DynamicSearchResult Result)
		{
			return this._mprDataAcess.GetListItems(Result);
		}
		public bool deleteMPRDocument(MPRDocument mprDocument)
		{
			return this._mprDataAcess.deleteMPRDocument(mprDocument);
		}

		public bool deleteMPRItemInfo(MPRItemInfo mprItemInfo)
		{
			return this._mprDataAcess.deleteMPRItemInfo(mprItemInfo);
		}
		public bool deleteMPRVendor(MPRVendorDetail mprVendor)
		{
			return this._mprDataAcess.deleteMPRVendor(mprVendor);
		}

		public bool deleteMPRDocumentation(MPRDocumentation MPRDocumentation)
		{
			return this._mprDataAcess.deleteMPRDocumentation(MPRDocumentation);
		}
		public MPRRevision getMPRRevisionDetails(int RevisionId)
		{
			return this._mprDataAcess.getMPRRevisionDetails(RevisionId);
		}
		public List<MPRRevisionDetail> getMPRList(mprFilterParams mprfilterparams)
		{
			return this._mprDataAcess.getMPRList(mprfilterparams);
		}
		public List<Employee> getEmployeeList()
		{
			return this._mprDataAcess.getEmployeeList();
		}
		public List<MPRRevision> getMPRRevisionList(int RequisitionId)
		{
			return this._mprDataAcess.getMPRRevisionList(RequisitionId);
		}
		public MPRRevision statusUpdate(MPRStatusUpdate mprStatus)
		{
			return this._mprDataAcess.statusUpdate(mprStatus);
		}
		public List<SCMStatu> getStatusList()
		{
			return this._mprDataAcess.getStatusList();
		}
	}
}
