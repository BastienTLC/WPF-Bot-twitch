using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace BotTwitchInterface
{
   public class Program
    {
        private static string _broadcasterName = "gtp_goupil29";
        private static string ColorMode = "off";
        private static string ComValue = "COM13";
        public string Couleur;
        public string message;
        public Program(IrcClient irc, SerialPort sp, ref string message2)
        {

            message = irc.ReadMessage();
            //irc.SendPublicChatMessage(message);
            if (message.Contains("PRIVMSG"))
            {
                message2 = message;
            }
            Console.WriteLine(message); // Print raw irc messages

            if (message.Contains("PRIVMSG"))
            {

                int intIndexParseSign = message.IndexOf('!');
                string userName = message.Substring(1, intIndexParseSign - 1);
                intIndexParseSign = message.IndexOf(" :");
                message = message.Substring(intIndexParseSign + 2);

                if (userName.Equals(_broadcasterName) || (userName.Equals("wizebot")))
                {

                    if (message.Equals("!exitbot") || message.Contains("Le LIVE est OFFLINE"))
                    {
                        randomColor();
                        irc.SendPublicChatMessage("Bye! Have a beautiful time!");
                        RunExecutor("!off", userName);
                        Environment.Exit(0);

                    }
                    else if (message.Contains("FOLLOW"))
                    {
                        randomColor();
                        irc.SendPublicChatMessage("Bye! Have a beautiful time!");
                        RunExecutor("!follow", userName);
                    }
                }


                if (message.Equals("!info"))
                {
                    randomColor();
                    irc.SendPublicChatMessage("/me imGlitch BOT QUI CHANGE LA COULEUR DES NEONS -> !infocommande imGlitch");
                }

                else if (message.Equals("!infocommande"))
                {
                    randomColor();
                    irc.SendPublicChatMessage("/me PurpleStar !wave !cyclergb !multi !static !off !suprcar !flu !wip !fad !runing !meteo");
                }

                else if (message.Equals("!cyclergb") || (message.Equals("!off")) || (message.Equals("!wave") || (message.Equals("!multi"))))
                {
                    ColorMode = message;
                    RunExecutor(ColorMode, userName);
                }


                else if (message.Contains("!static") || message.Contains("!suprcar") || message.Contains("!flu") || message.Contains("!wip") || message.Contains("!fad") || message.Contains("!runing") || message.Contains("!meteo"))
                {

                    try
                    {
                        ColorMode = message;
                        string[] final = ColorMode.Split(' ');
                        ColorMode = (final[0] + " " + final[1] + " " + final[2] + " " + final[3]);
                        RunExecutor(ColorMode, userName);
                    }
                    catch
                    {
                        randomColor();
                        ColorMode = message;
                        irc.SendPublicChatMessage("/me " + message + " RRR GGG BBB ( MIN 0, MAX 255 ) ou message + !colorlist");

                    }
                }


            }
            //}

            void RunExecutor(string _ColorMode, string sendComName)
            {
                try
                {
                    sp.PortName = ComValue;
                    sp.BaudRate = 9600;
                    sp.Open();

                    if (sp.IsOpen)
                    {
                        sp.WriteLine(_ColorMode);
                        while (sp.ReadLine() != _ColorMode)
                        {
                            sp.WriteLine(_ColorMode);
                            Console.WriteLine(sp.ReadLine());
                            Console.WriteLine(" ERROR");

                        }
                        Console.WriteLine(sp.ReadLine() + " SUCCES");
                        irc.SendPublicChatMessage("/me " + sendComName + " Applique " + _ColorMode + " sur le neon");
                        sp.Close();
                    }

                    else
                    {
                        sp.PortName = ComValue;
                        sp.BaudRate = 9600;
                        sp.Open();
                    }


                }
                catch
                {

                    irc.SendPublicChatMessage("/me Une erreur est survenue. ");
                }
            }

            void randomColor()
            {

                Random random = new Random();

                string[] ColorArray = new string[] { "Blue", "BleuViolet", "CadetBlue", "Chocolate", "Coral", "DodgerBlue", "Firebrick", "GoldenRod", "Green", "HotPink" };
                int numIndex = random.Next(ColorArray.Length);
                irc.SendPublicChatMessage("/color " + ColorArray[numIndex]);


            }

        }
    }
}
