using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using ExcelObj = Microsoft.Office.Interop.Excel;
//using Microsoft.AspNetCore.Hosting;

namespace SCMAPI.Common
{
    public class utilities
    {
        
        public bool ExportExcel(DataSet exportDs, out string DestinationPath)
        {
            ExcelObj.Application excel = null;
            ExcelObj.Workbook workbook = null;
            ExcelObj.Worksheet worksheet = null;

            ExcelObj.Range DocNo = null;
            ExcelObj.Range DocDesc = null;
            ExcelObj.Range NameClient = null;
            ExcelObj.Range Department = null;
            ExcelObj.Range JobName = null;
            ExcelObj.Range ProjectManager = null;
            ExcelObj.Range SONO = null;
            ExcelObj.Range BuyerGrp = null;
            ExcelObj.Range JOBCode = null;
            ExcelObj.Range TargetSpend = null;
            ExcelObj.Range PurchaseType = null;

            ExcelObj.Range SrNo = null;

            ExcelObj.Range RowMSCODE = null;
            ExcelObj.Range RowName = null;
            ExcelObj.Range RowDescription = null;
            ExcelObj.Range RowQty = null;

            ExcelObj.Range PONO = null;
            ExcelObj.Range PODATE = null;
            ExcelObj.Range POUP = null;
            ExcelObj.Range POTP = null;

            ExcelObj.Range VendorName = null;
            ExcelObj.Range MPRQTY = null;
            ExcelObj.Range Price = null;
            ExcelObj.Range DiscountPer = null;
            ExcelObj.Range HAPer = null;
            ExcelObj.Range IFRAPer = null;
            ExcelObj.Range INSAPer = null;
            ExcelObj.Range DAPer = null;
            ExcelObj.Range FRAPer = null;
            ExcelObj.Range PFAPer = null;
            ExcelObj.Range RfqDocStatus = null;
            ExcelObj.Range ADP = null;
            ExcelObj.Range DP = null;
            ExcelObj.Range MT = null;
            ExcelObj.Range HC = null;
            ExcelObj.Range TP = null;


            ExcelObj.Range Total = null;
            ExcelObj.Range TotalPOPrice = null;
            ExcelObj.Range TotalMatarialCost = null;
            ExcelObj.Range TotalHandlingCharge = null;
            ExcelObj.Range TotalLandedCost = null;

            DataTable DtCompare = exportDs.Tables["CompareTable"];

            string sourcePath = ConfigurationManager.AppSettings["AttachedDocPath"], fileName = "";
            string targetPath = ConfigurationManager.AppSettings["DownloadVexcel"];//, filePath="";
            string sourceFile = "";
            
            List<VenderModel> venderModelsList = new List<VenderModel>();
            try
            {
                //sourcePath = Server.MapPath("~") + "\\WarrantyTemplate\\";
                //sourcePath = sourcePath.Replace("\\\\", "\\");
                //System.IO.File.Copy("", "", true);

                //string webRootPath = .WebRootPath;
                //string contentRootPath = _hostingEnvironment.ContentRootPath;

                targetPath = targetPath + "\\RFQ comparison";
                sourceFile = sourcePath+"\\RFQComparision.xlsx";
                fileName = targetPath+"\\"+ DtCompare.Rows[0]["DocumentNo"].ToString().Replace("/","_")+"_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx";
                //sourceFile = System.IO.Path.Combine(sourcePath, "\\RFQComparision.xlsx");
                //fileName = System.IO.Path.Combine(targetPath, "\\RFQComparision_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xlsx");
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                }

                System.IO.File.Copy(sourceFile, fileName, true);

                excel = new ExcelObj.Application();

                workbook = excel.Workbooks.Open(fileName, ReadOnly: false, Editable: true);
                worksheet = workbook.Worksheets.Item[1] as ExcelObj.Worksheet;

                if (worksheet == null)
                {
                    DestinationPath = "";
                    return false;
                }

                DocNo = worksheet.Rows.Cells[6, 3];
                DocDesc = worksheet.Rows.Cells[7, 3];
                BuyerGrp = worksheet.Rows.Cells[8, 3];
                Department = worksheet.Rows.Cells[6, 5];
                JobName = worksheet.Rows.Cells[7, 5];
                ProjectManager = worksheet.Rows.Cells[8, 5];
                SONO = worksheet.Rows.Cells[6, 7];
                NameClient = worksheet.Rows.Cells[7, 7];
                JOBCode = worksheet.Rows.Cells[8, 7];
                TargetSpend = worksheet.Rows.Cells[6, 9];
                PurchaseType = worksheet.Rows.Cells[7, 9];

                DocNo.Value = DtCompare.Rows[0]["DocumentNo"];
                DocDesc.Value = DtCompare.Rows[0]["DocumentDescription"];
                NameClient.Value = DtCompare.Rows[0]["ClientName"];
                Department.Value = DtCompare.Rows[0]["DepartmentName"];
                JobName.Value = DtCompare.Rows[0]["JobName"];
                ProjectManager.Value = DtCompare.Rows[0]["ProjectManagerName"];
                SONO.Value = DtCompare.Rows[0]["SaleOrderNo"];
                BuyerGrp.Value = DtCompare.Rows[0]["BuyerGroupName"];
                JOBCode.Value = DtCompare.Rows[0]["JobCode"];
                TargetSpend.Value = DtCompare.Rows[0]["TargetSpend"];
                PurchaseType.Value = DtCompare.Rows[0]["PurchaseType"];


                List<int> ids = new List<int>(DtCompare.Rows.Count);
                foreach (DataRow row in DtCompare.Rows)
                    ids.Add((int)row["Itemdetailsid"]);
                IEnumerable<int> uniqueItems = ids.Distinct<int>();
                

                int excelItemIndex = 14;
                float PoPriceCalculate = 0;
                int srNo = 0;
                foreach (var item in uniqueItems)
                {
                    srNo++;
                    string itddd = item.ToString();
                    DataView Dv = new DataView(DtCompare);
                    Dv.RowFilter = "Itemdetailsid=" + itddd;

                    List<DataTable> listItem = new List<DataTable>();
                    listItem.Add(Dv.ToTable());

                    SrNo = worksheet.Rows.Cells[excelItemIndex, 1];
                    RowMSCODE = worksheet.Rows.Cells[excelItemIndex, 2];
                    RowName = worksheet.Rows.Cells[excelItemIndex + 1, 2];
                    RowDescription = worksheet.Rows.Cells[excelItemIndex + 2, 2];
                    RowQty = worksheet.Rows.Cells[excelItemIndex + 3, 2];
                    
                    SrNo.Value = srNo;
                    RowMSCODE.Value = "MS Code";
                    RowName.Value = "Name";
                    RowDescription.Value = "Description";
                    RowQty.Value = "Qty";

                    RowMSCODE = worksheet.Rows.Cells[excelItemIndex, 3];
                    RowName = worksheet.Rows.Cells[excelItemIndex + 1, 3];
                    RowDescription = worksheet.Rows.Cells[excelItemIndex + 2, 3];
                    RowQty = worksheet.Rows.Cells[excelItemIndex + 3, 3];

                    RowMSCODE.Value = Dv[0]["ItemId"];
                    RowName.Value = Dv[0]["ItemName"];
                    RowDescription.Value = Dv[0]["ItemDescription"];
                    RowQty.Value = Dv[0]["MprQuantity"];

                    // Previous Price
                    PONO = worksheet.Rows.Cells[excelItemIndex, 4];
                    PODATE = worksheet.Rows.Cells[excelItemIndex + 1, 4];
                    POUP = worksheet.Rows.Cells[excelItemIndex + 2, 4];
                    POTP = worksheet.Rows.Cells[excelItemIndex + 3, 4];

                    PONO.Value = "PO NO";
                    PODATE.Value = "PO DATE";
                    POUP.Value = "PO UP";
                    POTP.Value = "PO TP";

                    PONO = worksheet.Rows.Cells[excelItemIndex, 5];
                    PODATE = worksheet.Rows.Cells[excelItemIndex + 1, 5];
                    POUP = worksheet.Rows.Cells[excelItemIndex + 2, 5];
                    POTP = worksheet.Rows.Cells[excelItemIndex + 3, 5];

                    PONO.Value = Dv[0]["PONumber"];
                    PODATE.Value = Dv[0]["PODate"];
                    POUP.Value = Dv[0]["POUnitPrice"];
                    POTP.Value = Dv[0]["POPrice"];
                    if (!string.IsNullOrEmpty(Dv[0]["POPrice"].ToString()))
                        PoPriceCalculate = PoPriceCalculate + float.Parse(Dv[0]["POPrice"].ToString());

                    int vendorExcelIndex = 6;

                    Dv.Sort = "VendorCode";
                    foreach (DataRow vendorList in Dv.ToTable().Rows)
                    {
                        VendorName = worksheet.Rows.Cells[10, vendorExcelIndex];
                        VendorName.Value = vendorList["VendorName"]+ "\nRFQ Ref No :" + vendorList["RFQNo"];

                  

                        MPRQTY = worksheet.Rows.Cells[excelItemIndex, vendorExcelIndex];
                        Price = worksheet.Rows.Cells[excelItemIndex + 1, vendorExcelIndex];
                        DiscountPer = worksheet.Rows.Cells[excelItemIndex + 2, vendorExcelIndex];
                        HAPer = worksheet.Rows.Cells[excelItemIndex + 3, vendorExcelIndex];
                        IFRAPer = worksheet.Rows.Cells[excelItemIndex + 4, vendorExcelIndex];
                        INSAPer = worksheet.Rows.Cells[excelItemIndex + 5, vendorExcelIndex];
                        DAPer = worksheet.Rows.Cells[excelItemIndex + 6, vendorExcelIndex];
                        FRAPer = worksheet.Rows.Cells[excelItemIndex + 7, vendorExcelIndex];
                        PFAPer = worksheet.Rows.Cells[excelItemIndex + 8, vendorExcelIndex];
                        RfqDocStatus = worksheet.Rows.Cells[excelItemIndex + 9, vendorExcelIndex];
                        ADP = worksheet.Rows.Cells[excelItemIndex + 10, vendorExcelIndex];
                        DP = worksheet.Rows.Cells[excelItemIndex + 11, vendorExcelIndex];
                        MT = worksheet.Rows.Cells[excelItemIndex + 12, vendorExcelIndex];
                        HC = worksheet.Rows.Cells[excelItemIndex + 13, vendorExcelIndex];
                        TP = worksheet.Rows.Cells[excelItemIndex + 14, vendorExcelIndex];

                        setBorder(MPRQTY);
                        setBorder(Price);
                        setBorder(DiscountPer);
                        setBorder(HAPer);
                        setBorder(IFRAPer);
                        setBorder(INSAPer);
                        setBorder(DAPer);
                        setBorder(FRAPer);
                        setBorder(PFAPer);
                        setBorder(RfqDocStatus);
                        setBorder(ADP);
                        setBorder(DP);
                        setBorder(MT);
                        setBorder(HC);
                        setBorder(TP);

                        MPRQTY.Value = "MPR Qty";
                        Price.Value = "Price";
                        DiscountPer.Value = "Discount(" + vendorList["DiscountPercentage"] + "%)";
                        HAPer.Value = "HA(" + vendorList["HandlingPercentage"] + "%)";
                        IFRAPer.Value = "IFRA(" + vendorList["ImportFreightPercentage"] + "%)";
                        INSAPer.Value = "INSA(" + vendorList["InsurancePercentage"] + "%)";
                        DAPer.Value = "DA(" + vendorList["DutyPercentage"] + "%)";
                        FRAPer.Value = "FRA(" + vendorList["FreightPercentage"] + "%)";
                        PFAPer.Value = "PFA(" + vendorList["PFPercentage"] + "%)";
                        RfqDocStatus.Value = "DOC STATUS";
                        ADP.Value = "ADP";
                        DP.Value = "DP";
                        MT.Value = "MT";
                        HC.Value = "HC";
                        TP.Value = "TP";

                        MPRQTY = worksheet.Rows.Cells[excelItemIndex, vendorExcelIndex + 1];
                        Price = worksheet.Rows.Cells[excelItemIndex + 1, vendorExcelIndex + 1];
                        DiscountPer = worksheet.Rows.Cells[excelItemIndex + 2, vendorExcelIndex + 1];
                        HAPer = worksheet.Rows.Cells[excelItemIndex + 3, vendorExcelIndex + 1];
                        IFRAPer = worksheet.Rows.Cells[excelItemIndex + 4, vendorExcelIndex + 1];
                        INSAPer = worksheet.Rows.Cells[excelItemIndex + 5, vendorExcelIndex + 1];
                        DAPer = worksheet.Rows.Cells[excelItemIndex + 6, vendorExcelIndex + 1];
                        FRAPer = worksheet.Rows.Cells[excelItemIndex + 7, vendorExcelIndex + 1];
                        PFAPer = worksheet.Rows.Cells[excelItemIndex + 8, vendorExcelIndex + 1];
                        RfqDocStatus = worksheet.Rows.Cells[excelItemIndex + 9, vendorExcelIndex + 1];
                        ADP = worksheet.Rows.Cells[excelItemIndex + 10, vendorExcelIndex + 1];
                        DP = worksheet.Rows.Cells[excelItemIndex + 11, vendorExcelIndex + 1];
                        MT = worksheet.Rows.Cells[excelItemIndex + 12, vendorExcelIndex + 1];
                        HC = worksheet.Rows.Cells[excelItemIndex + 13, vendorExcelIndex + 1];
                        TP = worksheet.Rows.Cells[excelItemIndex + 14, vendorExcelIndex + 1];

                        setBorder(MPRQTY);
                        setBorder(Price);
                        setBorder(DiscountPer);
                        setBorder(HAPer);
                        setBorder(IFRAPer);
                        setBorder(INSAPer);
                        setBorder(DAPer);
                        setBorder(FRAPer);
                        setBorder(PFAPer);
                        setBorder(RfqDocStatus);
                        setBorder(ADP);
                        setBorder(DP);
                        setBorder(MT);
                        setBorder(HC);
                        setBorder(TP);

                        float TPcalculated =  calculateTPCalculated(vendorList);
                        float HACalcutated = calculateHAPer(vendorList, TPcalculated);
                        float IFRACalculated = calculateIFRAPer(vendorList, HACalcutated, TPcalculated);
                        float INSACalculated = calculateINSAPer(vendorList, HACalcutated, IFRACalculated, TPcalculated);
                        float DACalculated = calculateDAPer(vendorList, HACalcutated, IFRACalculated, INSACalculated, TPcalculated);
                        float FRAamtCalculated = calculateFRAAmt(vendorList, TPcalculated);
                        float PFAamtCalculated = calculatePFAAmt(vendorList, TPcalculated);
                        float MTpriceCalculated = calculateMTPrice(vendorList, FRAamtCalculated, PFAamtCalculated, TPcalculated);
                        float HCamtCalculated = HACalcutated + IFRACalculated + INSACalculated + DACalculated;
                        float TotalamtCalculated = TPcalculated+FRAamtCalculated + PFAamtCalculated + HACalcutated + IFRACalculated + INSACalculated + DACalculated;


                        MPRQTY.Value = vendorList["MprQuantity"];
                        Price.Value = vendorList["UnitPrice"];
                        DiscountPer.Value =convertTo2digit(calculateDiscountPer(vendorList));
                        HAPer.Value =convertTo2digit(HACalcutated);
                        IFRAPer.Value = convertTo2digit(IFRACalculated);
                        INSAPer.Value = convertTo2digit(INSACalculated);
                        DAPer.Value = convertTo2digit(DACalculated);
                        FRAPer.Value = convertTo2digit(FRAamtCalculated);
                        PFAPer.Value = convertTo2digit(PFAamtCalculated);
                        RfqDocStatus.Value = vendorList["RfqDocStatus"];
                        ADP.Value = vendorList["Status"];
                        DP.Value = vendorList["DeliveryMinWeeks"] + "-" + vendorList["DeliveryMaxWeeks"];
                        MT.Value = convertTo2digit(MTpriceCalculated);
                        HC.Value = convertTo2digit(HCamtCalculated);
                        TP.Value = convertTo2digit(TotalamtCalculated);

                        //totalVenderCal.Vendorid = vendorList["VendorCode"].ToString();
                        //totalVenderCal.TotalAMt = TotalamtCalculated;

                        venderModelsList.Add(new VenderModel { Vendorid = vendorList["VendorCode"].ToString(), MaterialTotal = MTpriceCalculated, HandlingCharges= HCamtCalculated, TotalLandedCost= TotalamtCalculated });

                        vendorExcelIndex = vendorExcelIndex + 2;
                    }
                    //totalVenderCal.Vender = venderModelsList;
                    //totalVenderCalList.Add(totalVenderCal);

                    excelItemIndex = excelItemIndex + 18;
                }

                var VendorGroupedByCode = venderModelsList.GroupBy(Vendor => Vendor.Vendorid).
                    Select(g=>new { 
                    key=g.Key,
                        MaterialTotal = g.Sum(s=>s.MaterialTotal),
                        HandlingCharges = g.Sum(s => s.HandlingCharges),
                        TotalLandedCost = g.Sum(s => s.TotalLandedCost)
                    });

                int venindex = 6;
                foreach (var item in VendorGroupedByCode)
                {

                    TotalMatarialCost = worksheet.Rows.Cells[excelItemIndex, venindex];
                    TotalHandlingCharge = worksheet.Rows.Cells[excelItemIndex+1, venindex];
                    TotalLandedCost = worksheet.Rows.Cells[excelItemIndex + 2, venindex];

                    setBorder(TotalMatarialCost);
                    setBorder(TotalHandlingCharge);
                    setBorder(TotalLandedCost);
                    
                    TotalMatarialCost.Value = "Material Total";
                    TotalHandlingCharge.Value = "Handling Charges";
                    TotalLandedCost.Value = "Total Landed Cost";

                    TotalMatarialCost = worksheet.Rows.Cells[excelItemIndex, venindex+1];
                    TotalHandlingCharge = worksheet.Rows.Cells[excelItemIndex+1, venindex + 1];
                    TotalLandedCost = worksheet.Rows.Cells[excelItemIndex+2, venindex + 1];

                    setBorder(TotalMatarialCost);
                    setBorder(TotalHandlingCharge);
                    setBorder(TotalLandedCost);

                    TotalMatarialCost.Value = convertTo2digit(item.MaterialTotal);
                    TotalHandlingCharge.Value = convertTo2digit(item.HandlingCharges);
                    TotalLandedCost.Value = convertTo2digit(item.TotalLandedCost);

                    venindex = venindex + 2;
                }    

                Total = worksheet.Rows.Cells[excelItemIndex, 2];
                TotalPOPrice = worksheet.Rows.Cells[excelItemIndex, 4];
                Total.Value = "TOTAL";
                TotalPOPrice.Value = "PO PRICE";
                TotalPOPrice = worksheet.Rows.Cells[excelItemIndex, 5];
                TotalPOPrice.Value = PoPriceCalculate;


                excel.Application.ActiveWorkbook.Save();
                excel.Application.Quit();
                excel.Quit();
                DestinationPath = fileName;
                return true;
            }
            catch (Exception Ex)
            {
                DestinationPath = "";
                return false;

            }

        }

