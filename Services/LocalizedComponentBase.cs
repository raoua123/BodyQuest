using Microsoft.AspNetCore.Components;

namespace BodyQuest.Services;

/// <summary>Pages that inherit this re-render automatically when the language changes.</summary>
public abstract class LocalizedComponentBase : ComponentBase, IDisposable
{
    [Inject] protected LanguageService L { get; set; } = default!;

    protected override void OnInitialized() => L.Changed += OnLang;

    protected virtual Task OnLanguageChangedAsync() => Task.CompletedTask;

    private void OnLang() => _ = InvokeAsync(async () =>
    {
        await OnLanguageChangedAsync();
        StateHasChanged();
    });

    public void Dispose() => L.Changed -= OnLang;
}