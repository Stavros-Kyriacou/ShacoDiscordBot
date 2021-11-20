using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;

namespace ShacoDiscordBot
{
    /*
    how to get voice working
    need to have VoiceNext package installed from nuget
    need to have opus and sodium library files in main directory if VoiceNext wasnt installed using nuget
    need to have ffmpeg executables located on computer somewhere
    Resources
    https://dsharpplus.github.io/articles/audio/voicenext/prerequisites.html
    https://dsharpplus.readthedocs.io/en/stable/articles/voicenext.html
    https://github.com/DSharpPlus/Example-Bots/blob/master/DSPlus.Examples.CSharp.Ex04/ExampleVoiceCommands.cs
    */
    public class VoiceCommands : VoiceNextCommands
    {
        [Command("join"), Description("Bot joins voice channel the message writer is currently in")]
        public async Task JoinCommand(CommandContext ctx, DiscordChannel channel = null)
        {
            channel ??= ctx.Member.VoiceState?.Channel;
            await channel.ConnectAsync();
        }
        [Command("leave"), Description("Bot leaves voice channel")]
        public async Task LeaveCommand(CommandContext ctx)
        {
            var vnext = ctx.Client.GetVoiceNext();
            var connection = vnext.GetConnection(ctx.Guild);

            connection.Disconnect();
            await ctx.RespondAsync("Bye bye...");
        }
        [Command("playquote")]
        public async Task PlayRandomQuote(CommandContext ctx)
        {
            VoiceLine voiceLine = Shaco.RandomVoiceLine();
            await Play(ctx, voiceLine.Path, voiceLine.Description);
        }
        [Command("playquote")]
        public async Task PlayQuote(CommandContext ctx, int quoteNumber)
        {
            VoiceLine voiceLine;
            if (quoteNumber > 0 && quoteNumber <= 10)
            {
                voiceLine = Shaco.voiceLines[quoteNumber - 1];
            }
            else
            {
                voiceLine = Shaco.RandomVoiceLine();
            }

            await Play(ctx, voiceLine.Path, voiceLine.Description);
        }
        [Command("laugh")]
        public async Task Laugh(CommandContext ctx)
        {
            VoiceLine laugh = Shaco.RandomLaugh();
            await Play(ctx, laugh.Path, laugh.Description);
        }
        [Command("smooth")]
        public async Task Smooth(CommandContext ctx)
        {
            await Play(ctx, $@"Audio\Smooth.mp3", "Playing Smooth B)");
        }



        // [Command("playquote"), Description("Plays a Shaco quote in a voice channel. Use ?join command for bot to join channel")]
        // public async Task PlayQuote(CommandContext ctx,
        // [Description("Enter a number between 1 and 10 to play a specific quote, enter anything else to play a random quote.\n1. How about a magic trick?\n2.The joke's on you!\n3. Look... behind you.\n4. This will be fun!\n5. Here we go!\n6. March, march, march, march!\n7. Now you see me, now you don't!\n8. Just a little bit closer!\n9. Why so serious?\n10. For my next trick, I'll make you disappear!")] string quote)
        // {
        //     VoiceLine voiceLine;
        //     int index = 0;
        //     int.TryParse(quote, out index);
        //     if (index > 0 && index <= 10)
        //     {
        //         voiceLine = Shaco.voiceLines[index - 1];
        //     }
        //     else
        //     {
        //         voiceLine = Shaco.RandomVoiceLine();
        //     }

        //     // check whether VNext is enabled
        //     var vnext = ctx.Client.GetVoiceNext();
        //     if (vnext == null)
        //     {
        //         // not enabled
        //         await ctx.RespondAsync("VNext is not enabled or configured.");
        //         return;
        //     }

        //     // check whether we aren't already connected
        //     var vnc = vnext.GetConnection(ctx.Guild);
        //     if (vnc == null)
        //     {
        //         // already connected
        //         await ctx.RespondAsync("Not connected to voice channel, type ?join");
        //         return;
        //     }

        //     // check if file exists
        //     if (!File.Exists(voiceLine.Path))
        //     {
        //         // file does not exist
        //         await ctx.RespondAsync($"File `{voiceLine.Path}` does not exist.");
        //         return;
        //     }

        //     // wait for current playback to finish
        //     while (vnc.IsPlaying)
        //     {
        //         await vnc.WaitForPlaybackFinishAsync();
        //     }

