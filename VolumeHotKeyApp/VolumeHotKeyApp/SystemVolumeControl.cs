using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VolumeHotKeyApp
{
    class SystemVolumeControl
    {
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        Form form;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public SystemVolumeControl(Form newForm)
        {
            form = newForm;
        }

        public void Mute()
        {
            SendMessageW(form.Handle, WM_APPCOMMAND, form.Handle,
                (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        public void VolDown()
        {
            SendMessageW(form.Handle, WM_APPCOMMAND, form.Handle,
                (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        public void VolUp()
        {
            SendMessageW(form.Handle, WM_APPCOMMAND, form.Handle,
                (IntPtr)APPCOMMAND_VOLUME_UP);
        }
    }
}
