using System;
using System.IO;
using System.Windows;

namespace AppTray
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        [System.STAThreadAttribute()]
        static public void Main()
        {
            const string SemaphoreName = "AppTray";
            bool createdNew;

            // Semaphoreクラスのインスタンスを生成し、アプリケーション終了まで保持する
            using (var semaphore = new System.Threading.Semaphore(1, 1, SemaphoreName, out createdNew))
            {
                if (!createdNew)
                {
                    // 他のプロセスが先にセマフォを作っていた
                    WriteLog("すでに起動しています (AppTray)");
                    return; // プログラム終了
                }
                var app = new App();
                app.InitializeComponent();
                app.StartupUri = new Uri("Views/MainWindow.xaml", UriKind.Relative);
                app.DispatcherUnhandledException += (o, e) => WriteLog(e.Exception);
                app.Run();
            }
        }

        private static void WriteLog(Exception ex)
        {
            File.AppendAllText(@".\applog.log", $"[{DateTime.Now.ToString("yyyyMMddHHmmss")}] {ex.ToString()}" + Environment.NewLine);
        }

        private static void WriteLog(string message)
        {
            File.AppendAllText(@".\applog.log", $"[{DateTime.Now.ToString("yyyyMMddHHmmss")}] {message}" + Environment.NewLine);
        }
    }
}
