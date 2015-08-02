'use strict';
app.controller('viewItemsGridController', ['itemsService', function (itemsService) {
    var self = this;

    self.viewItemsCollection = itemsService.getViewItemsList();
    self.displayed = [];
    self.printBusy = true;
    self.selectBusy = true;
    self.message = "";
    self.printedSuccessfully = false;
    self.isSearching = itemsService.getItemsSearching();

    self.displayed = self.viewItemsCollection;

    self.printBarcodes = function() {
        
        self.printBusy = false;
        itemsService.printBarcodes(self.viewItemsCollection).then(function (results) {

            if (results.success === true)
                self.printBusy = true;
            else
                self.message = "Error retrieving PDF. Please contact the store to get your barcodes emailed to you.";

        }, function (error) {
            //alert(error.data.message);
        });
    };

    self.selectAll = function() {

        self.selectBusy = false;
        for (var i = 0; i < self.viewItemsCollection.length; i++)
            self.viewItemsCollection[i].print = !self.viewItemsCollection[i].print;

        self.selectBusy = true;
    };

    
     //var getBalance = function() {
     //   var sum = 0.0;
     //   for (var i = 0; i < self.viewItemsCollection.length; i++)
     //       sum = self.viewItemsCollection[i].paymentAmount + sum;

        //subtract payments
        
     //};

     //self.balance = getBalance();

}]);