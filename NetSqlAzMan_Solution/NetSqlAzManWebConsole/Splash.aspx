<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Splash.aspx.cs" Inherits="NetSqlAzManWebConsole.Splash" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<link rel="shortcut icon" href="favicon.ico" type="image/x-icon" />
<meta http-equiv="Page-Enter" content="revealTrans(Duration=3.0,Transition=0)" />
<meta http-equiv="Page-Exit" content="revealTrans(Duration=1.0,Transition=0)" />
<script type="text/javascript" src="javascript/common.js" language="javascript"></script>
<title>NetSqlAzMan Web Console</title>
</head>
<body>
    <script type="text/javascript" language="javascript">
    <!-- 
        doHourglass();
    -->
    </script>
    <form id="form1" runat="server">
    <div class="splash">
    <center>
        <asp:Image ID="imgSplash" runat="server" ImageUrl="~/images/SplashBitmap2.jpg" />
        <br />
        <asp:Image ID="progressBar" runat="server" ImageUrl="~/images/WorkingSB.gif" />
    </center>
    </div>
    </form>
    <script language="javascript" type="text/javascript">
        <!--
            setTimeout("window.location = 'Splash.aspx?Redirect=true';",2000);
        -->
    </script>
</body>
</html>
