
$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    $.ajax({
        type: 'GET',
        url: '../Stats/GetMessageInfoFromDatabase?serverid=' + detailsServerId,
        success: barGraphHourlyMessageActivity,
        error: handleError
    });

    $.ajax({
        type: 'GET',
        url: '../Stats/GetUsersFromDatabase?serverid=' + detailsServerId,
        success: graphMessageDropDownBox,
        error: handleError
    });
})

var messageActivityData = [];
var tempMessageActivityData = [];
var messagesChart;
var startDate = new Date("December 17, 2020");
var endDate = new Date();

function graphMessageDropDownBox(data) {
    var allUsers = document.getElementById("allUsers");

    for (i = 0; i < data.length; i++) {
        var opt = data[i];
        var elMessage = document.createElement("option");
        elMessage.textContent = opt.username;
        elMessage.value = opt.id;
        allUsers.appendChild(elMessage);
    }
}

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}

$("#allUsers").change(function () {
    $("#usersHourlyAllTimeChart").empty();

    var newList = []
    if ($(this).val() == "All Users") {
        if (messagesChart != null) {
            messagesChart.destroy();
        }
        graphingMessageActivity(messageActivityData)    }
    else {
        for (var i = 0; i < messageActivityData.length; i++) {
            if (messageActivityData[i].userId == $(this).val()) {
                newList.push(messageActivityData[i]);
            }
        }
        if (messagesChart != null) {
            messagesChart.destroy();
        }
        tempPresenceActivityData = newList
        graphingMessageActivity(tempMessageActivityData)
    }
});

$("#start").change(function () {
    startDate = new Date($(this).val() + " 00:00");
    if (messagesChart != null) {
        messagesChart.destroy();
    }
    graphingMessageActivity(tempMessageActivityData);

});

$("#end").change(function () {
    endDate = new Date($(this).val() + " 00:00");
    if (messagesChart != null) {
        messagesChart.destroy();
    }
    graphingMessageActivity(tempMessageActivityData);
});

function barGraphHourlyMessageActivity(data) {
    messageActivityData = data
    tempMessageActivityData = data
    graphingMessageActivity(tempMessageActivityData)
}

function graphingMessageActivity(data) {
    $("#usersHourlyAllTimeChart").empty();
    //Good Chart
    var xValues = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
    var yValues = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];


    //for (var i = 0; i < data.length; i++) {
    //    var date = new Date(data[i].createdAt)
    //    //alert(date + ">" + startDate + "&&" + date + "<" + endDate)
    //    if (date > startDate && date < endDate) {
    //        let hour = new Date(data[i].createdAt).getHours();
    //        subtraction = hour - 4;
    //        if (subtraction < 0) {
    //            subtraction = yValues.length + subtraction;
    //        }
    //        yValues[subtraction] += 1;
    //    }
    //}

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

    messagesChart = new Chart("usersHourlyAllTimeChart", {
        type: "bar",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: "green",
                data: yValues,
            }]
        },

        options: {
            plugins: {
                legend: {
                    display: false
                },
                title: {
                    display: true,
                    text: "Active Messaging Time",
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
                                    text: 'Messages Sent',
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