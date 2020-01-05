using System.Text.Json.Serialization;

namespace Crow.OrgChart.Models
{
    public class OrganizationChartItemViewModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("parent")]
        public string Parent { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
