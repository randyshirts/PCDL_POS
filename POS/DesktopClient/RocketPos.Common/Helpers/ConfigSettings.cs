using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace RocketPos.Common.Helpers
{
    public static class ConfigSettings
    {

        public static string DEFAULT_STATE
        {
            get
            {
                return ConfigurationManager.AppSettings
                        ["DEFAULT_STATE"].ToString();
            }
        }


        public static double ND_FEE
        {
            get
            {
                return Convert.ToDouble(ConfigurationManager.AppSettings
                        ["ND_FEE"]);
            }
        }

        public static double CONS_CREDIT_PAYOUT_PCT
        {
            get
            {
                return Convert.ToDouble(ConfigurationManager.AppSettings
                        ["CONS_CREDIT_PAYOUT_PCT"]);
            }
        }

        public static double CONS_CASH_PAYOUT_PCT
        {
            get
            {
                return Convert.ToDouble(ConfigurationManager.AppSettings
                        ["CONS_CASH_PAYOUT_PCT"]);
            }
        }

        public static double MEMBER_CONS_CREDIT_PAYOUT_PCT
        {
            get
            {
                return Convert.ToDouble(ConfigurationManager.AppSettings
                        ["MEMBER_CONS_CREDIT_PAYOUT_PCT"]);
            }
        }

        public static double MEMBER_PURCHASE_DISCOUNT_PCT
        {
            get
            {
                return Convert.ToDouble(ConfigurationManager.AppSettings
                        ["MEMBER_PURCHASE_DISCOUNT_PCT"]);
            }
        }

        public static bool IS_ACTIVE_FLAG
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings
                         ["IS_ACTIVE_FLAG"]);
            }
        }

        public static double MIN_LISTED_PRICE
        {
            get
            {
                return Convert.ToDouble(ConfigurationManager.AppSettings
                        ["MIN_LISTED_PRICE"]);
            }
        }

        public static string accessKeyId
        {
            get
            {
                return ConfigurationManager.AppSettings
                        ["accessKeyId"].ToString();
            }
        }

        public static string ASSOCIATE_TAG
        {
            get
            {
                return ConfigurationManager.AppSettings
                        ["ASSOCIATE_TAG"].ToString();
            }
        }

        public static string secretKey
        {
            get
            {
                return ConfigurationManager.AppSettings
                        ["secretKey"].ToString();
            }
        }

        public static void Refresh()
        {
            ConfigurationManager.RefreshSection("appSettings");
        }

        //Example of how to use the configsettings.refresh method
        //private void RefreshControls()
        //{
        //    ConfigSettings opts = (ConfigSettings)((ObjectDataProvider)
        //        Application.Current.
        //            FindResource("odpSettings")).ObjectInstance;

        //    opts.Refresh();

        //    txtDefaultState.GetBindingExpression(
        //        TextBox.TextProperty).UpdateTarget();
        //    chkIsActiveFlag.GetBindingExpression(
        //        CheckBox.IsCheckedProperty).UpdateTarget();
        //}
    }



}
