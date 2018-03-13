using System.Windows.Forms;

namespace System
{
    internal class MouseEventHandler
    {
        private Action<object, MouseEventArgs> listViewAll_MouseClick;

        public MouseEventHandler(Action<object, MouseEventArgs> listViewAll_MouseClick)
        {
            this.listViewAll_MouseClick = listViewAll_MouseClick;
        }
    }
}