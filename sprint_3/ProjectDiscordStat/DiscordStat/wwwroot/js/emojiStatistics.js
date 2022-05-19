$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    $.ajax({
        type: 'GET',
        url: '../Stats/GetMessageInfoFromDatabase?serverid=' + detailsServerId,
        success: barGraphHourlyEmojiActivity,
        error: handleError
    });
})

var emojiActivityData = [];
var tempEmojiActivityData = [];
var emojisChart;
var startDate = new Date($("#startDateGraph").val() + " 23:59");
var endDate = new Date($("#endDateGraph").val() + " 23:59");


function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}


$("#allUsers").change(function () {
    $("#emojiStats").empty();

    var newList = []
    if ($(this).val() == "All Users") {

        if (emojisChart != null) {
            emojisChart.destroy();
        }
        tempEmojiActivityData = emojiActivityData
        graphingEmojiActivity(tempEmojiActivityData)
    }
    else {
        for (var i = 0; i < emojiActivityData.length; i++) {
            if (emojiActivityData[i].userId == $(this).val()) {
                newList.push(emojiActivityData[i]);
            }
        }
        if (emojisChart != null) {
            emojisChart.destroy();
        }
        tempEmojiActivityData = newList
        graphingEmojiActivity(tempEmojiActivityData)
    }
});

$("#startDateGraph").change(function () {
    startDate = new Date($(this).val() + " 23:59");
    if (emojisChart != null) {
        emojisChart.destroy();
    }
    graphingEmojiActivity(tempEmojiActivityData);

});

$("#endDateGraph").change(function () {
    endDate = new Date($(this).val() + " 23:59");
    if (emojisChart != null) {
        emojisChart.destroy();
    }
    graphingEmojiActivity(tempEmojiActivityData);
});

function barGraphHourlyEmojiActivity(data) {
    emojiActivityData = data
    tempEmojiActivityData = data
    graphingEmojiActivity(tempEmojiActivityData)
}

function graphingEmojiActivity(data) {
    $("#emojiStats").empty();
    //Good Chart
    var xValues = [];
    var yValues = [];

    for (var i = 0; i < data.length; i++) {
        var date = new Date(data[i].createdAt)
        //var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))

        
        if (date > startDate && date < endDate) {
            var emojis = data[i].emojis.split(",")
            for (var e = 0; e < emojis.length - 1; e++) {


                if (xValues.includes(String.fromCodePoint("0x" + emojis[e])) == false) {
                    xValues.push(String.fromCodePoint("0x" + emojis[e]))
                    yValues.push(1)
                }
                else if (xValues.includes(String.fromCodePoint("0x" + emojis[e]))) {
                    xValues.some(function (entry, i) {
                        if (entry == String.fromCodePoint("0x" + emojis[e])) {
                            index = i;
                            return true;
                        }
                    });
                    yValues[index] += 1
                }
            }

        }

    }

    emojisChart = new Chart("emojiStats", {
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
                    text: "Most Popular Emoji",
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
                        text: 'Emojis Sent',
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