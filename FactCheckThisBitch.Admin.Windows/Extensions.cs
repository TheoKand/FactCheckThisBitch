using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FactCheckThisBitch.Admin.Windows
{
    public static class Extensions
    {
        public static void RemoveTabPage(this TabControl control, string idInTag)
        {
            foreach (TabPage page in control.TabPages)
            {
                if (page.Tag.ToString() == idInTag)
                {
                    control.TabPages.Remove(page);
                    return;
                }
            }
        }
    }
}
