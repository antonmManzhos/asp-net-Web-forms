using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace etstagain
{
    public partial class Summary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


        }
        public string GetNoShowHtml()
        {
            StringBuilder html = new StringBuilder();
            var noData = ResponseRepository.GetRepository().GetAllResponses().Where(r => !r.WillAttend.Value);
            foreach (var rsvp in noData)
            {
                html.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td>", rsvp.Name, rsvp.Email, rsvp.Phone));
            }
            return html.ToString();
        }

        /*code from server*/




        /*code out from server*/
    }
}