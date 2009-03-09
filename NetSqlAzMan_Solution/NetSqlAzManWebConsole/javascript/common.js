// MODAL DIALOG BEGIN //
// http://aspadvice.com/blogs/joteke/archive/2006/08/05/20331.aspx
// Used to define the method to cause postback 
function openDialog(page, offset)
{
    doHourglass();
    // Giving the arguments to the dialog
    var width = 800;
    var height = 600;
    var dummy = 0;
    if (offset!=null)
    {
        dummy = 30*offset;
    }
    window.open(page,'','modal=yes,width='+ width +',height='+ height +',scrollbars=yes, resizable=no,top='+((screen.availHeight/2)-(height/2) + dummy)+',left='+((screen.availWidth/2)-(width/2) + dummy)+')');
    doHourglassOff();
}
function openDialogWithArguments(page, argumentName, argumentIdValue, offset)
{
    var url = page + '?' + argumentName + '=' + document.getElementById(argumentIdValue).value;
    openDialog(url, 2);    

}
function closewindow(doPostBack) 
{
    if (doPostBack)
    {
        opener.doPostBack();
    }
    window.close();
}
function showMessage(msg, title, type)
{
    if (msg!='')
    {
        window.alert(msg);
    }
}

function confirmClient(msg)
{
    var x = window.confirm(msg);
    if (x)
    {
        return true;
    }
    else
    {
        return false;
    }
}

function resizeMainPanel()
{
    var winW = 630, winH = 460;
    var dummy = 15;
    if (parseInt(navigator.appVersion)>3) {
     if (navigator.appName=="Netscape") {
      winW = window.innerWidth;
      winH = window.innerHeight;
     }
     if (navigator.appName.indexOf("Microsoft")!=-1) {
      winW = document.documentElement.clientWidth;
      winH = document.documentElement.clientHeight;
     }
    }
    document.getElementById('mainPanel').style.height = winH - (document.getElementById('headerPanel').clientHeight + document.getElementById('footerPanel').clientHeight*2 + dummy) + 'px';
    document.innerHTML = document.innerHTML;
    //document.getElementById('mainPanel').innerHTML = document.getElementById('mainPanel').innerHTML; //force refresh for IE6
}

function showHidePanel(panel, show)
{
    if (show)
        document.getElementById(panel).style.visibility = 'visible';
    else
        document.getElementById(panel).style.visibility = 'hidden';
}

function copyToClipBoard(textarea)
{
    window.clipboardData.setData('Text', document.getElementById(textarea).innerText);
    window.alert('Copied into the Clipboard.');
}
function doHourglass()
{
  document.body.style.cursor = 'wait';
}
function doHourglassOff()
{
  document.body.style.cursor = 'auto';
}