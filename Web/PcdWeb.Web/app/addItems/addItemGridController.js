'use strict';
app.controller('addItemGridController', ['itemsService', function (itemsService) {
    var self = this;

    self.itemsCollection = itemsService.getItemsList();
    self.saveBusy = true;
    self.message = "";
    self.printedSuccessfully = false;

    self.printBarcodes = function() {
        
        self.saveBusy = false;
        itemsService.printBarcodes(self.itemsCollection).then(function () {
 
                self.saveBusy = true;
           
        }, function (error) {
            //alert(error.data.message);
        });
    };
    
}]);