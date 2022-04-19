$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    let gameDetailsGameName = $("#GameName").attr('value');
    console.log("URL: " + '../Stats/GetVoiceStatesFromDatabase?gamename=' + gameDetailsGameName + '&' + 'serverid=' + detailsServerId);

    $.ajax({
        type: 'GET',
        url: '../Stats/GetVoiceStatesFromDatabase?serverid=' + detailsServerId,
        success: barGraphHourlyVoiceStateActivity,
        error: handleError
    });

    $.ajax({
        type: 'GET',
        url: '../Stats/GetUsersFromDatabase?serverid=' + detailsServerId,
        success: graphVoiceDropDownBox,
        error: handleError
    });

})

var voiceStateActivityData = [];
var voiceStatesChart;

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}


function graphVoiceDropDownBox(data) {
    var allUsersVoiceStates = document.getElementById("allUsersVoice");
    for (i = 0; i < data.length; i++) {
        var opt = data[i];
        var elVoiceState = document.createElement("option");
        elVoiceState.textContent = opt.username;
        elVoiceState.value = opt.id;
        allUsersVoiceStates.appendChild(elVoiceState);
    }
}

$("#allUsersMessages").change(function () {
    $("#usersHourlyAllTimeChart").empty();

    var newList = []
    if ($(this).val() == "All Users") {

        if (voiceStatesChart != null) {
            voiceStatesChart.destroy();
        }
        graphingVoiceStateActivity(voiceStateActivityData)
    }
    else {
        for (var i = 0; i < voiceStateActivityData.length; i++) {
            if (voiceStateActivityData[i].userId == $(this).val()) {
                newList.push(voiceStateActivityData[i]);
            }
        }
        if (voiceStatesChart != null) {
            voiceStatesChart.destroy();
        }
        graphingVoiceStateActivity(newList)
    }
});


function barGraphHourlyVoiceStateActivity(data) {
    voiceStateActivityData = data
    graphingVoiceStateActivity(voiceStateActivityData)
}


function graphingVoiceStateActivity(data) {


    $("#usersVoiceHourlyAllTimeChart").empty();

    var count = 0;
    var xValues = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
    var yValues = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];


    for (var i = 0; i < data.length; i++) {
        let hour = new Date(data[i].createdAt).getHours();
        subtraction = hour - 4;
        if (subtraction < 0) {
            subtraction = yValues.length + subtraction;
        }
        console.log(subtraction);
        yValues[subtraction] += 1;
    }
    console.log(xValues);
    console.log(yValues);



    voiceStatesChart = new Chart("usersVoiceHourlyAllTimeChart", {
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