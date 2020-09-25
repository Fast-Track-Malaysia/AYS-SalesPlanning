using System;
using System.Collections.Generic;
using System.Text;

namespace FT_ADDON.AYS
{
    class Sysform_SalesQuotation
    {
        public static void processDataEventbefore(SAPbouiCOM.Form oForm, ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, ref bool BubbleEvent)
        {
            try
            {
                switch (BusinessObjectInfo.EventType)
                {
                    case SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD:
                        SAPbouiCOM.DBDataSource ods = oForm.DataSources.DBDataSources.Item("OQUT");
                        SAPbouiCOM.DBDataSource ods1 = oForm.DataSources.DBDataSources.Item("QUT1");
                        string errMsg = "", limitType = "";
                        double different = 0, c_usage = 0, t_limit = 0, c_limit = 0;
                        if (ft_Functions.CheckCreditTerm(oForm, ods, ods1, ref errMsg))
                        {
                            SAP.SBOApplication.MessageBox("There is/are invoices overdue for this customer", 1, "Ok", "", "");
                        }
                        errMsg = "";
                        int cnt = 0;
                        cnt = ft_Functions.CheckCreditLimit(oForm, ods, ods1, ref errMsg, ref limitType, ref different, ref c_usage, ref t_limit, ref c_limit);
                        if (cnt == -1)
                        {
                            BubbleEvent = false;
                            break;
                        }
                        else if (cnt >= 1)
                        {
                            SAP.SBOApplication.MessageBox("Credit Limit Exceeded " + Environment.NewLine + "Limit Type - " +
                            limitType + Environment.NewLine + " Over Limit Amount - RM " + different.ToString("#,###,###,###.00"), 1, "Ok", "", "");
                        }
                        //BubbleEvent = false;
                        break;
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
