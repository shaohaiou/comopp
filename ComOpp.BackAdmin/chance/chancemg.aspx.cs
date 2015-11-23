using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComOpp.Components;

namespace ComOpp.BackAdmin.chance
{
    public partial class chancemg : AdminBase
    {
        protected override void Check()
        {
            if (!HXContext.Current.AdminCheck)
            {
                Response.Redirect("~/login.aspx");
                return;
            }
        }

        private CorporationInfo currentcorporation = null;
        public CorporationInfo CurrentCorporation
        {
            get
            {
                if (currentcorporation == null)
                {
                    if (GetInt("corpid") > 0)
                        currentcorporation = Corporations.Instance.GetModel(GetInt("corpid"));
                    else if (Corporation != null)
                        currentcorporation = Corporation;
                }

                return currentcorporation;
            }
        }

        public bool IsProcess
        {
            get
            {
                if (CurrentCorporation == null) return false;
                else return CurrentCorporation.IsProcess == 1;
            }
        }

        public int Forcedoffday 
        {
            get
            {
                if (CurrentCorporation == null) return 0;
                else return CurrentCorporation.Forcedoffday;
            }
        }

        public string Offcustomerlevel 
        {
            get
            {
                if (CurrentCorporation == null) return "";
                else return CurrentCorporation.Offcustomerlevel;
            }
        }

        public int Forcedoutday 
        {
            get
            {
                if (CurrentCorporation == null) return 0;
                else return CurrentCorporation.Forcedoutday;
            }
        }

        public string Forcedoutdaylevel 
        {
            get
            {
                if (CurrentCorporation == null) return "";
                else return CurrentCorporation.Forcedoutdaylevel;
            }
        }

        public int Trackmove 
        {
            get
            {
                if (CurrentCorporation == null) return 1;
                else return CurrentCorporation.Trackmove;
            }
        }

        public int Movecheck 
        {
            get
            {
                if (CurrentCorporation == null) return 1;
                else return CurrentCorporation.Movecheck;
            }
        }

        public int Voluntaryoffday 
        {
            get
            {
                if (CurrentCorporation == null) return 0;
                else return CurrentCorporation.Voluntaryoffday;
            }
        }

        public int Voluntaryoutday 
        {
            get
            {
                if (CurrentCorporation == null) return 0;
                else return CurrentCorporation.Voluntaryoutday;
            }
        }

        public int Offcheck 
        {
            get
            {
                if (CurrentCorporation == null) return 1;
                else return CurrentCorporation.Offcheck;
            }
        }

        public int Outcheck 
        {
            get
            {
                if (CurrentCorporation == null) return 1;
                else return CurrentCorporation.Outcheck;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string action = GetString("action");
            if (!string.IsNullOrEmpty(action))
            {
                if (GetString("action") == "del" && !string.IsNullOrEmpty(GetString("ids")) && GetInt("corpid") > 0)
                    Customers.Instance.Delete(GetString("ids"), GetInt("corpid"));

                Customers.Instance.ReloadCustomerListByCorporationCache(GetInt("corpid"));
                Response.Clear();
                Response.Write("ok");
                Response.End();
            }
            if (!IsPostBack)
            {
                ddlCorporationSearch.DataSource = Corporations.Instance.GetList(true);
                ddlCorporationSearch.DataTextField = "Name";
                ddlCorporationSearch.DataValueField = "ID";
                ddlCorporationSearch.DataBind();
                ddlCorporationSearch.Items.Insert(0, new ListItem("集团公池", "0"));

                SetSelectedByValue(ddlCorporationSearch, GetString("corpid"));
            }
            else
                Response.Redirect("chancemg.aspx?corpid=" + ddlCorporationSearch.SelectedValue + "&state=" + GetInt("state", 0));
        }
    }
}