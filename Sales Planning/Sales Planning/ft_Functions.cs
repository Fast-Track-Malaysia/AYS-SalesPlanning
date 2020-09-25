using System;
using System.Collections.Generic;
using System.Text;

namespace FT_ADDON.AYS
{
    public class ft_Functions
    {
        public ft_Functions() { }
        public static bool CheckCreditTerm(SAPbouiCOM.Form oForm, SAPbouiCOM.DBDataSource ods, SAPbouiCOM.DBDataSource ods1, ref string errMsg)
        {
            try
            {
                SAPbobsCOM.Recordset rs = (SAPbobsCOM.Recordset)SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string yyyy = "", MM = "", dd = "", cardcode = "";
                DateTime date = new DateTime();
                if (ods.TableName.Contains("@"))
                {
                    yyyy = ods.GetValue("U_docdate", 0).ToString().Substring(0, 4);
                    MM = ods.GetValue("U_docdate", 0).ToString().Substring(4, 2);
                    dd = ods.GetValue("U_docdate", 0).ToString().Substring(6, 2);
                    cardcode = ods.GetValue("U_cardcode", 0).ToString();
                }
                else
                {
                    yyyy = ods.GetValue("docdate", 0).ToString().Substring(0, 4);
                    MM = ods.GetValue("docdate", 0).ToString().Substring(4, 2);
                    dd = ods.GetValue("docdate", 0).ToString().Substring(6, 2);
                    cardcode = ods.GetValue("cardcode", 0).ToString();
                }

                date = DateTime.Parse(yyyy + "-" + MM + "-" + dd);
                rs.DoQuery("select T0.* from oinv T0 inner join OCRD T1 on T0.cardcode = T1.cardcode where T0.cardcode='" + cardcode + "' and docstatus='O' and dateadd(day,isnull(T0.U_Grace,0),DATEADD (dd, -1, DATEADD(mm, DATEDIFF(mm, 0, docduedate) + 1, 0))) <= '" + date.ToString("yyyy-MM-dd") + "' ");
                //dateadd(day,isnull(T1.U_Grace,0),docduedate) <= '" + date.ToString("yyyy-MM-dd") + "' ");
                if (rs.RecordCount > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                SAP.SBOApplication.MessageBox("CheckCreditTerm " + ex.Message, 1, "Ok", "", "");
                return false;
            }
        }

        public static int CheckCreditLimit(SAPbouiCOM.Form oForm, SAPbouiCOM.DBDataSource ods, SAPbouiCOM.DBDataSource ods1, ref string errMsg, ref string limitType, ref double different, ref double currentUsage, ref double temporaryLimit, ref double customerLimit)
        {
            //return -1 if error
            //return 0 if not found
            //return > 0 if found
            try
            {
                SAPbobsCOM.Recordset rs = (SAPbobsCOM.Recordset)SAP.SBOCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string mainCurncy = "", docCur = "", MasterDBName = "", sql = "", cardcode = "", newCardCode = "", yyyy = "", MM = "", dd = "";
                double docRate = 1, limit = 0, doctotal = 0, totalDue = 0;
                int cnt = 0;
                DateTime date = new DateTime();
           
                if (ods.TableName.Contains("@"))
                {
                    yyyy = ods.GetValue("U_docdate", 0).ToString().Substring(0, 4);
                    MM = ods.GetValue("U_docdate", 0).ToString().Substring(4, 2);
                    dd = ods.GetValue("U_docdate", 0).ToString().Substring(6, 2);
                    cardcode = ods.GetValue("U_cardcode", 0).ToString();
                    doctotal = 0;
                }
                else
                {
                    yyyy = ods.GetValue("docdate", 0).ToString().Substring(0, 4);
                    MM = ods.GetValue("docdate", 0).ToString().Substring(4, 2);
                    dd = ods.GetValue("docdate", 0).ToString().Substring(6, 2);
                    cardcode = ods.GetValue("cardcode", 0).ToString();
                    doctotal = double.Parse(ods.GetValue("doctotal", 0).ToString());
                }
                date = DateTime.Parse(yyyy + "-" + MM + "-" + dd);

                rs.DoQuery("select * from oadm");
                mainCurncy = rs.Fields.Item("mainCurncy").Value.ToString();
                MasterDBName = rs.Fields.Item("U_MDBName").Value.ToString();

                //newCardCode = cardcode.IndexOf("-") == -1 ? cardcode : cardcode.Substring(0, cardcode.IndexOf("-"));

                //if (docCur.ToUpper() != mainCurncy.ToUpper())
                //{
                //    doctotal = doctotal * docRate;
                //}

                if (MasterDBName != "")
                {
                    // ykw 20180417
                    string tablename = ods.TableName;
                    string temp = "Exec " + MasterDBName + "..FT_CheckCreditLimit '" + cardcode + "', '" + date.ToString("yyyy-MM-dd") + "', " + @doctotal.ToString() + ", '" + tablename + "', '" + SAP.SBOCompany.CompanyDB + "'";
                    rs.DoQuery(temp);
                    if (rs.RecordCount <= 0)
                    {
                        if (SAP.SBOCompany.InTransaction) SAP.SBOCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        SAP.SBOApplication.MessageBox("CheckCreditLimit error." + MasterDBName + "..FT_CheckCreditLimit", 1, "Ok", "", "");
                        return -1;
                    }
                    else
                    {
                        if (rs.Fields.Item(0).Value.ToString() != "0")
                        {
                            limitType = rs.Fields.Item(1).Value.ToString();
                            different = double.Parse(rs.Fields.Item(2).Value.ToString());
                            currentUsage = double.Parse(rs.Fields.Item(3).Value.ToString());
                            temporaryLimit = double.Parse(rs.Fields.Item(4).Value.ToString());
                            customerLimit = double.Parse(rs.Fields.Item(5).Value.ToString());
                            return 1;
                        }
                    }
                    return 0;
                    // ykw 20180417

                    rs.DoQuery("select * from " + MasterDBName + ".dbo.ocrd where cardcode='" + cardcode + "'");
                    newCardCode = rs.Fields.Item("U_GroupIndicator").Value.ToString();

                    rs.DoQuery("select * from  " + MasterDBName + ".dbo.[@CUST_TMP_CRED_LIMIT] " +
                        " where U_CUST = '" + newCardCode + "' and   U_DB='" + SAP.SBOCompany.CompanyDB + "' and '" + date.ToString("yyyy-MM-dd") + "' >= U_SDATE and  '" + date.ToString("yyyy-MM-dd") + "' <= U_EDATE ");

                    // Get Temporary Credit Limit
                    if (rs.RecordCount > 0)
                    {
                        limit = double.Parse(rs.Fields.Item("U_CLIMIT").Value.ToString());
                        temporaryLimit = limit;
                    }

                    //Get Group / Individual Credit Limit
                    rs.DoQuery("select * from  " + MasterDBName + ".dbo.[@GRP_CUST_CRED_LIMIT] where U_CUST = '" + newCardCode + "' ");
                    if (rs.RecordCount > 0)
                    {
                        limit += double.Parse(rs.Fields.Item("U_CLIMIT").Value.ToString());
                        customerLimit = double.Parse(rs.Fields.Item("U_CLIMIT").Value.ToString());
                        limitType = "Group";
                    }
                    else
                    {
                        rs.DoQuery("select * from  " + MasterDBName + ".dbo.[@IND_CUST_CRED_LIMIT] where U_CUST = '" + newCardCode + "' and U_DB='" + SAP.SBOCompany.CompanyDB + "'");
                        if (rs.RecordCount > 0)
                        {
                            limit += double.Parse(rs.Fields.Item("U_CLIMIT").Value.ToString());
                            customerLimit = double.Parse(rs.Fields.Item("U_CLIMIT").Value.ToString());
                            limitType = "Individual";
                        }
                    }
                    if (limitType == "Individual")
                    {
                        sql += " select sum(T0.DocTotal - T0.PaidToDate) as total from " + SAP.SBOCompany.CompanyDB + ".dbo.ordr T0 inner join ocrd T1 on T0.cardcode = T1.cardcode where T0.DocStatus = 'O' and T1.U_GroupIndicator='" + newCardCode + "' " +
                                    " union all " +
                                    " select sum(T0.DocTotal - T0.PaidToDate) as total from " + SAP.SBOCompany.CompanyDB + ".dbo.odln T0 inner join ocrd T1 on T0.cardcode = T1.cardcode where T0.DocStatus = 'O' and T1.U_GroupIndicator='" + newCardCode + "' " +
                                    " union all " +
                                    " select sum(T0.DocTotal - T0.PaidToDate) as total from " + SAP.SBOCompany.CompanyDB + ".dbo.oinv T0 inner join ocrd T1 on T0.cardcode = T1.cardcode where T0.DocStatus = 'O' and T1.U_GroupIndicator='" + newCardCode + "' " +
                                    " union all " +
                                    " select -sum(T0.DocTotal - T0.PaidToDate) as total from " + SAP.SBOCompany.CompanyDB + ".dbo.orin T0 inner join ocrd T1 on T0.cardcode = T1.cardcode where T0.DocStatus = 'O' and T1.U_GroupIndicator='" + newCardCode + "' ";

                    }
                    else
                    {
                        // Get Current Total Due
                        rs.DoQuery("select * from " + MasterDBName + ".dbo.[@FT_CHILDDB]");
                        if (rs.RecordCount > 0)
                        {
                            while (!rs.EoF)
                            {
                                if (cnt > 0)
                                {
                                    sql += " union all ";
                                }
                                sql += " select sum(T0.DocTotal - T0.PaidToDate) as total from " + rs.Fields.Item("U_DBName").Value.ToString() + ".dbo.ordr T0 inner join ocrd T1 on T0.cardcode = T1.cardcode where T0.DocStatus = 'O' and T1.U_GroupIndicator='" + newCardCode + "' " +
                                    " union all " +
                                    " select sum(T0.DocTotal - T0.PaidToDate) as total from " + rs.Fields.Item("U_DBName").Value.ToString() + ".dbo.odln T0 inner join ocrd T1 on T0.cardcode = T1.cardcode where T0.DocStatus = 'O' and T1.U_GroupIndicator='" + newCardCode + "' " +
                                    " union all " +
                                    " select sum(T0.DocTotal - T0.PaidToDate) as total from " + rs.Fields.Item("U_DBName").Value.ToString() + ".dbo.oinv T0 inner join ocrd T1 on T0.cardcode = T1.cardcode where T0.DocStatus = 'O' and T1.U_GroupIndicator='" + newCardCode + "' " +
                                    " union all " +
                                    " select -sum(T0.DocTotal - T0.PaidToDate) as total from " + rs.Fields.Item("U_DBName").Value.ToString() + ".dbo.orin T0 inner join ocrd T1 on T0.cardcode = T1.cardcode where T0.DocStatus = 'O' and T1.U_GroupIndicator='" + newCardCode + "' ";

                                cnt++;
                                rs.MoveNext();
                            }
                            //sql = "select sum(total) as [total] from (" + sql + " ) T0";
                            //rs.DoQuery(sql);
                            //if (rs.RecordCount > 0)
                            //{
                            //    totalDue = double.Parse(rs.Fields.Item("total").Value.ToString());
                            //}
                        }
                    }
                    sql = "select sum(total) as [total] from (" + sql + " ) T0";
                    rs.DoQuery(sql);
                    if (rs.RecordCount > 0)
                    {
                        totalDue = double.Parse(rs.Fields.Item("total").Value.ToString());
                    }
                    if (ods.TableName.ToString().ToUpper() == "OQUT" || ods.TableName.Contains("@"))
                    {
                        currentUsage = totalDue;
                    }
                    else
                    {
                        currentUsage = totalDue + doctotal;
                    }
                    different = currentUsage - limit;
                    if (limit > 0 && currentUsage > limit)
                        return 1;

                }

                return 0;
            }
            catch (Exception ex)
            {
                SAP.SBOApplication.MessageBox("CheckCreditLimit " + ex.Message, 1, "Ok", "", "");
                return -1;
            }
        }

    }
}
