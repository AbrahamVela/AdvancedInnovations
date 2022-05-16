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

function GetActiveMemberInServer(data) {
    var startDate = document.getElementById('start').value;
    var endDate = document.getElementById('end').value;
    let detailsServerId = $("#ServerId").attr('value');
    let formatWithDetailsServerId = data + ":" + detailsServerId

    $.ajax({
        type: "GET",
        url: '../Stats/GetServerMemberFromDatabaseWithDateToDownload?formatWithDetailsServerId=' + formatWithDetailsServerId + '&' + 'startDate=' + startDate + '&' + 'endDate=' + endDate,
        success: DataForActiveMemberInServer,
        error: handleError
    });
}


function DataForActiveMemberInServer(data) {
    var xValues = [];
    var yValues = [];


    for (var i = 0; i < data.dataFromDB.length; i++) {

        var datecheck = new Date(data.dataFromDB[i].date).toLocaleDateString()
        xValues[i] = datecheck;
        yValues[i] = data.dataFromDB[i].members;
    }

    if (data.startDate != "1-1-0001" && data.endDate != "1-1-0001") {
        xValues.push("Start Date");
        yValues.push(data.startDate);
        xValues.push("End Date");
        yValues.push(data.endDate);
    }

    var obj = {};
    for (var i = 0; i < xValues.length; i++) {
        obj[xValues[i]] = yValues[i];
    };


    downloadStatuses(obj,data.format);

}

function downloadStatuses(obj,  format) {
    var zero = JSON.stringify(obj);

    var element = document.createElement('a');
    if (format == 0) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(zero));

        element.setAttribute('download', "Status.json");
    }
    else {
        var something = JSONToCSVConvertor(zero);

        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(something));
        element.setAttribute('download', "Status.csv");
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

