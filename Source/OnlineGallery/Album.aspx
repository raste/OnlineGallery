<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Album.aspx.cs" 
Inherits="OnlineGallery.Album" Theme="MainTheme"%>

<%@ Register assembly="Microsoft.Web.GeneratedImage" namespace="Microsoft.Web" tagprefix="cc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width:100%;">

        <cc2:Accordion ID="userAccordion" runat="server" FramesPerSecond="40" 
            RequireOpenedPane="False" SelectedIndex="-1"
        TransitionDuration="1" HeaderCssClass="accordionHeader" 
            HeaderSelectedCssClass="accordionHeader" CssClass="marginsTB">
           <Panes>
               <cc2:AccordionPane ID="AccordionPane1" runat="server">
               <Header>
               <asp:Panel ID="ShowUserPnl" runat="server" Visible="True" CssClass="userInfo">
               <asp:Label ID="lblShowUserInfo" runat="server"  
                Text="Информация за притежателя на албума" ForeColor="#FF9900" Font-Size="Larger"></asp:Label>
                </asp:Panel>
               </Header>
               
               <Content>
                   <div class="userInfo">
                   <table>
                   <tr>
                   <td>
                    <asp:HyperLink ID="hlUserAvatar" runat="server">         
            <cc1:GeneratedImage ID="generatedImgUserImg" runat="server" 
                ImageHandlerUrl="~/GetImageHandler.ashx" CssClass="galleryImage">
            </cc1:GeneratedImage>
            </asp:HyperLink>
           
            <asp:Label ID="lblUserName" runat="server" Text="User name"></asp:Label>
            <br />
            <asp:Label ID="lblUserInfo" runat="server" Text="lblUserInfo"></asp:Label>
                   </td>
                   </tr>
                   </table>
           
            </div>
               </Content>
               
               </cc2:AccordionPane>
           </Panes>
          
        </cc2:Accordion>
      <br />
        <asp:Panel ID="pnlCategoryInfo" runat="server" CssClass="marginsTB">
            <asp:Label ID="lblCatName" runat="server" Text="Category name"></asp:Label>
            <asp:Label ID="lblCatInfo" runat="server" Text="Category Info"></asp:Label>
            <br />
            <br />
        </asp:Panel>
      
  
        <table class="style1" style="border-collapse: collapse; empty-cells: hide">
            <tr>
                <td style="width: 400px;">
      
   
   
   
        <asp:Table ID="tblPagesTop" runat="server">
        </asp:Table>

   
                </td>
                <td>

   
        <div runat="server" id="divSortBy">
        Сортирай по :
        <asp:HyperLink ID="hlSortByDate" runat="server">дата</asp:HyperLink>
&nbsp;<asp:HyperLink ID="hlSortByRating" runat="server">оценка</asp:HyperLink>
&nbsp;<asp:HyperLink ID="hlSortByComments" runat="server">коментари</asp:HyperLink>
&nbsp;<asp:HyperLink ID="hlSortByVisits" runat="server">посещения</asp:HyperLink>
        </div>

                </td>
            </tr>
        </table>

    <asp:Table ID="tblImages" runat="server">
    </asp:Table>
    
        <asp:Table ID="tblPagesBtm" runat="server">
        </asp:Table>
    <br />

</div>
</asp:Content>
