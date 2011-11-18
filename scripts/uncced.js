function dateDiff(secs) {

    diff = new Date();
    var d1 = new Date('12/1/2009 12:00:00 AM');
    var d2 = new Date();
    var diff2 = (d2.getTime() - d1.getTime());
    var tsecs = Math.floor((diff2 / 1000) - secs);


    timediff = tsecs * 1000;

    weeks = Math.floor(timediff / (1000 * 60 * 60 * 24 * 7));
    timediff -= weeks * (1000 * 60 * 60 * 24 * 7);

    days = Math.floor(timediff / (1000 * 60 * 60 * 24));
    timediff -= days * (1000 * 60 * 60 * 24);

    hours = Math.floor(timediff / (1000 * 60 * 60));
    timediff -= hours * (1000 * 60 * 60);

    mins = Math.floor(timediff / (1000 * 60));
    timediff -= mins * (1000 * 60);

    secs = Math.floor(timediff / 1000);
    timediff -= secs * 1000;
    if (weeks > 0) { return weeks + (mins == 1 ? ' last week' : ' weeks ago'); }
    else if (days > 0) { return days + (mins == 1 ? ' yesterday' : ' days ago'); }
    else if (hours > 0) { return hours + (hours == 1 ? ' hr' : ' hrs') + ' ago '; }
    else if (mins > 0) { return mins + (mins == 1 ? ' min' : ' mins') + ' ago '; }
    else { return secs + ' secs ago '; }

}
function renderMsgs() {
    var html = '';
    for (var i = 0; i < msgs.length; i++) {
        html += formatMsg(msgs[i]);
    }

    var elm = document.getElementById('msgs');
    elm.innerHTML = html;
}


function scrollWin() {
    $('html, body').animate({
        scrollTop: 0//$('#wrap').offset().top
    }, 1500);
}
function renderNewMsgs() {
    var x = msgs.length;
    if (x > 0) {
        for (var i = x - 1; i >= 0; i--) {
            var h = $(formatMsg(msgs[i]));
            $('#msgs').prepend(h.hide());
            h.show('slow');
        }
        scrollWin();
    }

}
function formatMsg(obj) {
    var u = obj.isUser ? 'un-star' : 'un-anon';
    var cmt_shell = obj.isMine ? 'cmt-shell-mine' : 'cmt-shell';
    var html = '<div>';
    html += '<div style="float:left;width:120px;"><div class="' + u + '">' + obj.name + '</div><div id="t-' + obj.time + '" class="cmt-time">' + dateDiff(obj.time) + '</div></div>';
    html += '<div class="' + cmt_shell + ' cmt-shell-lf">' + obj.comment + '</div>';
    html += '</div>';
    return html;
}
//window.onload = renderMsgs;

function updateTimes() {

    $('.cmt-time').each(function(i) {
        var secs = $(this).attr('id').split('-')[1];
        $(this).text(dateDiff(secs));
    });
}

function checkNew() {
    updateTimes();
    $.post('services/get_comments.ashx?u=', { u: urlId, lk: lastKey }, function(d) {
        if (d.success) {
            if (d.lastcommentid > 0)
                lastKey = d.lastcommentid;
            //add new comments to view
            msgs = d.comments;
            renderNewMsgs();
        }
        else {
            //$('#div_so_ret').html("<span style=\"color:red;\">" + d.msg + "</span>");
            alert("failed");
        }
    }, 'json');
}

function doInvite(emails, dlg) {
    $.post('services/send_invites.ashx?u=', { u: urlId, emls: emails }, function(d) {
        if (d.success) {
            console.log('all good, return list with page users to display');

            if (dlg) {
                $(dlg).dialog('destroy').remove();
            }

        }
        else {
            //$('#div_so_ret').html("<span style=\"color:red;\">" + d.msg + "</span>");
            alert("failed");
        }
    }, 'json');
}

function add(n, cmt, dlg) {
    $.post('services/add_comment.ashx?u=', { u: urlId, un: n, cmt: cmt, lk: lastKey }, function(d) {
        if (d.success) {

            lastKey = d.lastcommentid;
            //add new comments to view
            msgs = d.comments;

            if (dlg) {
                $(dlg).dialog('destroy').remove();
            }
            renderNewMsgs();
        }
        else {
            //$('#div_so_ret').html("<span style=\"color:red;\">" + d.msg + "</span>");
            alert("failed");
        }
    }, 'json');
}
function checkEmail(inputvalue) {
    var pattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
    if (pattern.test(inputvalue)) {
        return true;
    } else {
        return false;
    }
}

