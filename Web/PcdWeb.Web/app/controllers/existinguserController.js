'use strict';
app.controller('existinguserController', ['$scope', 'authService', function ($scope, authService) {

    //$scope.existingUsers = [{"IsSelected":false,"firstName":"Randy","lastName":"Shirts","phoneNumber":"1234567890"}];
    $scope.existingUsers = [];
    $scope.message = "";

    $scope.userinfo =
    {
        emailaddress: "",
        password: "",
        passwordrepeat: ""
    }

    $scope.signUp = function () {
        authService.getUsers($scope.userinfo).then(function (results) {
            
                $scope.existingUsers = results.data;

        }, function (error) {
            //alert(error.data.message);
        });
    }

    $scope.sendConfirmation = function() {
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
            $scope.confirmationMessage = "Please only select one record";
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
            authService.updateRegistration(user).then(function (results) {
                //$scope.existingUsers = results.data;
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
    }
}]);