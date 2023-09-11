using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Events;
using Content.Server.Mind;
using Content.Shared.AlphaCentauri.CCCVar;
using Content.Shared.Roles.Jobs;

namespace Content.Server.AlphaCentauri.Monitoring;

public sealed partial class MonitoringSystem : EntitySystem
{

    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly SharedJobSystem _jobs = default!;
    [Dependency] private readonly MindSystem _minds = default!;


    private const string BaseUrl = "https://discord.com/api/v10";
    private readonly HttpClient _http = new();

    private string _discordAppid = string.Empty;
    private string _discordBotToken = string.Empty;
    private bool _discordMonitoringEnable = false;
    private string _discordMonitoringChannelId = string.Empty;

    private string _discordMonitoringMessageId = string.Empty;
    private int RoundId = 0;

    public override void Initialize()
    {
        _cfg.OnValueChanged(CCCVars.DiscordApiMonitoringEnable, v => _discordMonitoringEnable = v, true);

        if (!_discordMonitoringEnable)
            return;

        _cfg.OnValueChanged(CCCVars.DiscordApiAppId, v => _discordAppid = v, true);
        _cfg.OnValueChanged(CCCVars.DiscordApiBotToken, v => _discordBotToken = v, true);
        _cfg.OnValueChanged(CCCVars.DiscordApiMonitoringChannelId, v => _discordMonitoringChannelId = v, true);

        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerStatusChanged);
        SubscribeLocalEvent<RoundStartingEvent>(OnRoundStart);
    }

    public void OnRoundStart(RoundStartingEvent even)
    {
        RoundId = even.Id;
    }

    public void OnPlayerStatusChanged(PlayerSpawnCompleteEvent message)
    {
        var players = _playerManager.GetAllPlayers();
        var content = GetMessageContent(players);
        UpdateMessage(content);
    }

    public async Task<HttpResponseMessage> UpdateMessage(string content)
    {
        var messageId = await GetMessageId();
        if (messageId == string.Empty)
            return await CreateMessage(content);

        var url = $"{GetUrlMonitoring()}/{messageId}";
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", _discordBotToken);
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var res = await _http.PatchAsJsonAsync(url, GetPayload(content));
        var message = await res.Content.ReadAsStringAsync();

        return res;
    }

    public async Task<HttpResponseMessage> CreateMessage(string content)
    {
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", _discordBotToken);
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var res = await _http.PostAsJsonAsync(GetUrlMonitoring(), GetPayload(content));
        var message = await res.Content.ReadFromJsonAsync<MonitoringChannelMessage>();
        _discordMonitoringMessageId = message.Id;

        return res;
    }

    public async Task<string> GetMessageId()
    {
        if (_discordMonitoringMessageId != string.Empty)
            return _discordMonitoringMessageId;

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", _discordBotToken);
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        List<MonitoringChannelMessage>? res = await _http.GetFromJsonAsync<List<MonitoringChannelMessage>>(GetUrlMonitoring());

        if (res == null)
            return string.Empty;

        string id = string.Empty;

        res.ForEach(delegate (MonitoringChannelMessage message) {
            if (message.Author.Bot == true && message.Author.Id == _discordAppid)
                id = message.Id;
        });

        _discordMonitoringMessageId = id;

        return id;
    }

    private string GetUrlMonitoring()
    {
        return $"{BaseUrl}/channels/{_discordMonitoringChannelId}/messages";
    }
    private string GetMessageContent(List<IPlayerSession> players)
    {
        string content = string.Empty;
        players.ForEach(delegate (IPlayerSession session) {
            string playerName = string.Empty;
            string startingRole = string.Empty;
            if (session.AttachedEntity != null)
                playerName = EntityManager.GetComponent<MetaDataComponent>(session.AttachedEntity.Value).EntityName;
            if (_minds.TryGetMind(session, out var mindId, out _))
                startingRole = _jobs.MindTryGetJobName(mindId);
            if(playerName!=string.Empty)
                content += playerName + ": " + startingRole + "\n";
        });
        return content;
    }
    private MonitoringPayload GetPayload(string content)
    {
        var embends = new List<MonitoringPayloadEmbed>();
        embends.Add(new MonitoringPayloadEmbed
        {
            Type = "rich",
            Title = "Манифест экипажа:",
            Description = content,
            Color = "16580705",
            Footer = new MonitoringPayloadEmbedFooter
            {
                Text = "Раунд: " + RoundId.ToString()
            }
        });
        var payload = new MonitoringPayload
        {
            Embeds = embends
        };
        return payload;
    }
}
