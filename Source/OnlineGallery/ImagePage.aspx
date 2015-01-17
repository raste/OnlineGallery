<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ImagePage.aspx.cs" 
Inherits="OnlineGallery.ImagePage" Theme="MainTheme"%>

<%@ Register assembly="Microsoft.Web.GeneratedImage" namespace="Microsoft.Web" tagprefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width:100%;">

        <div style="text-align:center;">
        <div class="imageDiv">
            <div class="insideDiv">
            
                  <asp:HyperLink ID="hlToImage" runat="server" Target="_blank">
                  <cc1:GeneratedImage ID="giCurrImg" runat="server">
                  </cc1:GeneratedImage>
                  </asp:HyperLink>
            </div>

        </div>


      
 </div>
 
        <table class="style2" style="border-collapse: collapse; empty-cells: hide">
            <tr>
                <td valign="top">
 
                    <table style="display: inline; border-collapse: collapse; empty-cells: hide">
                        <tr>
                            <td>
                                <asp:Image ID="imgComments" runat="server" Height="20px" 
                                    ImageUrl="~/images/messages.png" ToolTip="коментари" />
                            </td>
                            <td>
                                <asp:Label ID="lblComments" runat="server" Text="Comments"></asp:Label>
                            </td>
                            <td>
&nbsp;<asp:Image ID="imgVisits" runat="server" Height="20px" ImageUrl="~/images/visits.png" 
                                    ToolTip="посещени€" />
                            </td>
                            <td>
                                <asp:Label ID="lblVisits" runat="server" Text="Visits"></asp:Label>
                            </td>
                            <td>
&nbsp;<asp:Image ID="imgLiked" runat="server" Height="20px" ImageUrl="~/images/rate_plus.png" 
                                    ToolTip="харесана" />
                            </td>
                            <td>
                                <asp:Label ID="lblLiked" runat="server" Text="Liked"></asp:Label>
                            </td>
                            <td>
&nbsp;<asp:Image ID="imgDisliked" runat="server" Height="20px" ImageUrl="~/images/rate_minus.png" 
                                    ToolTip="не харесана" />
                            </td>
                            <td>
                                <asp:Label ID="lblDisliked" runat="server" Text="Disliked"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <br />
        <asp:Label ID="lblUploadedBy" runat="server" Text="Uploaded by"></asp:Label>
        <asp:HyperLink ID="hlToImgsUser" runat="server">image`s owner</asp:HyperLink>
        <asp:Label ID="lblDateUploaded" runat="server" Text="Date uploaded"></asp:Label>
        <asp:Label ID="lblImgCategory" runat="server" Text="image category"></asp:Label>
        <asp:HyperLink ID="hlImgCategory" runat="server">Category</asp:HyperLink>
 
 
    <asp:Label ID="lblImgDescription" runat="server" Text="image description" ForeColor="#FF9900"></asp:Label>
                </td>
                <td valign="top" align="right" style="width: 220px;">
 
        <table runat="server" id="tblRateImg" style="display: inline;">
            <tr>
                <td class="style4">
                    ќцени :
                </td>
                <td>
                    <asp:ImageButton ID="btnLikeImg" runat="server" 
                        ImageUrl="~/images/rate_plus.png" onclick="btnLikeImg_Click" 
                        Height="30px" ToolTip="харесва ми" />
                </td>
                <td>
                    <asp:ImageButton ID="btnDislikeImg" runat="server" 
                        ImageUrl="~/images/rate_minus.png" onclick="btnDislikeImg_Click" 
                        Height="30px" ToolTip="не ми харесва" 
                         />
                </td>
            </tr>
        </table>
 &nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="imgPrev" runat="server" Height="30px" 
                        ImageUrl="~/images/img_previous.png" ToolTip="предишна картинка" />
                    <asp:ImageButton ID="imgNext" runat="server" Height="30px" 
                        ImageUrl="~/images/img_next.png" ToolTip="следваща картинка" />
&nbsp;&nbsp;&nbsp;<asp:Panel ID="pnlEdit" runat="server" Visible="False" HorizontalAlign="Right" 
            CssClass="marginsTop">
            <asp:HyperLink ID="hlEditImage" runat="server">промени</asp:HyperLink>
            &nbsp;<asp:Button ID="btnDelete" runat="server" onclick="btnDelete_Click" 
                Text="изтрии" />
        </asp:Panel>
 
 
                </td>
            </tr>
        </table>
 
        <asp:Panel ID="pnlNotification" runat="server" Visible="False" 
            CssClass="notificationDivs">
            <asp:Label ID="lblOperSuccess" runat="server" Text="Success"></asp:Label>
        </asp:Panel>
 
    <asp:Table ID="tblComments" runat="server" Width="98%" CssClass="marginsTB" 
            CellSpacing="0">
    </asp:Table>
    
    <asp:Panel ID="pnlAddComment" runat="server">
        <asp:TextBox ID="tbComment" runat="server" Columns="50" Rows="5" 
            TextMode="MultiLine" CssClass="marginsBottom"></asp:TextBox>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button 
            ID="btnAddComment" runat="server" onclick="btnAddComment_Click" 
            Text="ƒобави коментар" />
        &nbsp;<br />
        <asp:Label ID="lblError" runat="server" Text="Error" Visible="False" 
            CssClass="errorLabels"></asp:Label>
        <br />
    </asp:Panel>
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
        .style4
        {
            width: 53px;
        }
    </style>

</asp:Content>