        //     // play
        //     Exception exc = null;
        //     await ctx.Message.RespondAsync($"{voiceLine.Description}");

        //     try
        //     {
        //         await vnc.SendSpeakingAsync(true);

        //         var psi = new ProcessStartInfo
        //         {
        //             FileName = $@"ffmpeg\ffmpeg.exe",
        //             Arguments = $@"-i ""{voiceLine.Path}"" -ac 2 -f s16le -ar 48000 pipe:1 -loglevel quiet",
        //             RedirectStandardOutput = true,
        //             UseShellExecute = false
        //         };
        //         var ffmpeg = Process.Start(psi);
        //         var ffout = ffmpeg.StandardOutput.BaseStream;

        //         var txStream = vnc.GetTransmitSink();
        //         await ffout.CopyToAsync(txStream);
        //         await txStream.FlushAsync();
        //         await vnc.WaitForPlaybackFinishAsync();
        //     }
        //     catch (Exception ex) { exc = ex; }
        //     finally
        //     {
        //         await vnc.SendSpeakingAsync(false);
        //         // await ctx.Message.RespondAsync($"Finished playing `{file}`");
        //     }

        //     if (exc != null)
        //         await ctx.RespondAsync($"An exception occured during playback: `{exc.GetType()}: {exc.Message}`");
        // }
        // [Command("playquote"), Description("Plays a Shaco quote in a voice channel. Use ?join command for bot to join channel")]
        // public async Task PlayQuoteRandom(CommandContext ctx)
        // {
        //     VoiceLine voiceLine = Shaco.RandomVoiceLine();

        //     // check whether VNext is enabled
        //     var vnext = ctx.Client.GetVoiceNext();
        //     if (vnext == null)
        //     {
        //         // not enabled
        //         await ctx.RespondAsync("VNext is not enabled or configured.");
        //         return;
        //     }

        //     // check whether we aren't already connected
        //     var vnc = vnext.GetConnection(ctx.Guild);
        //     if (vnc == null)
        //     {
        //         // already connected
        //         await ctx.RespondAsync("Not connected to voice channel, type ?join");
        //         return;
        //     }

        //     // check if file exists
        //     if (!File.Exists(voiceLine.Path))
        //     {
        //         // file does not exist
        //         await ctx.RespondAsync($"File `{voiceLine.Path}` does not exist.");
        //         return;
        //     }

        //     // wait for current playback to finish
        //     while (vnc.IsPlaying)
        //     {
        //         await vnc.WaitForPlaybackFinishAsync();
        //     }

        //     // play
        //     Exception exc = null;
        //     await ctx.Message.RespondAsync($"{voiceLine.Description}");

        //     try
        //     {
        //         await vnc.SendSpeakingAsync(true);

        //         var psi = new ProcessStartInfo
        //         {
        //             FileName = $@"ffmpeg\ffmpeg.exe",
        //             Arguments = $@"-i ""{voiceLine.Path}"" -ac 2 -f s16le -ar 48000 pipe:1 -loglevel quiet",
        //             RedirectStandardOutput = true,
        //             UseShellExecute = false
        //         };
        //         var ffmpeg = Process.Start(psi);
        //         var ffout = ffmpeg.StandardOutput.BaseStream;

        //         var txStream = vnc.GetTransmitSink();
        //         await ffout.CopyToAsync(txStream);
        //         await txStream.FlushAsync();
        //         await vnc.WaitForPlaybackFinishAsync();
        //     }
        //     catch (Exception ex) { exc = ex; }
        //     finally
        //     {
        //         await vnc.SendSpeakingAsync(false);
        //         // await ctx.Message.RespondAsync($"Finished playing `{file}`");
        //     }

        //     if (exc != null)
        //         await ctx.RespondAsync($"An exception occured during playback: `{exc.GetType()}: {exc.Message}`");
        // }
        // [Command("laugh"), Description("Shaco laughs in a voice channel. Use ?join command for bot to join channel")]
        // public async Task Laugh(CommandContext ctx)
        // {
        //     VoiceLine voiceLine = Shaco.RandomLaugh();

        //     // check whether VNext is enabled
        //     var vnext = ctx.Client.GetVoiceNext();
        //     if (vnext == null)
        //     {
        //         // not enabled
        //         await ctx.RespondAsync("VNext is not enabled or configured.");
        //         return;
        //     }

