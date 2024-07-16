export default function loadStatistics(calculating, hasMessage, hasNotMessage, userId) {
    "use strict";
    var connection = new signalR.HubConnectionBuilder().withUrl("/cadsHub").build();

    connection.on("ReceiveStatistics", function (userCadsCount, unvCadsCount) {
        console.log('receiving statistics');
        $('#user-cads-count').text(calculating);
        $('#unvalidated-cads-count').text(calculating);
        
        if (userCadsCount > 0) {
            $('#user-cads-count').text(`${hasMessage} ${userCadsCount}`);
            console.log($('#user-cads-count').text());
        } else {
            $('#user-cads-count').text(hasNotMessage);
            console.log($('#user-cads-count').text());
        }

        if (unvCadsCount > 0) {
            $('#unvalidated-cads-count').text(`${hasMessage} ${unvCadsCount}`);
            console.log($('##unvalidated-cads-count').text());
        } else {
            $('#unvalidated-cads-count').text(hasNotMessage);
            console.log($('##unvalidated-cads-count').text());
        }
    });

    connection.start().then(function () {
        $('#user-cads-wrapper').removeClass('d-none');
        $('#unvalidated-cads-wrapper').removeClass('d-none');
        
        connection.invoke("SendStatistics", userId).catch(function (err) {
            console.error(err.toString());
        });
    }).catch(function (err) {
        console.error('Error fetching product count:', err);
    });
}