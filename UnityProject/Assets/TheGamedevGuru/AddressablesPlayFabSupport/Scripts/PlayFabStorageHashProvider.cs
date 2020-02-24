using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace TheGamedevGuru
{
/// <summary>
/// This class provides Addressables'' hash content
/// </summary>
public class PlayFabStorageHashProvider : ResourceProviderBase
{
    public override void Provide(ProvideHandle provideHandle)
    {
        var addressableId = provideHandle.Location.InternalId.Replace("playfab://", "");
        PlayFabClientAPI.GetContentDownloadUrl(
            new GetContentDownloadUrlRequest() {Key = addressableId, ThruCDN = PlayFabAddressablesManager.UseCdn},
            result =>
            {
                var resourceLocation = new ResourceLocationBase(result.URL, result.URL, typeof(TextDataProvider).FullName, typeof(string));
                provideHandle.ResourceManager.ProvideResource<string>(resourceLocation).Completed += handle =>
                {
                    var contents = handle.Result;
                    provideHandle.Complete(contents, true, handle.OperationException);
                };
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }
}
}