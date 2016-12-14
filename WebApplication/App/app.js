var app = angular.module('app', [
    'ngRoute',
]);


app.config(function($routeProvider) {
    $routeProvider
        .when('/home', {
            templateUrl: 'index.html',
            controller: '',
            resolve: {}
        })
        //.otherwise({ redirectTo: '/home' })
    ;
});


app.service('clientSettings', function () {
    this.deleteCacheAfterManualRefresh = true;

    this.getServerInstance = function () {
        return "/td";
    };
});

app.service('microservices', function () {
    this.modules = [];

    this.add = function (moduleName) {
        this.modules.push(moduleName);
    };

    this.get = function() {
        return this.modules;
    }

});

app.controller(
    'homeCtrl',
    function ($rootScope, $scope, $http, $location, $interval, microservices) {
        $scope.modules = [];

        // todo - получать перечень модулей по подписке или единожды после всех добавлений
        $interval(function () {
            $scope.modules = microservices.get();
            console.log("microservices - " + microservices.get().length);
        }, 1000);
    });