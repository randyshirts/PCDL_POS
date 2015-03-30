'use strict';
app.controller('changeEmailController', ['$location', '$timeout', 'authService', function ($location, $timeout, authService) {

    var self = this;

    self.message = "";
    self.savedSuccessfully = false;
    self.currentUser = authService.authentication.userName;

    self.userInfo =
    {
        currentEmail: self.currentUser,
        newEmail: ""
    }

    self.busyDisabled = true;

    self.updateEmail = function () {

        self.busyDisabled = false;
        authService.changeEmail(self.userInfo).then(function (results) {

            self.busyDisabled = true;
            if (results.data === "success") {
                self.savedSuccessfully = true;
                self.message = "Email successfully changed";
                self.currentUser = authService.authentication.userName;
                self.userInfo.newEmail = "";
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