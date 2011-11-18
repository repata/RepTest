<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>Email:</td>
                <td>
                    <asp:TextBox ID="tb_email" runat="server" MaxLength="128"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ErrorMessage="Required." ControlToValidate="tb_email"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ErrorMessage="Invalid Email." ControlToValidate="tb_email"></asp:RegularExpressionValidator>
                    </td>
            </tr>
            <tr>
                <td>First Name:</td>
                <td><asp:TextBox ID="tb_firstname" runat="server" MaxLength="32"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="Required." ControlToValidate="tb_firstname"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td>Last Name:</td>
                <td><asp:TextBox ID="tb_lastname" runat="server" MaxLength="32"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="Required." ControlToValidate="tb_lastname"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td>Password:</td>
                <td><asp:TextBox ID="tb_password1" runat="server" MaxLength="20" TextMode="Password"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic" ErrorMessage="Required." ControlToValidate="tb_password1"></asp:RequiredFieldValidator></td>
            </tr>
            
            <tr>
                <td>Retype Password:</td>
                <td><asp:TextBox ID="tb_password2" runat="server" MaxLength="20" TextMode="Password"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords do not match." Display="Dynamic" ControlToValidate="tb_password2" ControlToCompare="tb_password1"></asp:CompareValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="No Workie" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
