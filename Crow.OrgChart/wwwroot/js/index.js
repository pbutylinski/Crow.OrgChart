function initChart(data) {
    new TreeChart(data, "#orgChart");
}

$(function () {
    $.ajax({
        type: "GET",
        url: 'Home/ChartData',
        success: function (data) {
            initChart(data);
            console.log(data);
        }
    });
});