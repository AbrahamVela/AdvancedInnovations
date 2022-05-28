var xValuesPopEmojis = [];
var yValuesPopEmojis = [];

$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    $.ajax({
        type: 'GET',
        url: '../Stats/GetMessageInfoFromDatabase?serverid=' + detailsServerId,
        success: barGraphHourlyReactionActivity,
        error: handleError
    });
})

function GetPopReaction(data) {
    setUpForDownLoadPopEmojis(data);
};

var reactionActivityData = [];
var tempReactionActivityData = [];
var reactionsChart;
var startDate = new Date($("#startDateGraph").val() + " 23:59");
var endDate = new Date($("#endDateGraph").val() + " 23:59");

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
        tempReactionActivityData = reactionActivityData
        graphingReactionActivity(tempReactionActivityData)
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
    startDate = new Date($(this).val() + " 23:59");
    if (reactionsChart != null) {
        reactionsChart.destroy();
    }
    graphingReactionActivity(tempReactionActivityData);

});

$("#endDateGraph").change(function () {
    endDate = new Date($(this).val() + " 23:59");
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

    xValuesPopEmojis = [];
    yValuesPopEmojis = [];

    for (var i = 0; i < data.length; i++) {
        var date = new Date(data[i].createdAt)
        //var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))


        if (date > startDate && date < endDate) {
            var reactions = data[i].reactions.split(",")
            for (var e = 0; e < reactions.length - 1; e++) {


                if (xValuesPopEmojis.includes(String.fromCodePoint("0x" + reactions[e])) == false) {
                    xValuesPopEmojis.push(String.fromCodePoint("0x" + reactions[e]))
                    yValuesPopEmojis.push(1)
                }
                else if (xValuesPopEmojis.includes(String.fromCodePoint("0x" + reactions[e]))) {
                    xValuesPopEmojis.some(function (entry, i) {
                        if (entry == String.fromCodePoint("0x" + reactions[e])) {
                            index = i;
                            return true;
                        }
                    });
                    yValuesPopEmojis[index] += 1
                }
            }

        }

    }

    reactionsChart = new Chart("reactionStats", {
        type: "bar",
        data: {
            labels: xValuesPopEmojis,
            datasets: [{
                backgroundColor: "green",
                data: yValuesPopEmojis,
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



function setUpForDownLoadPopEmojis(data) {
    xValuesPopEmojis.push("Start Date");
    yValuesPopEmojis.push(startDate);
    xValuesPopEmojis.push("End Date");
    yValuesPopEmojis.push(endDate);

    var obj = {};
    for (var i = 0; i < xValuesPopEmojis.length; i++) {
        obj[xValuesPopEmojis[i]] = yValuesPopEmojis[i];
    }

    downloadPopEmojis(obj, data);
}
function downloadPopEmojis(obj, data) {

    var element = document.createElement('a');

    if (data == 1) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(JSON.stringify(obj)));

        element.setAttribute('download', "PopularReaction.json");
    }
    if (data == 2) {
        var something = JSONToCSVConvertor(JSON.stringify(obj));
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(something));
        element.setAttribute('download', "PopularReaction.csv");
    }
    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}