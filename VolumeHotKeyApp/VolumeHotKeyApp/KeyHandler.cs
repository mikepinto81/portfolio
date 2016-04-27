using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace VolumeHotKeyApp
{
    public static class Constants
    {
        //windows message id for hotkey
        public const int WM_HOTKEY_MSG_ID = 0x0312;

        //Modifiers
        public enum Modifiers { NOMOD = 0, ALT = 1, CTRL = 2, SHIFT = 4, WIN = 8 };
        
        //Convert Modifiers to readable strings
        public static string ModToReadableString(Modifiers mod)
        {
            switch(mod)
            {
                case (Modifiers.WIN):
                    return "Windows Key";
                case (Modifiers.NOMOD):
                    return "NONE";
                default:
                    return mod.ToString();
            }
        }

        //Get Modifier from string
        public static Modifiers ReadableStringToMod(string inputString)
        {
            switch(inputString)
            {
                case ("CTRL"):
                    return Modifiers.CTRL;
                case ("ALT"):
                    return Modifiers.ALT;
                case ("SHIFT"):
                    return Modifiers.SHIFT;
                case ("Windows Key"):
                    return Modifiers.WIN;
                case ("WIN"):
                    return Modifiers.WIN;
                default:
                    return Modifiers.NOMOD;
            }
        }
    }

    public class KeyHandler
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        //key we are hooking into
        private int key;
        public int getKey { get { return key; } }

        private IntPtr hWnd;
        
        //unique id for this handler
        private int id;
        public int getID { get { return id; } }

        //private int _modifiers;
        private List<Constants.Modifiers> _modifiers;     
        public List<Constants.Modifiers> getModifiers { get { return _modifiers; } }   

        //has this KeyHandler been registered
        bool registered = false;
        public bool isRegistered { get { return registered; } }

        //Constructor
        public KeyHandler(Keys key, List<Constants.Modifiers> modifiers, Form form)
        {
            this.key = (int)key;
            this.hWnd = form.Handle;
            id = this.GetHashCode();
            _modifiers = new List<Constants.Modifiers>(modifiers);           
        }

        //Destructor
        ~KeyHandler()
        {
            if(registered)
                Unregister();
        }

        //get hash code from key
        public override int GetHashCode()
        {
            return key ^ hWnd.ToInt32();
        }

        public void SetKey(Keys key)
        {
            this.key = (int)key;      
        }

        public void SetKey(int key)
        {
            this.key = key;
        }

        public void SetKey(string key)
        {
            
            Keys newKey;
            if (Enum.TryParse<Keys>(key, out newKey))
                SetKey(newKey);
            else
                Console.Write("Error: " + key + " not valid.");
            
            
        }

        //Register hotkey
        public bool Register()
        {
            registered = true;
            return RegisterHotKey(hWnd, id, SumModifiers(), key);
        }

        //unregister hotkey
        public bool Unregister()
        {
            registered = false;
            return UnregisterHotKey(hWnd, id);
        }

        //reset registration (like when you change modifiers)
        public void ResetRegistration()
        {
            if (registered)
                Unregister();
            Register();
        }
        //*********************
        //Modifier methods

        //add together all modifiers so they can be used like CTRL + ALT + ...
        public int SumModifiers()
        {
            int returnint = 0;
            foreach (Constants.Modifiers mod in _modifiers)
            {
                returnint += (int)mod;
            }

            return returnint;
        }

        //remove a modifier
        public void RemoveModifier(Constants.Modifiers mod)
        {
            _modifiers.Remove(mod);            
        }

        //add a modifier
        public void AddModifier(Constants.Modifiers mod)
        {
            if (!_modifiers.Contains(mod))
                _modifiers.Add(mod);
        }

        //clear modifiers
        public void ClearModifiers()
        {
            _modifiers.Clear();
        }

        //set list of modifiers
        public void SetModifiers(List<string> newMods)
        {
            //clear old mods            
            ClearModifiers();

            //set new mods
            foreach (string item in newMods)
            {
                AddModifier(Constants.ReadableStringToMod(item));                
            }
        }

    }
}
