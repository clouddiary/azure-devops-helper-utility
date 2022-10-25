using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using FileHelpers;


namespace helper_utility
{
    public partial class MembersCollection
    {
        [JsonProperty("totalCount")]
        public long totalCount { get; set; }

        [JsonProperty("members")]
        public Members[] Members { get; set; }
    }

    [DelimitedRecord("!")]
    public partial class Members
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }        
    }

    public partial class FormattedMembersCollection
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public FormattedMembers[] Value { get; set; }
    }

    [DelimitedRecord(",")]
    public partial class FormattedMembers
    {
        [JsonProperty("groupid")]
        public string GroupId { get; set; }

        [JsonProperty("GroupId")]
        public string GroupName { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }        
    }
}
