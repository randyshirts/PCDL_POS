'use strict';
app.controller('viewItemsInfoController', ['itemsService', '$filter', function (itemsService, $filter) {
    var self = this;

    self.balance= itemsService.getBalance();

}]);