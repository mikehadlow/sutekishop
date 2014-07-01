<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Shop.Master" Inherits="Suteki.Shop.ViewPage<HowDidYouHearOfUsViewModel>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">

<h1>How Did You Hear Of Us Report</h1>
    <div>
        <p>Select Date range:</p>
    <% using(Html.BeginForm()) { %>

        <div class="contentLeftColumn">
            <%= this.TextBox(x => x.From).Label("From (dd/mm/yyyy)") %>
            <%= this.TextBox(x => x.To).Label("To (dd/mm/yyyy)") %>

            <input type="submit" value="Update" />
        </div>

    <% } %>

        <div class="contentRightColumn">
        <table>
            <tr>
                <th>Option</th>
                <th class="number">Count</th>
            </tr>
    <% foreach (var line in Model.Lines)
       { %> 
            <tr>
                <td><%= line.Option %></td>
                <td class="number"><%= line.Count %></td>
            </tr>
    <% } %>

        </table>
        </div>
    </div>
</asp:Content>
