define(['plugins/router'],
    function (router) {

        return new function () {
            var self = this;

            self.router = router;

            self.languages = abp.localization.languages;
            self.currentLanguage = abp.localization.currentLanguage;

            self.menu = abp.nav.menus.MainMenu;

            self.activate = function () {
                router.map([
                    { route: '', title: abp.localization.localize('HomePage', 'PcdWeb'), moduleId: 'viewmodels/home', nav: true, menuName: 'Home' },
                    { route: 'login', title: abp.localization.localize('Login', 'PcdWeb'), moduleId: 'viewmodels/login', nav: true, menuName: 'Login' },
                    { route: 'about', title: abp.localization.localize('About', 'PcdWeb'), moduleId: 'viewmodels/about', nav: true, menuName: 'About' }
                ]).buildNavigationModel();

                return self.router.activate();
            };
        };
    });