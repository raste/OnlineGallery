<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" 
Inherits="OnlineGallery.EditUser" Theme="MainTheme"%>

<%@ Register assembly="Microsoft.Web.GeneratedImage" namespace="Microsoft.Web" tagprefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="editUser" style="width:100%;">

    <br />
    <table class="marginsTB" 
        style="border: thin solid #000000; width: 100%; background-color: #555555;">
        <tr>
            <td>
            <div style="text-align:center">Вашият профил в момента.</div>
                <cc1:GeneratedImage ID="giCurrUser" runat="server" CssClass="Avatar" 
                    ImageAlign="Left" ImageHandlerUrl="~/GetImageHandler.ashx">
                </cc1:GeneratedImage>
                <asp:Label ID="lblUserInfo" runat="server" Text="User Info"></asp:Label>
            </td>
        </tr>
    </table>

    <div runat="server" id="notifDiv" class="notificationDivs">
        <hr />
    <asp:Label ID="lblOpSucc" runat="server" Text="Success"></asp:Label>
        <hr />
    </div>
    <table class="style2">
        <tr>
            <td valign="top" style="padding: 1px">
                <br />
                Информация при промяна на полета :<asp:BulletedList ID="blList" runat="server">
                    <asp:ListItem Value="0">При натискане на &quot;смени&quot; при непопълнено поле, ще се изтрие информацията ако има такава.</asp:ListItem>
                    <asp:ListItem Value="0">Информацията в полетата не може да започва и завършва с &quot; &quot;.</asp:ListItem>
                    <asp:ListItem Value="0">След промяна на информация, тя ще се обнови в полето с информацията за профил.</asp:ListItem>
                </asp:BulletedList>
            </td>
            <td style="padding: 2px; border: thin solid #C0C0C0; width: 450px;">
    
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                нова парола :
                <asp:TextBox ID="tbPassword" runat="server" Columns="20" TextMode="Password"></asp:TextBox>
&nbsp;<br />
    
    &nbsp;
                повтори паролата :
                <asp:TextBox ID="tbRepPass" runat="server" Columns="20" TextMode="Password"></asp:TextBox>
                <asp:Button ID="btnNewPassword" runat="server" onclick="btnNewPassword_Click" 
                    Text="смени" />
                <br />
                &nbsp;
                <asp:Label ID="lblErrorPass" runat="server" Text="Error" Visible="False" 
                    CssClass="errorLabels"></asp:Label>
                <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                име :
                <asp:TextBox ID="tbName" runat="server" Columns="20"></asp:TextBox>
                <asp:Button ID="btnNewName" runat="server" onclick="btnNewName_Click" 
                    Text="смени" />
                <br />
                &nbsp;
                <asp:Label ID="lblErrorName" runat="server" Text="Error" Visible="False" 
                    CssClass="errorLabels"></asp:Label>
                <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                град :
                <asp:TextBox ID="tbCity" runat="server" Columns="20"></asp:TextBox>
                <asp:Button ID="btnNewCity" runat="server" onclick="btnNewCity_Click" 
                    Text="смени" />
                <br />
                &nbsp;
                <asp:Label ID="lblErrorCity" runat="server" Text="Error" Visible="False" 
                    CssClass="errorLabels"></asp:Label>
                <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                поща :
                <asp:TextBox ID="tbEmail" runat="server" Columns="30"></asp:TextBox>
                <asp:Button ID="btnNewMail" runat="server" onclick="btnNewMail_Click" 
                    Text="смени" />
                <br />
                &nbsp;
                <asp:Label ID="lblErrorMail" runat="server" Text="Error" Visible="False" 
                    CssClass="errorLabels"></asp:Label>
                <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                аватар :
                <asp:FileUpload ID="fuImage" runat="server" />
                <asp:Button ID="btnNewAvatar" runat="server" onclick="btnNewAvatar_Click" 
                    Text="смени" />
                <br />
                &nbsp;
                <asp:Label ID="lblErrorAvatar" runat="server" Text="Error" Visible="False" 
                    CssClass="errorLabels"></asp:Label>
                <br />
                <table class="style2">
                    <tr>
                        <td class="style3">
                друга информация :</td>
                        <td>
                <asp:TextBox ID="tbMoreInfo" runat="server" Columns="30" Rows="5" 
                    TextMode="MultiLine"></asp:TextBox>
                            <br />
                <asp:Button ID="btnNewInfo" runat="server" onclick="btnNewInfo_Click" 
                    Text="смени" />
                        </td>
                    </tr>
                </table>
                &nbsp;&nbsp;<asp:Label ID="lblErrorMoreInfo" runat="server" Text="Error" Visible="False" 
                    CssClass="errorLabels"></asp:Label>
                <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; рожденна дата :
                <asp:DropDownList ID="ddlDay" runat="server">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlMonth" runat="server">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem Value="1">Януари</asp:ListItem>
                    <asp:ListItem Value="2">Февруари</asp:ListItem>
                    <asp:ListItem Value="3">Март</asp:ListItem>
                    <asp:ListItem Value="4">Април</asp:ListItem>
                    <asp:ListItem Value="5">Май</asp:ListItem>
                    <asp:ListItem Value="6">Юни</asp:ListItem>
                    <asp:ListItem Value="7">Юли</asp:ListItem>
                    <asp:ListItem Value="8">Август</asp:ListItem>
                    <asp:ListItem Value="9">Септември</asp:ListItem>
                    <asp:ListItem Value="10">Октомври</asp:ListItem>
                    <asp:ListItem Value="11">Ноември</asp:ListItem>
                    <asp:ListItem Value="12">Декември</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlYear" runat="server">
                </asp:DropDownList>
                <asp:Button ID="btnNewBirthDate" runat="server" onclick="btnNewBirthDate_Click" 
                    Text="смени" />
                <br />
                &nbsp;
                <asp:Label ID="lblErrorBirthDate" runat="server" Text="Error" Visible="False" 
                    CssClass="errorLabels"></asp:Label>
                <br />

            </td>
        </tr>
    </table>
    <br />

</div>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">

    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 100%;
        }
        .style3
        {
            width: 134px;
        }
    </style>

</asp:Content>

