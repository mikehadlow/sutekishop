<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HeaderViewData>" %>

        <div class="footer">
            <div class="rightText login">
                <p>
                    <%= Html.LoginLink() %> |
                    <%= Html.LoginStatus() %>
                </p>                
            </div>
                
            <p>
                <%= Model.Copyright %> |
                <%= Html.ActionLink<SiteMapController>(c => c.Index(), "Site Map") %>
            </p>
        </div><!--/footer-->
