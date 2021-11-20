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
    public class VoiceNextCommands : BaseCommandModule
    {
        public async Task Play(CommandContext ctx, string path, string description)
        {
            var vnext = ctx.Client.GetVoiceNext();
            if (vnext == null)
            {
                // not enabled
                await ctx.RespondAsync("VNext is not enabled or configured.");
                return;
            }

            // check whether we aren't already connected
            var vnc = vnext.GetConnection(ctx.Guild);
            if (vnc == null)
            {
                // already connected
                await ctx.RespondAsync("Not connected to voice channel, type ?join");
                return;
            }

            // check if file exists
            if (!File.Exists(path))
            {
                // file does not exist
                await ctx.RespondAsync($"File `{path}` does not exist.");
                return;
            }

            // wait for current playback to finish
            while (vnc.IsPlaying)
            {
                await vnc.WaitForPlaybackFinishAsync();
            }

            // play
            Exception exc = null;
            await ctx.Message.RespondAsync($"{description}");
            try
            {
                if (vnc.IsPlaying)
                {
                    return;
                }
                else
                {
                    await vnc.SendSpeakingAsync(true);
                }

                var psi = new ProcessStartInfo
                {
                    FileName = $@"ffmpeg\ffmpeg.exe",
                    Arguments = $@"-i ""{path}"" -ac 2 -f s16le -ar 48000 pipe:1 -loglevel quiet",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };
                var ffmpeg = Process.Start(psi);
                var ffout = ffmpeg.StandardOutput.BaseStream;

                var txStream = vnc.GetTransmitSink();
                await ffout.CopyToAsync(txStream);
                await txStream.FlushAsync();
                await vnc.WaitForPlaybackFinishAsync();
            }
            catch (Exception ex) { exc = ex; }
            finally
            {
                await vnc.SendSpeakingAsync(false);
                // await ctx.Message.RespondAsync($"Finished playing `{file}`");
            }

            if (exc != null)
                await ctx.RespondAsync($"An exception occured during playback: `{exc.GetType()}: {exc.Message}`");
        }
    }
}