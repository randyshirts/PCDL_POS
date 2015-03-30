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

            self.saveBusy = true;
            var file = new Blob([results.data], { type: 'application/pdf' });
            var fileURL = URL.createObjectURL(file);
            window.open(fileURL);

        }, function (error) {
            //alert(error.data.message);
        });
    };
    
}]);