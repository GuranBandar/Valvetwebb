using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using Valvetwebb.Aktivitet;
using Valvetwebb.Kontroller;
using Valvetwebb.Objekt;

namespace Valvetwebb
{
    public partial class Valvlista : PageBase
    {

        public GUI_kontroller gUI_Kontroller;
        public int PostID { get; set; }
        /// <summary>
        /// FelID från metodanrop till GUI:et
        /// </summary>
        public static string FelID = "";
        /// <summary>
        /// FelText från metodanrop till GUI:et
        /// </summary>
        public static string Feltext = "";

        private string FromDatum { get; set; }

        private string Sidan { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Navigation"] == null)
            {
                Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {
                gUI_Kontroller = new GUI_kontroller();
                GetCurrentCulture();
                Session["Referencepage"] = "Valvlista.aspx";
                Session["MessageTitle"] = "Valvlista";
                //Session["MessageText"] = string.Empty;
                this.knappSearch_Click(sender, e);
                txtSearchPost.Focus();
                knappSkapaPdf.Visible = true;
            }
            else
            {
                Session["SearchPost"] = txtSearchPost.Text;
            }
        }

        /// <summary>
        /// Visa-knappen i listan här 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgList_CellContentClick(object sender, DataGridCommandEventArgs e)
        {
            TableCell itemCell = e.Item.Cells[0];
            string item = itemCell.Text;

            try
            {
                //postID = (int)itemPost.Text;
                Sidan = "ValvPostInfo.aspx";
                VisaSida(Sidan, "PostID", item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void VisaSida(string sidan, string idFalt, string id)
        {
            string NavigateUrl;
            NavigateUrl = sidan + "?" + idFalt + "= " + id;
            Response.Redirect(NavigateUrl);
        }

        private void VisaSida(string sidan)
        {
            string NavigateUrl;
            NavigateUrl = sidan;
            Response.Redirect(NavigateUrl);
        }

        protected void knappSearch_Click(object sender, EventArgs e)
        {

            if (Session["SearchPost"] != null)
            {
                txtSearchPost.Text = Session["SearchPost"].ToString();
            }
            else
            {
                txtSearchPost.Text = string.Empty;
            }

            Session["SearchPost"] = txtSearchPost.Text;
            dgList.DataSource = InitieraValvlista();
            dgList.DataBind();
        }

        protected void knappNy_Click(object sender, EventArgs e)
        {
            Session["hfiNyPost"] = "Ja";
            Sidan = "ValvPostInfo.aspx";
            VisaSida(Sidan);
        }

        /// <summary>
        /// Tillbaka knappen tryckt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void knappAvbryt_Click(object sender, EventArgs e)
        {
            Session["SearchPost"] = string.Empty;
            Response.Redirect("Meny.aspx");
        }

        /// <summary>
        /// Dags att logga ut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void knappLogout_Click(object sender, EventArgs e)
        {
            SessionLogout();
            Response.Redirect("LogIn.aspx");
        }

        /// <summary>
        /// Sakap Pdf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void knappSkapaPdf_Click(object sender, EventArgs e)
        {
            PDFLista.WebUser = (Anvandare)Session["WebUser"];
            var isMobile = DeviceControl.IsMobile(Context.Request.Headers["user-agent"].ToString()); 

            if (isMobile)
            {
                return;
            }

            PDFLista pdf = new PDFLista();
            string fileName = pdf.ExportToPdf();

            Session["MessageText"] = "PDF skapad = " + fileName.ToString();
            MessageBox();

            HttpResponse currentResponse = HttpContext.Current.Response;
            currentResponse.Clear();
            currentResponse.ClearHeaders();
            currentResponse.ContentType = "application/pdf";
            currentResponse.AppendHeader("Content-Disposition", "attachment; " + fileName);

            //create the "my.pdf" here

            currentResponse.Flush();
            currentResponse.End();
        }

        /// <summary>
        /// Sök Valvpost med konto och kankse postnamn
        /// </summary>
        /// <returns></returns>
        private DataView InitieraValvlista()
        {
            Anvandare webUser = (Anvandare)Session["WebUser"];
            List<ValvPost> valvpostList = null;
            ValvPostAktivitet ValvpostAktivitet = new ValvPostAktivitet();
            valvpostList = ValvpostAktivitet.SökValvPost(webUser.Konto, Session["SearchPost"].ToString());

            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("PostID", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Postnummer", typeof(Int32)));
            dt.Columns.Add(new DataColumn("Postnamn", typeof(string)));
            int postnummer = 0;

            if (valvpostList.Count > 0)
            {
                foreach (ValvPost valvpost in valvpostList)
                {
                    postnummer++;
                    dr = dt.NewRow();
                    dr[0] = valvpost.PostID;
                    dr[1] = postnummer;
                    dr[2] = valvpost.Postnamn;
                    dt.Rows.Add(dr);
                }
            }
            else
            {
                if (Session["MessageText"].ToString() != string.Empty)
                {
                    //Då är ett meddelande redan visat
                    Session["MessageText"] = string.Empty;
                }
                else
                {
                    if (Session["SearchPost"].ToString() == string.Empty)
                    {
                        Session["MessageText"] = "Finns inga valvposter";
                    }
                    else
                    {
                        Session["MessageText"] = "Finns inga valvposter med postnamn = eller < '" + Session["SearchPost"] + "'";
                    }
                    //Session["SearchPost"] = string.Empty;
                    MessageBox();
                }
            }

            DataView dv = new DataView(dt);
            return dv;
        }
    }
}