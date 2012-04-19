<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PostageResultViewData>" %>

    <tr>
        <td>Postage</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td class="number"><%= Model.PostageTotal%></td>
        <td>&nbsp;</td>
    </tr>

    <tr>
        <td>(<%= Model.Description %>)</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
    </tr>

    <tr class="total">
        <td>Total With Postage</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
        <td class="number"><%= Model.TotalWithPostage%></td>
        <td>&nbsp;</td>
    </tr>
