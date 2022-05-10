$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');

    $.ajax({
        type: 'GET',
        url: '../Stats/GetAllPresencesFromDatabase?serverid=' + detailsServerId,
        success: barGraphHourlyAllPresenceActivity,
        error: handleError
    });
})

var allPresenceActivityData = [];
var tempAllPresenceActivityData = [];
var allPresencesChart;
var startDate = new Date("December 17, 2020");
var endDate = new Date();

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}


$("#startDateGraph").change(function () {
    startDate = new Date($(this).val() + " 00:00");
    if (allPresencesChart != null) {
        allPresencesChart.destroy();
    }
    graphingAllPresenceActivity(tempAllPresenceActivityData);

});

$("#endDateGraph").change(function () {

    endDate = new Date($(this).val() + " 00:00");
    if (allPresencesChart != null) {
        allPresencesChart.destroy();
    }
    graphingAllPresenceActivity(tempAllPresenceActivityData);
});

$("#allUsers").change(function () {
    $("#allPresenceHourlyAllTimeChart").empty();

    var newList = []
    if ($(this).val() == "All Users") {

        if (allPresencesChart != null) {
            allPresencesChart.destroy();
        }
        graphingAllPresenceActivity(allPresenceActivityData)
    }
    else {
        for (var i = 0; i < allPresenceActivityData.length; i++) {
            if (allPresenceActivityData[i].userId == $(this).val()) {
                newList.push(allPresenceActivityData[i]);
            }
        }
        if (allPresencesChart != null) {
            allPresencesChart.destroy();
        }
        tempAllPresenceActivityData = newList
        graphingAllPresenceActivity(tempAllPresenceActivityData)
    }
});


function barGraphHourlyAllPresenceActivity(data) {
    allPresenceActivityData = data
    tempAllPresenceActivityData = data
    graphingAllPresenceActivity(tempAllPresenceActivityData)
}

function graphingAllPresenceActivity(data) {
    //broken chart

    $("#allPresenceHourlyAllTimeChart").empty();

    var count = 0;
    var xValues = [];
    var yValues = [];


    for (var i = 0; i < data.length; i++) {
        var dateUTC = new Date(data[i].createdAt)
        var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))

        if (date > startDate && date < endDate) {
            if (xValues.includes(data[i].name) == false) {
                xValues.push(data[i].name)
                yValues.push(1)
                var index;
            }
            else if (xValues.includes(data[i].name)) {
                xValues.some(function (entry, i) {
                    if (entry == data[i].name) {
                        index = i;
                        return true;
                    }
                });
                yValues[index] += 1
            }
        }
    }



    allPresencesChart = new Chart("allPresenceHourlyAllTimeChart", {
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
                    text: "Hours per Game",
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
                        text: 'Hours Played',
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



function GetHoursPerGame() {
    let detailsServerId = $("#ServerId").attr('value');
    $.ajax({
        type: "GET",
        url: '../Stats/GetAllPresencesFromDatabase?serverid=' + detailsServerId,
        success: DataHoursPerGame,
        error: handleError
    });
}


function DataHoursPerGame(data) {
    var xValues = [];
    var yValues = [];

    var dauserOrUserstes = GetUserOrUsers(data);
    for (var i = 0; i < data.length; i++) {
        var dateUTC = new Date(data[i].createdAt)
        var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))

        if (date > startDate && date < endDate) {
            if (xValues.includes(data[i].name) == false) {
                xValues.push(data[i].name)
                yValues.push(1)
                var index;
            }
            else if (xValues.includes(data[i].name)) {
                xValues.some(function (entry, i) {
                    if (entry == data[i].name) {
                        index = i;
                        return true;
                    }
                });
                yValues[index] += 1
            }
        }
    }
    var obj = {};
    for (var i = 0; i < xValues.length; i++) {
        obj[xValues[i]] = yValues[i];
    }

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        url: '../Stats/HoursPerGame?data=' + JSON.stringify(obj),
        success: function (msg) {
        }
    });

}


function GetUserOrUsers(data) {

    var allUsers = document.getElementById("allUsers");

    for (i = 0; i < data.length; i++) {
        var opt = data[i];
        var elMessage = document.createElement("option");
        elMessage.textContent = opt.username;
        elMessage.value = opt.id;
        allUsers.appendChild(elMessage);
    }

    var userOrUsers = allUsers

    return userOrUsers
}