        private float calculateDiscountPer(DataRow dataRow)
        {
            float result = 0;
            try
            {
                if (!string.IsNullOrEmpty(dataRow["DiscountPercentage"].ToString()) && !string.IsNullOrEmpty(dataRow["UnitPrice"].ToString()) && !string.IsNullOrEmpty(dataRow["vendorQuoteQty"].ToString()))
                    result = (float.Parse(dataRow["DiscountPercentage"].ToString()) / 100) * (float.Parse(dataRow["UnitPrice"].ToString()) * (float.Parse(dataRow["vendorQuoteQty"].ToString())));
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private float calculateTPCalculated(DataRow dataRow)
        {
            float result = 0;
            try
            {
                if (!string.IsNullOrEmpty(dataRow["UnitPrice"].ToString()) && !string.IsNullOrEmpty(dataRow["vendorQuoteQty"].ToString()))
                {
                    result = (float.Parse(dataRow["UnitPrice"].ToString()) * (float.Parse(dataRow["vendorQuoteQty"].ToString())));

                }
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private float calculateHAPer(DataRow dataRow, float TPCalculated)
        {
            float result = 0;

            try
            {
                if (!string.IsNullOrEmpty(dataRow["HandlingPercentage"].ToString()))
                {
                    result = TPCalculated * (float.Parse(dataRow["HandlingPercentage"].ToString()) / 100);
                }
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private float calculateIFRAPer(DataRow dataRow, float HAcalculated, float TPCalculated)
        {
            float result = 0;
            try
            {
                if (!string.IsNullOrEmpty(dataRow["ImportFreightPercentage"].ToString()))
                {
                    result = (TPCalculated + HAcalculated) * (float.Parse(dataRow["ImportFreightPercentage"].ToString()) / 100);
                }
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private float calculateINSAPer(DataRow dataRow, float HACalulated, float IFRACalculated, float TPCalculated)
        {
            float result = 0;
            try
            {
                if (!string.IsNullOrEmpty(dataRow["InsurancePercentage"].ToString()))
                    result = ((TPCalculated + HACalulated + IFRACalculated) * float.Parse(dataRow["InsurancePercentage"].ToString()) / 100);
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private float calculateDAPer(DataRow dataRow, float HACalulated, float IFRACalculated, float INSACalculated, float TPCalculated)
        {
            float result = 0;
            try
            {
                if (!string.IsNullOrEmpty(dataRow["DutyPercentage"].ToString()))
                    result = ((TPCalculated + HACalulated + IFRACalculated + INSACalculated )* float.Parse(dataRow["DutyPercentage"].ToString()) / 100);
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private float calculateFRAAmt(DataRow dataRow, float TPCalculated)
        {
            float result = 0;
            try
            {
                if (!string.IsNullOrEmpty(dataRow["FreightPercentage"].ToString()))
                    result = (TPCalculated * float.Parse(dataRow["FreightPercentage"].ToString()) / 100);
                else
                    result = !string.IsNullOrEmpty(dataRow["FreightAmount"].ToString()) ? float.Parse(dataRow["FreightAmount"].ToString()) : 0;
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private float calculatePFAAmt(DataRow dataRow, float TPCalculated)
        {
            float result = 0;
            try
            {
                if (!string.IsNullOrEmpty(dataRow["PFPercentage"].ToString()))
                    result = (TPCalculated * float.Parse(dataRow["PFPercentage"].ToString()) / 100);
                else
                    result = !string.IsNullOrEmpty(dataRow["PFAmount"].ToString()) ? float.Parse(dataRow["PFAmount"].ToString()) : 0;

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private float calculateMTPrice(DataRow dataRow, float FRAamtcalculated, float PFAamtCalculated, float TPCalculated)
        {
            float result = 0;
            try
            {
                result = TPCalculated + FRAamtcalculated + PFAamtCalculated;
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private void setBorder(ExcelObj.Range cellName)
        {
            cellName.Cells.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDouble;
            cellName.Cells.Borders.Weight = 2d;
        }
        private double convertTo2digit(float f)
        {
            return Math.Round((Double)f, 2);
            //(double)Math.Round(f * 100f) / 100f;
        }



    }
    public class VenderModel
    {
        public string Vendorid { get; set; }
        public float MaterialTotal { get; set; }
        public float HandlingCharges { get; set; }
        public float TotalLandedCost { get; set; }
    }
}