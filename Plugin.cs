using BepInEx;
using HarmonyLib;
using System.IO;
using System.Text;

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
        [HarmonyPatch(typeof(XDSelectionListItemDisplay_Track), "PlayOnCurrentDifficulty")]
        [HarmonyPrefix]
        private static void Play_Prefix(XDSelectionListItemDisplay_Track __instance)
        {
            ITrackItem trackItem = __instance.Item.item as ITrackItem;
            var metadata = __instance.Item.GetMetadata();
            var track_info_metadata = __instance.Item.GetMetadata().TrackInfoMetadata;
            var track_data_metadata = __instance.Item.GetTrackDataMetadata();

            // JSONフォーマットの文字列中の特殊文字扱い「\」「/」「"」をエスケープ
            // https://www.json.org/json-en.html
            var title = track_info_metadata.title.Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("/", @"\/");
            var subtitle = track_info_metadata.subtitle.Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("/", @"\/");
            var artist = track_info_metadata.artistName.Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("/", @"\/");
            var charter = track_info_metadata.charter.Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("/", @"\/");

            using (var writer = new StreamWriter("CurrentSongInfo.json", false))
            {
                writer.WriteLine("{");
                writer.WriteLine($"\"title\": \"{title}\",");
                writer.WriteLine($"\"subtitle\": \"{subtitle}\",");
                writer.WriteLine($"\"artist\": \"{artist}\",");
                writer.WriteLine($"\"charter\": \"{charter}\",");
                writer.WriteLine($"\"is_custom\": {metadata.IsCustom.ToString().ToLower()},");
                writer.WriteLine($"\"difficulty_type\": \"{track_data_metadata.DifficultyType.ToString()}\",");
                writer.WriteLine($"\"difficulty_rate\": {track_data_metadata.DifficultyRating},");
                writer.WriteLine($"\"lowest_bpm\": {track_data_metadata.LowestBpm},");
                writer.WriteLine($"\"highest_bpm\": {track_data_metadata.HighestBpm},");
                writer.WriteLine($"\"duration\": {(int)track_data_metadata.Duration}");
                writer.WriteLine("}");
            }
        }
    }
}
