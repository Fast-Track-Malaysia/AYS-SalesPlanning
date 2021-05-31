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
            try
            {

                if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED)
                {
                    if (pVal.ItemUID == "1" && oForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                    {
                        string formtype = oForm.TypeEx;
                        string cardcode = "";
                        try
                        {
                            if (formtype == "FT_SPLAN" || formtype == "FT_TPPLAN")
                                cardcode = oForm.DataSources.DBDataSources.Item(0).GetValue("U_CardCode", 0);
                            else if (formtype == "1250000100" || formtype == "139" || formtype == "149")
                                cardcode = oForm.DataSources.DBDataSources.Item(0).GetValue("CardCode", 0);

                        }
                        catch
                        { }
                        if (cardcode != null)
                        {
                            SAPbouiCOM.Item oItem = null;
                            SAPbouiCOM.ComboBox oComboapp = null;
                            SAPbouiCOM.EditText oEdit = null;

                            string NotifyV = "";
                            string errMsg = "";
                            string sql = "Select U_NotifyV from [@FT_SPODCTRL] where Code='" + formtype + "'";
                            SAPbobsCOM.Recordset query = SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset) as SAPbobsCOM.Recordset;
                            query.DoQuery(sql);
                            if (query.RecordCount > 0)
                            {
                                query.MoveFirst();
                                NotifyV = query.Fields.Item(0).Value.ToString();
                                if (NotifyV != "NA")
                                {
                                    if (ft_Functions.CheckCreditTerm(oForm, oForm.DataSources.DBDataSources.Item(0), oForm.DataSources.DBDataSources.Item(1), ref errMsg))
                                    {
                                        if (formtype == "139")
                                        {
                                            oEdit = (SAPbouiCOM.EditText)oForm.Items.Item("U_CTERM").Specific;
                                            oEdit.String = "Y";
                                        }
                                        if (NotifyV == "MSG_BLOCK")
                                        {
                                            if (formtype == "FT_SPLAN" || formtype == "FT_TPPLAN")
                                            {
                                                oForm.DataSources.DBDataSources.Item("@" + formtype).SetValue("U_APP", 0, "W");
                                                //oForm.DataSources.DBDataSources.Item("@" + formtype).SetValue("U_APPBY", 0, SAP.SBOCompany.UserName);
                                                //oForm.DataSources.DBDataSources.Item("@" + formtype).SetValue("U_APPDATE", 0, DateTime.Today.ToString("yyyyMMdd"));
                                                //oForm.DataSources.DBDataSources.Item("@" + formtype).SetValue("U_APPTIME", 0, DateTime.Now.ToString("HHmm"));
                                            }
                                            else if (formtype == "139")
                                            {
                                                oForm.DataSources.DBDataSources.Item("ORDR").SetValue("U_APP", 0, "W");
                                            }
                                            SAP.SBOApplication.MessageBox("There is/are invoices overdue for this customer", 1, "Ok", "", "");
                                        }

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
                                NotifyV = query.Fields.Item(0).Value.ToString();
                                if (NotifyV != "NA")
                                {
                                    cnt = ft_Functions.CheckCreditLimit(oForm, oForm.DataSources.DBDataSources.Item(0), oForm.DataSources.DBDataSources.Item(1), ref errMsg, ref limitType, ref different, ref c_usage, ref t_limit, ref c_limit);
                                    if (cnt > 0)
                                    {
                                        if (formtype == "139")
                                        {
                                            oItem = oForm.Items.Item("U_CUsage");
                                            oEdit = (SAPbouiCOM.EditText)oItem.Specific;
                                            oEdit.DataBind.SetBound(true, "ORDR", "U_CUsage");
                                            oEdit.Value = c_usage.ToString();

                                            oItem = oForm.Items.Item("U_TLimit");
                                            oEdit = (SAPbouiCOM.EditText)oItem.Specific;
                                            oEdit.DataBind.SetBound(true, "ORDR", "U_TLimit");
                                            oEdit.Value = t_limit.ToString();

                                            oItem = oForm.Items.Item("U_CLimit");
                                            oEdit = (SAPbouiCOM.EditText)oItem.Specific;
                                            oEdit.DataBind.SetBound(true, "ORDR", "U_CLimit");
                                            oEdit.Value = c_limit.ToString();
                                        }
                                        if (NotifyV == "MSG_BLOCK")
                                        {
                                            if (formtype == "FT_SPLAN" || formtype == "FT_TPPLAN")
                                            {
                                                oForm.DataSources.DBDataSources.Item("@" + formtype).SetValue("U_APP", 0, "W");
                                                //oForm.DataSources.DBDataSources.Item("@" + formtype).SetValue("U_APPBY", 0, SAP.SBOCompany.UserName);
                                                //oForm.DataSources.DBDataSources.Item("@" + formtype).SetValue("U_APPDATE", 0, DateTime.Today.ToString("yyyyMMdd"));
                                                //oForm.DataSources.DBDataSources.Item("@" + formtype).SetValue("U_APPTIME", 0, DateTime.Now.ToString("HHmm"));
                                            }
                                            else if (formtype == "139")
                                            {
                                                oItem = oForm.Items.Item("U_APP");
                                                oComboapp = (SAPbouiCOM.ComboBox)oItem.Specific;
                                                oComboapp.Select("W", SAPbouiCOM.BoSearchKey.psk_ByValue);

                                            }

                                        }

                                        SAP.SBOApplication.MessageBox("Credit Limit Exceeded " + Environment.NewLine + "Limit Type - " +
                                            limitType + Environment.NewLine + " Over Limit Amount - RM " + different.ToString("#,###,###,###.00"), 1, "Ok", "", "");
                                    }
                                    else if (cnt == -1)
                                    {
                                        BubbleEvent = false;
                                        SAP.SBOApplication.MessageBox("Credit Limit SP Error");
                                    }
                                }
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                SAP.SBOApplication.MessageBox("Data Event Before " + ex.Message, 1, "Ok", "", "");
                BubbleEvent = false;
            }
        }
    }
}
