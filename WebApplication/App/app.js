var app = angular.module('app', [
    'ngRoute',
]);


app.config(function($routeProvider) {
    $routeProvider
        .when('/home', {
            templateUrl: 'index.html',
            controller: '',
            resolve: {}
        });
});


app.service('clientSettings', function () {
    this.deleteCacheAfterManualRefresh = true;

    this.getServerInstance = function () {
        return "/td";
    };
});