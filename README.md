# SRXD Current Song Info Mod

When a song is selected in Spin Rhythm XD, this mod writes the song information to a JSON file. The write destination is **`CurrentSongInfo.json`** in the directory where Spin Rhythm XD (.exe) exists.

If you monitor the file with watchdog, etc., you can get information on the song currently being played. Might be useful for something...

### Example

```json
{
  "title": "Track title",
  "subtitle": "Subtitle",
  "artist": "Artist",
  "charter": "Charter name",
  "is_custom": true,
  "custom_filename": "FILENAME",
  "difficulty_type": "XD",
  "difficulty_rate": 23,
  "lowest_bpm": 160,
  "highest_bpm": 160,
  "duration": 199
}
```

## Requirements

This mod works with [BepInEx5](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.21).
Please refer to [this repository](https://github.com/SRXDModdingGroup/SpinSearch) for a detailed description of how to install BepInEx on Spin Rhythm XD.

## Installation

1. Download `SRXDCurrentSongInfo.zip` from the [latest release](https://github.com/voltaney/SRXDCurrentSongInfoMod/releases/latest)
2. Extract the zip file in the `Spin Rhythm/BepInEx/plugins` directory
