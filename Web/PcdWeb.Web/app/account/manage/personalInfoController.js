'use strict';
app.controller('personalInfoController', [
    '$scope', '$location', '$timeout', 'authService', function ($scope, $location, $timeout, authService) {

        var self = this;
        //$scope.accountInfo = [{"firstName":"Randy","lastName":"Shirts"}];
        self.accountInfo = [];
        self.message = "";
        self.savedSuccessfully = false;
        self.editEnabled = false;
        self.currentUser = authService.authentication.userName;

        self.userinfo =
        {
            firstname: "",
            lastname: "",
            emailaddress: self.currentUser,
            password: "",
            passwordrepeat: ""
        }

        self.showInfo = false;
        self.busyDisabled = true;
        self.saveBusy = true;


        self.init = function () {

            self.busyDisabled = false;
            authService.getUserInfo(self.userinfo).then(function (results) {

                self.busyDisabled = true;
                if (results.data.message === "success") {
                    self.accountInfo = results.data;
                    self.showInfo = true;
                } else {
                    self.message = results.data.message;
                    if (results.data.message === "Session Timed Out") {
                        self.message = "Session Timed Out";
                        authService.logOut();
                        startTimer();
                    }
                    self.savedSuccessfully = false;
                }

            }, function (error) {
                //alert(error.data.message);
            });
        }

        self.init();

        self.savePersonalInfo = function () {

            self.saveBusy = false;
            authService.savePersonalInfo(self.accountInfo).then(function (results) {

                self.saveBusy = true;
                if (results.data === "success") {
                    self.message = "Saved successfully";
                    self.savedSuccessfully = true;
                } else {
                    self.message = results.data;
                    self.savedSuccessfully = false;
                }

            }, function (error) {
                //alert(error.data.message);
            });
        }

        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path('/login');
            }, 2000);
        }

    }]);