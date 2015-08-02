'use strict';
app.controller('viewTransactionsGridController', ['itemsService','$filter', function (itemsService, $filter) {
    var self = this;

    self.viewTransactionsCollection = itemsService.getViewTransactionsList();

    self.displayed = [];
    self.printBusy = true;
    self.selectBusy = true;
    self.isSearching = itemsService.getTransactionsSearching();
    self.message = "";
    
    self.displayed = self.viewTransactionsCollection;


}]);