<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="EditCategory.aspx.cs" 
Inherits="OnlineGallery.EditCategory" Theme="MainTheme"%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="editCategory" style="width:100%">
        <asp:Panel ID="pnlNotifPanel" runat="server" CssClass="notificationDivs" 
            Visible="False">
            <asp:Label ID="lblChangeDone" runat="server" Text="Change successful"></asp:Label>
        </asp:Panel>
        <br />
    <asp:Panel ID="pnlAddCategory" runat="server" CssClass="marginsTB" style="margin-right:10px;">

    
        <table style="width:100%">
            <tr>
                <td style="width: 350px;" valign="top">
                    <br />
                    <asp:BulletedList ID="blListAddCAtegory" runat="server">
                        <asp:ListItem Value="0">До 15 собствени категории може да има всеки потребител.</asp:ListItem>
                        <asp:ListItem Value="1">Име : от 2 до 20 символа.</asp:ListItem>
                        <asp:ListItem Value="0">Описание : от 0 до 1000 символа.</asp:ListItem>
                    </asp:BulletedList>
                </td>
                <td style="border: thin solid #CCCCCC">

        <table class="style1">
            <tr>
                <td align="right">
                    &nbsp;</td>
                <td>
                    Добави категория :</td>
            </tr>
            <tr>
                <td align="right" style="width: 80px;">
                    име :</td>
                <td>
                    <asp:TextBox ID="tbAddCatName" runat="server" ValidationGroup="1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvAddCatName" runat="server" 
                        ControlToValidate="tbAddCatName" ErrorMessage="*" ValidationGroup="1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    описание :</td>
                <td>
                    <asp:TextBox ID="tbAddCatDescr" runat="server" Columns="30" Rows="5" 
                        TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnAddCategory" runat="server" onclick="btnAddCategory_Click" 
                        Text="добави" ValidationGroup="1" />
                </td>
            </tr>
        </table>
        
                    &nbsp;
        
                    <asp:Label ID="lblAddCatError" runat="server" Text="Error" 
            CssClass="errorLabels"></asp:Label>
            
                </td>
            </tr>
        </table>
       
   </asp:Panel>
   
   <br />
   
    <asp:Panel ID="pnlEditCategory" runat="server" CssClass="marginsTB">
        <table style="width:100%">
            <tr>
                <td valign="top" style="border: thin solid #CCCCCC">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Промяна категория :<asp:UpdatePanel 
                        ID="upEdit" runat="server">
                        <ContentTemplate>
                            <table class="style1">
                                <tr>
                                    <td align="right" style="width: 130px;">
                                        категория :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlCategories" runat="server" AutoPostBack="True" 
                                            onselectedindexchanged="ddlCategories_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        ново име :</td>
                                    <td>
                                        <asp:TextBox ID="tbEditCatName" runat="server" ValidationGroup="2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvEditCatName" runat="server" 
                                            ControlToValidate="tbEditCatName" ErrorMessage="*" ValidationGroup="2"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        ново описание :</td>
                                    <td>
                                        <asp:TextBox ID="tbEditCatDescr" runat="server" Columns="30" Rows="5" 
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnEditCat" runat="server" 
                        onclick="btnEditCat_Click" Text="промени" ValidationGroup="2" />
                    &nbsp;<asp:Button ID="btnDelete" runat="server" onclick="btnDelete_Click" 
                        Text="изтрии" ValidationGroup="2" />
                    <br />
                    &nbsp;<asp:Label ID="lblEditCatError" runat="server" CssClass="errorLabels" 
                        Text="Error"></asp:Label>
                </td>
                <td style="width: 350px;" valign="top">
                
                    <asp:BulletedList ID="blListEditCategory" runat="server">
                        <asp:ListItem Value="0">При натискате на &#39;изтрии&#39; се изтрива категорията със всички снимки в нея.</asp:ListItem>
                        <asp:ListItem Value="0">При промяна категория може да се смени само името или описанието, както и двете едновременно.</asp:ListItem>
                        <asp:ListItem Value="0">За да се премахне описанието на категория натиснете &quot;промени&quot; със празно поле за описание.</asp:ListItem>
                    </asp:BulletedList>
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    
    
    
   
    
    </div>
</asp:Content>

