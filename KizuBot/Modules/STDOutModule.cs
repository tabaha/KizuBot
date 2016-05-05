using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace KizuBot.Modules {
    class STDOutModule : IModule {

        public STDOutModule() {

        }

        public void AddToAllowedChannels(ulong channelID) {
            
        }

        public void RemoveFromAllowedChannels(ulong channelID) {
            
        }

        public void ProcessInMessage(object sender, MessageEventArgs e) {
            if (!e.Message.IsAuthor) {
                
                Console.WriteLine("[" + e.Message.Timestamp.ToShortTimeString() + "]" + e.Server.Name + "." + e.Channel.Name + "." + "<" + e.User.Name + ">" + " " + e.Message.Text);
                //Console.WriteLine(">>" + e.Message.RawText);
            }
            else {
                Console.WriteLine("OUT " + "[" + e.Message.Timestamp.ToShortTimeString() + "]" + e.Server.Name + "." + e.Channel.Name + "." + "<" + e.User.Name + ">" + " " + e.Message.Text);
            }
        }


    }
}
