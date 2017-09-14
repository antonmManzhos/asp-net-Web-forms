using MasterSystem.Common;
using MasterSystem.Common.Web;
using MasterSystem.Web.OMS_GMW;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace etstagain
{
    public class SomeC
    {
        public static DataTable GetCategories()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("select categoryid, case when parentid = 0 then null else parentid end as parentid,");
            sql.Append("'../General/BrowseCategory.aspx?CategoryId=' + rtrim(categoryid) as url,");
            sql.Append("categoryName from webcategory where active = 1 ");

            if (HttpContext.Current.Session[WebParameterConst.ECommerceCustomer] != null)
            {
                ECommerceCustomer user = (ECommerceCustomer)HttpContext.Current.Session[WebParameterConst.ECommerceCustomer];
                if (!user.IsSpecial)
                    sql.Append(" and IsSpecial = 0 ");
            }
            else
                sql.Append(" and IsSpecial = 0 ");


            sql.Append("order by ParentID, sequence, categoryname");

            SqlConnection connection = new SqlConnection(AppSettings.B2BConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(sql.ToString(), connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            DataTable disTable = new DataTable();
            disTable = dataTable.Copy();
            disTable.PrimaryKey = new DataColumn[] { disTable.Columns["categoryid"] };

            DataRow delrow = null;
            foreach (DataRow drtmp in dataTable.Rows)
            {
                if ((drtmp["parentid"].ToString() != string.Empty))
                {
                    delrow = disTable.Rows.Find(Conversion.ToInt(drtmp["parentid"].ToString()));
                    if (delrow == null)
                        drtmp.Delete();
                }
            }

            return dataTable;

            //treeView.DataNavigateUrlField = "url";
            //treeView.DataValueField = "categoryid";
            //treeView.DataTextField = "categoryName";
            //treeView.DataFieldID = "categoryid";
            //treeView.DataFieldParentID = "parentid";
            //treeView.DataSource = dataTable;
            //treeView.DataBind();
        }
    }
}