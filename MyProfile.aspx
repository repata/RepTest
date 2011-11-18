<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MyProfile.aspx.cs" Inherits="MyProfile" %>

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
            <td>Name: </td>
            <td>
                <asp:TextBox ID="tb_name" runat="server" MaxLength="32"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Email: </td>
            <td>
                <asp:TextBox ID="tb_email" runat="server" MaxLength="128" Width="350"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btn_save" runat="server" Text="Update Profile" /></td>
        </tr>
    </table>
    
    </div>
    </form>
</body>
</html>
