$(document).ready(function () {
    var data = 0;
    ajaxPresenceForActiveGamingTime(data);
});

function GetActiveGamingTime(data) {
    ajaxPresenceForActiveGamingTime(data);
};

function ajaxPresenceForActiveGamingTime(data) {
    let detailsServerId = $("#ServerId").attr('value');
    let formatWithDetailsServerId = data + ":" + detailsServerId;
    $.ajax({
        type: "GET",
        url: '../Stats/GetAllPresencesFromDatabaseForGraphAndDownload?formatWithServerId=' + formatWithDetailsServerId,
        success: barGraphHourlyUserPresenceActivity,
        error: handleError
    });
};


var userPresenceActivityData = [];
var tempUserPresenceActivityData = [];
var userPresencesChart;
var startDate = new Date($("#startDateGraph").val() + " 23:59");
var endDate = new Date($("#endDateGraph").val() + " 23:59");

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}


$("#startDateGraph").change(function () {
    startDate = new Date($(this).val() + " 23:59");
    if (userPresencesChart != null) {
        userPresencesChart.destroy();
    }
    tempUserPresenceActivityData.format = 0;
    graphingUserPresenceActivity(tempUserPresenceActivityData.dataFromDB, tempUserPresenceActivityData.format);

});

$("#endDateGraph").change(function () {

    endDate = new Date($(this).val() + " 23:59");
    if (userPresencesChart != null) {
        userPresencesChart.destroy();
    }
    tempUserPresenceActivityData.format = 0;
    graphingUserPresenceActivity(tempUserPresenceActivityData.dataFromDB, tempUserPresenceActivityData.format);
});

$("#allUsers").change(function () {
    $("#userPresenceHourlyAllTimeChart").empty();

    var newList = []
    if ($(this).val() == "All Users") {

        if (userPresencesChart != null) {
            userPresencesChart.destroy();
        }
        tempUserPresenceActivityData = userPresenceActivityData
        graphingUserPresenceActivity(tempUserPresenceActivityData.dataFromDB)

    }
    else {
        for (var i = 0; i < userPresenceActivityData.dataFromDB.length; i++) {
            if (userPresenceActivityData.dataFromDB[i].userId == $(this).val()) {
                newList.push(userPresenceActivityData.dataFromDB[i]);
            }
        }
        if (userPresencesChart != null) {
            userPresencesChart.destroy();
        }
        tempUserPresenceActivityData = newList
        graphingUserPresenceActivity(tempUserPresenceActivityData, tempUserPresenceActivityData.format)
    }
});


function barGraphHourlyUserPresenceActivity(data) {
    userPresenceActivityData = data
    tempUserPresenceActivityData = data
    graphingUserPresenceActivity(tempUserPresenceActivityData.dataFromDB, data.format)
}


function graphingUserPresenceActivity(data, format) {
    //broken chart

    $("#userPresenceHourlyAllTimeChart").empty();

    var count = 0;
    var xValues = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
    var yValues = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];


    for (var i = 0; i < data.length; i++) {
        var dateUTC = new Date(data[i].createdAt)
        var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))
        console.log("Presences Data Date: " + dateUTC)
        console.log("Presences Data DateUTC: " + date)

        if (date > startDate && date < endDate) {
            let hour = date.getHours()
            subtraction = hour - 4;
            if (subtraction < 0) {
                subtraction = yValues.length + subtraction;
            }
            yValues[subtraction] += 1;
        }
    }

    if (format != 0) {
        xValues.push("Start Date");
        yValues.push(startDate);
        xValues.push("End Date");
        yValues.push(endDate);

        var obj = {};
        for (var i = 0; i < xValues.length; i++) {
            obj[xValues[i]] = yValues[i];
        }

        downloadUserPresenceActivity(obj, format);
    }

    else {
        userPresencesChart = new Chart("userPresenceHourlyAllTimeChart", {
            type: "bar",
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: "green",
                    data: yValues,
                    ticks: {
                        beginAtZero: false
                    }
                }]
            },
            options: {
                plugins: {
                    legend: {
                        display: false
                    },
                    title: {
                        display: true,
                        text: "Active Gaming Time",
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
                            text: 'Game Frequency',
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
                },

            }

        })
    }


};

function downloadUserPresenceActivity(obj, format) {

    var element = document.createElement('a');

    if (format == 1) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(JSON.stringify(obj)));

        element.setAttribute('download', "ActiveGamingTime.json");
    }
    if (format == 2) {
        var something = JSONToCSVConvertor(JSON.stringify(obj));
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(something));
        element.setAttribute('download', "ActiveGamingTime.csv");
    }
    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}