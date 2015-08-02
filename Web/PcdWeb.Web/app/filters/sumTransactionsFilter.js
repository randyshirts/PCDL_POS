app.filter('sumTransactionsFilter', function () {
    return function (groups) {
        var totals = 0;
        for (i = 0; i < groups.length; i++) {
            totals = totals + groups[i].amount;
        };
        return totals;
    };
});