'use strict';
app.controller('forgotPasswordController', ['$scope', '$location', '$timeout', 'authService', function ($scope, $location, $timeout, authService) {

    $scope.message = "";
    $scope.sentSuccessfully = false;

    $scope.userinfo =
    {
        emailaddress: ""
    }

    $scope.busyDisabled = true;

    $scope.recoverPassword = function () {

        $scope.busyDisabled = false;
        authService.recoverPassword($scope.userinfo).then(function (results) {

            $scope.busyDisabled = true;
            if (results.data === "success") {
                $scope.sentSuccessfully = true;
                $scope.message = "A temporary password was sent to your email address. Open up the email from Play Create Discover and click on the link";
                //startTimer();
            } else {
                $scope.message = results.data;
                $scope.sentSuccessfully = false;
            }

        }, function (error) {
            //alert(error.data.message);
        });
    }

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/login');
        }, 5000);
    }
  
}]);