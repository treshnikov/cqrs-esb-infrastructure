var app = angular.module('app');

app.controller(
    'TimeServiceClientCtrl',
    function ($rootScope, $scope, $http, $location, $interval, microservices) {
        $scope.goToTimeForm = function () {
            console.log("go to time");
            $location.path('time/');
        }
    }); 


