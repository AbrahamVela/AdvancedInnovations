var xValuesEmojis = [];
var yValuesEmojis = [];

$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    $.ajax({
        type: 'GET',
        url: '../Stats/GetMessageInfoFromDatabase?serverid=' + detailsServerId,
        success: barGraphHourlyEmojiActivity,
        error: handleError
    });
})

function GetEmojis(data) {
    setUpForDownLoadEmojis(data);
};


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
    xValuesEmojis = [];
    yValuesEmojis = [];

    for (var i = 0; i < data.length; i++) {
        var date = new Date(data[i].createdAt)
        //var date = new Date(Date.UTC(dateUTC.getUTCFullYear(), dateUTC.getMonth(), dateUTC.getDate(), dateUTC.getHours()))


        if (date > startDate && date < endDate) {
            var emojis = data[i].emojis.split(",")
            for (var e = 0; e < emojis.length - 1; e++) {
                var index;
                var duplicate = false;
                for (var j = 0; j < xValuesEmojis.length; j++) {
                    if (duplicate == true) {
                        break
                    }
                    if (xValuesEmojis[j] == String.fromCodePoint("0x" + emojis[e])) {
                        index = j;
                        duplicate = true
                    }
                }
                if (duplicate == true) {
                    yValuesEmojis[index] += 1
                }
                if (duplicate == false) {
                    xValuesEmojis.push(String.fromCodePoint("0x" + emojis[e]));
                    yValuesEmojis.push(1);
                }

            }

        }

    }

    emojisChart = new Chart("emojiStats", {
        type: "bar",
        data: {
            labels: xValuesEmojis,
            datasets: [{
                backgroundColor: "green",
                data: yValuesEmojis,
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


function setUpForDownLoadEmojis(data) {
    xValuesEmojis.push("Start Date");
    yValuesEmojis.push(startDate);
    xValuesEmojis.push("End Date");
    yValuesEmojis.push(endDate);

    var obj = {};
    for (var i = 0; i < xValuesEmojis.length; i++) {
        obj[xValuesEmojis[i]] = yValuesEmojis[i];
    }

    downloadEmojis(obj, data);
}
function downloadEmojis(obj, data) {

    var element = document.createElement('a');

    if (data == 1) {
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(JSON.stringify(obj)));

        element.setAttribute('download', "Emoji.json");
    }
    if (data == 2) {
        var something = JSONToCSVConvertor(JSON.stringify(obj));
        element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(something));
        element.setAttribute('download', "Emoji.csv");
    }
    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}