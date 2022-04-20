$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    let gameDetailsGameName = $("#GameName").attr('value');
    console.log("URL: " + '../Stats/GetPresencesFromDatabase?gamename=' + gameDetailsGameName + '&' + 'serverid=' + detailsServerId);

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
var presencesChart;

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}


function graphDropDownBox(data) {
    var allUsersPresences = document.getElementById("allUsersPresences");
    for (i = 0; i < data.length; i++) {
        console.log(data[i].username);
        var opt = data[i];
        var elPresence = document.createElement("option");
        elPresence.textContent = opt.username;
        elPresence.value = opt.id;
        allUsersPresences.appendChild(elPresence);
    }
}

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
        graphingPresenceActivity(newList)
    }
});


function barGraphHourlyPresenceActivity(data) {
    presenceActivityData = data
    graphingPresenceActivity(presenceActivityData)
}


function graphingPresenceActivity(data) {


    $("#presencesHourlyAllTimeChart").empty();

    var count = 0;
    var xValues = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
    var yValues = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];


    for (var i = 0; i < data.length; i++) {
        var date = new Date(data[i].createdAt)
        var dateUTC = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDay(), date.getHours(), date.getMinutes(), date.getSeconds(), date.getMilliseconds()))
        let hour = new Date(dateUTC.toString()).getHours()
        subtraction = hour - 1 + timezone;
        if (subtraction < 0) {
            subtraction = yValues.length + subtraction;
        }
        console.log(subtraction);
        yValues[subtraction] += 1;
    }
    console.log(xValues);
    console.log(yValues);



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
            legend: { display: false },
            title: {
                display: true,
                text: "Activity Frequency"
            }


        }
    })
};