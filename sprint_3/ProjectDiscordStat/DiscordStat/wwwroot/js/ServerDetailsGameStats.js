$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');

    $.ajax({
        type: 'GET',
        url: '../Stats/GetAllPresencesFromDatabase?serverid=' + detailsServerId,
        success: barGraphHourlyUserPresenceActivity,
        error: handleError
    });
})

var userPresenceActivityData = [];
var tempUserPresenceActivityData = [];
var userPresencesChart;
var startDate = new Date("December 17, 2020");
var endDate = new Date();

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}


$("#startDateGraph").change(function () {
    startDate = new Date($(this).val() + " 00:00");
    if (userPresencesChart != null) {
        userPresencesChart.destroy();
    }
    graphingUserPresenceActivity(tempUserPresenceActivityData);

});

$("#endDateGraph").change(function () {

    endDate = new Date($(this).val() + " 00:00");
    if (userPresencesChart != null) {
        userPresencesChart.destroy();
    }
    graphingUserPresenceActivity(tempUserPresenceActivityData);
});

$("#allUsers").change(function () {
    $("#userPresenceHourlyAllTimeChart").empty();

    var newList = []
    if ($(this).val() == "All Users") {

        if (userPresencesChart != null) {
            userPresencesChart.destroy();
        }
        graphingUserPresenceActivity(userPresenceActivityData)
    }
    else {
        for (var i = 0; i < userPresenceActivityData.length; i++) {
            if (userPresenceActivityData[i].userId == $(this).val()) {
                newList.push(userPresenceActivityData[i]);
            }
        }
        if (userPresencesChart != null) {
            userPresencesChart.destroy();
        }
        tempUserPresenceActivityData = newList
        graphingUserPresenceActivity(tempUserPresenceActivityData)
    }
});


function barGraphHourlyUserPresenceActivity(data) {
    userPresenceActivityData = data
    tempUserPresenceActivityData = data
    graphingUserPresenceActivity(tempUserPresenceActivityData)
}


function graphingUserPresenceActivity(data) {
    //broken chart

    $("#userPresenceHourlyAllTimeChart").empty();

    var count = 0;
    var xValues = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
    var yValues = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];


    for (var i = 0; i < data.length; i++) {
        var dateUTC = new Date(data[i].createdAt)
        var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))

        if (date > startDate && date < endDate) {
            let hour = date.getHours()
            subtraction = hour - 4;
            if (subtraction < 0) {
                subtraction = yValues.length + subtraction;
            }
            yValues[subtraction] += 1;
        }
    }



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


};

function GetActiveGamingTime() {
    let detailsServerId = $("#ServerId").attr('value');
    $.ajax({
        type: "GET",
        url: '../Stats/GetAllPresencesFromDatabase?serverid=' + detailsServerId,
        success: DataForUserPresenceActivity,
        error: handleError
    });
}


function DataForUserPresenceActivity(data) {
    var xValues = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
    var yValues = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

    for (var i = 0; i < data.length; i++) {
        var dateUTC = new Date(data[i].createdAt)
        var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))

        if (date > startDate && date < endDate) {
            let hour = date.getHours()
            subtraction = hour - 4;
            if (subtraction < 0) {
                subtraction = yValues.length + subtraction;
            }
            console.log(subtraction);
            yValues[subtraction] += 1;
        }
    }

    var obj = {};
    for (var i = 0; i < xValues.length; i++) {
        obj[xValues[i]] = yValues[i];
    }

    downloadUserPresenceActivity(obj);
}


function downloadUserPresenceActivity(text) {

    var element = document.createElement('a');
    element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(JSON.stringify(text)));
    element.setAttribute('download', "ActiveGamingTime.json");

    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}
