using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using FileHelpers;

namespace helper_utility
{
    [DelimitedRecord(",")]
    public partial class VariableGroupCollection
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public VariableGroup[] Value { get; set; }
    }

    [DelimitedRecord("!")]
    public partial class VariableGroupFormattedCollection
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public VariableGroupFormatted[] Value { get; set; }
    }

    public partial class VariableGroup
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("createdBy")]
        public AuthoredBy CreatedBy { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        public override string ToString()
        {
            return this.Id;
        }
    }

    [DelimitedRecord("!")]
    public partial class VariableGroupFormatted
    {
        [JsonProperty("vgid")]
        public string VGId { get; set; }

        [JsonProperty("vgname")]
        public string VGName { get; set; }        
        public string ProjectId { get; set; }

        [JsonProperty("projectname")]
        public string ProjectName { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }        
    }
}
