using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using FileHelpers;

namespace helper_utility
{

    [DelimitedRecord(",")]
    public partial class BuildPipelineCollection
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public BuildPipeline[] Value { get; set; }
    }

    [DelimitedRecord("!")]
    public partial class BuildPipelineFormattedCollection
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public BuildPipelineFormatted[] Value { get; set; }
    }

    public partial class BuildPipeline
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("queueStatus")]
        public string QueueStatus { get; set; }

        [JsonProperty("createdDate")]
        public string CreatedDate { get; set; }
        
        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("authoredBy")]
        public AuthoredBy AuthoredBy { get; set; }
        
        public override string ToString()
        {
            return this.Id;
        }
    }

    [DelimitedRecord("!")]
    public partial class BuildPipelineFormatted
    {
        [JsonProperty("buildpipelneid")]
        public string BuildPipelneId { get; set; }

        [JsonProperty("buildpipelinename")]
        public string BuildPipelineName { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("queuestatus")]
        public string QueueStatus { get; set; }

        [JsonProperty("createddate")]
        public string CreatedDate { get; set; }

        [JsonProperty("displayname")]
        public string DisplayName { get; set; }

        [JsonProperty("uniquename")]
        public string UniqueName { get; set; }

        [JsonProperty("projectid")]
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
