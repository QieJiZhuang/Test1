<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="Scripts/jquery-3.4.1.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button runat="server"  Text="获取供应信息" ID="btn1" OnClick="btn1_Click" />
            <asp:Button runat="server"  Text="获取需求信息" ID="btn2" OnClick="btn2_Click" />


            <input type="button" onclick="aaa()" value="dainji1" />
        </div>
    </form>
</body>
</html>
