using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VolumeHotKeyApp
{
    class SaveLoad
    {
        static string saveDirectoryPath { get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\VolumeHotKeyApp"; } }
        static string saveFilePath { get { return saveDirectoryPath + "\\VolumeHotKeyApp.settings"; } }

        //send a Settings object to get data.  Eventually move KeyHandler data to Settings object
        public static void Save(KeyHandler upKey, KeyHandler downKey, KeyHandler muteKey, Settings extraSettingsObject)
        {

            Directory.CreateDirectory(saveDirectoryPath);
            string savedata = "";

            //first save on or off
            bool on = true;
            if (!upKey.isRegistered || !downKey.isRegistered)
                on = false;
            savedata += on.ToString() + Environment.NewLine;

            //save up key
            savedata += upKey.getKey.ToString() + Environment.NewLine;

            //save down key
            savedata += downKey.getKey.ToString() + Environment.NewLine;

            //save mods
            savedata += downKey.getModifiers[0];
            for(int i = 1; i < downKey.getModifiers.Count; i++)
            {
                savedata += "," + downKey.getModifiers[i];
            }
            savedata += Environment.NewLine;

            //save minimize to tray
            savedata += extraSettingsObject.sendToTray.ToString() + Environment.NewLine;

            //save mute key
            savedata += muteKey.getKey.ToString() + Environment.NewLine;

            //write to file
            File.WriteAllText(saveFilePath, savedata);
        }

        public static bool Load(KeyHandler upKey, KeyHandler downKey, KeyHandler muteKey, Settings extraSettings)
        {
            if (!File.Exists(saveFilePath) || downKey == null || upKey == null)
                return false;

            //get array of lines from the save file
            string[] lines = File.ReadAllLines(saveFilePath);

            //on off
            bool on = true;
            bool.TryParse(lines[0], out on);

            //up
            int savedupKey = 0;
            if (!int.TryParse(lines[1], out savedupKey))
                return false;

            //down
            int saveddownKey = 0;
            if (!int.TryParse(lines[2], out saveddownKey))
                return false;

            //mods
            string[] mods = lines[3].Split(',');

            //minimize to tray
            bool minimizeToTray = false;
            if (lines.Count() > 4)
                bool.TryParse(lines[4], out minimizeToTray);

            //mute key
            int savedmuteKey = 0;
            if (lines.Count() > 5)
                int.TryParse(lines[5], out savedmuteKey);                    
            

            //set variables
            //**note that this should eventually be moved to changing the Settings object and then the caller
            //would change the KeyHandlers like what happens with minimizeToTray
            upKey.SetKey(savedupKey);
            downKey.SetKey(saveddownKey);
            if (lines.Count() > 5) //make sure mute key was saved
                muteKey.SetKey(savedmuteKey);

            //set mods
            upKey.SetModifiers(new List<string>(mods));
            downKey.SetModifiers(new List<string>(mods));
            muteKey.SetModifiers(new List<string>(mods));
            

            if (on)
            {
                upKey.Register();
                downKey.Register();
            }

            extraSettings.SetSendToTray(minimizeToTray);

            return true;
        }
    }
}
