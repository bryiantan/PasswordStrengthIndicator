<%
lblMsg = ""
If (Request.Form("btnSubmit") <> "") Then
    If (CheckPasswordPolicy(Request.Form("txtPassword"))) Then
        lblMsg ="Password confront password policy."
    Else
        lblMsg = "Password does not confront password policy."
    End If
End If

Public Function CheckPasswordPolicy(ByVal strInput)
        SET passwordPolicy = New RegExp

        SET xmlObj = Server.CreateObject("Microsoft.XMLDOM")
        xmlObj.async = False
        xmlObj.load(Server.MapPath("PasswordPolicy.xml"))

        isValid = False

        If xmlObj.parseError.errorCode <> 0 Then
            'error
        End If

        SET xmlNode = xmlObj.getElementsByTagName("Password")

        Dim passwordReg


        If (xmlNode(0).selectSingleNode("maxConsecutiveRepeatedChars").text > 0) Then
            passwordReg = passwordReg &  "(?!.*\s)^(?:(?!([0-9a-zA-Z" & xmlNode(0).selectSingleNode("specialChars").text & "]+)" & _
                          "\1{" & xmlNode(0).selectSingleNode("maxConsecutiveRepeatedChars").text & ",}).)*$"
        ELSE
            passwordReg = passwordReg &  "(?!.*\s)[0-9a-zA-Z" & xmlNode(0).selectSingleNode("specialChars").text & "]*$"
        End If

        passwordPolicy.pattern = passwordReg

        passwordPolicy.Global = True
        passwordPolicy.IgnoreCase = True

        SET xmlObj = Nothing
        SET xmlNC = Nothing

        isValid = passwordPolicy.Test(strInput)

        CheckPasswordPolicy = isValid

    End Function

%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>ASP.NET - jQuery Password Strength Indicator</title>
    <style type="text/css">
        body
        {
            font-family: Verdana;
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <h2>ASP.NET - jQuery Password Strength Indicator</h2>

    <form action="Password_Strength_Indicator.asp" method="post">
        <div style="height: 400px">
            <br />
            <label for="txtPassword">Enter Password:</label>
            <input id="txtPassword" name="txtPassword" type="text" />
            <br />
            <a id="passwordPolicy" href="#">Password policy</a>
            <br />
            <br />
            <input id="btnSubmit" name="btnSubmit" type="submit" value="Submit" />
            <br />
            <div><%= lblMsg %></div>
        </div>
    </form>

    <script src="Script/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="Script/jquery.password-strength.js" type="text/javascript"></script>
    <script src="Script/jquery.blockUI.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var myPlugin = $("[id='txtPassword']").password_strength({
                //web site alias if any/xml folder if any/
                appFolderXMLPath: '/Password_Strength_Indicator/MyXML/',
                passwordPolicyLinkId: 'passwordPolicy'
            });

            $("[id='btnSubmit']").click(function () {
                return myPlugin.metReq(); //return true or false
            });
        });
    </script>
</body>
</html>



