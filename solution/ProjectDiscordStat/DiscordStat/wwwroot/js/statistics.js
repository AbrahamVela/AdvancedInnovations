﻿var xValuesMessages = [];
var yValuesMessages = [];
$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    $.ajax({
        type: "GET",
        url: '../Stats/GetMessageInfoFromDatabase?ServerId=' + detailsServerId,
        success: barGraphHourlyMessageActivity,
        error: handleError
    });

    $.ajax({
        type: 'GET',
        url: '../Stats/GetUsersFromDatabase?serverid=' + detailsServerId,
        success: graphUsersDropDownBox,
        error: handleError
    });

});

function GetActiveMessageTime(data) {
    setUpForDownLoadMessages(data);
};


var messageActivityData = [];
var tempMessageActivityData = [];
var messagesChart;
var startDate = new Date($("#startDateGraph").val() + " 23:59");
var endDate = new Date($("#endDateGraph").val() + " 23:59");

function graphUsersDropDownBox(data) {
    var allUsers = document.getElementById("allUsers");

    for (i = 0; i < data.length; i++) {
        var opt = data[i];
        var elMessage = document.createElement("option");
        elMessage.textContent = opt.username;
        elMessage.value = opt.id;
        allUsers.appendChild(elMessage);
    }
}

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}


$("#allUsers").change(function () {
    $("#usersHourlyAllTimeChart").empty();

    var newList = []
    if ($(this).val() == "All Users") {

        if (messagesChart != null) {
            messagesChart.destroy();
        }
        tempMessageActivityData = messageActivityData
        graphingMessageActivity(tempMessageActivityData)
    }
    else {
        for (var i = 0; i < messageActivityData.length; i++) {
            if (messageActivityData[i].userId == $(this).val()) {
                newList.push(messageActivityData[i]);
            }
        }
        if (messagesChart != null) {
            messagesChart.destroy();
        }
        tempMessageActivityData = newList
        graphingMessageActivity(tempMessageActivityData)
    }
});

$("#startDateGraph").change(function () {
    startDate = new Date($(this).val() + " 00:00");
    if (messagesChart != null) {
        messagesChart.destroy();
    }
    tempMessageActivityData.format = 0;
    graphingMessageActivity(tempMessageActivityData);

});

$("#endDateGraph").change(function () {
    endDate = new Date($(this).val() + " 23:59");
    if (messagesChart != null) {
        messagesChart.destroy();
    }
    tempMessageActivityData.format = 0;
    graphingMessageActivity(tempMessageActivityData);
});

function barGraphHourlyMessageActivity(data) {
    messageActivityData = data
    tempMessageActivityData = data
    graphingMessageActivity(tempMessageActivityData)
}

function graphingMessageActivity(data) {
    $("#usersHourlyAllTimeChart").empty();
    //Good Chart
    xValuesMessages = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
    yValuesMessages = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];


    //for (var i = 0; i < data.length; i++) {
    //    var date = new Date(data[i].createdAt)
    //    //alert(date + ">" + startDate + "&&" + date + "<" + endDate)
    //    if (date > startDate && date < endDate) {
    //        let hour = new Date(data[i].createdAt).getHours();
    //        subtraction = hour - 4;
    //        if (subtraction < 0) {
    //            subtraction = yValues.length + subtraction;
    //        }
    //        yValues[subtraction] += 1;
    //    }
    //}

    for (var i = 0; i < data.length; i++) {
        var date = new Date(data[i].createdAt)
        //var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))
        //console.log("Messages Data Date: " + dateUTC)
        //console.log("Messages Data DateUTC: " + date)

        if (date > startDate && date < endDate) {
            let hour = date.getHours()
            subtraction = hour - 4;
            if (subtraction < 0) {
                subtraction = yValuesMessages.length + subtraction;
            }
            yValuesMessages[subtraction] += 1;
        }
    }

    messagesChart = new Chart("usersHourlyAllTimeChart", {
        type: "bar",
        data: {
            labels: xValuesMessages,
            datasets: [{
                backgroundColor: "green",
                data: yValuesMessages,
            }]
        },

        options: {
            plugins: {
                legend: {
                    display: false
                },
                title: {
                    display: true,
                    text: "Active Messaging Time",
                    padding: 10,
                    color: 'black',
                    font: {
                        size: 25
                    }
                },
            },
            scales: {
                y: {
                    title: {
                        display: true,
                        text: 'Messages Sent',
                        padding: 10,
                        color: 'black',
                        font: {
                            size: 25
                        }
                    },
                    ticks: {
                        beginAtZero: false,
                        precision: 0,
                        color: 'black',
                        font: {
                            size: 20
                        }
                    }

                },
                x: {
                    ticks: {
                        precision: 0,
                        color: 'Black',
                        font: {
                            size: 16,
                            family: 'Helvetica'
                        }
                    }
                }
            }

        },

    })

};

function setUpForDownLoadMessages(data) {
    xValuesMessages.push("Start Date");
    yValuesMessages.push(startDate);
    xValuesMessages.push("End Date");
    yValuesMessages.push(endDate);

        var obj = {};
    for (var i = 0; i < xValuesMessages.length; i++) {
        obj[xValuesMessages[i]] = yValuesMessages[i];
        }

    downloadForHourlyMessageActivity(obj, data);

}
function downloadForHourlyMessageActivity(obj, data) {

    var element = document.createElement('a');

    if (data == 1) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(JSON.stringify(obj)));

        element.setAttribute('download', "ActiveMessagingTime.json");
    }
    if (data == 2) {
        var something = JSONToCSVConvertor(JSON.stringify(obj));
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(something));
        element.setAttribute('download', "ActiveMessagingTime.csv");
    }
    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}