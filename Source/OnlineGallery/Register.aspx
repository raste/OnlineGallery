<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs"
 Inherits="OnlineGallery.Register" Theme="MainTheme" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="registerImage" style="width:100%;">

    <asp:Panel ID="pnlRegister" runat="server">
        <table class="style2">
            <tr>
                <td valign="top">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Онлайн Фотография - лесно и безплатно!<br />
                    <br />
                    &nbsp;Тук можеш да получиш оценка и коментар за твойте снимки или просто да ги 
                    покажеш на приятели.<br />
                    <br />
                    &nbsp;Освен това ще можеш да коментираш и оценяваш снимките на другите потребители.<br />
                    <br />
                    &nbsp;И ще получиш :<br />
                    &nbsp;&nbsp; - Място за твоите снимки<br />
                    &nbsp;&nbsp; - Удобен организатор на снимки<br />
                    &nbsp;&nbsp;
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Тука твоите снимки изглеждат добре!<br />
                    &nbsp;&nbsp;&nbsp; Ако фотографията ти е хоби. Добре дошъл!<br />
                    <br />
                    &nbsp;Ако вече имаш съществуващ акаунт Влез чрез формата в горния десен ъгъл.</td>
                <td style="width: 400px;" valign="top">
                    <table class="style1" style="border: thin solid #CCCCCC">
                        <tr>
                            <td align="right" style="width: 150px;">
                                потребител :</td>
                            <td>
                                <asp:TextBox ID="tbUsername" runat="server" ValidationGroup="10"></asp:TextBox>
                                *<asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                                    ControlToValidate="tbUsername" ValidationGroup="10"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                парола :</td>
                            <td>
                                <asp:TextBox ID="tbPassword" runat="server" TextMode="Password" 
                                    ValidationGroup="10" Width="148px"></asp:TextBox>
                                *<asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                    ControlToValidate="tbPassword" ValidationGroup="10"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                повтори парола :</td>
                            <td>
                                <asp:TextBox ID="tbRepPassword" runat="server" TextMode="Password" 
                                    ValidationGroup="10" Width="148px"></asp:TextBox>
                                *<asp:RequiredFieldValidator ID="rfvRepPassword" runat="server" 
                                    ControlToValidate="tbRepPassword" ValidationGroup="10"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                email :</td>
                            <td>
                                <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                име :</td>
                            <td>
                                <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                град :</td>
                            <td>
                                <asp:TextBox ID="tbCity" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                картинка :</td>
                            <td>
                                <asp:FileUpload ID="fuImage" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                дата на раждане :</td>
                            <td>
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
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                друга информация :</td>
                            <td>
                                <asp:TextBox ID="tbOtherInfo" runat="server" Columns="28" Rows="5" 
                                    TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                <asp:Button ID="tbRegister" runat="server" onclick="tbRegister_Click" 
                                    Text="Създай профил" ValidationGroup="10" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="style2">
            <tr>
                <td style="color: Red;" valign="top">
                    * Полетата със звездички са задължителни !
                    <br />
                    &nbsp;Останалите полета може да не се попълват.<br />
                    &nbsp;Промяна на информация е възможна след регистрация. </td>
                <td style="width: 400px;" valign="top">
                    <asp:Label ID="lblError" runat="server" ForeColor="#CC3300" Text="Error" 
                        Visible="False"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>

    <asp:Panel ID="pnlRegSucc" runat="server" Visible="False" HorizontalAlign="Center">
        Честито
        <asp:Label ID="lblName" runat="server" Text="Name"></asp:Label>
        &nbsp;! Регистрирахте се успешно, можете да влезете в профила си.
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
    </style>


</asp:Content>

