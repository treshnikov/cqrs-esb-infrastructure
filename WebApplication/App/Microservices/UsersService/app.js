// todo разобраться почему без таймаута не удается вызвать сервис

setTimeout(function () {
    angular.element(document.body).injector().get('microservices').add(
    {
        id: "D9ECC812-28F6-4C26-AE9B-D830B20862A6",
        ident: "UsersService",
        name: "Справочник пользователей",
        permissionsNeeded: ["users"],
        url: "Microservices/UsersService/Client/Index.html"
    });
}, 500);
