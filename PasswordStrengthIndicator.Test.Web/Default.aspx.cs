using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSubmit.Click += new EventHandler(btnSubmit_Click);
    }

    void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtPassword.Text) && PasswordStrengthIndicator.Core.Helper.IsPasswordMeetPolicy(txtPassword.Text))
        {
            ResultLabel.Text = "Password meet password policy!";
        }
        else
        {
            ResultLabel.Text = "Password does not meet password policy!";
        }
    }
}
