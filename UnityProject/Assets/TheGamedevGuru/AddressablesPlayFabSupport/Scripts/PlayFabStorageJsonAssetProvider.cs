using System.ComponentModel;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace TheGamedevGuru
{
/// <summary>
/// This class provides JSON content over PlayFab.
/// </summary>
[DisplayName("PlayFab Json Asset Provider")]
public class PlayFabStorageJsonAssetProvider : JsonAssetProvider
{
    public override string ProviderId => typeof(JsonAssetProvider).FullName;
    
    public override void Provide(ProvideHandle provideHandle)
    {
        if (provideHandle.Location.InternalId.StartsWith("playfab://") == false)
        {
            base.Provide(provideHandle);
            return;
        }
        
        var addressableId = provideHandle.Location.InternalId.Replace("playfab://", "");
        PlayFabClientAPI.GetContentDownloadUrl(
            new GetContentDownloadUrlRequest() {Key = addressableId, ThruCDN = PlayFabAddressablesManager.UseCdn},
            result =>
            {
                Assert.IsTrue(provideHandle.Location.ResourceType == typeof(ContentCatalogData), "Only catalogs supported");
                var resourceLocation = new ResourceLocationBase(result.URL, result.URL, typeof(JsonAssetProvider).FullName, typeof(string));
                provideHandle.ResourceManager.ProvideResource<ContentCatalogData>(resourceLocation).Completed += handle =>
                {
                    var contents = handle.Result;
                    provideHandle.Complete(contents, true, handle.OperationException);
                };
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }
}
}