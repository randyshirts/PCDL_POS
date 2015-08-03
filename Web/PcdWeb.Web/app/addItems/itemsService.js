'use strict';
app.factory('itemsService', ['$http', 'ngAuthSettings', 'authService', '$location', '$filter', function ($http, ngAuthSettings, auth, $location, $filter) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var itemsServiceFactory = {};

    //api methods
    var _getItems = function () {

        return $http.get(serviceBase + 'api/items').then(function (results) {
            return results;
        });
    };

    var _addItem = function(itemInfo) {

        return $http.post(serviceBase + 'api/items/additem', itemInfo).then(function(results) {
            return results;
        }); 
    };

    var _searchItems = function (searchItemInfo) {

        return $http.post(serviceBase + 'api/items/searchItems', searchItemInfo).then(function (results) {
            return results;
        });
    };

    var _searchTransactions = function(searchTransactionInfo) {
        return $http.post(serviceBase + 'api/items/searchTransactions', searchTransactionInfo).then(function (results) {
            return results;
        });
    };

    //For sharing data with other addItem controllers
    var itemList = [];
    var viewItemList = [];
    var viewTransactionsList = [];
    var itemsSearching = { val: true };
    var transactionsSearching = { val: true };
    var balance = { val: 0.0 };

    var _addItemsList = function (newObj) {
        itemList.push(newObj);
    }

    var _getItemsList = function () {
        return itemList;
    }

    var _setViewItemsList = function (newList) {
        viewItemList.length = 0;
        for (var i = 0; i < newList.length; i++)
            viewItemList.push(newList[i]);
    }

    var _getViewItemsList = function () {
        return viewItemList;
    }

    var _setViewTransactionsList = function (newList) {
        viewTransactionsList.length = 0;
        for (var i = 0; i < newList.length; i++)
            viewTransactionsList.push(newList[i]);
    }

    var _getViewTransactionsList = function () {
        return viewTransactionsList;
    }

    var _getItemsSearching = function() {
        return itemsSearching;
    }

    var _setItemsSearching = function (newVal) {
        itemsSearching.val = newVal;
    }

    var _getTransactionsSearching = function () {
        return transactionsSearching;
    }

    var _setTransactionsSearching = function (newVal) {
        transactionsSearching.val = newVal;
    }

    var _getBalance = function() {
        return balance;
    };

    var _setBalance = function (newVal) {
        balance.val = newVal;
    };


    var _downloadFile = function(data, status, headers){

        var octetStreamMime = 'application/octet-stream';
        var pdfMime = 'application/pdf'
        var success = false;

        // Get the headers
        headers = headers();

        // Get the filename from the x-filename header or default to "download.bin"
        var filename = headers['x-filename'] || 'download.pdf';

        // Determine the content type from the header or default to "application/octet-stream"
        var contentType = headers['content-type'] || pdfMime;

        try {
            // Try using msSaveBlob if supported
            console.log("Trying saveBlob method ...");
            var blob = new Blob([data], { type: contentType });
            if (navigator.msSaveBlob)
                navigator.msSaveBlob(blob, filename);
            else {
                // Try using other saveBlob implementations, if available
                var saveBlob = navigator.webkitSaveBlob || navigator.mozSaveBlob || navigator.saveBlob;
                if (saveBlob === undefined) throw "Not supported";
                saveBlob(blob, filename);
            }
            console.log("saveBlob succeeded");
            success = true;
        } catch (ex) {
            console.log("saveBlob method failed with the following exception:");
            console.log(ex);
        }

        if (!success) {
            // Get the blob url creator
            var urlCreator = window.URL || window.webkitURL || window.mozURL || window.msURL;
            if (urlCreator) {
                // Try to use a download link
                var link = document.createElement('a');
                if ('download' in link) {
                    // Try to simulate a click
                    try {
                        // Prepare a blob URL
                        console.log("Trying download link method with simulated click ...");
                        var blob = new Blob([data], { type: contentType });
                        var url = urlCreator.createObjectURL(blob);
                        link.setAttribute('href', url);

                        // Set the download attribute (Supported in Chrome 14+ / Firefox 20+)
                        link.setAttribute("download", filename);

                        // Simulate clicking the download link
                        var event = document.createEvent('MouseEvents');
                        event.initMouseEvent('click', true, true, window, 1, 0, 0, 0, 0, false, false, false, false, 0, null);
                        link.dispatchEvent(event);
                        console.log("Download link method with simulated click succeeded");
                        success = true;

                    } catch (ex) {
                        console.log("Download link method with simulated click failed with the following exception:");
                        console.log(ex);
                    }
                }

                if (!success) {
                    // Fallback to window.location method
                    try {
                        // Prepare a blob URL
                        // Use application/octet-stream when using window.location to force download
                        console.log("Trying download link method with window.location ...");
                        var blob = new Blob([data], { type: octetStreamMime });
                        var url = urlCreator.createObjectURL(blob);
                        window.location = url;
                        console.log("Download link method with window.location succeeded");
                        success = true;
                    } catch (ex) {
                        console.log("Download link method with window.location failed with the following exception:");
                        console.log(ex);
                    }
                }

            }
        }

        if (!success) {
            // Fallback to window.open method
            console.log("No methods worked for saving the arraybuffer, using last resort window.open");
            var file = new Blob([data], { type: 'application/pdf' });
            var fileUrl = URL.createObjectURL(file);
            window.open(fileUrl);
            success = true;
        }

        return success;
    };
    
    var _printBarcodes = function (itemsCollection) {

        return $http.post(serviceBase + 'api/items/printNewBarcodes', itemsCollection, { responseType: 'arraybuffer' }).then(function (data, status, headers) {
            _downloadFile(data.data, data.status, data.headers);
        });
    };

    //Define factory
    itemsServiceFactory.getItems = _getItems;
    itemsServiceFactory.addItem = _addItem;
    itemsServiceFactory.searchItems = _searchItems;
    itemsServiceFactory.searchTransactions = _searchTransactions;
    itemsServiceFactory.addItemsList = _addItemsList;
    itemsServiceFactory.getItemsList = _getItemsList;
    itemsServiceFactory.setViewItemsList = _setViewItemsList;
    itemsServiceFactory.getViewItemsList = _getViewItemsList;
    itemsServiceFactory.setViewTransactionsList = _setViewTransactionsList;
    itemsServiceFactory.getViewTransactionsList = _getViewTransactionsList;
    itemsServiceFactory.printBarcodes = _printBarcodes;
    itemsServiceFactory.downloadFile = _downloadFile;
    itemsServiceFactory.getItemsSearching = _getItemsSearching;
    itemsServiceFactory.setItemsSearching = _setItemsSearching;
    itemsServiceFactory.getTransactionsSearching = _getTransactionsSearching;
    itemsServiceFactory.setTransactionsSearching = _setTransactionsSearching;
    itemsServiceFactory.getBalance = _getBalance;
    itemsServiceFactory.setBalance = _setBalance;

    return itemsServiceFactory;

}]);