'use strict';
app.controller('existinguserController', ['$scope', '$location', '$timeout', 'authService', function ($scope, $location, $timeout, authService) {

    //$scope.existingUsers = [{"IsSelected":false,"firstName":"Randy","lastName":"Shirts","phoneNumber":"1234567890"}];
    $scope.existingUsers = [];
    $scope.message = "";
    $scope.savedSuccessfully = false;

    $scope.userinfo =
    {
        emailaddress: "",
        password: "",
        passwordrepeat: ""
    }

    $scope.busyDisabled = true;

    $scope.signUp = function () {

        $scope.busyDisabled = false;
        authService.getUsers($scope.userinfo).then(function (results) {

            $scope.busyDisabled = true;
            if (results.data.message === "success") {
                $scope.existingUsers = results.data.users;
            } else {
                $scope.message = results.data.message;
                $scope.savedSuccessfully = false;
            }

        }, function (error) {
            //alert(error.data.message);
        });
    }

    $scope.updateRegistration = function() {
        var count = [];
        var selectedUsers = [];
        var listedUsers = $scope.existingUsers;
        for (var i = 0; i < listedUsers.length; i++) {
            if (listedUsers[i].isSelected === true) {
                count.push(i);
                selectedUsers.push(listedUsers[i]);
            }
        }
        if (count.length > 1) {
            $scope.confirmationMessage = "Please select only one record";
        }
        if (count.length === 0) {
            $scope.confirmationMessage = "Please select a record";
        }
        if (count.length === 1) {
            var user = selectedUsers[0];
            user.emailaddress = $scope.userinfo.emailaddress;
            user.password = $scope.userinfo.password;
            user.passwordrepeat = $scope.userinfo.passwordrepeat;

            //Register
            $scope.busyDisabled = false;
            authService.updateRegistration(user).then(function (results) {
                $scope.busyDisabled = true;
                var result = results.data;
                    if (result.message === "success") {
                        $scope.message = "Registration successful - you will be redirected in 2 seconds";
                        startTimer();
                    } else {
                        $scope.message = result.message;
                    }

                },
            function (error) {
                //alert(error.data.message);
            });

            ////Send Email
            //authService.sendConfirmation(user).then(function (results) {
            //    //$scope.existingUsers = results.data;
            //},
            //function (error) {
            //    //alert(error.data.message);
            //});
        }

        var startTimer = function () {
            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $location.path('/emailSent');
            }, 2000);
        }
    }
}]);