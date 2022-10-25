using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using FileHelpers;

namespace helper_utility
{
    public partial class GroupCollection
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public User[] Value { get; set; }
    }

    [DelimitedRecord("!")]
    public partial class Group
    {
        [JsonProperty("originId")]
        public string OriginId { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("principalName")]
        public string PrincipalName { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        
        [JsonProperty("subjectKind")]
        public string SubjectKind { get; set; }
               
    } 
}
