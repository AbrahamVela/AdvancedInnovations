$(document).ready(function () {
    var data = 0;
    ajaxPresenceForHoursPerGame(data);
});
    
function GetHoursPerGame(data) {
    ajaxPresenceForHoursPerGame(data);
};

function ajaxPresenceForHoursPerGame(data) {
    let detailsServerId = $("#ServerId").attr('value');
    let formatWithDetailsServerId = data + ":" + detailsServerId;
    $.ajax({
        type: "GET",
        url: '../Stats/GetAllPresencesFromDatabaseForGraphAndDownload?formatWithServerId=' + formatWithDetailsServerId,
        success: barGraphHourlyAllPresenceActivity,
        error: handleError
    });
};

var allPresenceActivityData = [];
var tempAllPresenceActivityData = [];
var allPresencesChart;
var startDate = new Date($("#startDateGraph").val() + " 23:59");
var endDate = new Date($("#endDateGraph").val() + " 23:59");

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
};


$("#startDateGraph").change(function () {
    startDate = new Date($(this).val() + " 23:59");
    if (allPresencesChart != null) {
        allPresencesChart.destroy();
    }
    tempAllPresenceActivityData.format = 0;
    graphingAllPresenceActivity(tempAllPresenceActivityData.dataFromDB, tempAllPresenceActivityData.format);

});

$("#endDateGraph").change(function () {

    endDate = new Date($(this).val() + " 23:59");
    if (allPresencesChart != null) {
        allPresencesChart.destroy();
    }
    tempAllPresenceActivityData.format = 0;
    graphingAllPresenceActivity(tempAllPresenceActivityData.dataFromDB, tempAllPresenceActivityData.format);
});

$("#allUsers").change(function () {
    $("#allPresenceHourlyAllTimeChart").empty();

    var newList = []
    if ($(this).val() == "All Users") {

        if (allPresencesChart != null) {
            allPresencesChart.destroy();
        }
        tempAllPresenceActivityData = allPresenceActivityData
        graphingAllPresenceActivity(tempAllPresenceActivityData.dataFromDB)
    }
    else {
        for (var i = 0; i < allPresenceActivityData.dataFromDB.length; i++) {
            if (allPresenceActivityData.dataFromDB[i].userId == $(this).val()) {
                newList.push(allPresenceActivityData.dataFromDB[i]);
            }
        }
        if (allPresencesChart != null) {
            allPresencesChart.destroy();
        }
        tempAllPresenceActivityData = newList
        graphingAllPresenceActivity(tempAllPresenceActivityData, allPresenceActivityData.format)
    }
});


function barGraphHourlyAllPresenceActivity(data) {
    allPresenceActivityData = data
    tempAllPresenceActivityData = data
    graphingAllPresenceActivity(tempAllPresenceActivityData.dataFromDB, data.format)
}

function graphingAllPresenceActivity(data, format) {
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

    if (format != 0) {
        xValues.push("Start Date");
        yValues.push(startDate);
        xValues.push("End Date");
        yValues.push(endDate);

        var obj = {};
        for (var i = 0; i < xValues.length; i++) {
            obj[xValues[i]] = yValues[i];
        }

        downloadDataHoursPerGame(obj, format);
    }

    else {
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
    }

};

function downloadDataHoursPerGame(obj, format) {

    var element = document.createElement('a');

    if (format == 1) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(JSON.stringify(obj)));

        element.setAttribute('download', "HoursPerGame.json");
    }
    if (format == 2) {
        var something = JSONToCSVConvertor(JSON.stringify(obj));
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(something));
        element.setAttribute('download', "HoursPerGame.csv");
    }
    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}