using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using FileHelpers;

namespace helper_utility
{
    [DelimitedRecord(",")]
    public partial class TaskGroupCollection
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public VariableGroup[] Value { get; set; }
    }

    [DelimitedRecord("!")]
    public partial class TaskGroupFormattedCollection
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public VariableGroupFormatted[] Value { get; set; }
    }

    public partial class TaskGroup
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        public override string ToString()
        {
            return this.Id;
        }
    }

    [DelimitedRecord("!")]
    public partial class TaskGroupFormatted
    {
        [JsonProperty("vgid")]
        public string VGId { get; set; }

        [JsonProperty("vgname")]
        public string VGName { get; set; }        
        public string ProjectId { get; set; }

        [JsonProperty("projectname")]
        public string ProjectName { get; set; }

        [JsonProperty("projectdescription")]
        public string ProjectDescription { get; set; }

        [JsonProperty("projecturl")]
        public string ProjectUrl { get; set; }

        [JsonProperty("projectvisibility")]
        public string ProjectVisibility { get; set; }

        [JsonProperty("projectlastUpdateTime")]
        public string ProjectLastUpdateTime { get; set; }
    }
}
