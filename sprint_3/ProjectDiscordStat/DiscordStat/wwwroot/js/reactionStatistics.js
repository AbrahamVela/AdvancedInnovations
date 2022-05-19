$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    $.ajax({
        type: 'GET',
        url: '../Stats/GetMessageInfoFromDatabase?serverid=' + detailsServerId,
        success: barGraphHourlyReactionActivity,
        error: handleError
    });
})

var reactionActivityData = [];
var tempReactionActivityData = [];
var reactionsChart;
var startDate = new Date("December 17, 2020");
var endDate = new Date();

function graphReactionDropDownBox(data) {
    var allUsers = document.getElementById("allUsers");

    for (i = 0; i < data.length; i++) {
        var opt = data[i];
        var elReaction = document.createElement("option");
        elReaction.textContent = opt.username;
        elReaction.value = opt.id;
        allUsers.appendChild(elReaction);
    }
}

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}


$("#allUsers").change(function () {
    $("#reactionStats").empty();

    var newList = []
    if ($(this).val() == "All Users") {

        if (reactionsChart != null) {
            reactionsChart.destroy();
        }
        graphingReactionActivity(reactionActivityData)
    }
    else {
        for (var i = 0; i < reactionActivityData.length; i++) {
            if (reactionActivityData[i].userId == $(this).val()) {
                newList.push(reactionActivityData[i]);
            }
        }
        if (reactionsChart != null) {
            reactionsChart.destroy();
        }
        tempReactionActivityData = newList
        graphingReactionActivity(tempReactionActivityData)
    }
});

$("#startDateGraph").change(function () {
    startDate = new Date($(this).val() + " 00:00");
    if (reactionsChart != null) {
        reactionsChart.destroy();
    }
    graphingReactionActivity(tempReactionActivityData);

});

$("#endDateGraph").change(function () {
    endDate = new Date($(this).val() + " 00:00");
    if (reactionsChart != null) {
        reactionsChart.destroy();
    }
    graphingReactionActivity(tempReactionActivityData);
});

function barGraphHourlyReactionActivity(data) {
    reactionActivityData = data
    tempReactionActivityData = data
    graphingReactionActivity(tempReactionActivityData)
}

function graphingReactionActivity(data) {
    $("#reactionStats").empty();
    //Good Chart

    var xValues = [];
    var yValues = [];

    for (var i = 0; i < data.length; i++) {
        var dateUTC = new Date(data[i].createdAt)
        var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))


        if (date > startDate && date < endDate) {
            var reactions = data[i].reactions.split(",")
            for (var e = 0; e < reactions.length - 1; e++) {


                if (xValues.includes(String.fromCodePoint("0x" + reactions[e])) == false) {
                    xValues.push(String.fromCodePoint("0x" + reactions[e]))
                    yValues.push(1)
                }
                else if (xValues.includes(String.fromCodePoint("0x" + reactions[e]))) {
                    xValues.some(function (entry, i) {
                        if (entry == String.fromCodePoint("0x" + reactions[e])) {
                            index = i;
                            return true;
                        }
                    });
                    yValues[index] += 1
                }
            }

        }

    }

    reactionsChart = new Chart("reactionStats", {
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
                    text: "Most Popular Reaction",
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
                        text: 'Reactions Sent',
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
            }

        },

    })

};