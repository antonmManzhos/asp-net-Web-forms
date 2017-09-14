using MasterSystem.Common;
using MasterSystem.Common.Db;
using MasterSystem.Web.OMS_GMW;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace etstagain
{
   
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            myButton.Text = "bye";

        }

        public static string getHtmlTires()
        {
            StringBuilder html = new StringBuilder();
            foreach (var tire in getLastTires())
            {
                html.Append("<div class='col-md-4 col-sm-6 col-xs-12 item'>" +
                                "<div>" +
                                    "<div class='img-container'><a href='#' ><img src='/pic/" + tire.imgUrl + "' alt='Product'></a></div>" +
                                    "<h4 class='product-title'><a href='#'> " + tire.shortDescription + "</a></h4>" +
                                    "<span class='price'>$" + tire.price + "</span>" +
                                    "<a href='#' class='order'><i class='fa fa-shopping-cart' aria-hidden='true'></i><span class='triangle'></span><span class='add'>ADD TO CART</span></a>" +
                                    "<a href='#' class='more'>DETAILS</a>" +
                                "</div></div>");

            }
            return html.ToString();
        }
        public static List<Tires> getLastTires()
        {
            List<Tires> last6Tires = null;
            try
            {
                string sqlQuery = "SELECT COUNT(*) FROM [omsdata].[dbo].[WebInv] WITH (NOLOCK)";
                int count = (int)DbUtility.ExecuteScalar(sqlQuery, AppSettings.B2BConnectionString);
                if (count > 0)
                {
                    last6Tires = new List<Tires>();
                    sqlQuery = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY(select null) ASC) AS rownumber, * FROM[omsdata].[dbo].[WebInv] ) AS foo WHERE rownumber > " + (count - 6);
                    DataTable lastAddedItems = DbUtility.GetDataTable(sqlQuery, AppSettings.B2BConnectionString);

                    foreach (DataRow item in lastAddedItems.Rows)
                    {
                        Tires newTire = new Tires();
                        newTire.shortDescription = (string)item.ItemArray[15];
                        newTire.imgUrl = (string)item.ItemArray[16];
                        newTire.PROD_CD = (string)item.ItemArray[1];
                        if(newTire.PROD_CD!="")
                        {
                            string sqlQueryPrice = "SELECT* FROM [omsdata].[dbo].[WebInvPrice] WHERE [omsdata].[dbo].[WebInvPrice].[PROD_CD] ='" + newTire.PROD_CD + "'";
                            DataRow price = DbUtility.GetDataRow(sqlQueryPrice, AppSettings.B2BConnectionString);
                            newTire.price = decimal.Parse(price.ItemArray[6].ToString());
                        }
                        last6Tires.Add(newTire);
                    }
                }
            }
            catch (FormatException ex) { }

            return last6Tires;
        }
        public class Tires
        {
            public string shortDescription { get; set; }
            public string imgUrl { get; set; }
            public decimal price { get; set; }
            public string PROD_CD { get; set; }
        }
    }
}
