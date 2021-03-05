using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace FT_ADDON.AYS
{
    class MyFromEvent
    {
        public static void processItemEventbefore(SAPbouiCOM.Form oForm, ref SAPbouiCOM.ItemEvent pVal, ref bool BubbleEvent)
        {
            if (pVal.ItemUID == "1" && oForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
            {
                string formtype = oForm.TypeEx;
                string cardcode = "";
                try
                {
                    if (formtype == "FT_SPLAN" || formtype == "FT_TPPLAN" || formtype == "FT_CHARGE")
                        cardcode = oForm.DataSources.DBDataSources.Item(0).GetValue("U_CardCode", 0);
                    else if (formtype == "1250000100" || formtype == "139" || formtype == "149")
                        cardcode = oForm.DataSources.DBDataSources.Item(0).GetValue("CardCode", 0);

                }
                catch
                { }
                if (cardcode != null)
                {
                    string NotifyV = "";
                    string errMsg = "";
                    string sql = "Select U_NotifyV from [@FT_SPODCTRL] where Code='" + formtype  + "'";
                    SAPbobsCOM.Recordset query = SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset) as SAPbobsCOM.Recordset;
                    query.DoQuery(sql);
                    if (query.RecordCount > 0)
                    {
                        query.MoveFirst();
                        NotifyV = query.Fields.Item(0).ToString();
                        if (NotifyV != "NA")
                        {
                            if (ft_Functions.CheckCreditTerm(oForm, oForm.DataSources.DBDataSources.Item(0), oForm.DataSources.DBDataSources.Item(1), ref errMsg))
                            {
                                SAP.SBOApplication.MessageBox("There is/are invoices overdue for this customer", 1, "Ok", "", "");
                                if (NotifyV == "MSG_BLOCK")
                                    BubbleEvent = false;

                            }
                        }
                    }

                    string limitType = "";
                    double different = 0, c_usage = 0, t_limit = 0, c_limit = 0;
                    int cnt = 0;
                    sql = "Select U_NotifyV from [@FT_SPCLCTRL] where Code='" + formtype + "'";
                    query = SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset) as SAPbobsCOM.Recordset;
                    query.DoQuery(sql);
                    if (query.RecordCount > 0)
                    {
                        query.MoveFirst();
                        NotifyV = query.Fields.Item(0).ToString();
                        if (NotifyV != "NA")
                        {
                            cnt = ft_Functions.CheckCreditLimit(oForm, oForm.DataSources.DBDataSources.Item(0), oForm.DataSources.DBDataSources.Item(1), ref errMsg, ref limitType, ref different, ref c_usage, ref t_limit, ref c_limit);
                            if (cnt > 0)
                            {
                                SAP.SBOApplication.MessageBox("Credit Limit Exceeded " + Environment.NewLine + "Limit Type - " +
                                    limitType + Environment.NewLine + " Over Limit Amount - RM " + different.ToString("#,###,###,###.00"), 1, "Ok", "", "");
                                if (NotifyV == "MSG_BLOCK")
                                    BubbleEvent = false;

                            }
                            else if (cnt == -1)
                                BubbleEvent = false;
                        }
                    }

                }

            }
        }
    }
}
