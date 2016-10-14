using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Web.WebControls;

namespace BVNetwork.Attend.Admin.Partials
{
    public partial class EventFilter : EPiServer.UserControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public DateTime StartDate {
            get {

                DateTime result = DateTime.MinValue;
                DateTime.TryParse(DateStart.Text, out result);
                return result;
            }
        }

        public DateTime EndDate
        {
            get
            {

                DateTime result = DateTime.MaxValue;
                DateTime.TryParse(DateEnd.Text, out result);
                return result;
            }
        }


        protected void InvoicesNextMonth_Click(object sender, EventArgs e)
        {
            int year = (DateTime.Now.Month == 12) ? DateTime.Now.Year + 1 : DateTime.Now.Year;
            int month = (DateTime.Now.Month == 12) ? 1 : DateTime.Now.Month + 1;
            int day = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(2).AddDays(-1).Day;
            DateStart.Text = new DateTime(year, month, 1).ToShortDateString();
            DateEnd.Text = new DateTime(year, month, day).ToShortDateString();
        }
        protected void InvoicesLastMonth_Click(object sender, EventArgs e)
        {
            int year = (DateTime.Now.Month == 1) ? DateTime.Now.Year - 1 : DateTime.Now.Year;
            int month = (DateTime.Now.Month == 1) ? 12 : DateTime.Now.Month - 1;
            int day = DateTime.DaysInMonth(year, month);
            DateStart.Text = new DateTime(year, month, 1).ToShortDateString();
            DateEnd.Text = new DateTime(year, month, day).ToShortDateString();
        }
        protected void InvoicesThisMonth_Click(object sender, EventArgs e)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateStart.Text = new DateTime(year, month, 1).ToShortDateString();
            DateEnd.Text = new DateTime(year, month, day).ToShortDateString();
        }

        protected void InvoicesFuture_Click(object sender, EventArgs e)
        {
            DateStart.Text = DateTime.Now.ToShortDateString();
            DateEnd.Text = DateTime.Now.AddYears(2).ToShortDateString();
        }

        protected void GetInvoiceData_Click(object sender, EventArgs e) { 
            
        }
    }
}