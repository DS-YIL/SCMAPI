using BALayer.ASN;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SCMModels;
using SCMModels.SCMModels;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace SCMAPI.Controllers
{
	[RoutePrefix("Api/ASN")]
	public class ASNController : ApiController
	{
		// private readonly IASNBA _asnBusinessAccess;

		private readonly ASNBA _asnBusinessAccess;
		public ASNController(ASNBA ASNBA)
		{
			this._asnBusinessAccess = ASNBA;
		}

		[Route("ASNInitiate")]
		[HttpPost]
		public IHttpActionResult ASNInitiate([FromBody] ASNInitiation model)
		{
			return Ok(_asnBusinessAccess.ASNInitiate(model));
		}

		[HttpPost]
		[Route("getAsnList")]
		public IHttpActionResult getAsnList(ASNfilters ASNfilters)
		{
			return Ok(_asnBusinessAccess.getAsnList(ASNfilters));
		}

		[HttpGet]
		[Route("getAsnDetails/{ASNId}")]
		public IHttpActionResult getAsnDetailsByAsnNo(int ASNId)
		{
			return Ok(_asnBusinessAccess.getAsnDetailsByAsnNo(ASNId));
		}

		[HttpPost]
		[Route("GetInvoiceDetails")]
		public IHttpActionResult GetInvoiceDetails(InvoiceDetail invoiceDetails)
		{
			return Ok(_asnBusinessAccess.GetInvoiceDetails(invoiceDetails));
		}

		[Route("updateASNComminications")]
		[HttpPost]
		public IHttpActionResult updateASNComminications([FromBody] ASNCommunication model)
		{
			return Ok(_asnBusinessAccess.updateASNComminications(model));
		}

		[HttpPost]
		[Route("MergeInvoiceDocs")]
		public HttpResponseMessage MergeInvoiceDocs(InvoiceDetail invoiceDetails)
		{
			InvoiceDetail invoiceData = _asnBusinessAccess.GetInvoiceDetails(invoiceDetails);
			//Create HTTP Response.
			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
			if (invoiceData != null)
			{
				List<InvoiceDocument> invoiceDocs = new List<InvoiceDocument>();
				foreach (InvoiceDocument item in invoiceData.InvoiceDocuments)
				{
					if (item.DocumentTypeId != 6 && item.DocumentTypeId != 7)
						invoiceDocs.Add(item);
				}
				string[] fileArray = new string[invoiceDocs.Count];
				int count = 0;
				foreach (InvoiceDocument item in invoiceDocs)
				{
					//var path = item.Path.Replace(/\\/ g, "/");
					fileArray[count] = HttpContext.Current.Server.MapPath("~/SCMDocs") + "\\" + item.Path;
					//fileArray[count] = HttpContext.Current.Server.MapPath("~/SCMDocs") + "\\" + "VSCMUI.pdf";
					count++;
				}
				PdfReader reader = null;
				Document sourceDocument = null;
				PdfCopy pdfCopyProvider = null;
				PdfImportedPage importedPage;
				string outputPdfPath = HttpContext.Current.Server.MapPath("~/SCMDocs") + "\\" + "InvoiceMerge_" + invoiceDetails.ASNId + ".pdf";

				sourceDocument = new Document();
				pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

				//output file Open  
				sourceDocument.Open();


				//files list wise Loop  
				for (int f = 0; f < fileArray.Length; f++)
				{
					int pages = TotalPageCount(fileArray[f]);

					reader = new PdfReader(fileArray[f]);
					//Add pages in new file  
					for (int i = 1; i <= pages; i++)
					{
						importedPage = pdfCopyProvider.GetImportedPage(reader, i);
						pdfCopyProvider.AddPage(importedPage);
					}

					reader.Close();
				}
				//save the output file  
				sourceDocument.Close();


				//Set the File Path.
				string filePath = outputPdfPath;

				//Check whether File exists.
				if (!File.Exists(filePath))
				{
					//Throw 404 (Not Found) exception if File not found.
					response.StatusCode = HttpStatusCode.NotFound;
					response.ReasonPhrase = string.Format("File not found: {0} .", outputPdfPath);
					throw new HttpResponseException(response);
				}

				//Read the File into a Byte Array.
				byte[] bytes = File.ReadAllBytes(filePath);

				//Set the Response Content.
				response.Content = new ByteArrayContent(bytes);

				//Set the Response Content Length.
				response.Content.Headers.ContentLength = bytes.LongLength;

				//Set the Content Disposition Header Value and FileName.
				response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
				response.Content.Headers.ContentDisposition.FileName = outputPdfPath;

				//Set the File Content Type.
				response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(outputPdfPath));
				File.Delete(outputPdfPath);
			}
			return response;
		}



		private static int TotalPageCount(string file)
		{
			using (StreamReader sr = new StreamReader(System.IO.File.OpenRead(file)))
			{
				Regex regex = new Regex(@"/Type\s*/Page[^s]");
				MatchCollection matches = regex.Matches(sr.ReadToEnd());

				return matches.Count;
			}
		}


	}
}
