using BTCore;
using BTCore.Classes;
using BTCore.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTDesigner
{
    public partial class WindowManager : Form
    {
        public static Form CurrentWindow =null;

        public WindowManager()
        {
            InitializeComponent();
            InitializeProperties();
        }


        private void WindowManager_Activated(object sender, EventArgs e)
        {
            SetCurrentWindow();
        }

        private void SetCurrentWindow()
        {
                    bool res = Core.CurrentProfile.IsAuthenticated;
            if (res)
            {
                CurrentWindow = new WMain();
            }
            else
            {
                CurrentWindow = new WAuthenticate();
            }
            CurrentWindow.ShowDialog();
        }


        private void InitializeProperties()
        {
            new Core();
        }
    }
}
