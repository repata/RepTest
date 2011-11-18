<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PageCofirm.aspx.cs" Inherits="PageCofirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="media/css/base.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.3.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        
    </style>
    <script type="text/javascript">
    
    </script>
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
          
           
        </div>
        <div class="pghdr-bar-shad"></div>
    </div>
    
    <div id="wrap">
    
    
        
    <table align="center" style="width:650px;">
        <tr>
            <td style="width:250px;" valign="top">
            
            <p><strong>Here is the link to your new page!</strong></p>
            
            <p>
                <asp:Label ID="lbl_newlink" runat="server" CssClass="newlink" Text=""></asp:Label>
            </p>
            (<em>an email has been sent to you as a reminder</em>)
            </td>
            <td style="width:250px;" valign="top">
                <p>A quick question to help us improve unCCed:</p>
                
                <p>General Purpose for Page?</p>
                <asp:RadioButtonList ID="rbl_purpose" runat="server">
                    <asp:ListItem Value="NO" Selected="True">Prefer not to say</asp:ListItem>
                    <asp:ListItem Value="PE">Plan Event</asp:ListItem>
                    <asp:ListItem Value="DT">Discuss Topic</asp:ListItem>
                    <asp:ListItem Value="TS">Talk Smack</asp:ListItem>
                    <asp:ListItem Value="SE">Something Else</asp:ListItem>
                </asp:RadioButtonList>
                
                <div style="font-weight:bold;margin-top:12px;">Page Setup Options<br /><span style="color:Gray;font-size:11px;">(setup your page with additional options)</span></div>
                <div id="options"></div>
                
                <p>
                    <asp:Button ID="btn_continue" runat="server" 
                        Text="Continue to your page &gt;&gt;" onclick="btn_continue_Click" />
                </p>
                
                
            </td>
        </tr>
    </table>
    
    
    </div>
    </form>
</body>
</html>
