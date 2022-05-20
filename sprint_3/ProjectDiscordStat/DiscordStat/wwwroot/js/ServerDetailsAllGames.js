var xValuesAllGamesStats = [];
var yValuesAllGamesStats = [];
$(document).ready(function () {
    var data = 0;
    let detailsServerId = $("#ServerId").attr('value');
    let formatWithDetailsServerId = data + ":" + detailsServerId;
    $.ajax({
        type: "GET",
        url: '../Stats/GetAllPresencesFromDatabaseForGraphAndDownload?formatWithServerId=' + formatWithDetailsServerId,
        success: barGraphHourlyAllPresenceActivity,
        error: handleError
    });
});
    
function GetHoursPerGame(data) {
    setUpForDownLoadAllGamesStats(data);
};


var allPresenceActivityData = [];
var tempAllPresenceActivityData = [];
var allPresencesChart;
var startDate = new Date("December 17, 2020");
var endDate = new Date();

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
};


$("#startDateGraph").change(function () {
    startDate = new Date($(this).val() + " 00:00");
    if (allPresencesChart != null) {
        allPresencesChart.destroy();
    }
   
    graphingAllPresenceActivity(tempAllPresenceActivityData);

});

$("#endDateGraph").change(function () {

    endDate = new Date($(this).val() + " 23:59");
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
        
        graphingAllPresenceActivity(tempAllPresenceActivityData)
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
    allPresenceActivityData = data.dataFromDB
    tempAllPresenceActivityData = data.dataFromDB
    graphingAllPresenceActivity(tempAllPresenceActivityData)
}

function graphingAllPresenceActivity(data) {
    //broken chart

    $("#allPresenceHourlyAllTimeChart").empty();

    var count = 0;
    xValuesAllGamesStats = [];
    xValuesAllGamesStats = [];


    for (var i = 0; i < data.length; i++) {
        var dateUTC = new Date(data[i].createdAt)
        var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))

        if (date > startDate && date < endDate) {
            if (xValuesAllGamesStats.includes(data[i].name) == false) {
                xValuesAllGamesStats.push(data[i].name)
                yValuesAllGamesStats.push(1)
                var index;
            }
            else if (xValuesAllGamesStats.includes(data[i].name)) {
                xValuesAllGamesStats.some(function (entry, i) {
                    if (entry == data[i].name) {
                        index = i;
                        return true;
                    }
                });
                yValuesAllGamesStats[index] += 1
            }
        }
    }

        allPresencesChart = new Chart("allPresenceHourlyAllTimeChart", {
            type: "bar",
            data: {
                labels: xValuesAllGamesStats,
                datasets: [{
                    backgroundColor: "green",
                    data: yValuesAllGamesStats,
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
  //  }

};

function setUpForDownLoadAllGamesStats(data) {
    xValuesAllGamesStats.push("Start Date");
    yValuesAllGamesStats.push(startDate);
    xValuesAllGamesStats.push("End Date");
    yValuesAllGamesStats.push(endDate);

        var obj = {};
    for (var i = 0; i < xValuesAllGamesStats.length; i++) {
        obj[xValuesAllGamesStats[i]] = yValuesAllGamesStats[i];
        }

    downloadDataHoursPerGame(obj, data);
}
function downloadDataHoursPerGame(obj, data) {

    var element = document.createElement('a');

    if (data == 1) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(JSON.stringify(obj)));

        element.setAttribute('download', "HoursPerGame.json");
    }
    if (data == 2) {
        var something = JSONToCSVConvertor(JSON.stringify(obj));
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(something));
        element.setAttribute('download', "HoursPerGame.csv");
    }
    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}