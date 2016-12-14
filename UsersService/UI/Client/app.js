var app = angular.module('app');

app.controller(
    'UsersServiceClientCtrl',
    function ($rootScope, $scope, $http, $location, $interval, microservices) {
        $scope.goToUsersForm = function () {
            console.log("go to users");
            $location.path('users/');
        }
    }); 


