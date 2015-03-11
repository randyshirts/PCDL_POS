requirejs.config({
    paths: {
        'text': '../../Scripts/text',
        'durandal': '../../Scripts/durandal',
        'plugins': '../../Scripts/durandal/plugins',
        'transitions': '../../Scripts/durandal/transitions',
        'knockout': '../../Scripts/knockout-3.2.0',
        'knockout.validation': '../../Scripts/knockout.validation',
        'bootstrap': '../../Scripts/bootstrap',
        'jquery': '../../Scripts/jquery-2.1.1',
        'jquery.utilities': '../../Scripts/jquery.utilities',
        'toastr': '../../Scripts/toastr',
        'global': '../global',
        'services': '../services'
    },
    shim: {
        'jquery.utilities': {
            deps: ['jquery']
        },
        'bootstrap': {
            deps: ['jquery'],
            exports: 'jQuery'
        },
        '': {
            deps: ['knockout']
        }
    }
});

define(['durandal/system', 'durandal/app', 'durandal/viewLocator', 'durandal/composition', 'global/session', 'knockout', 'knockout.validation', 'durandal/viewEngine', 'durandal/activator', 'plugins/history'],
    function (system, app, viewLocator, composition, session, ko, validation, viewEngine, activator, history) {
        //>>excludeStart("build", true);
        system.debug(true);
        //>>excludeEnd("build");

        app.title = 'Play Create Discover';

        app.configurePlugins({
            router: true,
            dialog: true,
            widget: true,
            observable: true
        });

        //This is needed to use cshtml files as view
        viewEngine.convertViewIdToRequirePath = function (viewId) {
            return this.viewPlugin + '!/AbpAppView/Load?viewUrl=/App/Main/' + viewId + '.cshtml';
        };

        composition.addBindingHandler('hasFocus');

        configureKnockout();

        //This is needed to return deffered as return values in methods like canActivate.
        activator.defaults.interpretResponse = function (value) {
            if (system.isObject(value)) {
                value = value.can == undefined ? true : value.can;
            }

            if (system.isString(value)) {
                return ko.utils.arrayIndexOf(this.affirmations, value.toLowerCase()) !== -1;
            }

            return value == undefined ? true : value;
        };

        app.start().then(function () {
            //Replace 'viewmodels' in the moduleId with 'views' to locate the view.
            //Look for partial views in a 'views' folder in the root.
            viewLocator.useConvention();
            
            //Show the app by setting the root view model for our application with a transition.
            app.setRoot('viewmodels/shell', 'entrance');
        });

        function configureKnockout() {
            ko.validation.init({
                insertMessages: true,
                decorateElement: true,
                errorElementClass: 'has-error',
                errorMessageClass: 'help-block'
            });

            if (!ko.utils.cloneNodes) {
                ko.utils.cloneNodes = function (nodesArray, shouldCleanNodes) {
                    for (var i = 0, j = nodesArray.length, newNodesArray = []; i < j; i++) {
                        var clonedNode = nodesArray[i].cloneNode(true);
                        newNodesArray.push(shouldCleanNodes ? ko.cleanNode(clonedNode) : clonedNode);
                    }
                    return newNodesArray;
                };
            }

            ko.bindingHandlers.ifIsInRole = {
                init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    ko.utils.domData.set(element, '__ko_withIfBindingData', {});
                    return { 'controlsDescendantBindings': true };
                },
                update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    var withIfData = ko.utils.domData.get(element, '__ko_withIfBindingData'),
                        dataValue = ko.utils.unwrapObservable(valueAccessor()),
                        shouldDisplay = session.userIsInRole(dataValue),
                        isFirstRender = !withIfData.savedNodes,
                        needsRefresh = isFirstRender || (shouldDisplay !== withIfData.didDisplayOnLastUpdate),
                        makeContextCallback = false;

                    if (needsRefresh) {
                        if (isFirstRender) {
                            withIfData.savedNodes = ko.utils.cloneNodes(ko.virtualElements.childNodes(element), true /* shouldCleanNodes */);
                        }

                        if (shouldDisplay) {
                            if (!isFirstRender) {
                                ko.virtualElements.setDomNodeChildren(element, ko.utils.cloneNodes(withIfData.savedNodes));
                            }
                            ko.applyBindingsToDescendants(makeContextCallback ? makeContextCallback(bindingContext, dataValue) : bindingContext, element);
                        } else {
                            ko.virtualElements.emptyNode(element);
                        }

                        withIfData.didDisplayOnLastUpdate = shouldDisplay;
                    }
                }
            };
        }
    });