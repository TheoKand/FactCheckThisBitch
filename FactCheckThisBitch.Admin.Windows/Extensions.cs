﻿using System.Drawing;
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

        public static string ToCoordinates(this Rectangle input)
        {
            return $"{input.Left},{input.Top}-{input.Right},{input.Bottom}";
        }
    }
}