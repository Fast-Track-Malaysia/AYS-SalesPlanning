using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace FT_ADDON.AYS
{
    class Sysform_SalesInvoice
    {
        public static void processRightClickEventbefore(SAPbouiCOM.Form oForm, ref SAPbouiCOM.ContextMenuInfo pVal, ref bool BubbleEvent)
        {
            try
            {
            }
            catch (Exception ex)
            {
                FT_ADDON.SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");
            }
        }
        public static void processRightClickEventafter(SAPbouiCOM.Form oForm, ref SAPbouiCOM.ContextMenuInfo pVal)
        {
        }
        public static void processItemEventbefore(SAPbouiCOM.Form oForm, ref SAPbouiCOM.ItemEvent pVal, ref bool BubbleEvent)
        {
            try
            {

                if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD)
                {
                    SAPbouiCOM.Item oItem = null;
                    SAPbouiCOM.EditText oEdit = null;
                    oItem = oForm.Items.Add("U_ADDONIND", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                    oItem.Left = oForm.Width + 100;
                    oEdit = (SAPbouiCOM.EditText)oItem.Specific;
                    oEdit.DataBind.SetBound(true, "OINV", "U_ADDONIND");

                    //SAPbouiCOM.UserDataSource uds = oForm.DataSources.UserDataSources.Add("cfluid", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 50);
                    //uds.Value = "";
                }
                //if (oForm.DataSources.UserDataSources.Count > 0)
                //{
                //    if (oForm.DataSources.UserDataSources.Item("cfluid").Value.ToString() != "")
                //    {
                //        string FUID = oForm.DataSources.UserDataSources.Item("cfluid").Value.ToString();
                //        SAPbouiCOM.Form oSForm = SAP.SBOApplication.Forms.Item(FUID);
                //        oSForm.Select();
                //        BubbleEvent = false;
                //    }
                //}
                //if (((SAPbouiCOM.EditText)oForm.Items.Item("FUID").Specific).Value != "")
                //{
                //    string FUID = ((SAPbouiCOM.EditText)oForm.Items.Item("FUID").Specific).Value.ToString();
                //    SAPbouiCOM.Form oSForm = FT_ADDON.SAP.SBOApplication.Forms.Item(FUID);
                //    oSForm.Select();
                //    BubbleEvent = false;
                //}
                switch (pVal.EventType)
                {
                    case SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK:
                        //if (pVal.ItemUID == "38")
                        //{
                        //    if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                        //    {
                        //        if (pVal.Row >= 0 && pVal.ColUID == "256")
                        //        {
                        //            int docentry = int.Parse(oForm.DataSources.DBDataSources.Item("INV1").GetValue("DOCENTRY", pVal.Row - 1).ToString());
                        //            int linenum = int.Parse(oForm.DataSources.DBDataSources.Item("INV1").GetValue("LINENUM", pVal.Row - 1).ToString());

                        //            InitForm.TEXT(oForm.UniqueID, docentry, linenum, pVal.Row, "INV1", "38", oForm.DataSources.DBDataSources.Item("INV1").GetValue("TEXT", pVal.Row - 1).ToString());
                        //            BubbleEvent = false;
                        //        }
                        //    }
                        //}
                        break;
                    case SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED:
                        if (pVal.ItemUID == "1")
                        {
                            if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                            {
                                SAPbouiCOM.EditText oEditText = (SAPbouiCOM.EditText)oForm.Items.Item("U_ADDONIND").Specific;
                                oEditText.String = "I";
                            }
                            
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                FT_ADDON.SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");
                BubbleEvent = false;
            }
        }
        public static void processItemEventafter(SAPbouiCOM.Form oForm, ref SAPbouiCOM.ItemEvent pVal)
        {
            try
            {
                switch (pVal.EventType)
                {
                    case SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK:
                        //if (pVal.ItemUID == "38")
                        //{
                        //    if (oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                        //    {
                        //        if (pVal.Row >= 0 && pVal.ColUID != "256")
                        //        {
                        //            int docentry = int.Parse(oForm.DataSources.DBDataSources.Item("INV1").GetValue("DOCENTRY", pVal.Row - 1).ToString());
                        //            int linenum = int.Parse(oForm.DataSources.DBDataSources.Item("INV1").GetValue("LINENUM", pVal.Row - 1).ToString());
                        //            //if (pVal.ColUID == "U_BookNo")
                        //            //{
                        //            //    InitForm.CONM(oForm.UniqueID, docentry, linenum, pVal.Row, "FT_APDOC", "U_CONNO,U_BookNo");
                        //            //}
                        //            //else if (pVal.ColUID == "U_ProdType")
                        //            //{
                        //            //    InitForm.DOPTM(oForm.UniqueID, docentry, linenum, pVal.Row, "FT_APDOPT", "");
                        //            //}
                        //            //else
                        //            //{
                        //                InitForm.SDM(oForm.UniqueID, docentry, linenum, pVal.Row, "INV1", "38");
                        //            //}
                        //        }
                        //    }
                        //}
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                FT_ADDON.SAP.SBOApplication.MessageBox(ex.Message, 1, "Ok", "", "");
            }
        }
        public static void processMenuEventbefore(SAPbouiCOM.Form oForm, ref SAPbouiCOM.MenuEvent pVal, ref bool BubbleEvent)
        {
            return;            
        }
        public static void processMenuEventafter(SAPbouiCOM.Form oForm, ref SAPbouiCOM.MenuEvent pVal)
        {
            return;
        }
        public static void processDataEventafter(SAPbouiCOM.Form oForm, ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo)
        {
            JEHeader jehdr = new JEHeader();
            JEDetails jedtl;
            List<JEDetails> JEdtls = new List<JEDetails>();
            try
            {
                switch (BusinessObjectInfo.EventType)
                {
                    case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD:
                        if (!BusinessObjectInfo.ActionSuccess) return;

                        SAPbobsCOM.Recordset rs = (SAPbobsCOM.Recordset)SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        SAPbobsCOM.Recordset rs1 = (SAPbobsCOM.Recordset)SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                        xmlDoc.LoadXml(BusinessObjectInfo.ObjectKey);
                        string bpType = "", cogsAcct = "";
                        DateTime date = new DateTime();
                        int retcode = 0;
                        double temp = 0;
                        double invtotaldiff = 0;
                        int currentline = 0;
                        string productgroup = "";
                        string ChargeNo = "";
                        string sql = "";
                        string delno = "";


                        //sql = "select T0.docdate, T0.docnum, T4.U_Type, min(T1.docnum) as [DelNo] " +
                        //    "from oinv T0 inner join " +
                        //    "( " +
                        //    "select oinv.docentry, isnull(odln.U_Diff, 0) as U_Diff, odln.docnum " +
                        //    "from oinv inner join inv1 on oinv.docentry = inv1.docentry " +
                        //    "inner join dln1 on inv1.baseentry = dln1.docentry and inv1.basetype = 15 " +
                        //    "and inv1.BaseLine = dln1.LineNum " +
                        //    "inner join odln  on dln1.docentry = odln.docentry and odln.CANCELED = 'N' " +
                        //    "where oinv.DocEntry = " + xmlDoc.InnerText + " " +
                        //    "group by oinv.docentry, odln.U_Diff, odln.docnum " +
                        //    ") T1 on T0.docentry = T1.docentry " +
                        //    "inner join ocrd T4 on T0.cardcode = T4.cardcode " +
                        //    "where T0.docentry = " + xmlDoc.InnerText + " " +
                        //    "group by T0.docdate, T0.docnum, T4.U_Type";
                        sql = "select T0.docdate, T0.docnum, T4.U_Type, min(T3.docnum) as [DelNo] " +
                            " from oinv T0 inner join inv1 T1 on T0.docentry = T1.docentry " +
                            " inner join dln1 T2 on T1.baseentry = T2.docentry and T1.basetype = 15 " +
                            " and T1.BaseLine = T2.LineNum " +
                            " inner join odln T3 on T2.docentry = T3.docentry " +
                            " inner join ocrd T4 on T0.cardcode = T4.cardcode " +
                            " where T0.CANCELED = 'N' and T0.docentry = " + xmlDoc.InnerText + " " +
                            " group by T0.docdate, T0.docnum, T4.U_Type";
                        rs.DoQuery(sql);
                        if (rs.RecordCount > 0)
                        {
                            rs.MoveFirst();

                            //different = double.Parse(rs.Fields.Item("U_Diff").Value.ToString());

                            //if (different != 0)
                            {
                                SAPbobsCOM.JournalEntries oJE = (SAPbobsCOM.JournalEntries)SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);

                                date = DateTime.Parse(rs.Fields.Item("docdate").Value.ToString());
                                oJE.ReferenceDate = date;
                                oJE.Memo = "DO Charge Out";
                                oJE.UserFields.Fields.Item("U_ARInvNo").Value = rs.Fields.Item("docnum").Value.ToString();
                                oJE.UserFields.Fields.Item("U_DelNo").Value = rs.Fields.Item("DelNo").Value.ToString();

                                jehdr.DocType = "ARIV";
                                jehdr.RefDate = oJE.ReferenceDate;
                                jehdr.U_ARInvNo = oJE.UserFields.Fields.Item("U_ARInvNo").Value.ToString();
                                jehdr.U_DelNo = oJE.UserFields.Fields.Item("U_DelNo").Value.ToString();
                                jehdr.Memo = oJE.Memo;

                                bpType = rs.Fields.Item("U_Type").Value.ToString();
                                switch (bpType.ToUpper())
                                {
                                    case "LOCAL":
                                        cogsAcct = "500001";
                                        break;
                                    case "OVERSEA":
                                        cogsAcct = "500100";
                                        break;
                                    case "INTER-COMPANY":
                                        cogsAcct = "500200";
                                        break;
                                }


                                rs.DoQuery("select T3.U_ChargeNo, T3.docnum" +
                                    " from oinv T0 inner join inv1 T1 on T0.docentry = T1.docentry " +
                                    " inner join dln1 T2 on T1.baseentry = T2.docentry and T1.basetype = 15 " +
                                    " and T1.BaseLine = T2.LineNum " +
                                    " inner join odln T3 on T2.docentry = T3.docentry " +
                                    " where T0.docentry = " + xmlDoc.InnerText +
                                    " group by T3.U_ChargeNo, T3.docnum");
                                if (rs.RecordCount > 0)
                                {
                                    rs.MoveFirst();

                                    while (!rs.EoF)
                                    {
                                        //productgroup = rs.Fields.Item("U_CostCenter").Value.ToString();
                                        ChargeNo = rs.Fields.Item("U_ChargeNo").Value.ToString();
                                        delno = rs.Fields.Item("docnum").Value.ToString();
                                        //if (string.IsNullOrEmpty(productgroup)) productgroup = "";
                                        if (!string.IsNullOrEmpty(ChargeNo))
                                        {
                                            sql = " select T1.LineId, T5.U_CostCenter, (T1.U_QUANTITY * isnull(T6.AvgPrice,0)) - (T1.U_QUANTITY * isnull(T7.AvgPrice,0)) as total " +
                                                " from [@FT_CHARGE] T0 inner join [@FT_CHARGE1] T1 on T0.docentry = T1.docentry and T1.U_SOITEMCO <> T1.U_ITEMCODE" +
                                                " inner join oitm T4 on T1.U_SOITEMCO = T4.itemcode and isnull(T4.invntitem,'N') = 'Y'" +
                                                " inner join oitb T5 on T4.itmsgrpcod = T5.itmsgrpcod" +
                                                " inner join oitm T8 on T1.U_ITEMCODE = T8.itemcode and isnull(T8.invntitem,'N') = 'Y'" +
                                                " inner join ( select T1.itemcode, avg(T1.stockprice) as AvgPrice from OIGN T0 inner join IGN1 T1 on T0.DocEntry = T1.DocEntry where T0.U_DelNo = " + delno + " group by T1.itemcode ) T6 on T1.U_SOITEMCO = T6.itemcode" +
                                                " inner join ( select T1.itemcode, avg(T1.stockprice) as AvgPrice from OIGE T0 inner join IGE1 T1 on T0.DocEntry = T1.DocEntry where T0.U_DelNo = " + delno + " group by T1.itemcode ) T7 on T1.U_ITEMCODE = T7.itemcode" +
                                                " where T0.DocNum = " + ChargeNo +
                                                " order by T1.LineId";
                                            rs1.DoQuery(sql);
                                            if (rs1.RecordCount > 0)
                                            {
                                                rs1.MoveFirst();
                                                while (!rs1.EoF)
                                                {
                                                    productgroup = rs1.Fields.Item("U_CostCenter").Value.ToString();
                                                    temp = double.Parse(rs1.Fields.Item("total").Value.ToString());
                                                    temp = Math.Round(temp, 2, MidpointRounding.AwayFromZero);
                                                    currentline++;
                                                    if (currentline > 1)
                                                    {
                                                        oJE.Lines.Add();
                                                        oJE.Lines.SetCurrentLine(currentline - 1);
                                                    }
                                                    oJE.Lines.AccountCode = cogsAcct;
                                                    if (temp > 0)
                                                        oJE.Lines.Credit = temp;
                                                    else if (temp < 0)
                                                        oJE.Lines.Debit = -temp;
                                                    if (!string.IsNullOrEmpty(productgroup))
                                                        oJE.Lines.CostingCode = productgroup;

                                                    jedtl = new JEDetails();
                                                    jedtl.AccountCode = oJE.Lines.AccountCode;
                                                    jedtl.CostingCode = oJE.Lines.CostingCode;
                                                    jedtl.Debit = oJE.Lines.Debit;
                                                    jedtl.Credit = oJE.Lines.Credit;
                                                    JEdtls.Add(jedtl);

                                                    invtotaldiff = invtotaldiff + temp;

                                                    rs1.MoveNext();
                                                }
                                            }

                                        }
                                        rs.MoveNext();
                                    }

                                    if (invtotaldiff != 0)
                                    {
                                        currentline++;
                                        if (currentline > 1)
                                        {
                                            oJE.Lines.Add();
                                            oJE.Lines.SetCurrentLine(currentline - 1);
                                        }
                                        oJE.Lines.AccountCode = "150500";// "150400"; Provision for Cost of Goods Sold
                                        if (invtotaldiff > 0)
                                            oJE.Lines.Debit = invtotaldiff;
                                        else if (invtotaldiff < 0)
                                            oJE.Lines.Credit = -invtotaldiff;

                                        jedtl = new JEDetails();
                                        jedtl.AccountCode = oJE.Lines.AccountCode;
                                        jedtl.Credit = oJE.Lines.Credit;
                                        jedtl.Debit = oJE.Lines.Debit;
                                        JEdtls.Add(jedtl);
                                    }

                                }
                                if (invtotaldiff != 0)
                                {
                                    retcode = oJE.Add();
                                    if (retcode != 0)
                                    {
                                        jehdr.ErrMsg = SAP.SBOCompany.GetLastErrorDescription();
                                        jehdr.ErrDT = DateTime.Now.ToString("yyyyMMddHHmmss");
                                        ObjectFunctions.ErrorLog(jehdr, JEdtls);
                                        //if (SAP.SBOCompany.InTransaction) SAP.SBOCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                                        SAP.SBOApplication.MessageBox(SAP.SBOCompany.GetLastErrorDescription(), 1, "Ok", "", "");
                                        //FT_ADDON.SAP.SBOApplication.StatusBar.SetText(SAP.SBOCompany.GetLastErrorDescription(), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                jehdr.ErrMsg = "Data Event After " + ex.Message;
                jehdr.ErrDT = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                ObjectFunctions.ErrorLog(jehdr, JEdtls);
                //if (SAP.SBOCompany.InTransaction) SAP.SBOCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                SAP.SBOApplication.MessageBox("Data Event After " + ex.Message, 1, "Ok", "", "");
            }
        }
    }
}
