$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');

    console.log("URL: " + '../Stats/GetServerMemberFromDatabase?serverid=' + detailsServerId);

    $.ajax({
        type: 'GET',
        url: '../Stats/GetServerMemberFromDatabase?serverid=' + detailsServerId,
        success: barGraphHourlyPresenceActivity,
        error: handleError
    });

})

function UpdateMessages() {
    var startDate = document.getElementById('start').value;
    var endDate = document.getElementById('end').value;
    let detailsServerId = $("#ServerId").attr('value');

    $.ajax({
        type: "GET",
        url: '../Stats/GetServerMemberFromDatabaseWithDate?serverid=' + detailsServerId + '&' + 'startDate=' + startDate + '&' + 'endDate=' + endDate,
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
    graphingPresenceActivity(tempPresenceActivityData)
}


function graphingPresenceActivity(data) {


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
};
