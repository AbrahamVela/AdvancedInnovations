$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    let gameDetailsGameName = $("#GameName").attr('value');
    console.log("URL: " + '../Stats/GetVoiceStatesFromDatabase?serverid=' + detailsServerId);

    $.ajax({
        type: 'GET',
        url: '../Stats/GetVoiceStatesFromDatabase?serverid=' + detailsServerId,
        success: barGraphHourlyVoiceStateActivity,
        error: handleError
    });

    //$.ajax({
    //    type: 'GET',
    //    url: '../Stats/GetUsersFromDatabase?serverid=' + detailsServerId,
    //    success: graphVoiceDropDownBox,
    //    error: handleError
    //});

})

var voiceStateActivityData = [];
var voiceStatesChart;
var startDate = new Date("December 17, 2020");
var endDate = new Date();

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}


$("#start").change(function () {
    startDate = new Date($(this).val() + " 00:00");
    if (voiceStatesChart != null) {
        voiceStatesChart.destroy();
    }
    graphingVoiceStateActivity(voiceStateActivityData);

});

$("#end").change(function () {
    endDate = new Date($(this).val() + " 00:00");
    if (voiceStatesChart != null) {
        voiceStatesChart.destroy();
    }
    graphingVoiceStateActivity(voiceStateActivityData);
});

$("#allUsers").change(function () {
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
    //broken chart

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
            console.log(subtraction);
            yValues[subtraction] += 1;
        }
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
                        text: 'Voice Channel Frequency',
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
//        options: {
//            legend: { display: false },
//            title: {
//                display: true,
//                text: "Activity Frequency"
//            }


//        }
//    })
//};