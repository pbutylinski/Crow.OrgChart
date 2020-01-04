function initChart(data) {
    org_chart = $('#orgChart').orgChart({
        data: data,
        showControls: false,
        allowEdit: false,
        onClickNode: function (node) {
            window.location.href = 'Organization/Level/' + node.data.id;
        }
    });
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