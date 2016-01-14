
using System;
using System.Web.UI.WebControls;
using BVNetwork.Attend.Business.Email;
using BVNetwork.Attend.Business.Participant;
using BVNetwork.Attend.Business.Settings;


namespace BVNetwork.Attend.Admin
{
    public partial class SettingsEdit : System.Web.UI.Page
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ParticipantProvidersDropDown.Items.Clear();
            string defaultParticipantProvider = Settings.GetSetting("DefaultParticipantProviderString");
            Type[] ParticipantProviders = ParticipantProviderManager.GetParticipantProviders();
            foreach (Type t in ParticipantProviders)
            {
                ListItem li = new ListItem(t.Name, t.FullName + ", " + t.Assembly.FullName.Substring(0, t.Assembly.FullName.IndexOf(',')));
                li.Selected = li.Value == defaultParticipantProvider;
                ParticipantProvidersDropDown.Items.Add(li);
            }
            ParticipantProvidersDropDown.DataBind();
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ProviderOverview.DataBind();
            BVNetwork.Attend.Business.Localization.FixEditModeCulture.TryToFix();
            if (!IsPostBack)
            {
                SMSUrlTextBox.Text = Settings.GetSetting("smsurl");
            }
        }

        protected void Save_OnClick(object sender, EventArgs e)
        {
            Settings.AddSetting("smsurl", SMSUrlTextBox.Text);
        }

        protected void Test_OnClick(object sender, EventArgs e)
        {
            ResultLiteral.Mode = LiteralMode.Encode;
            ResultLiteral.Text = SmsSender.SendSMS(TestToTextBox.Text, TestFromTextBox.Text, TestMessageTextBox.Text);
        }

        protected void SearchProviders_Click(object sender, EventArgs e)
        {
            SaveProviderButton.Visible = true;
            ProviderDropDowns.Visible = true;
            ProviderOverview.Visible = false;

        }

        protected void SaveSettings_Click(object sender, EventArgs e)
        {
            Settings.AddSetting("DefaultParticipantProviderString", ParticipantProvidersDropDown.SelectedValue);
            ParticipantProviderManager.Initialize();
            ProviderDropDowns.Visible = false;
            SaveProviderButton.Visible = false;
            ProviderOverview.Visible = true;
            ProviderOverview.DataBind();
        }


    }
}