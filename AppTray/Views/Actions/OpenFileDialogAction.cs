using AppTray.Views.Messages;
using Livet.Behaviors.Messaging.IO;
using Livet.Messaging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTray.Views.Actions
{
    public class OpenFileDialogAction : OpenFileDialogInteractionMessageAction
    {
        protected override void InvokeAction(InteractionMessage message)
        {
            var msg = message as OpeningFileSelectionMessageEx;

            var dialog = new OpenFileDialog();
            dialog.DereferenceLinks = msg.DereferenceLinks;
            dialog.Multiselect = msg.MultiSelect;
            dialog.Title = msg.Title;
            dialog.Filter = msg.Filter;
            dialog.AddExtension = msg.AddExtension;
            dialog.InitialDirectory = msg.InitialDirectory;
            dialog.FileName = msg.FileName;

            if (dialog.ShowDialog() != true)
            {
                msg.Response = null;
                    return;
            }
            msg.Response = dialog.FileNames;
        }

    }
}