        //     // check whether we aren't already connected
        //     var vnc = vnext.GetConnection(ctx.Guild);
        //     if (vnc == null)
        //     {
        //         // already connected
        //         await ctx.RespondAsync("Not connected to voice channel, type ?join");
        //         return;
        //     }

        //     // check if file exists
        //     if (!File.Exists(voiceLine.Path))
        //     {
        //         // file does not exist
        //         await ctx.RespondAsync($"File `{voiceLine.Path}` does not exist.");
        //         return;
        //     }

        //     // wait for current playback to finish
        //     while (vnc.IsPlaying)
        //     {
        //         await vnc.WaitForPlaybackFinishAsync();
        //     }

        //     // play
        //     Exception exc = null;
        //     await ctx.Message.RespondAsync($"{voiceLine.Description}");

        //     try
        //     {
        //         await vnc.SendSpeakingAsync(true);

        //         var psi = new ProcessStartInfo
        //         {
        //             FileName = $@"ffmpeg\ffmpeg.exe",
        //             Arguments = $@"-i ""{voiceLine.Path}"" -ac 2 -f s16le -ar 48000 pipe:1 -loglevel quiet",
        //             RedirectStandardOutput = true,
        //             UseShellExecute = false
        //         };
        //         var ffmpeg = Process.Start(psi);
        //         var ffout = ffmpeg.StandardOutput.BaseStream;

        //         var txStream = vnc.GetTransmitSink();
        //         await ffout.CopyToAsync(txStream);
        //         await txStream.FlushAsync();
        //         await vnc.WaitForPlaybackFinishAsync();
        //     }
        //     catch (Exception ex) { exc = ex; }
        //     finally
        //     {
        //         await vnc.SendSpeakingAsync(false);
        //         // await ctx.Message.RespondAsync($"Finished playing `{file}`");
        //     }

        //     if (exc != null)
        //         await ctx.RespondAsync($"An exception occured during playback: `{exc.GetType()}: {exc.Message}`");
        // }

        // [Command("smooth"), Description("smooth")]
        // public async Task Smooth(CommandContext ctx)
        // {
        //     VoiceLine voiceLine = Shaco.RandomVoiceLine();
        //     string path = $@"Audio\Smooth.mp3";

        //     // check whether VNext is enabled
        //     var vnext = ctx.Client.GetVoiceNext();
        //     if (vnext == null)
        //     {
        //         // not enabled
        //         await ctx.RespondAsync("VNext is not enabled or configured.");
        //         return;
        //     }

        //     // check whether we aren't already connected
        //     var vnc = vnext.GetConnection(ctx.Guild);
        //     if (vnc == null)
        //     {
        //         // already connected
        //         await ctx.RespondAsync("Not connected to voice channel, type ?join");
        //         return;
        //     }

        //     // check if file exists
        //     if (!File.Exists(path))
        //     {
        //         // file does not exist
        //         await ctx.RespondAsync($"File `{path}` does not exist.");
        //         return;
        //     }

        //     // wait for current playback to finish
        //     while (vnc.IsPlaying)
        //     {
        //         await vnc.WaitForPlaybackFinishAsync();
        //     }

        //     // play
        //     Exception exc = null;
        //     await ctx.Message.RespondAsync("Playing Smooth");

        //     try
        //     {
        //         if (vnc.IsPlaying)
        //         {
        //             return;
        //         }
        //         await vnc.SendSpeakingAsync(true);

        //         var psi = new ProcessStartInfo
        //         {
        //             FileName = $@"ffmpeg\ffmpeg.exe",
        //             Arguments = $@"-i ""{path}"" -ac 2 -f s16le -ar 48000 pipe:1 -loglevel quiet",
        //             RedirectStandardOutput = true,
        //             UseShellExecute = false
        //         };
        //         var ffmpeg = Process.Start(psi);
        //         var ffout = ffmpeg.StandardOutput.BaseStream;

        //         var txStream = vnc.GetTransmitSink();
        //         await ffout.CopyToAsync(txStream);
        //         await txStream.FlushAsync();
        //         await vnc.WaitForPlaybackFinishAsync();
        //     }
        //     catch (Exception ex) { exc = ex; }
        //     finally
        //     {
        //         await vnc.SendSpeakingAsync(false);
        //         // await ctx.Message.RespondAsync($"Finished playing `{file}`");
        //     }

        //     if (exc != null)
        //         await ctx.RespondAsync($"An exception occured during playback: `{exc.GetType()}: {exc.Message}`");
        // }
    
    }
}