
var app = angular.module('AngularAuthApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', 'ng-currency', 'smart-table']);

app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/app/main/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/app/account/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/app/account/signup.html"
    });

    $routeProvider.when("/existinguser", {
        controller: "existinguserController",
        templateUrl: "/app/account/existinguser.html"
    });

    $routeProvider.when("/orders", {
        controller: "ordersController",
        templateUrl: "/app/views/orders.html"
    });

    $routeProvider.when("/refresh", {
        controller: "refreshController",
        templateUrl: "/app/account/refresh.html"
    });

    $routeProvider.when("/tokens", {
        controller: "tokensManagerController",
        templateUrl: "/app/account/tokens.html"
    });

    $routeProvider.when("/associate", {
        controller: "associateController",
        templateUrl: "/app/account/associate.html"
    });

    $routeProvider.when("/emailSent", {
        controller: "emailSentController",
        templateUrl: "/app/account/emailSent.html"
    });

    $routeProvider.when("/emailConfirmed", {
        controller: "emailConfirmedController",
        templateUrl: "/app/account/emailConfirmed.html"
    });

    $routeProvider.when("/signupChoice", {
        controller: "signupChoiceController",
        templateUrl: "/app/account/signupChoice.html"
    });

    $routeProvider.when("/forgotPassword", {
        controller: "forgotPasswordController",
        templateUrl: "/app/account/forgotPassword.html"
    });

    $routeProvider.when("/resetPassword", {
        controller: "resetPasswordController",
        templateUrl: "/app/account/resetPassword.html"
    });

    $routeProvider.when("/manageAccount", {
        controller: "manageAccountController",
        templateUrl: "/app/account/manage/manageAccount.html"
    });

    $routeProvider.when("/addItemsMain", {
        controller: "addItemsMainController",
        templateUrl: "/app/addItems/addItemsMain.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });

});

var serviceBase = 'http://localhost:61754/';
//var serviceBase = 'http://ngauthenticationapi.azurewebsites.net/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);


