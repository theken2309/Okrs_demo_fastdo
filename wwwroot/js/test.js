(function () {
    'use strict';

    angular.module('test', [
        // Angular modules 
        'ngRoute'

        // Custom modules 

        // 3rd Party Modules
    ])
        .run(['$rootScope', function ($rootScope) {
            // Thêm hàm drawLineChart vào $rootScope để có thể gọi từ bất kỳ controller nào
            $rootScope.drawLineChart = (canvasId, data) => {
                var ctx = document.getElementById(canvasId).getContext('2d');
                new Chart(ctx, {
                    type: 'line',
                    data: data,
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true
                            }
                        }
                    }
                });
            };
        }]);
})();
