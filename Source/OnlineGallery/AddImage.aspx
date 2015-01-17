<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddImage.aspx.cs" 
Inherits="OnlineGallery.AddImage" Theme="MainTheme"%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width:100%;">

    <br />

    <asp:Panel ID="pnlNotification" runat="server" CssClass="notificationDivs" 
        Visible="False">
        <asp:Label ID="lblInfo" runat="server" Text="Info"></asp:Label>
    </asp:Panel>
    <table class="marginsTB" style="width: 99%;">
        <tr>
            <td style="width: 300px;" valign="top">
     
                <asp:BulletedList ID="blListAddCategory" runat="server" 
                    style="margin-bottom: 16px; margin-top: 13px;">
                    <asp:ListItem Value="0">Височината и широчината на картинката трябва да са по-големи от 499 .</asp:ListItem>
                    <asp:ListItem Value="0">Максимален размер 2мб за картинка.</asp:ListItem>
                    <asp:ListItem Value="0">Описанието не е задължително, от 0 до 1000 символа.</asp:ListItem>
                    <asp:ListItem Value="0">След добавяне на картинка, ще може да се променя нейната категория и описание.</asp:ListItem>
                </asp:BulletedList>
            </td>
            <td style="border: thin solid #CCCCCC" valign="top">
    <table style="width: 100%">
        <tr>
            <td>
                &nbsp;</td>
            <td>
                Добави картинка</td>
        </tr>
        <tr>
            <td align="right" style="width: 130px;">
                местоположение :</td>
            <td>
                <asp:FileUpload ID="fuImage" runat="server" />
                <asp:RequiredFieldValidator ID="rfvFuImage" runat="server" 
                    ControlToValidate="fuImage" ErrorMessage="*" ValidationGroup="5"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                категория :
            </td>
            <td>
                <asp:DropDownList ID="ddlCategories" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                описание :</td>
            <td>
                <asp:TextBox ID="tbDescription" runat="server" Columns="30" Rows="5" 
                    TextMode="MultiLine"></asp:TextBox>
                <asp:ImageButton ID="btnAddImage" runat="server" 
                    ImageUrl="~/images/uploadBtn.png" ImageAlign="Top" 
                    onclick="btnAddImage_Click" ToolTip="Качи" ValidationGroup="5" />
            </td>
        </tr>
        </table>
            
    <asp:Label ID="lblError" runat="server" Text="Error" Visible="False" CssClass="errorLabels"></asp:Label>
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
    </style>

</asp:Content>

