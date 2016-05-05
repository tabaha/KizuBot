using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace KizuBot.Modules {
    interface IModule {

        void ProcessInMessage(object sender, MessageEventArgs e);
        void AddToAllowedChannels(ulong channelID);
        void RemoveFromAllowedChannels(ulong channelID);
    }
}
