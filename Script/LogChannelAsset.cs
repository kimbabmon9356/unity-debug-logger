using System.Collections.Generic;
using UnityEngine;

public class LogChannelAsset : ScriptableObject
{
    public IReadOnlyList<LogChannelConfig> Channels => _channelConfigs;
    public string Prefix => string.IsNullOrEmpty(_prefix) ? "…" : _prefix;
    public string Suffix => string.IsNullOrEmpty(_suffix) ? "≫" : _suffix;

    [SerializeField]
    private string _prefix = "…";

    [SerializeField]
    private string _suffix = "≫";

    [SerializeField]
    private List<LogChannelConfig> _channelConfigs = new()
    {
        new LogChannelConfig() { channelName = "Default", channelColor = Color.white, enabled = true }
    };

    private Dictionary<int, LogChannelConfig> _indexCache;
    private Dictionary<string, LogChannelConfig> _nameCache;

    #region unity 
    private void OnEnable() => BuildCache();

    private void OnValidate()
    {
        BuildCache();
        LogFormatter.InvalidateHexCache();
    }

    #endregion

    private void BuildCache()
    {
        _indexCache = new Dictionary<int, LogChannelConfig>(_channelConfigs.Count);
        _nameCache = new Dictionary<string, LogChannelConfig>(_channelConfigs.Count);

        for (int i = 0; i < _channelConfigs.Count; i++)
        {
            var config = _channelConfigs[i];

            if (string.IsNullOrEmpty(config.channelName))
                continue;

            _indexCache.TryAdd(i, config);
            _nameCache.TryAdd(config.channelName, config);
        }
    }

    #region public
    public bool TryGetChannel(int index, out LogChannelConfig config)
    {
        if (_indexCache == null) BuildCache();
        return _indexCache.TryGetValue(index, out config);
    }

    public bool TryGetChannel(string name, out LogChannelConfig config)
    {
        if (_nameCache == null) BuildCache();
        return _nameCache.TryGetValue(name, out config);
    }

    #endregion
}