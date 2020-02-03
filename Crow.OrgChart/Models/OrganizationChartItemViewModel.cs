using System.Collections.Generic;
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

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("managers")]
        public ICollection<string> Managers { get; set; } = new List<string>();

        [JsonPropertyName("members")]
        public ICollection<string> Members { get; set; } = new List<string>();
    }
}
