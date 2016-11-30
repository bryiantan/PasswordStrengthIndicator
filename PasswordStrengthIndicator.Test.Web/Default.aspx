<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div style="height: 400px">
        <br />
        <asp:Label runat="server" ID="lblPassword"
            AssociatedControlID="txtPassword">Enter Password:</asp:Label>
        <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
        <br />
        <a id="passwordPolicy" href="#">Password policy</a>
        <br />
        <br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" />

        <br />
        <br />

        <asp:Label ID="ResultLabel" runat="server" Text=""></asp:Label>

    </div>

    <script src="Script/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="Script/jquery.password-strength-2.0.min.js" type="text/javascript"></script>
    <script src="Script/jquery.blockUI.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var myPlugin = $("#<%=txtPassword.ClientID%>").password_strength({
                //web site alias if any/xml folder if any/
                appFolderXMLPath: "<%= Page.ResolveUrl(@"~/") %>",
                passwordPolicyLinkId: 'passwordPolicy'
            });

            $("#<%=btnSubmit.ClientID%>").click(function () {
                return myPlugin.metReq(); //return true or false
            });
        });

    </script>

</asp:Content>

