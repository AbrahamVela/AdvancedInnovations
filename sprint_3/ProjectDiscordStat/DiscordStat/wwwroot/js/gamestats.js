$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    let gameDetailsGameName = $("#GameName").attr('value');

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
            yValues[subtraction] += 1;
        }
    }


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

function GetMostPopularPlayTime(data) {
    let detailsServerId = $("#ServerId").attr('value');
    let gameDetailsGameName = $("#GameName").attr('value');
    let formatWithDetailsServerId = data + ":" + detailsServerId

    $.ajax({
        type: 'GET',
        url: '../Stats/GetPresencesFromDatabaseToDownload?gamename=' + gameDetailsGameName + '&' + 'formatWithDetailsServerId=' + formatWithDetailsServerId,
        success: DataForMostPopularPlayTime,
        error: handleError
    });

}


function DataForMostPopularPlayTime(data) {

    var xValues = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
    var yValues = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];


    for (var i = 0; i < data.dataFromDB.length; i++) {
        var dateUTC = new Date(data.dataFromDB[i].createdAt)
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


    xValues.push("Start Date");
    yValues.push(startDate);
    xValues.push("End Date");
    yValues.push(endDate);


    var obj = {};
    for (var i = 0; i < xValues.length; i++) {
        obj[xValues[i]] = yValues[i];
    };


    downloadStatuses(obj, data.format);

}

function downloadStatuses(obj, format) {
    var zero = JSON.stringify(obj);

    var element = document.createElement('a');
    if (format == 0) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(zero));

        element.setAttribute('download', "MostPopularPlayTime.json");
    }
    else {
        var something = JSONToCSVConvertor(zero);

        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(something));
        element.setAttribute('download', "MostPopularPlayTime.csv");
    }
    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}


function JSONToCSVConvertor(JSONData) {

    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
    var CSV = '';
    //This condition will generate the Label/Header
    csvRows = [];

    // Headers is basically a keys of an
    // object which is id, name, and
    // profession
    const headers = Object.keys(arrData);

    // As for making csv format, headers
    // must be separated by comma and
    // pushing it into array
    csvRows.push(headers.join(','));

    // Pushing Object values into array
    // with comma separation
    const values = Object.values(arrData).join(',');
    csvRows.push(values)

    // Returning the array joining with new line
    return csvRows.join('\n')
}

