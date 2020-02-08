class TreeChart {
    /*
                        / V\
                      / `  /
                     <<   |
                     /    |
                   /      |
                 /        |
               /    \  \ /
              (      ) | |
      ________|   _/_  | |
    <__________\______)\__)
    */

    constructor(json_i, elemID) {

        if (json_i === null) {
            console.warn("No JSON specified!");
        }

        if (elemID === null) {
            elemID = "treeChart";
        }

        var convertJsonToChartConfig = (json_i) => {
            var nodesById = {};

            for (var i = 0; i < json_i.length; i++) {

                var node = {};

                if (json_i[i].name) {
                    node.text = {};
                    node.text.name = json_i[i].name;
                }

                if (json_i[i].url) {
                    node.link = {};
                    node.link.href = json_i[i].url;
                }

                if ((json_i[i].managers && json_i[i].managers.length > 0) ||
                    (json_i[i].members && json_i[i].members.length > 0)) {

                    node.innerHTML = '';

                    if (json_i[i].name) {
                        node.innerHTML = '<p class="node-name">' + json_i[i].name + '</p><hr>';
                    }

                    node.innerHTML += "<ul class='node-list'>";

                    if (json_i[i].managers) {
                        for (var j = 0; j < json_i[i].managers.length; j++) {
                            node.innerHTML += "<li class='node-menager'>- " + json_i[i].managers[j] + "</li>";
                        }
                    }

                    if (json_i[i].members) {
                        for (var j = 0; j < json_i[i].members.length; j++) {
                            node.innerHTML += "<li class='node-member'>- " + json_i[i].members[j] + "</li>";
                        }
                    }

                    node.innerHTML += "</ul>";
                }

                nodesById[json_i[i].id] = node;
            }

            var chart_config = [{}];

            for (var i = 0; i < json_i.length; i++) {
                var subNode = nodesById[json_i[i].id];

                if (json_i[i].parent) {
                    subNode.parent = nodesById[json_i[i].parent];
                }

                chart_config[i + 1] = subNode;
            }

            return chart_config;
        };

        var chartConfig = convertJsonToChartConfig(json_i);

        chartConfig[0] = {
            container: elemID,
            levelSeparation: 40,
            siblingSeparation: 25,
            subTeeSeparation: 50,
            nodeAlign: "BOTTOM",
            scrollbar: "fancy",
            padding: 15,
            node: { HTMLclass: "nodeTemplate" },
            connectors: {
                type: "step",
                style: {
                    "stroke-width": 2,
                    "stroke-dasharray": "- ",
                    "stroke-linecap": "round",
                    "stroke": "#444"
                }
            }
        };

        new Treant(chartConfig);
    }
}