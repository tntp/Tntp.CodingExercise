<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="Default.aspx.cs" Inherits="Tntp.CodingExercise._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script  type="text/javascript">
        var x;
        function Highlight(obj) {
            x = obj.style.backgroundColor;
            obj.style.backgroundColor = "#3791c3";
            obj.style.color = "White";
        }
        function UnHighlight(obj) {
            obj.style.backgroundColor = x;
            obj.style.color = "Black";
        }
    </script>

    <div id="outer">
    <div class="inner">
        <asp:Label runat="server" ID="lblerror" ForeColor="Red" />
        <h4>These are lists of developers notable for their contributions to software, their contributions and awards.
        </h4>
        <table style="margin: 20px auto; margin-bottom:20px;">
            <tr>
                <td colspan="4" style="text-align:center">
                    <h3>Migration & Database Info</h3>
                </td>
                </tr>
            <tr>
                <td>
                    <strong>Number of total developers in the database:</strong>
                </td>
                <td style="padding-right:10px;">
                    <asp:Label runat="server" ID="Developer_total" />
                </td>
                 <td>
                    <strong>Number of new added developers into the database:</strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Developer_added" />
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Number of total contributions in the database:</strong>
                </td>
                <td style="padding-right:10px;">
                    <asp:Label runat="server" ID="Contrib_total" />
                </td>
                <td>
                    <strong>Number of new added contributions into the database:</strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Contrib_added" />
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Number of total awards in the database:</strong>
                </td>
                <td style="padding-right:10px;">
                    <asp:Label runat="server" ID="Award_total" />
                </td>
               <td>
                    <strong>Number of new added awards into the database:</strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Award_added" />
                </td>
            </tr>
        </table>
        <h2>Developers</h2>
         <asp:GridView ID="gv_developers" runat="Server" AutoGenerateColumns="False" BorderWidth="1" BorderStyle="None" 
                DataKeyNames="ID" CellPadding="4" style="width:100%" OnRowDataBound="gv_RowDataBound"
                HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="Middle" OnSorting="gv_Sorting" 
                AllowSorting="true" ForeColor="#333333" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" Visible="false" />
                    <asp:TemplateField SortExpression="FirstName">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="FirstName" runat="server" CommandArgument="FirstName" CommandName="Sort">
                           First Name <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("FirstName") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="aka">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="aka" runat="server" CommandArgument="aka" CommandName="Sort">
                            aka <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                           <%# Eval("aka") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField SortExpression="Title">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="Title" runat="server" CommandArgument="Title" CommandName="Sort">
                            Title <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                           <%# Eval("Title") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="LastName">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="LastName" runat="server" CommandArgument="LastName" CommandName="Sort">
                            Last Name <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                           <%# Eval("LastName") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="BirthDateTime">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="BirthDateTime" runat="server" CommandArgument="BirthDateTime" CommandName="Sort">
                           Birth <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                           <asp:label id="lblDate" runat="server" text='<%# Eval("BirthDateTime", "{0:MM/dd/yyyy hh:mm tt}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="DeathDateTime">
                    <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="DeathDateTime" runat="server" CommandArgument="DeathDateTime" CommandName="Sort">
                            Death <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                            <asp:label id="lblDate" runat="server" text='<%# Eval("DeathDateTime", "{0:MM/dd/yyyy hh:mm tt}") %>' /> 
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#f2f2f2" ForeColor="#333333" VerticalAlign="Middle" HorizontalAlign="Left" />
                            <PagerStyle BackColor="#f2f2f2" CssClass="pagerStyle" ForeColor="#005882" Font-Size="14pt" VerticalAlign="Middle" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="White" Font-Bold="True" VerticalAlign="Middle" HorizontalAlign="Center" />
                            <AlternatingRowStyle BackColor="White" VerticalAlign="Middle" HorizontalAlign="Left"/>
            </asp:GridView>

         <h2>Contributions</h2>
         <asp:GridView ID="gv_contribs" runat="Server" AutoGenerateColumns="False" BorderWidth="1" BorderStyle="None" 
                DataKeyNames="ID" CellPadding="4" style="width:100%" OnRowDataBound="gv_RowDataBound"
                HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="Middle" OnSorting="gv_Sorting" 
                AllowSorting="true" ForeColor="#333333" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" Visible="false" />
                    <asp:TemplateField SortExpression="FirstName">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="FirstName" runat="server" CommandArgument="FirstName" CommandName="Sort">
                           First Name <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("FirstName") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="LastName">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="LastName" runat="server" CommandArgument="LastName" CommandName="Sort">
                            Last Name <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                           <%# Eval("LastName") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="BirthDateTime">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="BirthDateTime" runat="server" CommandArgument="BirthDateTime" CommandName="Sort">
                           Birth <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                           <asp:label id="lblDate" runat="server" text='<%# Eval("BirthDateTime", "{0:MM/dd/yyyy hh:mm tt}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="ContribName">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="ContribName" runat="server" CommandArgument="ContribName" CommandName="Sort">
                            Contribution <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                           <%# Eval("ContribName") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#f2f2f2" ForeColor="#333333" VerticalAlign="Middle" HorizontalAlign="Left" />
                            <PagerStyle BackColor="#f2f2f2" CssClass="pagerStyle" ForeColor="#005882" Font-Size="14pt" VerticalAlign="Middle" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="White" Font-Bold="True" VerticalAlign="Middle" HorizontalAlign="Center" />
                            <AlternatingRowStyle BackColor="White" VerticalAlign="Middle" HorizontalAlign="Left"/>
            </asp:GridView>

        <h2>Awards</h2>
         <asp:GridView ID="gv_awards" runat="Server" AutoGenerateColumns="False" BorderWidth="1" BorderStyle="None" 
                DataKeyNames="ID" CellPadding="4" style="width:100%" OnRowDataBound="gv_RowDataBound"
                HeaderStyle-HorizontalAlign="left" RowStyle-VerticalAlign="Middle" OnSorting="gv_Sorting" 
                AllowSorting="true" ForeColor="#333333" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" Visible="false" />
                    <asp:TemplateField SortExpression="FirstName">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="FirstName" runat="server" CommandArgument="FirstName" CommandName="Sort">
                           First Name <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("FirstName") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="LastName">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="LastName" runat="server" CommandArgument="LastName" CommandName="Sort">
                            Last Name <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                           <%# Eval("LastName") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="BirthDateTime">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="BirthDateTime" runat="server" CommandArgument="BirthDateTime" CommandName="Sort">
                           Birth <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                           <asp:label id="lblDate" runat="server" text='<%# Eval("BirthDateTime", "{0:MM/dd/yyyy hh:mm tt}") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="AwardName">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="AwardName" runat="server" CommandArgument="AwardName" CommandName="Sort">
                            Award <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                           <%# Eval("AwardName") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="Year">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="Year" runat="server" CommandArgument="Year" CommandName="Sort">
                            Year <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                           <%# Eval("Year") %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField SortExpression="GivenBy">
                        <HeaderTemplate>
                        <asp:LinkButton CssClass="Sort_header" ID="GivenBy" runat="server" CommandArgument="GivenBy" CommandName="Sort">
                            GivenBy <asp:Image runat="server" ImageUrl="~/Img/Sort_header.gif" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                        <ItemTemplate>
                           <%# Eval("GivenBy") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#f2f2f2" ForeColor="#333333" VerticalAlign="Middle" HorizontalAlign="Left" />
                            <PagerStyle BackColor="#f2f2f2" CssClass="pagerStyle" ForeColor="#005882" Font-Size="14pt" VerticalAlign="Middle" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="White" Font-Bold="True" VerticalAlign="Middle" HorizontalAlign="Center" />
                            <AlternatingRowStyle BackColor="White" VerticalAlign="Middle" HorizontalAlign="Left"/>
            </asp:GridView>

    </div>
        </div>


</asp:Content>
