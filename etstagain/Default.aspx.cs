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

                        //foreach (DataRow row in SomeC.GetCategories().Rows)
            //{

            //    DataTable products = null;

            //    int _CategoryId = (int)row["categoryId"];
            //    int _FristStatusId = 0;
            //    int _SecondStatusId = 0;
            //    string _BRAND_CD = "";
            //    string _CARRI_CD = "";
            //    string _MODEL_CD = "";
            //    string _STYLE_CD = "";
            //    string _COLOR_CD = "";
            //    string _EVENT_CD = "";
            //    string _CUS_ID = "";
            //    string _keyword = string.Empty;
            //    string _ReleaseDate = string.Empty;
            //    string _ReleaseType = string.Empty;
            //    int intDate = Conversion.ToDateInt(_ReleaseDate);
            //    products = GetCustomerCategoryProduct(AppSettings.OMSCode, _CategoryId, _FristStatusId, _SecondStatusId, intDate, _keyword, 1, Conversion.ToInt(_BRAND_CD, 0), Conversion.ToInt(_CARRI_CD, 0), Conversion.ToInt(_MODEL_CD, 0), Conversion.ToInt(_STYLE_CD, 0), _COLOR_CD.Trim(), _EVENT_CD.Trim(), AppSettings.ECommerceWarehouse, AppSettings.B2BConnectionString);
            //    ProductDataList.DataSource = products;
            //    ProductDataList.DataBind();
            //    int a = 10;


            //foreach (var listItem in ProductDataList.Items)
            //{

            //}
            //if(IsPostBack)
            //{
            //    GuestResponse rsvp = new GuestResponse();

            //    if (TryUpdateModel(rsvp, new FormValueProvider(ModelBindingExecutionContext)))
            //    {
            //        ResponseRepository.GetRepository().AddResponse(rsvp);
            //        if (rsvp.WillAttend.HasValue && rsvp.WillAttend.Value)
            //        {
            //            Response.Redirect("seeyouthere.html");
            //        }
            //        else
            //        {
            //            Response.Redirect("sorryyoucantcome.html");
            //        }
            //    }
            //}
            //}


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
        public static DataTable GetCustomerCategoryProduct(string pOMSCode, int categoryId, int firststatusId, int secondstatusId, int intDate, string keyword, int IsSpecial, int brandId, int carrierId, int modelId, int styleId, string colorCd, string eventCd, string whs_num, string connectionString) //, string ReleaseType, string pCUS_ID)

        {

            StringBuilder sql = new StringBuilder();

            sql.Append("Select ");

            sql.Append("w.*, dbo.func_GetWebInstkBySelectedWhs(w.PROD_CD, 'IN_STOCK') IN_STOCK, dbo.func_GetWebInstkBySelectedWhs(w.PROD_CD, 'ORDER_QTY') ORDER_QTY, dbo.func_GetWebInstkBySelectedWhs(w.PROD_CD, 'IN_STOCK') - dbo.func_GetWebInstkBySelectedWhs(w.PROD_CD, 'ORDER_QTY') - dbo.func_GetWebInstkBySelectedWhs(w.PROD_CD, 'BACK_QTY') AV_QTY, f.FirstStatusName, s.SecondStatusName, i.[MINSTOCK],");

            sql.Append("i.[PRS_TYPE], i.[RETAIL_PRS], i.[WHOLE_PRS], i.[WHOLE_PRS2], i.[WHOLE_PRS3],");

            sql.Append("i.[CORP_PRS],");

            if (pOMSCode == "OMS_CMA")

            {

                sql.Append("i.[FET_FEE],");

            }

            else

            {

                sql.Append("0 [FET_FEE],");

            }

            if (DbUtility.Exists("select name from sysobjects where name='invprice'", connectionString) && DbUtility.Exists("select name from sysobjects where name='itm_rsvq'", connectionString))

            {

                sql.Append("ISNULL(ipq.[PRICE_1],ISNULL(ips.[PRICE_1],i.[PRICE_1]))PRICE_1,");

                sql.Append("ISNULL(ipq.[PRICE_2],ISNULL(ips.[PRICE_2],i.[PRICE_2]))PRICE_2,");

                sql.Append("ISNULL(ipq.[PRICE_3],ISNULL(ips.[PRICE_3],i.[PRICE_3]))PRICE_3,");

                sql.Append("ISNULL(ipq.[PRICE_4],ISNULL(ips.[PRICE_4],i.[PRICE_4]))PRICE_4,");

                sql.Append("ISNULL(ipq.[PRICE_5],ISNULL(ips.[PRICE_5],i.[PRICE_5]))PRICE_5,");

                sql.Append("ISNULL(ipq.[PRICE_6],ISNULL(ips.[PRICE_6],i.[PRICE_6]))PRICE_6,");

                sql.Append("i.[RG_X],");

                sql.Append("ISNULL(ipq.[RG_0],ISNULL(ips.[RG_0],i.[RG_0]))RG_0,");

                sql.Append("ISNULL(ipq.[RG_1],ISNULL(ips.[RG_1],i.[RG_1]))RG_1,");

                sql.Append("ISNULL(ipq.[RG_2],ISNULL(ips.[RG_2],i.[RG_2]))RG_2,");

                sql.Append("ISNULL(ipq.[RG_3],ISNULL(ips.[RG_3],i.[RG_3]))RG_3,");

                sql.Append("ISNULL(ipq.[RG_4],ISNULL(ips.[RG_4],i.[RG_4]))RG_4,");

                sql.Append("ISNULL(ipq.[RG_5],ISNULL(ips.[RG_5],i.[RG_5]))RG_5,");

                sql.Append("ISNULL(ipq.[RG_6],ISNULL(ips.[RG_6],i.[RG_6]))RG_6,");

            }

            else

            {

                sql.Append(" [PRICE_1], [PRICE_2], [PRICE_3], [PRICE_4], [PRICE_5], [PRICE_6],");

                sql.Append(" [RG_X], [RG_0], [RG_1], [RG_2], [RG_3], [RG_4], [RG_5], [RG_6],");

            }



            sql.Append("i.[CLASS_CD], i.[MINSTOCK], cast(i.PC_CASE as decimal(5,0)) as PC_CASE ");



            if (categoryId > 0)

                sql.Append(",j.CategoryId, j.CategorizedDescription ");

            else

                sql.Append(",0 as CategoryId, '' as CategorizedDescription ");





            sql.Append(" FROM WebInv w ");

            sql.Append(" Inner Join inv i On w.PROD_CD = i.PROD_CD ");



            if (categoryId > 0)

                sql.Append(" Inner Join WebJoinCategoryInv j On w.PROD_CD= j.PROD_CD ");



            if (carrierId > 0)

                sql.Append(" Inner Join WebJoinCarrierInv cr On w.PROD_CD= cr.PROD_CD ");



            if (eventCd.Trim() != string.Empty)

                sql.Append(" Inner Join WebInvSpEvent ev On w.PROD_CD= ev.PROD_CD ");



            //            if (DbUtility.Exists("select name from sysobjects where name='invprice'", connectionString) && DbUtility.Exists("select name from sysobjects where name='itm_rsvq'", connectionString))

            //            {

            //                sql.Append(" Left Join itm_rsvq ipq On ipq.prod_cd = w.prod_cd and ipq.cus_id='" + DbUtility.ParseSql(pCUS_ID.Trim()) + "' ");

            //                sql.Append(" Left Join (select ips.* from invprice ips inner join customer cus on ips.prs_type=cus.cus_type and cus.cus_id='" + DbUtility.ParseSql(pCUS_ID.Trim()) + "')ips On ips.prod_cd = w.prod_cd ");

            //            }



            sql.Append(" Left Join WebFirstStatus f On w.FirstStatusId = f.FirstStatusId ");

            sql.Append(" Left Join WebSecondStatus s On w.SecondStatusId = s.SecondStatusId ");



            //            if (ReleaseType.Trim().ToUpper() == "P")

            //            {

            //                sql.Append(" Inner Join (select prod_cd, max(log_date) log_date from plog where pur_cd = 2 group by prod_cd having max(log_date) =" + intDate + ") p on p.prod_cd = w.prod_cd ");

            //            }



            //            sql.Append(" Inner Join( Select Sum(COALESCE(IN_STOCK, 0)) as IN_STOCK, Sum(COALESCE(ORDER_QTY, 0)) as ORDER_QTY, Sum(COALESCE(IN_STOCK, 0))-Sum(COALESCE(ORDER_QTY, 0)) as AV_QTY,  prod_cd From inv_data ");



            //            if ( (whs_num != "0") && (whs_num != string.Empty) )

            //                sql.Append(string.Format(" where whs_num = '{0}' ", whs_num));



            //            sql.Append(" group by prod_cd ) as t on t.prod_cd = i.prod_cd "); 

            sql.Append(" WHERE w.Active <> 0 ");

            sql.Append(" AND w.ShowForCustomer <> 0 and ( w.QtyControlCustomer=0 or w.QtyControlCustomer <> 0 and CASE w.TargetQtyOptionCustomer WHEN 1 THEN dbo.func_GetWebInstkBySelectedWhs(w.PROD_CD,'IN_STOCK')-dbo.func_GetWebInstkBySelectedWhs(w.PROD_CD,'ORDER_QTY') ELSE dbo.func_GetWebInstkBySelectedWhs(w.PROD_CD,'IN_STOCK') END>CASE w.LimitQtyOptionCustomer WHEN 1 THEN i.MINSTOCK  ELSE w.LimitQtyCustomer END )  ");



            if (keyword.Trim() != string.Empty)

                sql.Append(" AND (w.ShortDescription like '%" + DbUtility.ParseSql(keyword.Trim()) + "%' or w.prod_cd like '%" + DbUtility.ParseSql(keyword.Trim()) + "%') ");



            //            if (intDate > 0 && ReleaseType.Trim().ToUpper() != "P")

            //                sql.Append(" AND i.create_dt = " + intDate);



            if (IsSpecial == 0)

                sql.Append(" AND w.IsSpecial = " + IsSpecial);



            if (firststatusId > 0)

                sql.Append(" AND f.FirstStatusId = " + firststatusId);



            if (secondstatusId > 0)

                sql.Append(" AND s.SecondStatusId = " + secondstatusId);



            if (brandId > 0)

                sql.Append(" AND w.brandId = " + brandId);



            if (carrierId > 0)

            {

                sql.Append(" AND cr.carrierId = " + carrierId);

            }





            if (modelId > 0)

                sql.Append(" AND w.modelId = " + modelId);



            if (styleId > 0)

                sql.Append(" AND w.styleId = " + styleId);





            if (eventCd.Trim() != string.Empty)

                sql.Append(" AND ev.SPEVENT_CD='" + DbUtility.ParseSql(eventCd.Trim()) + "'");



            if (colorCd.Trim() != string.Empty)

                sql.Append(" AND i.unit_color='" + DbUtility.ParseSql(colorCd.Trim()) + "'");



            if (categoryId > 0)

            {

                sql.Append(" AND j.CategoryId = " + categoryId);

                sql.Append(" Order By j.Sequence, j.PROD_CD ");

            }

            else

                sql.Append(" Order By w.PROD_CD ");



            return DbUtility.GetDataTable(sql.ToString(), connectionString);

        }

    }
}
