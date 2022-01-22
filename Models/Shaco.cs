using System;

namespace ShacoDiscordBot
{
    public static class Shaco
    {
        public static VoiceLine[] voiceLines = {
            new VoiceLine{Path = $@"Audio\HowAboutAMagicTrick.ogg", Description = "How about a magic trick?"},
            new VoiceLine{Path = $@"Audio\TheJokesOnYou.ogg", Description = "The joke's on you!"},
            new VoiceLine{Path = $@"Audio\LookBehindYou.ogg", Description = "Look... behind you."},
            new VoiceLine{Path = $@"Audio\ThisWillBeFun.ogg", Description = "This will be fun!"},
            new VoiceLine{Path = $@"Audio\HereWeGo.ogg", Description = "Here we go!"},
            new VoiceLine{Path = $@"Audio\MarchMarchMarchMarch.ogg", Description = "March, march, march, march!"},
            new VoiceLine{Path = $@"Audio\NowYouSeeMe.ogg", Description = "Now you see me, now you don't!"},
            new VoiceLine{Path = $@"Audio\ALittleBitCloser.ogg", Description = "Just a little bit closer!"},
            new VoiceLine{Path = $@"Audio\WhySoSerious.ogg", Description = "Why so serious?"},
            new VoiceLine{Path = $@"Audio\Disappear.ogg", Description = "For my next trick, I'll make you disappear!"}
        };
        public static VoiceLine[] laughs = {
            new VoiceLine{Path = $@"Audio\Laugh1.ogg", Description = ":laughing:"},
            new VoiceLine{Path = $@"Audio\Laugh2.ogg", Description = ":laughing:"},
            new VoiceLine{Path = $@"Audio\Laugh3.ogg", Description = ":laughing:"},

        };
        public static string PermissionMessage = "You do not have permission to use this command, bitchass";
        public static VoiceLine RandomVoiceLine()
        {
            Random rand = new Random();
            return voiceLines[rand.Next(1, voiceLines.Length)];
        }
        public static VoiceLine RandomLaugh()
        {
            Random rand = new Random();
            return laughs[rand.Next(1, laughs.Length)];
        }
    }
    public struct VoiceLine
    {
        public string Path;
        public string Description;
    }
}