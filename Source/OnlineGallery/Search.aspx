<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" 
Inherits="OnlineGallery.Search" Theme="MainTheme"%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="width:100%;">

        <br />

    <asp:Label ID="lblSearchFor" runat="server" Text="search for :"></asp:Label>
        <br />
    <br />
    <asp:Table ID="tblSearchResults" runat="server" Width="98%">
    </asp:Table>
    <br />

</div>
</asp:Content>
