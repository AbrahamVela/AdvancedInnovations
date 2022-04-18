﻿
$(document).ready(function () {
    let detailsServerId = $("#ServerId").attr('value');
    $.ajax({
        type: 'GET',
        url: '../Stats/GetMessageInfoFromDatabase?serverid=' + detailsServerId,
        success: barGraphHourlyMessageActivity,
        error: handleError
    });

})

const timezone = -3

function handleError(xhr, ajaxOptions, thrownError) {
    console.log('ajax error: ' + xhr.status);
}

//function retrieveMessageInfoForServer(data) {

//    barGraphHourlyMessageActivity(data);
//};

function barGraphHourlyMessageActivity(data) {
    $("#usersHourlyAllTimeChart").empty();

    var count = 0;
    var xValues = ["4am", "5am", "6am", "7am", "8am", "9am", "10am", "11am", "12pm", "1pm", "2pm", "3pm", "4pm", "5pm", "6pm", "7pm", "8pm", "9pm", "10pm", "11pm", "12am", "1am", "2am", "3am"];
    var yValues = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
    //var yValues = [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1];


    for (var i = 0; i < data.length; i++) {
        let hour = new Date(data[i].createdAt).getHours();
        subtraction = hour - 1 + timezone;
        if (subtraction < 0) {
            subtraction = yValues.length + subtraction;
        }
        console.log(subtraction);
        yValues[subtraction] += 1;
    }
    console.log(xValues);
    console.log(yValues);



    new Chart("usersHourlyAllTimeChart", {
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