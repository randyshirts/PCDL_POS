'use strict';
app.factory('itemsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var itemsServiceFactory = {};

    //api methods
    var _getItems = function () {

        return $http.get(serviceBase + 'api/items').then(function (results) {
            return results;
        });
    };

    var _addItem = function(itemInfo) {

        return $http.post(serviceBase + 'api/items/additem', itemInfo).then(function (results) {
            return results;
        });
    };

    var _printBarcodes = function (itemsCollection) {

        return $http.post(serviceBase + 'api/items/printNewBarcodes', itemsCollection, { responseType: 'arraybuffer' }).then(function (results) {
            return results;
        });
    };

    //For sharing data with other addItem controllers
    var itemList = [];

    var _addItemsList = function (newObj) {
        itemList.push(newObj);
    }

    var _getItemsList = function () {
        return itemList;
    }

    //Define factory
    itemsServiceFactory.getItems = _getItems;
    itemsServiceFactory.addItem = _addItem;
    itemsServiceFactory.addItemsList = _addItemsList;
    itemsServiceFactory.getItemsList = _getItemsList;
    itemsServiceFactory.printBarcodes = _printBarcodes;

    return itemsServiceFactory;

}]);