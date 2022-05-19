$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    var data = 0;
    ajaxPresenceForMostPopularPlayTime(data);

    $.ajax({
        type: 'GET',
        url: '../Stats/GetUsersFromDatabase?serverid=' + detailsServerId,
        success: graphDropDownBox,
        error: handleError
    });

})

function GetMostPopularPlayTime(data) {
    ajaxPresenceForMostPopularPlayTime(data);
};

function ajaxPresenceForMostPopularPlayTime(data) {
    let detailsServerId = $("#ServerId").attr('value');
    let gameDetailsGameName = $("#GameName").attr('value');
    let formatWithDetailsServerId = data + ":" + detailsServerId

    $.ajax({
        type: 'GET',
        url: '../Stats/GetPresencesFromDatabaseForGraphAndDownload?gamename=' + gameDetailsGameName + '&' + 'formatWithDetailsServerId=' + formatWithDetailsServerId,
        success: barGraphHourlyPresenceActivity,
        error: handleError
    });

}

const timezone = -3
var presenceActivityData = [];
var tempPresenceActivityData = [];

var presencesChart;
var startDate = new Date($("#startDateGraph").val() + " 23:59");
var endDate = new Date($("#endDateGraph").val() + " 23:59");

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}

$("#start").change(function () {
    startDateUTC = new Date($(this).val() + " 23:59");
    if (presencesChart != null) {
        presencesChart.destroy();
    }
    tempPresenceActivityData.format = 0;
    graphingPresenceActivity(tempPresenceActivityData.dataFromDB, tempPresenceActivityData.format);

});

$("#end").change(function () {
    endDate = new Date($(this).val() + " 23:59");
    if (presencesChart != null) {
        presencesChart.destroy();
    }

    tempPresenceActivityData.format = 0;
    graphingPresenceActivity(tempPresenceActivityData.dataFromDB, tempPresenceActivityData.format);
});

$("#allUsersPresences").change(function () {
    $("#usersHourlyAllTimeChart").empty();

    var newList = []
    if ($(this).val() == "All Users") {

        if (presencesChart != null) {
            presencesChart.destroy();
        }
        tempPresenceActivityData = presenceActivityData
        graphingPresenceActivity(tempPresenceActivityData.dataFromDB)

    }
    else {
        for (var i = 0; i < presenceActivityData.dataFromDB.length; i++) {
            if (presenceActivityData.dataFromDB[i].userId == $(this).val()) {
                newList.push(presenceActivityData.dataFromDB[i]);
            }
        }
        if (presencesChart != null) {
            presencesChart.destroy();
        }

        tempPresenceActivityData = newList;
        graphingPresenceActivity(tempPresenceActivityData, tempPresenceActivityData.format);
    }
});


function barGraphHourlyPresenceActivity(data) {
    presenceActivityData = data
    tempPresenceActivityData = data
    graphingPresenceActivity(tempPresenceActivityData.dataFromDB, data.format)
}


function graphingPresenceActivity(data, format) {


    $("#presencesHourlyAllTimeChart").empty();

    var xValues = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
    var yValues = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];


    for (var i = 0; i < data.length; i++) {
        var dateUTC = new Date(data[i].createdAt)
        var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))

        if (date > startDate && date < endDate) {
            let hour = date.getHours()
            subtraction = hour - 1 + timezone;
            if (subtraction < 0) {
                subtraction = yValues.length + subtraction;
            }
            yValues[subtraction] += 1;
        }
    }

    if (format != 0) {
    if (data.startDate != "1-1-0001" && data.endDate != "1-1-0001") {
        xValues.push("Start Date");
        yValues.push(data.startDate);
        xValues.push("End Date");
        yValues.push(data.endDate);
    }

    var obj = {};
    for (var i = 0; i < xValues.length; i++) {
        obj[xValues[i]] = yValues[i];
    };


    downloadMostPopularPlayTime(obj,format);
    }

    else {
        presencesChart = new Chart("presencesHourlyAllTimeChart", {
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
                        text: "Most Popular Play Time",
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
                            text: 'Active Gamers',
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


function downloadMostPopularPlayTime(obj, format) {
    var zero = JSON.stringify(obj);

    var element = document.createElement('a');
    if (format == 1) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(zero));

        element.setAttribute('download', "MostPopularPlayTime.json");
    }
    if (format == 2) {
        var something = JSONToCSVConvertor(zero);

        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(something));
        element.setAttribute('download', "MostPopularPlayTime.csv");
    }
    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}


