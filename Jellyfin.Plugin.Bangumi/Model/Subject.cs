﻿using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Jellyfin.Plugin.Bangumi.Configuration;

namespace Jellyfin.Plugin.Bangumi.Model
{
    public class Subject
    {
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string OriginalName { get; set; } = null!;

        [JsonPropertyName("name_cn")]
        public string? ChineseName { get; set; }

        [JsonIgnore]
        public string Name => Plugin.Instance!.Configuration.TranslationPreference switch
        {
            TranslationPreferenceType.Chinese => string.IsNullOrEmpty(ChineseName) ? OriginalName : ChineseName,
            TranslationPreferenceType.Original => OriginalName,
            _ => OriginalName
        };

        public string? Summary { get; set; }

        [JsonPropertyName("date")]
        public string? AirDate { get; set; }

        public Dictionary<string, string>? Images { get; set; }

        [JsonIgnore]
        public string? DefaultImage => Images?["large"];

        [JsonPropertyName("eps")]
        public int? EpisodeCount { get; set; }

        [JsonPropertyName("rating")]
        public Rating? Rating { get; set; }

        [JsonPropertyName("tags")]
        public List<Tag> Tags { get; set; } = new();

        [JsonIgnore]
        public string[] PopularTags
        {
            get
            {
                var baseline = Tags.Sum(tag => tag.Count) / 25;
                return Tags.Where(tag => tag.Count >= baseline).Select(tag => tag.Name).ToArray();
            }
        }
    }
}