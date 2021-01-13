using DALayer.ASN;
using SCMModels;
using SCMModels.RemoteModel;
using SCMModels.SCMModels;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
namespace BALayer.ASN
{
	public class ASNBA : IASNBA
	{
		public readonly ASNDA _asnDataAcess;
		public ASNBA(ASNDA ASNDA)
		{
			this._asnDataAcess = ASNDA;
		}

		public bool ASNInitiate(ASNInitiation ASNInitiation)
		{
			return _asnDataAcess.ASNInitiate(ASNInitiation);
		}


		public List<ASNShipmentHeader> getAsnList(ASNfilters ASNfilters)
		{
			return _asnDataAcess.getAsnList(ASNfilters);
		}

		public ASNShipmentHeader getAsnDetailsByAsnNo(int ASNId)
		{
			return _asnDataAcess.getAsnDetailsByAsnNo(ASNId);
		}

		public bool updateASNComminications(ASNCommunication asncom)
		{
			return _asnDataAcess.updateASNComminications(asncom);
		}

		public InvoiceDetail GetInvoiceDetails(InvoiceDetail invoicedetails)
		{
			return _asnDataAcess.GetInvoiceDetails(invoicedetails);
		}
	}
}

