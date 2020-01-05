function initChart(data) {
    org_chart = $('#orgChart').orgChart({
        data: data,
        showControls: false,
        allowEdit: false,
        onClickNode: function (node) {
            if (node.data.id === '00000000-0000-0000-0000-000000000000') {
                window.location.href = 'Organization';
            }
            else {
                window.location.href = 'Organization/Level/' + node.data.id;
            }
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