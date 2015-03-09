

define(['service!pcdWebApi/item'],
    function (itemService) {
        return function () {
            var self = this;

            self.activate = function () { self.refreshItems(); }

            self.dataItems = ko.mapping.fromJS([]);

            self.localize = abp.localization.getSource('PcdWeb');
            //
            self.refreshItems = function () {
                abp.ui.setBusy( //Set whole page busy until getTasks complete
                    null,
                    //itemService.getItemsByConsignorName(ko.mapping.toJS([ "Jacob", "Shirts" ])
                        itemService.getAllItems()
                         .done(function(data) {
                            ko.mapping.fromJS(data.items, self.dataItems);
                        })
            )};

            // Non-editable catalog data - would come from the server
            self.availablePrice = [
                { priceName: "Standard", price: 0 },
                { priceName: "Premium", price: 34.95 },
                { priceName: "Ultimate", price: 90 }
            ];

            

            // Editable data
            self.items = ko.observableArray([
                new ConsignorItem("A Book", "Shelved", self.availablePrice[0]),
                new ConsignorItem("A Game", "Shelved", self.availablePrice[0])
            ]);


            //Computed data
            self.totalSurcharge = ko.computed(function () {
                var total = 0;
                for (var i = 0; i < self.items().length; i++)
                    total += self.items()[i].currentPrice().price;
                return total;
            });

            //Operations
            self.addItem = function () { self.items.push(new ConsignorItem("", "Not yet available", self.availablePrice[0])); }
            self.removeItem = function (item) { self.items.remove(item) }


            //ko.applyBindings(new ReservationsViewModel());

        };

    });


// Class to represent a row in the seat reservations grid
function ConsignorItem(title, status, currentPrice) {
    var self = this;
    self.title = title;
    self.status = status;
    self.currentPrice = ko.observable(currentPrice);
}