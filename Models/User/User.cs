﻿using Newtonsoft.Json;

namespace DiscordBot.Models.User
{
    public partial class User
    {
        [JsonProperty("object", Required = Required.Always)]

        public string Object { get; set; }
        [JsonProperty("data", Required = Required.Always)]
        public UserDatum[] Data { get; set; }
    }
    
}
