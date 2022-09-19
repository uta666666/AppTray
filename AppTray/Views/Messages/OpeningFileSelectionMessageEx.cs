using Livet.Messaging.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppTray.Views.Messages
{
    public class OpeningFileSelectionMessageEx : OpeningFileSelectionMessage
    {
        public OpeningFileSelectionMessageEx() : base() { }

        public OpeningFileSelectionMessageEx(string messageKey) : base(messageKey) { }

        public bool DereferenceLinks { get; set; } = true;

        public static DependencyProperty DereferenceLinksProperty = DependencyProperty.Register(nameof(DereferenceLinks), typeof(bool), typeof(OpeningFileSelectionMessageEx), new PropertyMetadata(null));
    }
}
