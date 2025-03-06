using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace SRXDCurrentSongInfo
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(Patch), PluginInfo.PLUGIN_GUID);
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded");
        }
    }



    public class Patch
    {
        private static string FileName = "CurrentSongInfo.json";

        [HarmonyPatch(typeof(XDSelectionListItemDisplay_Track), "PlayOnCurrentDifficulty")]
        [HarmonyPrefix]
        private static void Play_Prefix(XDSelectionListItemDisplay_Track __instance)
        {
            var metadata = __instance.Item.GetMetadata();
            var track_info_metadata = __instance.Item.GetMetadata().TrackInfoMetadata;
            var track_data_metadata = __instance.Item.GetTrackDataMetadata();

            var trackData = new Dictionary<string, object>
            {
                { "title", track_info_metadata.title },
                { "subtitle", track_info_metadata.subtitle },
                { "artist", track_info_metadata.artistName },
                { "charter", track_info_metadata.charter },
                { "is_custom", metadata.IsCustom },
                { "difficulty_type", track_data_metadata.DifficultyType.ToString() },
                { "difficulty_rate", track_data_metadata.DifficultyRating },
                { "lowest_bpm", track_data_metadata.LowestBpm },
                { "highest_bpm", track_data_metadata.HighestBpm },
                { "duration", track_data_metadata.Duration }
            };

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
            };

            string jsonString = JsonConvert.SerializeObject(trackData, settings);

            File.WriteAllText(FileName, jsonString);
        }
    }
}
