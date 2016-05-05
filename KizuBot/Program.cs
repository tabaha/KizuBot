using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using System.IO;
using System.Text.RegularExpressions;

namespace KizuBot {
    class Program {

        static void Main(string[] args) => new Program().Start();

        private DiscordClient _client;
        private Modules.PixivModule _PixivM;
        private Modules.STDOutModule _StdoutM;
        private string _BotToken;

        public void Start() {
            _client = new DiscordClient();
            _PixivM = new Modules.PixivModule();
            _PixivM.AddToAllowedChannels(139223862402351104);
            _PixivM.AddToAllowedChannels(132987444021821440);
            _PixivM.AddToAllowedChannels(131902567457357824); //gjm
            _StdoutM = new Modules.STDOutModule();
            using(var reader = new StreamReader("token")) {
                _BotToken = reader.ReadLine();
            }


            _client.MessageReceived += /*async*/ (s, e) =>
            {
                
            };

            _client.MessageReceived += _PixivM.ProcessInMessage;
            _client.MessageReceived += _StdoutM.ProcessInMessage;

            _client.MessageSent += /*async*/ (s, e) => {
                Console.WriteLine("sent message::" + e.Server.Name + "." + e.Channel.Name);
            };

            var stdinTask = new Task( async () => {
                ulong setChannel = 132987444021821440;
                while (true) {
                    var line = Console.ReadLine();
                    //Console.WriteLine(line);
                    var lineSplit = line.Split(' ');
                    if (lineSplit.Length == 0) continue;
                    
                    if (lineSplit[0] == "servers") {
                            Console.WriteLine("Server list:");
                            foreach (var server in _client.Servers) {
                            Console.WriteLine(server.Id + " : " + server.Name);
                        }
                    }
                    else if (lineSplit[0] == "channels" && lineSplit.Length == 2) {
                        //_client.Servers.Where(server => server.Name == lineSplit[1]);
                        ulong sid = 0;
                        if (!ulong.TryParse(lineSplit[1], out sid)) continue;
                        Console.WriteLine("Channel list");
                        foreach (var channel in _client.GetServer(sid).TextChannels) {
                            Console.WriteLine(channel.Name + " : " + channel.Id);
                        }
                    }
                    else if (lineSplit[0] == "msg" && lineSplit.Length >= 3) {
                        ulong channelID = 0;
                        if (!ulong.TryParse(lineSplit[1], out channelID)) continue;
                        await _client.GetChannel(channelID).SendMessage(line.Substring(lineSplit[0].Length + lineSplit[1].Length + 2));
                    }
                    else if(lineSplit[0] == "set" && lineSplit.LongLength == 2) {
                        ulong channelID = 0;
                        if (!ulong.TryParse(lineSplit[1], out channelID)) continue;
                        setChannel = channelID;
                    }
                    /*else if(lineSplit[0] == "pixiv") {
                        if (setChannel == 0) continue;
                        var image = GetRandomPixivImage();
                        await _client.GetChannel(setChannel).SendMessage(image.ToString());
                        await _client.GetChannel(setChannel).SendFile(image.Path);
                    }*/
                }
            });
            stdinTask.Start();

            _client.ExecuteAndWait(async () => {
                await _client.Connect(_BotToken);
                Console.WriteLine("connected");
                foreach(var server in _client.Servers) {
                    Console.WriteLine(server.Name);
                }
            });



        }

    }
}
