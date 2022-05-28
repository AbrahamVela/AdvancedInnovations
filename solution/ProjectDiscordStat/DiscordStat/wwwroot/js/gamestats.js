var xValuesGameDetails = [];
var yValuesGameDetails = [];

$(document).ready(function () {

    let detailsServerId = $("#ServerId").attr('value');
    let gameDetailsGameName = $("#GameName").attr('value');

    $.ajax({
        type: 'GET',
        url: '../Stats/GetPresencesFromDatabase?gamename=' + gameDetailsGameName + '&' + 'ServerId=' + detailsServerId,
        success: barGraphHourlyPresenceActivity,
        error: handleError
    })

    $.ajax({
        type: 'GET',
        url: '../Stats/GetUsersFromDatabase?serverid=' + detailsServerId,
        success: graphDropDownBox,
        error: handleError
    })

})

function graphDropDownBox(data) {
    var allUsers = document.getElementById("allUsersPresences");

    for (i = 0; i < data.length; i++) {
        var opt = data[i];
        var elMessage = document.createElement("option");
        elMessage.textContent = opt.username;
        elMessage.value = opt.id;
        allUsers.appendChild(elMessage);
    }
}

function GetMostPopularPlayTime(data) {
    downloadSetupGameDetails(data);
};

const timezone = -3
var presenceActivityData = [];
var tempPresenceActivityData = [];

var presencesChart;
var startDate = new Date("December 17, 2020");
var endDate = new Date();

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}

$("#start").change(function () {
    startDate = new Date($(this).val() + " 00:00");
    if (presencesChart != null) {
        presencesChart.destroy();
    }

    graphingPresenceActivity(tempPresenceActivityData);

});

$("#end").change(function () {
    endDate = new Date($(this).val() + " 23:59");
    if (presencesChart != null) {
        presencesChart.destroy();
    }

    graphingPresenceActivity(tempPresenceActivityData);
});

$("#allUsersPresences").change(function () {
    $("#usersHourlyAllTimeChart").empty();

    var newList = []
    if ($(this).val() == "All Users") {

        if (presencesChart != null) {
            presencesChart.destroy();
        }
        tempPresenceActivityData = presenceActivityData
        graphingPresenceActivity(tempPresenceActivityData)

    }
    else {
        for (var i = 0; i < presenceActivityData.length; i++) {
            if (presenceActivityData[i].userId == $(this).val()) {
                newList.push(presenceActivityData[i]);
            }
        }
        if (presencesChart != null) {
            presencesChart.destroy();
        }

        tempPresenceActivityData = newList;
        graphingPresenceActivity(tempPresenceActivityData);
    }
});


function barGraphHourlyPresenceActivity(data) {
    presenceActivityData = data
    tempPresenceActivityData = data

    if (data.includes("meta name")) {
        location.reload();
    }
    else {
        graphingPresenceActivity(tempPresenceActivityData)
    }
}


function graphingPresenceActivity(data) {


    $("#presencesHourlyAllTimeChart").empty();

    xValuesGameDetails = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
    yValuesGameDetails = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];


    for (var i = 0; i < data.length; i++) {
        var dateUTC = new Date(data[i].createdAt)
        var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))

        if (date > startDate && date < endDate) {
            let hour = date.getHours()
            subtraction = hour - 1 + timezone;
            if (subtraction < 0) {
                subtraction = yValuesGameDetails.length + subtraction;
            }
            yValuesGameDetails[subtraction] += 1;
        }
    }

    presencesChart = new Chart("presencesHourlyAllTimeChart", {
        type: "bar",
        data: {
            labels: xValuesGameDetails,
            datasets: [{
                backgroundColor: "green",
                data: yValuesGameDetails,
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
};

function downloadSetupGameDetails(data) {
    if (data.startDate != "1-1-0001" && data.endDate != "1-1-0001") {
        xValuesGameDetails.push("Start Date");
        yValuesGameDetails.push(data.startDate);
        xValuesGameDetails.push("End Date");
        yValuesGameDetails.push(data.endDate);
    }

    var obj = {};
    for (var i = 0; i < xValuesGameDetails.length; i++) {
        obj[xValuesGameDetails[i]] = yValuesGameDetails[i];
    };


    downloadMostPopularPlayTime(obj, data);
}

function downloadMostPopularPlayTime(obj, data) {
    var zero = JSON.stringify(obj);

    var element = document.createElement('a');
    if (data == 1) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(zero));

        element.setAttribute('download', "MostPopularPlayTime.json");
    }
    if (data == 2) {
        var something = JSONToCSVConvertor(zero);

        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(something));
        element.setAttribute('download', "MostPopularPlayTime.csv");
    }
    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}