$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    let gameDetailsGameName = $("#GameName").attr('value');
    console.log("URL: " + '../Stats/GetPresencesFromDatabase?gamename=' + gameDetailsGameName + '&' + 'serverid=' + detailsServerId);

    $.ajax({
        type: 'GET',
        url: '../Stats/GetPresencesFromDatabase?gamename=' + gameDetailsGameName + '&' + 'serverid=' + detailsServerId,
        success: barGraphHourlyPresenceActivity,
        error: handleError
    });

    $.ajax({
        type: 'GET',
        url: '../Stats/GetUsersFromDatabase?serverid=' + detailsServerId,
        success: graphDropDownBox,
        error: handleError
    });

})

const timezone = -3
var presenceActivityData = [];
var tempPresenceActivityData = [];

var presencesChart;
var startDate = new Date("December 17, 2020");
var endDate = new Date();

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}


function graphDropDownBox(data) {
    var allUsersPresences = document.getElementById("allUsersPresences");
    for (i = 0; i < data.length; i++) {
        console.log(data[i].username);
        var opt = data[i];
        var elPresence = document.createElement("option");
        elPresence.textContent = opt.username;
        elPresence.value = opt.id;
        allUsersPresences.appendChild(elPresence);
        //graphingPresenceActivity(newList)

    }
}

$("#start").change(function () {
    startDateUTC = new Date($(this).val() + " 00:00");
    if (presencesChart != null) {
        presencesChart.destroy();
    }
    graphingPresenceActivity(tempPresenceActivityData);

});

$("#end").change(function () {
    endDate = new Date($(this).val() + " 00:00");
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
        graphingPresenceActivity(presenceActivityData)
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

//$(function () {
//    $('input[name="daterange"]').daterangepicker({
//        opens: 'left'
//    }, function (start, end, label) {
//        console.log("A new date selection was made: " + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
//    });
//});

//$('#sandbox-container .input-daterange').datepicker({
//    endDate: "today"
//});

//$("#datepicker").change(function (startDate, endDate) {
//    alert("Start Date: " + startDate)
//    alert("End Date: " + endDate)

//})


function barGraphHourlyPresenceActivity(data) {
    presenceActivityData = data
    tempPresenceActivityData = data
    graphingPresenceActivity(tempPresenceActivityData)
}


function graphingPresenceActivity(data) {


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
            console.log(subtraction);
            yValues[subtraction] += 1;
        }
    }
    console.log(xValues);
    console.log(yValues);



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
};