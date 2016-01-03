﻿'use strict';
app.controller('viewItemsSharedController', ['authService', 'itemsService', '$location', '$timeout', function (authService, itemsService, $location, $timeout) {

    var self = this;
    self.currentUser = authService.authentication.userName;
    self.conditionMessage = "";
    self.searchBusy = true;
    self.message = "";
    self.beginOpened = false;
    self.endOpened = false;
    self.savedSuccessfully = false;
    self.today = new Date();
    self.maxDate = self.today;
    self.minDate = new Date("2015-04-19");

    self.searchItemInfo =
    {
        itemStatus: "All",
        beginDate: new Date("2015-04-19"),
        endDate: new Date(self.today),
        emailAddress: self.currentUser
    }

    self.searchTransactionInfo =
    {
        emailAddress: self.currentUser
    }

    self.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    self.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    self.format = self.formats[0];

    self.beginOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        self.beginOpened = true;
    };

    self.endOpen = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        self.endOpened = true;
    };

    self.searchItems = function () {
        //Start busy signal
        self.searchBusy = false;
        itemsService.setItemsSearching = true;

        //execute search
        itemsService.searchItems(self.searchItemInfo).then(function (results) {
            //Turn off busy signal
            self.searchBusy = true;

            //Do something with results
            itemsService.setViewItemsList(results.data.items);
            itemsService.setItemsSearching = false;

        }, function (error) {
            self.searchBusy = false;
            if (error === "Authorization has been denied for this request.") {
                self.message = "Session Timed Out";
                authService.logOut();
                self.startTimer();
            }
            //alert(error.data.message);
        });

    };

    self.changeDiscount = function () {
        if (self.itemInfo.discounted === "isDiscounted")
            self.discountMessage = "Price will drop 10% for every month it remains on the shelf - up to 50% after 5 months";
        else
            self.discountMessage = "The item will be marked 'ND' and the price will remain the same";
    };


    self.changedCondition = function () {
        if (self.itemInfo.condition === "New")
            self.conditionMessage = "Just like it sounds. A brand-new, unused, unopened item in its original packaging, with all original packaging materials included. Original protective wrapping, if any, is intact.";
        else if (self.itemInfo.condition === "Like New")
            self.conditionMessage = "An apparently untouched item in perfect condition. Original protective wrapping may be missing, but the original packaging is intact and pristine. There are absolutely no signs of wear on the item or its packaging.";
        else if (self.itemInfo.condition === "Good")
            self.conditionMessage = "A well-cared-for item that has seen limited use but remains in great condition. The item is complete, unmarked, and undamaged, but may show some limited signs of wear. Item works perfectly.";
        else if (self.itemInfo.condition === "Fair")
            self.conditionMessage = "The item is fairly worn but continues to work perfectly. Signs of wear can include aesthetic issues such as scratches, dents, and worn corners. The item may have identifying markings on it or show other signs of previous use.";
        else if (self.itemInfo.condition === "Beat Up")
            self.conditionMessage = "Any aspect of the item is obscured and not able to be read or viewed because of markings, stickers, or other damage. Item is damaged in a way that renders it difficult to use.";
        else {
            self.conditionMessage = "";
        }
    };


    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/login');
        }, 2000);
    }


    var init = function () {

        itemsService.setItemsSearching = true;
        self.searchBusy = true;

        itemsService.searchItems(self.searchItemInfo).then(function (results) {
            itemsService.setItemsSearching = false;
            if (results.data.message === "Session Timed Out") {
                self.message = "Session Timed Out";
                authService.logOut();
                startTimer();
            }
            itemsService.setViewItemsList(results.data.items);
            itemsService.setItemsSearching(false);

        });
        itemsService.searchTransactions(self.searchTransactionInfo).then(function (results) {
            itemsService.setViewTransactionsList(results.data.transactions);
            itemsService.setBalance(results.data.balance);
            itemsService.setTransactionsSearching(false);
        });
    };
    //fire it after definition
    init();

}]);