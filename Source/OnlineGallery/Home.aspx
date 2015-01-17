<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" 
Inherits="OnlineGallery.Home" Theme="MainTheme"%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="width:100%;">

    <asp:Panel ID="pnlInfo" runat="server" Font-Size="Large" ForeColor="#FF9933" 
        HorizontalAlign="Center">
        <br />
        <span style="font-size: larger">
        Здравейте и добре дошли в Онлайн Фотография!
        </span>
        <br />
        Пожелаваме Ви приятно изкарване с уникалните снимки качени в сайта.<br />
        <br />
    </asp:Panel>

<br />
    <asp:Panel ID="pnlLastImages" runat="server" HorizontalAlign="Center">
        <asp:Label ID="lblLastImages" runat="server" 
    Text="Последни качени картинки" Font-Size="Large" ForeColor="#FF9933"></asp:Label>
        <br />
        <br />
        <asp:Table ID="tblLastImages" runat="server" CellPadding="5" CellSpacing="0">
        </asp:Table>
    </asp:Panel>
<br />

</div>

</asp:Content>
