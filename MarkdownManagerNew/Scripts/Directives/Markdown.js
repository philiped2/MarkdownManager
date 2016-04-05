(function () {
    //var app = angular.module("app");

    //var markdown = function () {
    //    return {
    //        restrict: 'E',
    //        link: function (element) {
    //                var html = $sanitize(markdownConverter.makeHtml("#Test"()));
    //                element.html(html);
    //        }
    //    };
    //}

    //app.directive("markdown", markdown);

    var app = angular.module("app");

    var md = function () {
        if (typeof hljs !== 'undefined') {
            marked.setOptions({
                highlight: function (code, lang) {
                    if (lang) {
                        return hljs.highlight(lang, code).value;
                    } else {
                        return hljs.highlightAuto(code).value;
                    }
                }
            });
        }

        return {
            restrict: 'E',
            require: '?ngModel',
            link: function ($scope, $elem, $attrs, ngModel) {
                if (!ngModel) {
                    // render transcluded text
                    var html = marked($elem.text());
                    $elem.html(html);
                    return;
                }

                ngModel.$render = function () {
                    var html = marked(ngModel.$viewValue || '');
                    $elem.html(html);
                };
            }  // link function
        };
    }

    app.directive("md", md);


}());