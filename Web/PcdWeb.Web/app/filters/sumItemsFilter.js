app.filter('sumItemsFilter', function () {
    return function (groups) {
        var pmtTotals = 0;
        for (i = 0; i < groups.length; i++) {
            pmtTotals = pmtTotals + groups[i].paymentAmount;
        };
        return pmtTotals;
    };
});