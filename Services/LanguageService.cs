using Microsoft.JSInterop;

namespace BodyQuest.Services;

/// <summary>Current language (fr / en / ar), saved in the browser.</summary>
public sealed class LanguageService
{
    public const string StorageKey = "bodyquest.lang.v1";
    private readonly IJSRuntime _js;
    private Task? _init;

    public LanguageService(IJSRuntime js) => _js = js;

    public string Lang { get; private set; } = "fr";
    public bool IsRtl => Lang == "ar";
    public string SpeechLang => Lang switch { "ar" => "ar-SA", "en" => "en-US", _ => "fr-FR" };
    public event Action? Changed;

    public string this[string key] => UiText.Get(key, Lang);
    public string F(string key, params object[] args) => string.Format(UiText.Get(key, Lang), args);

    /// <summary>Reads the saved language once (call after the first render).</summary>
    public Task InitAsync() => _init ??= LoadAsync();

    private async Task LoadAsync()
    {
        try
        {
            var saved = await _js.InvokeAsync<string?>("bodyQuestWorlds.loadValue", StorageKey);
            if ((saved == "fr" || saved == "en" || saved == "ar") && saved != Lang)
            {
                Lang = saved;
                Changed?.Invoke();
            }
            await _js.InvokeVoidAsync("bodyQuestWorlds.setLang", Lang);
        }
        catch (JSException) { }
        catch (InvalidOperationException) { }
        catch (JSDisconnectedException) { }
    }

    public async Task SetAsync(string lang)
    {
        if (!(lang == "fr" || lang == "en" || lang == "ar") || lang == Lang) return;
        Lang = lang;
        Changed?.Invoke();
        try
        {
            await _js.InvokeVoidAsync("bodyQuestWorlds.saveValue", StorageKey, lang);
            await _js.InvokeVoidAsync("bodyQuestWorlds.setLang", lang);
        }
        catch (JSException) { }
        catch (InvalidOperationException) { }
        catch (JSDisconnectedException) { }
    }
}