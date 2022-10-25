using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using FileHelpers;

namespace helper_utility
{

    [DelimitedRecord(",")]
    public partial class ReleasePipelineCollection
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public ReleasePipeline[] Value { get; set; }
    }

    [DelimitedRecord("!")]
    public partial class ReleasePipelineFormattedCollection
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public ReleasePipelineFormatted[] Value { get; set; }
    }

    public partial class ReleasePipeline
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

       
        [JsonProperty("createdOn")]
        public string CreatedDate { get; set; }       

        [JsonProperty("createdBy")]
        public AuthoredBy CreatedBy { get; set; }

        [JsonProperty("modifiedOn")]
        public string ModifiedDate { get; set; }

        [JsonProperty("modifiedBy")]
        public AuthoredBy ModifiedBy { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        public override string ToString()
        {
            return this.Id;
        }
    }

    [DelimitedRecord("!")]
    public partial class ReleasePipelineFormatted
    {
        [JsonProperty("releasepipelneid")]
        public string ReleasePipelneId { get; set; }

        [JsonProperty("releasepipelinename")]
        public string ReleasePipelineName { get; set; }      

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
