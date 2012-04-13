using System.Windows.Forms;
using Limaki.Viewers;
using DialogResult = Limaki.Viewers.DialogResult;
using Limaki.View.Swf;
using MessageBoxButtons = Limaki.Viewers.MessageBoxButtons;

namespace Limaki.Swf.Backends {

    public class MessageBoxShow : IMessageBoxShow {

        public DialogResult Show(string title, string text, MessageBoxButtons buttons) {
            return Converter.Convert(MessageBox.Show(text, title, Converter.Convert(buttons)));
        }

    }
}