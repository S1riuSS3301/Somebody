using Robust.Shared;
using Robust.Shared.Configuration;

namespace Content.Shared.AlphaCentauri.CCCVar
{
    // ReSharper disable once InconsistentNaming
    [CVarDefs]
    public sealed class CCCVars : CVars
    {

        public static readonly CVarDef<string> DiscordApiAppId =
            CVarDef.Create("discord.app_id", "", CVar.SERVERONLY | CVar.CONFIDENTIAL);

        public static readonly CVarDef<string> DiscordApiPublicKey =
            CVarDef.Create("discord.public_key", "", CVar.SERVERONLY | CVar.CONFIDENTIAL);

        public static readonly CVarDef<string> DiscordApiBotToken =
            CVarDef.Create("discord.bot_token", "", CVar.SERVERONLY | CVar.CONFIDENTIAL);

        public static readonly CVarDef<string> DiscordApiPermissions =
            CVarDef.Create("discord.permissions", "", CVar.SERVERONLY);

        public static readonly CVarDef<bool> DiscordApiMonitoringEnable =
            CVarDef.Create("discord.monitoring_enable", false, CVar.SERVERONLY);

        public static readonly CVarDef<string> DiscordApiMonitoringChannelId =
            CVarDef.Create("discord.monitoring_channel_id", "", CVar.SERVERONLY);
    }
}
