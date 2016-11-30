<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ASP.NET - jQuery Password Strength Indicator v2.0</title>
    <style type="text/css">
        body
        {
            font-family: Verdana;
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h2>ASP.NET - jQuery Password Strength Indicator</h2>
        <div style="height: 400px">
            <br />
            <asp:Label runat="server" ID="lblPassword"
                AssociatedControlID="txtPassword">Enter Password:</asp:Label>
            <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
            <br />
            <a id="passwordPolicyLink" href="#">Password policy</a>
            <br />
            <br />
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" />

            <br />
            <br />

            <asp:Label ID="ResultLabel" runat="server" Text=""></asp:Label>
        </div>

        <script src="Script/jquery-1.4.4.min.js" type="text/javascript"></script>
        <script src="Script/jquery.password-strength-2.0.min.js" type="text/javascript"></script>
        <script src="Script/jquery.blockUI.js" type="text/javascript"></script>

        <script type="text/javascript">
            $(document).ready(function () {
                var myPlugin = $("#<%=txtPassword.ClientID%>").password_strength({
                    //web site alias if any/xml folder if any/
                    //Page.ResolveUrl(@"~/") return /web sie alias/
                    appFolderXMLPath: "<%= Page.ResolveUrl(@"~/") %>MyXML/",
                    passwordPolicyLinkId: 'passwordPolicyLink'
                });

                $("[id$='btnSubmit']").click(function () {
                    return myPlugin.metReq(); //return true or false
                });
            });

        </script>

    </form>
</body>
</html>
