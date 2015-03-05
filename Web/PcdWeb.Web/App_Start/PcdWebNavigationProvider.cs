using Abp.Application.Navigation;
using Abp.Localization;

namespace PcdWeb.Web
{
    /// <summary>
    /// This class defines menus for the application.
    /// It uses ABP's menu system.
    /// When you add menu items here, they are automatically appear in durandal application.
    /// See App/Main/views/layout.cshtml and App/Main/viewmodels/layout.js to know how to render menu.
    /// </summary>
    public class PcdWebNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        "Home",
                        new LocalizableString("HomePage", PcdWebConsts.WebLocalizationSourceName),
                        url: "#",
                        icon: "fa fa-home"
                        )
                ).AddItem(
                    new MenuItemDefinition(
                        "About",
                        new LocalizableString("About", PcdWebConsts.WebLocalizationSourceName),
                        url: "#about",
                        icon: "fa fa-info"
                        )
                ).AddItem(
                    new MenuItemDefinition(
                        "Login",
                        new LocalizableString("Login", PcdWebConsts.WebLocalizationSourceName),
                        url: "#login",
                        icon: "fa fa-lock"
                        )
                )
                ;
        }
    }
}
