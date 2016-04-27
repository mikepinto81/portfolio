using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeHotKeyApp
{
    /// <summary>
    /// Useful for sending settings around to save/load
    /// </summary>
    class Settings
    {
        public bool sendToTray { get { return _sendToTray; } }
        private bool _sendToTray = false;    
        public void SetSendToTray(bool set) { _sendToTray = set; }
    }
}
