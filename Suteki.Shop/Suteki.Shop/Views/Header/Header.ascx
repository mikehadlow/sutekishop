<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HeaderViewData>" %>

        <div class="header">
        
            <div class="logo">

                <div class="rightText">
                    <p>
                        <%= Html.Mailto(Model.Email, Model.Email)%><br /><%= Model.PhoneNumber%>
                    </p>                
                </div>
                
                <h1>
                    <%= Model.Title%>
                </h1>
                
            </div>
            
            <div>
				<% Html.RenderAction<MenuController>(c => c.MainMenu()); %>
            </div>
            
        </div>
