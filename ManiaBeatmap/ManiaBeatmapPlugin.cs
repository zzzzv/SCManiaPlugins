using StreamCompanionTypes.Interfaces;
using StreamCompanionTypes.Enums;
using StreamCompanionTypes.Interfaces.Services;
using StreamCompanionTypes.Attributes;
using StreamCompanionTypes.Interfaces.Sources;
using CollectionManager.Enums;
using CollectionManager.DataTypes;
using StreamCompanionTypes.DataTypes;
using StarRatingRebirth;

namespace ManiaBeatmap;

[SCPlugin(Name, Description, "zzzzv", "https://github.com/zzzzv/SCManiaPlugins/")]
public class ManiaBeatmapPlugin : IPlugin, ITokensSource
{
    public const string Name = "ManiaBeatmap";
    public const string Version = "0.1.1";
    public const string Description = $"Mania beatmap plugin v{Version}";

    private readonly IContextAwareLogger _logger;
    private Tokens.TokenSetter _tokenSetter;
    private ColumnHist _colHist = new(0);
    private Kps _kps = new(0);
    private double _xxySr = 0;

    public ManiaBeatmapPlugin(IContextAwareLogger logger)
    {
        _logger = logger;
        _tokenSetter = Tokens.CreateTokenSetter(Name);
        UpdateTokens();
    }

    public async Task CreateTokensAsync(IMapSearchResult map, CancellationToken cancellationToken)
    {
        if (map.SearchArgs.EventType != OsuEventType.MapChange ||
            map.PlayMode != PlayMode.OsuMania ||
            !File.Exists(map.SearchArgs.OsuFilePath))
        {
            return;
        }
        await Task.Run(() =>
        {
            var data = ManiaData.FromFile(map.SearchArgs.OsuFilePath);
            if (data.Notes.Count == 0) return;

            switch (map.SearchArgs.Mods)
            {
                case Mods.Dt:
                    data = data.DT();
                    break;
                case Mods.Ht:
                    data = data.HT();
                    break;
            }

            _colHist = ColumnHist.FromBeatmap(data);
            _kps = Kps.FromBeatmap(data, true);
            _xxySr = SRCalculator.Calculate(data);
            UpdateTokens();
        }, cancellationToken);
    }

    private void UpdateTokens()
    {
        _tokenSetter("maniaBeatmapColHist", _colHist, whitelist: OsuStatus.Listening);
        _tokenSetter("maniaBeatmapKps", _kps, whitelist: OsuStatus.Listening);
        _tokenSetter("maniaBeatmapXxySr", _xxySr, format: "{0:0.##}", whitelist: OsuStatus.Listening);
    }
}
