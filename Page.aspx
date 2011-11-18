<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Page.aspx.cs" Inherits="Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>unCCed - [TITLE]</title>
    <link rel="icon" type="image/png" href="fav-icon32.png">
    <link rel="SHORTCUT ICON" href="fav-icon32.png"/>
    <link href="media/css/base.css" rel="stylesheet" type="text/css" />
    <link href="media/jqui/css/ui-lightness/jquery-ui-1.7.2.custom.css" rel="stylesheet" type="text/css" />
    
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js" type="text/javascript"></script> 
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.7/jquery-ui.min.js" type="text/javascript"></script>

    <script src="scripts/jquery.autogrow.js" type="text/javascript"></script>
    <script src="scripts/uncced.js" type="text/javascript"></script>
    <style type="text/css">
        
    </style>
    
</head>
<body style="margin:0px;">
    <form id="form1" runat="server">
    
<script type="text/javascript">
var msgs = [], checkInt = 10, lastKey = <%=lastCommentKey %>, urlId = '<%=urlId %>', usersName = '<%=usersName %>'
    , defaultCmt = '[Type Your Comment Here]';
    //preload
    (new Image().src='media/images/big_comment_bubble.png');
    
   window.onload = function(){var bb = $('#bigbubble').click(function(e){
            $(this).fadeOut('slow');
        });
        bb.css({'top':$('#footer').offset().top,'left':95});
        bb.show().animate({top:$('#footer').offset().top - bb.height()},{duration:500, 'easing':'linear'});
        }
    
   $(document).ready(function(){
        window.setInterval(checkNew, 10000);
        
        
        $('#cmt-t').autogrow();
        //$('#myname').update(usersName);
        
        $('#calendar').datepicker();
        //$('#cal1').datepicker({showOn:'button', buttonImage:'media/icons/0177-calendar.png', buttonImageOnly: true});
        $('#add-bar-un').val(usersName);
        $('#add-bar-max').click(function(e){
            var c = $('#add-bar-cmt');
            showAdd(c.val());
            c.val(null);
            
        });
        $('#dp1').click(function(e){
            $('#calendar').toggle('slow');
            //$('#calendar').fadeIn('slow');
        });
        $('#rss1').click(function(e){
            var el = $('#rssfeeds');
            var ft = $('#footer');
            var y = ft.offset().top - el.innerHeight();
            var x = ft.width() - el.width()-20;
            //var x = ft.
            //t = 300;
            el.css({'top':y,'position':'absolute','left':x});
            el.toggle();
            
        });
        $('#add-bar-cmt').keyup(function(e){
            if (e.which==13){
                var un = usersName;
                if (usersName.length == 0) {
                    un = prompt('We need your name, nick name, initials... something that people will be able to identify you.');
                    if (un.replace(' ', '').length == 0) {
                        alert('You cannot post a comment without a name.');
                    } else { usersName = un; }
                }
                if (un.length > 0) {
                    var cmt = $('#add-bar-cmt').val();
                    if (cmt.length>0){
                        $('#add-bar-cmt').val(null);
                        add(usersName, cmt);
                    }
                }
                
            }
        }).focus(function(e){
            var t = $(this);
            if (t.val() == defaultCmt) {t.val(null);}
        });
        $('#hl-max').click(function(e){
            var c = $('#add-bar-cmt')
                , cmt = c.val();
            
            showAdd((cmt == defaultCmt ? null : cmt));
            c.val(null);
            
        });
        $('#add-bar-btn').click(function(e){
            var un = $('#add-bar-un').val()
                , cmt = $('#add-bar-cmt').val();
            if (un.length>0 && cmt.length>0) {
                usersName = un;
                $('#add-bar-cmt').val('');
                add(un, cmt);
            }
        });
        
        
        $('#myname').text(usersName);
        
   });
    </script>
    
    
     <div id="header" class="pghdr-bar">
            <div style="height:51px;background: transparent URL(media/images/hd-bg.png) repeat-x;">
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td style="width:200px;padding-left:20px;"><a href="Default.aspx" class="hl_logo"></a></td>
                            <td style="padding-left:16px;line-height:16px;">
                            <div style="overflow:hidden;height:16px;">
                                <asp:Label ID="lbl_title" runat="server" Text="" ForeColor="#FFFFFF" Font-Bold="true"></asp:Label></div>
                                <asp:Label ID="lbl_host" runat="server" Text="" ForeColor="#d7d7d7">Host: undefined</asp:Label></td>
                            <td align="right" style="padding-right:16px;line-height:18px;padding-top:6px;">
                                <a href="javascript:showAdd();" style="color:#f3934b;font-weight:bold;">Add Comment</a> | <a href="javascript:inviteOthers()">Invite Others</a><br /> Options | Log In
                                <a href="javascript:void(0);" onclick="hilitUser('');">Test</a>
                            </td>
                        </tr>
                    </table>
                
            </div>
            <div class="pghdr-bar-shad"></div>
        </div>
    
    
    <div id="wrap">
       
        <div style="padding-bottom:80px;">
            <div class="rtbar">
               
                <div style="">
                    <div>Your name: <span id="myname"></span></div>
                    <div style="margin-bottom:6px;"><asp:Label ID="lbl_usercount" runat="server" Text="Label"></asp:Label></div>
                    <div id="unccers" style="margin-bottom:6px;">
                        <asp:Literal ID="lit_unccers" runat="server"></asp:Literal>
                    </div>
                    <div id="calendar" class="textsm"></div>
                    <div id="polls">
                        <asp:Literal ID="lit_poll" runat="server"></asp:Literal>
                    </div>
                    <!--<div style="margin-bottom:6px;margin-top:12px;height:200px;border:solid 1px gray;padding:1px;margin-right:8px;"><div style="background-color:#FFFF99;height:200px;text-align:center;"><br /><br /><br /><br /><br /><br />AD PLACEMENT</div></div>-->
                </div>
            </div>
            <div class="content">
                <div style="margin-bottom:12px;height:65px;border:solid 1px gray;padding:1px;"><div style="background-color:#FFFF99;height:60px;text-align:center;"><br /><br />AD PLACEMENT</div></div>
                
                <div id="newcmt-top" style="margin-bottom:16px;">
                    <textarea id="cmt-1" name="cmt-t" style="width:450px;" rows="1">Type your comment here...</textarea>
                    <button id="cmt-t-save" value="value" class="ui-state-default ui-corner-all">Submit</button>
                </div>
                
                <div id="msgs"><asp:Literal ID="lit_msgs" runat="server"></asp:Literal></div>
                <asp:Literal ID="lit_origmsgs" runat="server"></asp:Literal>
                
                
            </div>
            
            
    <asp:Label ID="lbl_test" runat="server" Text=""></asp:Label>
        </div>
        
    </div>
    <div id="footer" style="background-color:Silver;z-index:999999;">
            
            <table width="100%">
                <tr>
                <td style="width:50px;text-align:center"><img src="media/images/comment_bubble.png" /></td>
                <td style="width:145px;">Type in your comment:<br /><span style="font-size:11px;color:Gray;">(hitting Enter key will submit)</span></td>
                <td style="width:24px;">
                    <img src="media/icons/404-right.png" style="margin-bottom:4px;" /><br />
                    <img src="media/icons/404-right.png" />
                </td>
                <td style="width:380px;"><textarea id="add-bar-cmt" name="add-bar-cmt" class="text ui-widget-content ui-corner-all" style="width:360px;height:42px;font-size:11px;">[Type Your Comment Here]</textarea></td>
                <td><a href="javascript:void(0);" id="hl-max" style="display:block;height:18px;padding-left:22px;background:transparent URL(media/icons/arrow_out.png) no-repeat;">Give me more room to type.</a></td>
                <td align="right" style="padding-right:12px;">
                    <a id="A1" href="javascript:void(0);" onclick="createPoll();"><img src="media/icons/0165-chart.png" style="border:0;margin-right:4px;" /></a>
                    <a id="dp1" href="javascript:void(0);"><img src="media/icons/0177-calendar.png" style="border:0;margin-right:4px;" /></a>
                    <a id="rss1" href="javascript:void(0);"><img src="media/icons/comment_rss_32.png" style="border:0;margin-right:4px;" /></a>
                    
                </td>
                </tr>
            </table>
            
    </div>
    <div id="rssfeeds" style="display:none;border:solid 1px #b4dcfe;border-bottom:0;height:20px;padding:4px;background-color:#d2eafe">
        RSS FEED:
        <a id="fhl_xml" runat="server" href="#"><img src="media/images/rss.png" style="border:0;vertical-align:middle;" /></a>
        <a id="fhl_google" runat="server" href="http://fusion.google.com/add?source=atgs&feedurl=http%3A//uncced.com/rss.ashx"><img src="http://buttons.googlesyndication.com/fusion/add.gif" border="0" alt="Add to Google" style="vertical-align:middle;"></a>
        <a id="fhl_yahoo" runat="server" href="http://add.my.yahoo.com/rss?url=http://www.rssicongallery.com/rss.php"><img src="http://us.i1.yimg.com/us.yimg.com/i/us/my/addtomyyahoo4.gif" alt="Add to Yahoo" width="91" height="17" border="0" style="vertical-align:middle;"></a>
        <a id="fhl_aol" runat="server" href="http://feeds.my.aol.com/add.jsp?url=http://www.rssicongallery.com/rss.php"><img src="http://myfeeds.aolcdn.com/vis/myaol_cta1.gif" alt="Add to AOL" width="63" height="14" border="0" style="vertical-align:middle;"></a>
        <a id="fhl_msn" runat="server" href="http://my.msn.com/addtomymsn.armx?id=rss&amp;ut=http://www.rssicongallery.com/rss.php"><img src="http://sc.msn.com/c/rss/rss_mymsn.gif" alt="Add to MSN" width="71" height="14" border="0" style="vertical-align:middle;"></a>
    </div>
    <div id="bigbubble" style="display:none;width:330px;height:287px;position:fixed;background:transparent URL(media/images/big_comment_bubble.png) no-repeat;cursor:pointer;">
        <div style="width:248px;height:135px;margin-left:42px;margin-top:44px">
            Welcome!
            <p>It is so easy to start commenting.  No registration required, just start typing in the comment field at the bottom of this page.</p>
        </div>
    </div>
    </form>
</body>
</html>
