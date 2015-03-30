'use strict';
app.controller('resetPasswordController', ['$scope', '$location', '$timeout', 'authService', function ($scope, $location, $timeout, authService) {

    $scope.message = "";
    $scope.savedSuccessfully = false;

    $scope.userinfo =
    {
        emailaddress: "",
        password: "",
        passwordrepeat: ""
    }

    $scope.busyDisabled = true;

    $scope.updatePassword = function () {

        $scope.busyDisabled = false;
        authService.resetPassword($scope.userinfo).then(function (results) {

            $scope.busyDisabled = true;
            if (results.data === "success") {
                $scope.savedSuccessfully = true;
                $scope.message = "Password reset successful. You will be redirected to the login page in 2 seconds";
                startTimer();
            } else {
                $scope.message = results.data;
                $scope.savedSuccessfully = false;
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