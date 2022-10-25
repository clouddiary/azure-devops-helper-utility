using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using FileHelpers;

namespace helper_utility
{
    public partial class UserCollection
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public User[] Value { get; set; }
    }

    [DelimitedRecord("!")]
    public partial class User
    {
        [JsonProperty("originId")]
        public string OriginId { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("subjectKind")]
        public string SubjectKind { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("mailAddress")]
        public string MailAddress { get; set; }             

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("directoryAlias")]
        public string DirectoryAlias { get; set; }       
    } 
}
