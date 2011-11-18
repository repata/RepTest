<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>unCCed</title>
    <link rel="icon" type="image/png" href="fav-icon32.png">
    <link rel="SHORTCUT ICON" href="fav-icon32.png"/>
    <link href="media/css/base.css" rel="stylesheet" type="text/css" />
    <link href="media/jqui/css/ui-lightness/jquery-ui-1.7.2.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="media/css/mbTooltip.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="scripts/jquery.dropshadow.js" type="text/javascript"></script>

    <script src="scripts/jquery.timers.js" type="text/javascript"></script>
    <script src="scripts/mbTooltip.pack.js" type="text/javascript"></script>
    <script type="text/javascript">
        

        $(document).ready(function() {
            $("img[id^='opt']").mbTooltip({
                opacity: .7,       //opacity
                wait: 500,           //before show
                cssClass: "black",  // default = default
                timePerWord: 70,      //time to show in milliseconds per word
                hasArrow: false, 		// if you whant a little arrow on the corner
                hasShadow: true,
                imgPath: "media/images/",
                ancor: "mouse", //"parent"  you can ancor the tooltip to the mouse position or at the bottom of the element
                shadowColor: "black", //the color of the shadow
                mb_fade: 200 
            });
            //            $("img[id^='opt']").each(function(i) { 
            //                $(this).mbTool
            //            });
        });
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
                    <td align="right" style="padding-right:20px;"><a href="Login.aspx">Log In</a> | <a href="Register.aspx">Register</a></td>
                </tr>
            </table>
          
           
        </div>
        <div class="pghdr-bar-shad"></div>
    </div>
    
    <div id="wrap">
    
    <table align="center">
        <tr>
            <td style="width:350px;padding-right:20px;" valign="top">
                <div class="ui-dialog ui-widget ui-widget-content ui-corner-all" style="width:350px;">
                    <div class="ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix"><span class="ui-dialog-title">Get unCCed in 3 Simple Steps</span></div>
                    <div style="margin:4px;">
                        <div style="margin-bottom:16px;">
                            <span style="font-size:14pt;font-weight:bold;">Step 1: </span> Get a page link.
                        </div>
                        
                        <div style="margin-bottom:16px;">
                            <span style="font-size:14pt;font-weight:bold;">Step 2: </span> Send link to friends, co-workers, etc.
                        </div>
                        
                        <div style="margin-bottom:30px;">
                            <span style="font-size:14pt;font-weight:bold;">Step 3: </span> Plan, discuss, talk smack…it's your world (or at least page!)
                        </div>
                        
                        <div style="font-weight:bold;margin-bottom:12px;">OK, We Can Do It in 1 Simple Step</div>
                        <div style="margin-bottom:16px;">
                            <span style="font-size:14pt;font-weight:bold;">Step 1: </span> If you get caught in an email chain, simply <em>Reply All</em>, but add <strong>uncced@repata.com</strong> 
                            in the TO or CC field.  We automatically create a page link, copy the original contents to the page and then send everyone an email with the new link.  
                        </div>
                    </div>
                </div>
            
            
            </td>
            <td style="width:350px" valign="top">
                <table>
                <tr><td colspan="2" style="padding-bottom:8px;">
                    <span style="color:#339900;font-size:14pt;margin-bottom:8px;">Get Started Now for Free!</span><br />
                        <em>(no accounts required)</em>
                </td></tr>
                <tr>
                    <td style="width:70px;">Your Email:</td>
                    <td><asp:TextBox ID="tb_email" runat="server" Width="250px" ValidationGroup="create"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ErrorMessage="Invalid Email." ControlToValidate="tb_email" Display="Dynamic"  ValidationGroup="create"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required" Display="Dynamic" ControlToValidate="tb_email" ValidationGroup="create"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Title:</td>
                    <td><asp:TextBox ID="tb_title" runat="server" Width="250px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" Display="Dynamic" ControlToValidate="tb_title" ValidationGroup="create"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr><td colspan="2">
                <div style="font-weight:bold;margin-top:12px;margin-bottom:4px;"><a href="javascript:void(0);" onclick="$('#options').toggle();">Page Setup Options:</a> <span style="color:Gray;font-size:11px;" onclick="$('#options').toggle();">(click to view)</span></div>
                <div id="options" style="display:none;">
                    <ul class="list1">
                        <li>Page Password: <asp:TextBox ID="tb_pw" runat="server" MaxLength="32" ValidationGroup="create"></asp:TextBox>
                            <img src="media/icons/help.png" id="opt-2" style="cursor:pointer" title="This provides a layer" />
                        </li>
                        <li><asp:CheckBox ID="chk_requirereg" runat="server" Text="Only Allow Validated Users" />
                            <img src="media/icons/help.png" id="opt-0" style="cursor:pointer" title="Validated users have created at least basic accounts and have validated the account by clicking on the validation link we send to their email address. This ensures you can identify who is commenting or viewing." /></li>
                        <li>
                            <asp:CheckBox ID="chk_inviteonly" runat="server" Text="Only Allow People I Personally Invite" />
                            <img src="media/icons/help.png" id="opt-1" style="cursor:pointer" title="This provides a layer" />
                        </li>
                        <li>
                            <asp:CheckBox ID="chk_showcalendar" runat="server" Text="Display a Calendar" />
                            <img src="media/icons/help.png" id="Img1" style="cursor:pointer" title="This option simply displays a calendar on the page to ease date references." />
                        </li>
                    </ul>
                    <div>
                        
                    </div>
                     <div>
                        
                    </div>
                    <a href='OptionsExplained.aspx#validusers'>Learn More</a> about all the options...
                    
                </div>
                
                </td></tr>
                <tr>
                    <td colspan="2" style="padding-top:12px;">
                        <asp:Button ID="btn_create" runat="server" Text="Create unCCed Page" CssClass="ui-state-default ui-corner-all" ValidationGroup="create" 
                            onclick="btn_create_Click" />
                    </td>
                </tr>
                <tr><td colspan="2" style="padding-top:24px;">
                    Can't find your email with the link?  Type in your email address and we'll send you all your active unCCed's you are apart of.<br /><br />
                    <asp:TextBox ID="tb_findem" runat="server" Width="250px" ValidationGroup="findem"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationGroup="findem"
                            ErrorMessage="Invalid Email." ControlToValidate="tb_findem" Display="Dynamic" 
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required" Display="Dynamic" ControlToValidate="tb_findem" ValidationGroup="findem"></asp:RequiredFieldValidator>
                    <div style="margin-top:8px;">
                        <asp:Button ID="btn_findem" runat="server" Text="Send Me My Links" CssClass="ui-state-default ui-corner-all" ValidationGroup="findem" 
                            onclick="btn_findem_Click" /><br />
                        <asp:Label ID="lbl_finem_msg" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </td></tr>
                </table>
            
            </td>
        </tr>
        <tr>
            <td style="width:350px;padding-right:20px;" valign="top">
            
            </td>
            <td style="width:350px" valign="top"><table>
                <tr><td colspan="2">
                   
                </td></tr>
                
                
                <tr><td colspan="2" style="padding-top:0;">
                    <div style="color:#339900;font-size:14pt;margin-bottom:8px;">Your Recent Pages:</div>
                
                    <asp:Literal ID="lit_open" runat="server"></asp:Literal>
                </td></tr>
                
            </table></td>
        </tr>
    </table>
    
         <br /><br />
         
         Features:<br />
         <ul>
         <li>allow someone to reply to all on a chain mail and automatically setup a page and email a link to all. Simply Reply All and add getit@unCCed.com and we'll do the rest.</li>
         <li>allow users to put in date available for Plan Event</li>
         <li>allow users to "check off" in or out for Plan Event</li>
    </ul>
         <br /><br />
    
        <asp:Label ID="lbl_debug" runat="server" Text="Label"></asp:Label>
    
    </div>
    
    
    
    </form>
</body>
</html>
