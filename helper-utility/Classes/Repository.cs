using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using FileHelpers;

namespace helper_utility
{
    [DelimitedRecord(",")]
    public partial class RepositoryCollection
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public Repository[] Value { get; set; }
    }

    [DelimitedRecord("!")]
    public partial class RepositoryFormattedCollection
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public RepositoryFormatted[] Value { get; set; }
    }

    public partial class Repository
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("weburl")]
        public string WebUrl { get; set; }

        [JsonProperty("repourl")]
        public string RepoUrl { get; set; }

        public Project Project { get; set; }

        public override string ToString()
        {
            return this.Id;
        }
    }

    [DelimitedRecord("!")]
    public partial class RepositoryFormatted
    {
        [JsonProperty("repoid")]
        public string RepoId { get; set; }

        [JsonProperty("reponame")]
        public string RepoName { get; set; }

        [JsonProperty("reposize")]
        public string RepoSize { get; set; }

        [JsonProperty("repourl")]
        public string RepoUrl { get; set; }

        [JsonProperty("repoweburl")]
        public string RepoWebUrl { get; set; }

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
