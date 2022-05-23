var xValuesActivityStatus = [];
var yValuesActivityStatus = [];

$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');

    $.ajax({
        type: 'GET',
        url: '../Stats/GetAllPresencesFromDatabase?serverid=' + detailsServerId,
        success: barGraphHourlyActivityStatus,
        error: handleError
    });
})

function GetActivityStatus(data) {
    setUpForDownLoadActivityStatus(data);
};

var activityStatusData = [];
var tempActivityStatusData = [];
var activityStatusChart;
var startDate = new Date($("#startDateGraph").val() + " 23:59");
var endDate = new Date($("#endDateGraph").val() + " 23:59");
//var startDate = new Date(Date.UTC(startDateDetailsPage.getUTCFullYear(), startDateDetailsPage.getMonth(), startDateDetailsPage.getDate(), startDateDetailsPage.getHours()))
//var endDate = new Date(Date.UTC(endDateDetailsPage.getUTCFullYear(), endDateDetailsPage.getMonth(), endDateDetailsPage.getDate(), endDateDetailsPage.getHours()))

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}


$("#startDateGraph").change(function () {
    startDate = new Date($(this).val() + " 23:59");
    if (activityStatusChart != null) {
        activityStatusChart.destroy();
    }
    graphingActivityStatus(tempActivityStatusData);

});

$("#endDateGraph").change(function () {

    endDate = new Date($(this).val() + " 23:59");
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
        tempActivityStatusData = activityStatusData
        graphingActivityStatus(tempActivityStatusData)
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

    xValuesActivityStatus = [];
    yValuesActivityStatus = [];


    for (var i = 0; i < data.length; i++) {
        var dateUTC = new Date(data[i].createdAt)
        var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))
        if (date > startDate && date < endDate) {
            if (xValuesActivityStatus.includes(data[i].activityType) == false) {
                xValuesActivityStatus.push(data[i].activityType)
                yValuesActivityStatus.push(1)
                var index;
            }
            else if (xValuesActivityStatus.includes(data[i].activityType)) {
                xValuesActivityStatus.some(function (entry, i) {
                    if (entry == data[i].activityType) {
                        index = i;
                        return true;
                    }
                });
                yValuesActivityStatus[index] += 1
            }
        }
    }



    activityStatusChart = new Chart("activityStatusHourlyAllTimeChart", {
        type: "bar",
        data: {
            labels: xValuesActivityStatus,
            datasets: [{
                backgroundColor: "green",
                data: yValuesActivityStatus,
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
                    text: "Activity Status",
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
                        text: 'Total',
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


function setUpForDownLoadActivityStatus(data) {
    xValuesActivityStatus.push("Start Date");
    yValuesActivityStatus.push(startDate);
    xValuesActivityStatus.push("End Date");
    yValuesActivityStatus.push(endDate);

    var obj = {};
    for (var i = 0; i < xValuesActivityStatus.length; i++) {
        obj[xValuesActivityStatus[i]] = yValuesActivityStatus[i];
    }

    downloadAcivityStatus(obj, data);
}
function downloadAcivityStatus(obj, data) {

    var element = document.createElement('a');

    if (data == 1) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(JSON.stringify(obj)));

        element.setAttribute('download', "ActivityStatus.json");
    }
    if (data == 2) {
        var something = JSONToCSVConvertor(JSON.stringify(obj));
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(something));
        element.setAttribute('download', "ActivityStatus.csv");
    }
    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}