﻿using SCMModels;
using SCMModels.SCMModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALayer.MPR
{
	public interface IMPRDA
	{

		MPRRevision updateMPR(MPRRevision mpr);
		DataTable GetListItems(DynamicSearchResult Result);
		bool deleteMPRDocument(MPRDocument mprDocument);
		bool deleteMPRItemInfo(MPRItemInfo mprItemInfo);
		bool deleteMPRVendor(MPRVendorDetail mprVendor);
		bool deleteMPRDocumentation(MPRDocumentation MPRDocumentation);
		MPRRevision getMPRRevisionDetails(int RevisionId);
		List<MPRRevisionDetail> getMPRList(mprFilterParams mprfilterparams);
		List<Employee> getEmployeeList();
		List<MPRRevision> getMPRRevisionList(int RequisitionId);
		MPRRevision statusUpdate(MPRStatusUpdate mprStatus);
		List<SCMStatu> getStatusList();
		
	}
}
