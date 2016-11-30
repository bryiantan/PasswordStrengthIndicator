using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Membership.OpenAuth;

namespace WebApplication.Account
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
            RegisterUser.CreatingUser += RegisterUser_CreatingUser;
        }

        void RegisterUser_CreatingUser(object sender, LoginCancelEventArgs e)
        {
            TextBox password = ((TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Password"));

            if (password != null && !string.IsNullOrEmpty(password.Text) && PasswordStrengthIndicator.Core.Helper.IsPasswordMeetPolicy(password.Text))
            {
                lblErrorMsg.Text = "Password meet password policy!";
            }
            else
            {
                lblErrorMsg.Text = "Password does not meet password policy!";
                e.Cancel = true;
            }
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            FormsAuthentication.SetAuthCookie(RegisterUser.UserName, createPersistentCookie: false);

            string continueUrl = RegisterUser.ContinueDestinationPageUrl;
            if (!OpenAuth.IsLocalUrl(continueUrl))
            {
                continueUrl = "~/";
            }
            Response.Redirect(continueUrl);
        }

        protected void RegisterUser_NextButtonClick(object sender, WizardNavigationEventArgs e)
        {
            //if (e.CurrentStepIndex == "")
            //{

            //}
        }

        protected void RegisterUser_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            TextBox password = ((TextBox)RegisterUser.CreateUserStep.ContentTemplateContainer.FindControl("Password"));

            if (password != null && !string.IsNullOrEmpty(password.Text) && PasswordStrengthIndicator.Core.Helper.IsPasswordMeetPolicy(password.Text))
            {
                lblErrorMsg.Text = "Password meet password policy!";
            }
            else
            {
                lblErrorMsg.Text = "Password does not meet password policy!";
            }
        }
    }
}