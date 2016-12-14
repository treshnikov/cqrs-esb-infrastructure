// todo разобраться почему без таймаута не удается вызвать сервис

setTimeout(function () {
    angular.element(document.body).injector().get('microservices').add(
    {
        id: "A9ECC812-28F6-4C26-AE9B-D830B20862A6",
        ident: "TimeService",
        name: "Сервис точного вреени",
        permissionsNeeded: ["time"],
        clientUiUrl: "Microservices/TimeService/Client/Index.html",
        adminUiUrl: "Microservices/TimeService/Admin/Index.html"
    });
}, 500);

var app = angular.module('app');

app.config(function ($routeProvider) {

    $routeProvider
        .when('/time', {
            templateUrl: "Microservices/TimeService/Client/time.html",
            controller: '',
            resolve: {}
        });

    console.log("time services route registered");
});
