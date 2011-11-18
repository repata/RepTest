<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>unCCed.com - Log In</title>
    <link href="media/css/base.css" rel="stylesheet" type="text/css" />
    <link href="media/jqui/css/ui-lightness/jquery-ui-1.7.2.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="scripts/jquery-1.3.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="pghdr-bar" style="z-index:99999">
        <div style="height:51px;">
            
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td style="width:200px;padding-left:20px;"><a href="Default.aspx" class="hl_logo"></a></td>
                    <td><asp:Label ID="lbl_id" runat="server" Text=""></asp:Label>pronounced "unkkT"</td>
                    <td align="right" style="padding-right:20px;"><a href="Login.aspx">Log In</a></td>
                </tr>
            </table>
          
           y
        </div>
        <div class="pghdr-bar-shad"></div>
    </div>
    
    <div id="wrap">
    
        <asp:Login ID="Login1" runat="server">
        </asp:Login>
    </div>
    </form>
    git
</body>
</html>
