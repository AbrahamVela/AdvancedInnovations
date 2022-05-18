$(document).ready(function () {
    var data = 0;
    ajaxVoice(data);
});

function GetActiveVoiceChannelTime(data) {
    ajaxVoice(data);
};

function ajaxVoice(data) {
    let detailsServerId = $("#ServerId").attr('value');   
    let formatWithDetailsServerId = data + ":" + detailsServerId;
    $.ajax({
        type: "GET",
        url: '../Stats/GetVoiceStatesFromDatabaseForGraphAndDownload?formatWithServerId=' + formatWithDetailsServerId,
        success: barGraphHourlyVoiceStateActivity,
        error: handleError
    });
};

var voiceStateActivityData = [];
var tempVoiceStateActivityData = [];
var voiceStatesChart;
var startDate = new Date("December 17, 2020");
var endDate = new Date();

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
};


$("#startDateGraph").change(function () {
    startDate = new Date($(this).val() + " 00:00");
    if (voiceStatesChart != null) {
        voiceStatesChart.destroy();
    }
    tempVoiceStateActivityData.format = 0;
    graphingVoiceStateActivity(tempVoiceStateActivityData.dataFromDB, tempVoiceStateActivityData.format);

});

$("#endDateGraph").change(function () {

    endDate = new Date($(this).val() + " 00:00");
    if (voiceStatesChart != null) {
        voiceStatesChart.destroy();
    }
    tempVoiceStateActivityData.format = 0;
    graphingVoiceStateActivity(tempVoiceStateActivityData.dataFromDB, tempVoiceStateActivityData.format);
});

$("#allUsers").change(function () {
    $("#usersVoiceHourlyAllTimeChart").empty();

    var newList = []
    if ($(this).val() == "All Users") {

        if (voiceStatesChart != null) {
            voiceStatesChart.destroy();
        }
        graphingVoiceStateActivity(voiceStateActivityData.dataFromDB)
    }
    else {
        for (var i = 0; i < voiceStateActivityData, dataFromDB.length; i++) {
            if (voiceStateActivityData.dataFromDB[i].userId == $(this).val()) {
                newList.push(voiceStateActivityData.dataFromDB[i]);
            }
        }
        if (voiceStatesChart != null) {
            voiceStatesChart.destroy();
        }
        tempVoiceStateActivityData = newList
        graphingVoiceStateActivity(tempVoiceStateActivityData, voiceStateActivityData.format)
    }
});


function barGraphHourlyVoiceStateActivity(data) {
    voiceStateActivityData = data
    tempVoiceStateActivityData = data
    graphingVoiceStateActivity(tempVoiceStateActivityData.dataFromDB, data.format)
}


function graphingVoiceStateActivity(data, format) {

    $("#usersVoiceHourlyAllTimeChart").empty();

    var count = 0;
    var xValues = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
    var yValues = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];


    for (var i = 0; i < data.length; i++) {
        var dateUTC = new Date(data[i].createdAt)
        var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))

        if (date > startDate && date < endDate) {
            let hour = date.getHours()
            subtraction = hour - 4;
            if (subtraction < 0) {
                subtraction = yValues.length + subtraction;
            }
            yValues[subtraction] += 1;
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

            downloadForVoiceChannelActivity(obj, format);
        }


    else {
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
                plugins: {
                    legend: {
                        display: false
                    },
                    title: {
                        display: true,
                        text: "Active Voice Channel Time",
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
                            text: 'Channel Frequency',
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


function downloadForVoiceChannelActivity(obj, format) {

    var element = document.createElement('a');

    if (format == 1) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(JSON.stringify(obj)));

        element.setAttribute('download', "ActiveVoiceChannelTime.json");
    }
    if (format == 2) {
        var something = JSONToCSVConvertor(JSON.stringify(obj));
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(something));
        element.setAttribute('download', "ActiveVoiceChannelTime.csv");
    }
    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}

