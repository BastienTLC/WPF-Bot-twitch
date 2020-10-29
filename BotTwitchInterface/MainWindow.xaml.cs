using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;
using System.ComponentModel;

namespace BotTwitchInterface
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string _botName = "bot_goupil";
        private static string _broadcasterName = "gtp_goupil29";
        private static string _twitchOAuth = "oauth:hykpdaxldn0rqyka5rohqwij6xpdzs";   //"oauth:xhjsna1pjxzm4p40w72hz1g5e5e7cg  hykpdaxldn0rqyka5rohqwij6xpdzs" // get chat bot's oauth from www.twitchapps.com/tmi/

        string test="ou";
        int i = 1;

        IrcClient irc;
        SerialPort sp;
        BackgroundWorker bgworker = new BackgroundWorker();

        public MainWindow()
        {
                InitializeComponent();
            irc = new IrcClient("irc.twitch.tv", 6667,
                        _botName, _twitchOAuth, _broadcasterName);

            sp = new SerialPort();

            PingSender ping = new PingSender(irc);
            ping.Start();

            Program _program = new Program(irc, sp, ref test);
            AswerTchat.Text = "RIEN";

            bgworker.DoWork += Bgworker_DoWork;
            bgworker.ProgressChanged += Bgworker_ProgressChanged;
            bgworker.WorkerReportsProgress = true;
            bgworker.WorkerSupportsCancellation = true;
        }

        private void Bgworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            char[] splitChars = { '#' };
            string[] AnswerRep = test.Split(splitChars);
            foreach(string x in AnswerRep)
            {
                AswerTchat.Text = x + "\n";
                //AswerTchat.Text =  AswerTchat.Text + x + "\n";

            }
            cmp.Content = i;
        }

        private void Bgworker_DoWork(object sender, DoWorkEventArgs e)
        {

                while (true)
                {
                    bgworker.ReportProgress(i);
                    Program _program = new Program(irc, sp, ref test);
                    if (bgworker.CancellationPending)
                    {
                        break;
                    }
                    System.Threading.Thread.Sleep(100);
                    i++;
                 }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!bgworker.IsBusy)
            {
                bgworker.RunWorkerAsync();
                Start.Content = "STOP";
            }
            else
            {
                bgworker.CancelAsync();
                Start.Content = "START";
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Message.Text = test;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
