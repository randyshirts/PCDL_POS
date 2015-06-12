'use strict';
app.controller('signupController', ['$scope', '$location', '$timeout', 'authService', function ($scope, $location, $timeout, authService) {

    $scope.savedSuccessfully = false;
    $scope.message = "";

    $scope.registration = {       
        emailaddress: "",
        phone: "",
        password: "",
        passwordRepeat: ""
    };

    $scope.busyDisabled = true;

    $scope.signUp = function () {
        $scope.busyDisabled = false;
        authService.saveRegistration($scope.registration).then(function (response) {

            $scope.busyDisabled = true;
                if (response.data.message === "success") {
                    $scope.savedSuccessfully = true;
                    $scope.message = "User has been registered successfully, you will be redicted to login page in 2 seconds.";
                    startTimer();
                } else {
                    $scope.savedSuccessfully = false;
                    $scope.message = response.data.message;
                }

            },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }
             $scope.busyDisabled = true;
             $scope.message = response.data.message;
         });
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/emailSent');
        }, 2000);
    }

}]);