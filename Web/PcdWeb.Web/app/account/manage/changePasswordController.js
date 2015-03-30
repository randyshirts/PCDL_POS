'use strict';
app.controller('changePasswordController', ['$location', '$timeout', 'authService', function ($location, $timeout, authService) {

    var self = this;

    self.message = "";
    self.savedSuccessfully = false;
    self.currentUser = authService.authentication.userName;

    self.userInfo =
    {
        emailaddress: self.currentUser,
        password: "",
        passwordrepeat: ""
    }

    self.busyDisabled = true;

    self.updatePassword = function () {

        self.busyDisabled = false;
        authService.changePassword(self.userInfo).then(function (results) {

            self.busyDisabled = true;
            if (results.data === "success") {
                self.savedSuccessfully = true;
                self.message = "Password change successful";
                //startTimer();
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