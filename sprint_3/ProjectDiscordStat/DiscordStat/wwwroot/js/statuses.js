var xValuesStatus = [];
var yValuesStatus = [];
var yValuesOnline = [];
var yValuesIdle = [];
var yValuesDnD = [];

$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    $.ajax({
        type: 'GET',
        url: '../Stats/GetStatusesFromDatabase?ServerId=' + detailsServerId,
        success: barGraphHourlyStatusActivity,
        error: handleError
    });
});

function GetStatuses(data) {
    setUpForDownLoadForStatus(data);
};



var statusActivityData = [];
var tempStatusActivityData = [];
var statusesChart;
var startDate = new Date("December 17, 2020");
var endDate = new Date();

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}


$("#startDateGraph").change(function () {
    startDate = new Date($(this).val() + " 00:00");
    if (statusesChart != null) {
        statusesChart.destroy();
    }
    graphingStatusActivity(tempStatusActivityData);
});

$("#endDateGraph").change(function () {

    endDate = new Date($(this).val() + " 23:59");
    if (statusesChart != null) {
        statusesChart.destroy();
    }
    graphingStatusActivity(tempStatusActivityData);
});

$("#allUsers").change(function () {
    $("#usersStatusHourlyChart").empty();

    var newList = []
    if ($(this).val() == "All Users") {

        if (statusesChart != null) {
            statusesChart.destroy();
        }
        tempStatusActivityData = statusActivityData;
        graphingStatusActivity(tempStatusActivityData)
    }
    else {
        for (var i = 0; i < statusActivityData.length; i++) {
            if (statusActivityData[i].userId == $(this).val()) {
                newList.push(statusActivityData[i]);
            }
        }
        if (statusesChart != null) {
            statusesChart.destroy();
        }
        tempStatusActivityData = newList
        graphingStatusActivity(tempStatusActivityData)
    }
});


function barGraphHourlyStatusActivity(data) {
    statusActivityData = data
    tempStatusActivityData = data
    graphingStatusActivity(tempStatusActivityData)
}


function graphingStatusActivity(data) {

    $("#usersStatusHourlyChart").empty();

    var count = 0;
     xValuesStatus = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
     yValuesOnline = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
     yValuesIdle = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
     yValuesDnD = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

    console.log("Length: " + data.length)
    for (var i = 0; i < data.length; i++) {
        var dateUTC = new Date(data[i].createdAt)
        var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))

        if (date > startDate && date < endDate) {
            let hour = date.getHours()
            subtraction = hour - 4;
            if (data[i].status1 == "idle") {
                if (subtraction < 0) {
                    subtraction = yValuesIdle.length + subtraction;
                }
                yValuesIdle[subtraction] += 1;
            }
            else if (data[i].status1 == "online") {
                if (subtraction < 0) {
                    subtraction = yValuesOnline.length + subtraction;
                }
                yValuesOnline[subtraction] += 1;
            }
            else if (data[i].status1 == "dnd") {
                if (subtraction < 0) {
                    subtraction = yValuesDnD.length + subtraction;
                }
                yValuesDnD[subtraction] += 1;
            }
        }
    }

        statusesChart = new Chart("usersStatusHourlyChart", {

            type: "line",
            data: {
                labels: xValuesStatus,
                datasets: [{
                    label: "Online",
                    backgroundColor: "green",
                    borderColor: 'green',
                    fill: true,
                    data: yValuesOnline,
                    ticks: {
                        beginAtZero: false
                    }
                },
                {
                    label: "Idle",
                    backgroundColor: "orange",
                    borderColor: 'orange',
                    fill: true,
                    data: yValuesIdle,
                    ticks: {
                        beginAtZero: false
                    }
                },
                {
                    label: "Do Not Disturb",
                    backgroundColor: "red",
                    borderColor: 'red',
                    fill: true,
                    data: yValuesDnD,
                    ticks: {
                        beginAtZero: false
                    }
                }
                ]
            },
          
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        display: true
                    },
                    title: {
                        display: true,
                        text: "Statuses",
                        padding: 10,
                        color: 'black',
                        font: {
                            size: 25
                        }
                    },
                },
                scales: {
                    y: {
                        stacked: true,
                        title: {
                            display: true,
                            text: 'Number of Users',
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

function setUpForDownLoadForStatus(data) {
    xValuesStatus.push("Start Date");
        yValuesOnline.push(startDate);
        yValuesIdle.push(startDate);
        yValuesDnD.push(startDate);
    xValuesStatus.push("End Date");
        yValuesOnline.push(endDate);
        yValuesIdle.push(endDate);
        yValuesDnD.push(endDate);
    xValuesStatus.push("Status Type");
        yValuesOnline.push("online");
        yValuesIdle.push("idle");
        yValuesDnD.push("do not disturb");

        var obj = {};
    for (var i = 0; i < xValuesStatus.length; i++) {
        obj[xValuesStatus[i]] = yValuesOnline[i];
        };
        var obj1 = {};
    for (var i = 0; i < xValuesStatus.length; i++) {
        obj1[xValuesStatus[i]] = yValuesIdle[i];
        };
        var obj2 = {};
    for (var i = 0; i < xValuesStatus.length; i++) {
        obj2[xValuesStatus[i]] = yValuesDnD[i];
        };

        downloadStatuses(obj, obj1, obj2, data);
    }


function downloadStatuses(obj, obj1, obj2, data) {
    var zero = JSON.stringify(obj);
    var one = JSON.stringify(obj1);
    var two = JSON.stringify(obj2);
    var allThree = zero + "\n" + one + "\n" + two;

    var element = document.createElement('a');
    if (data == 1) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(allThree));

        element.setAttribute('download', "Status.json");
    }
    if (data == 2)  {
        var something = JSONToCSVConvertor(zero);
        var something1 = JSONToCSVConvertorWithOutKeys(one);
        var something2 = JSONToCSVConvertorWithOutKeys(two);
        var all = something + "\n" + something1 + "\n" + something2;
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(all));
        element.setAttribute('download', "Status.csv");
    }
    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}