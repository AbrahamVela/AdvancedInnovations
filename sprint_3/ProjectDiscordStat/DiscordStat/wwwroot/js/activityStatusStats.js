$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');

    $.ajax({
        type: 'GET',
        url: '../Stats/GetAllPresencesFromDatabase?serverid=' + detailsServerId,
        success: barGraphHourlyActivityStatus,
        error: handleError
    });
})

var activityStatusData = [];
var tempActivityStatusData = [];
var activityStatusChart;
var startDate = new Date("December 17, 2020");
var endDate = new Date();

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}


$("#startDateGraph").change(function () {
    startDate = new Date($(this).val() + " 00:00");
    if (activityStatusChart != null) {
        activityStatusChart.destroy();
    }
    graphingActivityStatus(tempActivityStatusData);

});

$("#endDateGraph").change(function () {

    endDate = new Date($(this).val() + " 00:00");
    if (activityStatusChart != null) {
        activityStatusChart.destroy();
    }
    graphingActivityStatus(tempActivityStatusData);
});

$("#allUsers").change(function () {
    $("#activityStatusHourlyAllTimeChart").empty();

    var newList = []
    if ($(this).val() == "All Users") {

        if (activityStatusChart != null) {
            activityStatusChart.destroy();
        }
        graphingActivityStatus(activityStatusData)
    }
    else {
        for (var i = 0; i < activityStatusData.length; i++) {
            if (activityStatusData[i].userId == $(this).val()) {
                newList.push(activityStatusData[i]);
            }
        }
        if (activityStatusChart != null) {
            activityStatusChart.destroy();
        }
        tempActivityStatusData = newList
        graphingActivityStatus(tempActivityStatusData)
    }
});


function barGraphHourlyActivityStatus(data) {
    activityStatusData = data
    tempActivityStatusData = data
    graphingActivityStatus(tempActivityStatusData)
}

function graphingActivityStatus(data) {
    //broken chart

    $("#activityStatusHourlyAllTimeChart").empty();

    var count = 0;
    var xValues = [];
    var yValues = [];


    for (var i = 0; i < data.length; i++) {
        var dateUTC = new Date(data[i].createdAt)
        var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))

        if (date > startDate && date < endDate) {
            if (xValues.includes(data[i].activityType) == false) {
                xValues.push(data[i].activityType)
                yValues.push(1)
                var index;
            }
            else if (xValues.includes(data[i].activityType)) {
                xValues.some(function (entry, i) {
                    if (entry == data[i].activityType) {
                        index = i;
                        return true;
                    }
                });
                yValues[index] += 1
            }
        }
    }



    activityStatusChart = new Chart("activityStatusHourlyAllTimeChart", {
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