function showAdd(cmt) {
    var html = '<div title="Post a New Comment"><table width="100%" style="font-size:12px;"><tr><td style="width:75px;">Type in your comment:</td><td><textarea id="add_cmt" name="add_cmt" style="width:98%;height:150px;" class="text ui-widget-content ui-corner-all"  /></td></tr><tr><td colspan="2"><div id="div_add_ret">&nbsp;</div></td></tr></table></div>';

    $(html).dialog({
        modal: true, bigframe: true, resizable: false, width: 450
                , buttons: {
                    'Post Comment': function() {
                        var cmt = $('#add_cmt').val(), un = usersName;

                        if (usersName.length == 0) {
                            un = prompt('We need your name, nick name, initials... something that people will be able to identify you.');
                            if (un.replace(' ', '').length == 0) {
                                alert('You cannot post a comment without a name.');
                            } else { usersName = un; }
                        }
                        if (un.length > 0 && cmt.length > 0) {
                            t = $(this);
                            $('#div_add_ret').html("<img src=\"media/images/spinner2.gif\" align=\"absmiddle\"/><span style=\"color:red;\">Posting comment...</span>");
                            usersName = un;
                            add(un, cmt, t);
                        }
                        else { $('#div_add_ret').html("<span style=\"color:red;\">Incorrect Values</span>"); }
                    }
                    , 'Cancel': function() {
                        $(this).dialog('destroy').remove();

                    }
                }
    });
    if (cmt) { var c = $('#add_cmt'); c.val(cmt); c.focus(); }
}
function inviteOthers() {
    var html = '<div title="Invite Others to Comment"><table width="100%" style="font-size:12px;"><tr><td width="200px">Enter email addresses <em>1 email per line</em> in the box to the right. Each person will be sent an email with a link.</td><td>Email Addresses (1 per line):<br/><textarea id="inv_emls" name="inv_emls" style="width:98%;height:300px;" class="text ui-widget-content ui-corner-all"  /></td></tr><tr><td colspan="2"><div id="div_inv_ret">&nbsp;</div></td></tr></table></div>';

    $(html).dialog({
        modal: true, bigframe: true, resizable: false, width: 450
                , buttons: {
                    'Send Invites': function() {

                        var err = [];
                        var emls = $('#inv_emls').val().split('\n');
                        var gemls = [];
                        for (var i = 0; i < emls.length; i++) {
                            var eml = emls[i].replace(' ', '');
                            if (eml.length > 0) {
                                if (!checkEmail(eml)) { err.push(eml); }
                                else { gemls.push(eml); }
                            }
                        }

                        if (err.length > 0) {
                            alert('bad formatted emails: \n\n' + err.join('\n'));
                        }
                        else if (gemls.length > 0) {
                            t = $(this);
                            doInvite(gemls.join(','), t);
                        }

                    }
                    , 'Cancel': function() {
                        $(this).dialog('destroy').remove();

                    }
                }
    });

}

function hilitUser(uid) {
    $('div[id=usr-'+uid+']').animate({ opacity: 'toggle' }, 250).animate({ opacity: 'toggle' }, 250).animate({ opacity: 'toggle' }, 250).animate({ opacity: 'toggle' }, 250).animate({'color':'red'},1000);
}

function createPoll() {
    var html = '<div title="Create a Poll"><table width="100%" style="font-size:12px;"><tr><td width="75px">Question:</td><td><textarea id="npoll_q" name="npoll_q" style="width:98%;height:50px;" class="text ui-widget-content ui-corner-all"  /></td></tr><tr><td width="75px">Answers:<br />(1 per line)</td><td><textarea id="npoll_a" name="npoll_a" style="width:98%;height:120px;" class="text ui-widget-content ui-corner-all"  /></td></tr><tr><td colspan="2"><div id="div_inv_ret">&nbsp;</div></td></tr></table></div>';

    $(html).dialog({
        modal: true, bigframe: true, resizable: false, width: 450
                , buttons: {
                    'Create Poll': function() {
                        var q = $('#npoll_q').val()
                            , a = $('#npoll_a').val();
                        addpoll(q, a, $(this));

                    }
                    , 'Cancel': function() {
                        $(this).dialog('destroy').remove();

                    }
                }
    });
}

function addpoll(q,a, dlg) {
    $.post('services/polls/add_poll.ashx?u=', { u: urlId, q: q, a: a }, function(d) {
        if (d.success) {

            renderPoll(d.pollId, q, a);
            if (dlg) {
                $(dlg).dialog('destroy').remove();
            }

        }
        else {
            //$('#div_so_ret').html("<span style=\"color:red;\">" + d.msg + "</span>");
            alert("failed");
        }
    }, 'json');
}

function renderPoll(id, q, a) {
    var html = '<div id="poll-' + id + '" class="poll">';
    html += '<p>' + q + '</p>';
    html += '<div id="poll-' + id + '-a"><ul>';
    var an = a.split('\n');
    for (var i = 0; i < an.length; i++) {
        html += '<li><input type="radio" name="polla-' + id + '" value="' + (i+1) + '" /> ' + an[i] + '</li>';
    }
    html += '</ul></div><div class="btns"><button type="button" onclick="answerPoll(' + id + ')" value="Submit">Submit</button></div></div>';
    $('#polls').prepend(html);
}
function renderPollResults(obj) { 
    // obj = {answers
}
function answerPoll(id) {
    var a = $("input[@name='polla-" + id + "']:checked").val();
    if (a !== undefined) {
        $.post('services/polls/do_answer.ashx?u=', { u: urlId, k: id, a: a }, function(d) {
            if (d.success) {

                var html = '<table width="100%"><tr><td colspan="2">Total Votes: ' + d.totalVotes + '</td></tr>'
                    , r = d.results;
                for (var i = 0; i < d.results.length; i++) {
                    var pct = (r[i].pct * 100).toFixed(0)
                    html += '<tr><td>' + r[i].answer + '</td><td rowspan="2">' + pct + '% (' + r[i].votes + ')</td></tr>';
                    html += '<tr><td><div style="background-color:silver;width:' + pct + '%;height:4px;"></div></td></tr>';
                }
                html += '</table>';
                $('#poll-' + id + '-a').html(html);

            }
            else {
                //$('#div_so_ret').html("<span style=\"color:red;\">" + d.msg + "</span>");
                alert("failed");
            }
        }, 'json');    
    }
    
}