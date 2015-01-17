<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="EditImage.aspx.cs" 
Inherits="OnlineGallery.EditImage" Theme="MainTheme"%>

<%@ Register assembly="Microsoft.Web.GeneratedImage" namespace="Microsoft.Web" tagprefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width:100%;">

    <table style="border: thin solid #000000; width:100%; background-color: #555555;" 
            class="marginsTB">
    <tr>
    <td>
    <cc1:GeneratedImage ID="giCurrImage" runat="server" ImageAlign="Left" 
            CssClass="Avatar">
    </cc1:GeneratedImage>
        обратно към страницата на картинката :
        <asp:HyperLink ID="hlImageLink" runat="server">кликни тук</asp:HyperLink>
        <br />
    <asp:Label ID="lblDescription" runat="server" Text="Label"></asp:Label>
    </td>
    </tr>
    </table>
    
        <asp:Panel ID="pnlNotification" runat="server" CssClass="notificationDivs" 
            Visible="False">
            <asp:Label ID="lblEditSucces" runat="server" Text="Op success"></asp:Label>
        </asp:Panel>
        
        <table class="marginsTB" style="width: 100%;">
            <tr>
                <td style="width: 320px;" valign="top">
                    <br />
                    <asp:BulletedList ID="blListEditImage" runat="server" style="margin-left: 22px">
                        <asp:ListItem Value="0">Описание от 0 до 1000 символа.</asp:ListItem>
                        <asp:ListItem Value="0">За изтриване на описание оставете полето празно и натиснете &quot;смени описание&quot;. </asp:ListItem>
                    </asp:BulletedList>
                </td>
                <td style="padding: 2px; border: thin solid #C0C0C0;">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;смени категория : <asp:DropDownList ID="ddlCategory" runat="server">
                </asp:DropDownList>
                    <asp:Button ID="btnCategory" runat="server" onclick="btnCategory_Click" Text="смени" />
                    <br />
&nbsp;<asp:Label ID="lblErrorCategory" runat="server" Text="Error" Visible="False" 
                        CssClass="errorLabels"></asp:Label>
                    <table class="style2">
                        <tr>
                            <td align="right">
                                промени описанието :</td>
                            <td>
                <asp:TextBox ID="tbImageInfo" runat="server" Columns="30" Rows="5" 
                    TextMode="MultiLine"></asp:TextBox>
                                <br />
                <asp:Button ID="btnInfo" runat="server" onclick="btnInfo_Click" Text="смени описание" />
                            </td>
                        </tr>
                    </table>
&nbsp;<asp:Label ID="lblInfoError" runat="server" Text="error" Visible="False" 
                        CssClass="errorLabels"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <br />
    <br />
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
    </style>

</asp:Content>

