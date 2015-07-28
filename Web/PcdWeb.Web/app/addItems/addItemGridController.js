'use strict';
app.controller('addItemGridController', ['itemsService', function (itemsService) {
    var self = this;

    self.itemsCollection = itemsService.getItemsList();
    self.saveBusy = true;
    self.message = "";
    self.printedSuccessfully = false;

    self.printBarcodes = function() {
        
        self.saveBusy = false;
        itemsService.printBarcodes(self.itemsCollection).then(function (results) {

            if (results.success === true)
                self.saveBusy = true;
            else
                self.message = "Error retrieving PDF. Please contact the store to get your barcodes emailed to you.";

        }, function (error) {
            //alert(error.data.message);
        });
    };
    
}]);