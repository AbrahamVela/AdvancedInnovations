$(document).ready(function () {
    var data = 0;
    ajaxServerMembersForActiveMemberInServer(data);

    console.log("URL: " + '../Stats/GetServerMemberFromDatabase?serverid=' + detailsServerId);
});

function GetActiveMemberInServer(data) {
    ajaxServerMembersForActiveMemberInServer(data);
};

function UpdateMessages() {
    var data = 0;
    ajaxServerMembersForActiveMemberInServer(data);
};


function ajaxServerMembersForActiveMemberInServer(data) {
    var startDate = document.getElementById('start').value;
    var endDate = document.getElementById('end').value;
    let detailsServerId = $("#ServerId").attr('value');
    let formatWithDetailsServerId = data + ":" + detailsServerId

    $.ajax({
        type: "GET",
        url: '../Stats/GetServerMemberFromDatabaseWithDateForGraphAndDownload?formatWithDetailsServerId=' + formatWithDetailsServerId + '&' + 'startDate=' + startDate + '&' + 'endDate=' + endDate,
        success: barGraphHourlyPresenceActivity,
        error: handleError
    });
}


var presenceActivityData = [];
var tempPresenceActivityData = [];
var presencesChart;


function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}



function barGraphHourlyPresenceActivity(data) {
    if (presencesChart != null) {
        presencesChart.destroy();
    }
    presenceActivityData = data
    tempPresenceActivityData = data
    graphingPresenceActivity(tempPresenceActivityData.dataFromDB, data.format)
}


function graphingPresenceActivity(data, format) {


    $("#ServerGrowthAllTimeChart").empty();

    var xValues = [];
    var yValues = [];


    for (var i = 0; i < data.length; i++) {

        var datecheck = new Date(data[i].date).toLocaleDateString()
        xValues[i] = datecheck;
        yValues[i] = data[i].members;
    }
    console.log(xValues);
    console.log(yValues);

    if (format != 0) {
        var obj = {};
        for (var i = 0; i < xValues.length; i++) {
            obj[xValues[i]] = yValues[i];
        };

        downloadServerGrowth(data, obj, format);
    }

    else {
        presencesChart = new Chart("ServerGrowthAllTimeChart", {
            type: "line",
            data: {
                labels: xValues,
                datasets: [
                    {
                        borderColor: "black",
                        backgroundColor: "rgb(255, 99, 132)",
                        data: yValues,
                        ticks: {
                            beginAtZero: false
                        }
                    }]
            },
            options: {
                responsive: true,
                plugins: {
                    legend: {
                        display: false
                    },
                    title: {
                        display: true,
                        text: "Active Members in Server",
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
                            text: 'Members',
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
                            color: 'white',
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

function downloadServerGrowth(data, obj, format) {
    var zero = JSON.stringify(obj);

    var element = document.createElement('a');
    if (format == 1) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(zero));

        element.setAttribute('download', "ActiveMembersInServer.json");
        format = 0;
        graphingPresenceActivity(data, format)
    }
    if (format == 2) {
        var something = JSONToCSVConvertor(zero);

        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(something));
        element.setAttribute('download', "ActiveMembersInServer.csv");
        format = 0;
        graphingPresenceActivity(data, format)
    }
    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}