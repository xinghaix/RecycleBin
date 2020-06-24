using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using CSDeskBand;
using CSDeskBand.ContextMenu;

namespace RecycleBin
{
    [ComVisible(true)]
    [Guid("40FCD8E9-CE73-415D-A186-0603DD526F56")]
    [CSDeskBandRegistration(Name = "Sample wpf", ShowDeskBand = true)]
    public class Deskband : CSDeskBandWpf
    {
        public Deskband()
        {
            Options.ContextMenuItems = ContextMenuItems;
            // Options.MinHorizontalSize = new Size(90, 30);
        }

        protected override UIElement UIElement => new UserControl2();
        
        private List<DeskBandMenuItem> ContextMenuItems
        {
            get
            {
                var action = new DeskBandMenuAction("Action");
                return new List<DeskBandMenuItem>() { action };
            }
        }

        // private List<DeskBandMenuItem> ContextMenuItems
        // {
        //     get
        //     {
        //         var action = new DeskBandMenuAction("Action - Toggle submenu");
        //         var separator = new DeskBandMenuSeparator();
        //         var submenuAction = new DeskBandMenuAction("Submenu Action - Toggle checkmark");
        //         var submenu = new DeskBandMenu("Submenu")
        //         {
        //             Items = {submenuAction}
        //         };
        //
        //         action.Clicked += (sender, args) => submenu.Enabled = !submenu.Enabled;
        //         submenuAction.Clicked += (sender, args) => submenuAction.Checked = !submenuAction.Checked;
        //
        //         return new List<DeskBandMenuItem>() {action, separator, submenu};
        //     }
        // }
    }
}