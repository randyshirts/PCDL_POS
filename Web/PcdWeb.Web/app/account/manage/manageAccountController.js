'use strict';
app.controller('manageAccountController', [
    '$location', '$timeout', 'authService', function($location, $timeout, authService) {

        //var self = this;
        //$scope.accountInfo = [{"firstName":"Randy","lastName":"Shirts"}];
        //self.accountInfo = [];
        //self.message = "";
        //self.savedSuccessfully = false;

        //self.currentUser = authService.authentication.userName;

        //self.userinfo =
        //{
        //    firstname: "",
        //    lastname: "",
        //    emailaddress: self.currentUser,
        //    password: "",
        //    passwordrepeat: ""
        //}

        //self.showInfo = false;
        //self.busyDisabled = true;



        //self.init = function () {

        //    self.busyDisabled = false;
        //    authService.getUserInfo(self.userinfo).then(function (results) {

        //        self.busyDisabled = true;
        //        if (results.data.message === "success") {
        //            self.accountInfo = results.data;
        //            self.showInfo = true;
        //        } else {
        //            self.message = results.data.message;
        //            self.savedSuccessfully = false;
        //        }

        //    }, function (error) {
        //        //alert(error.data.message);
        //    });
        //}

        //self.init();
    }]);