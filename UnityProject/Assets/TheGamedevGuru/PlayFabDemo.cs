using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TheGamedevGuru
{
  public class PlayFabDemo : MonoBehaviour
  {
    [SerializeField] private AssetReference spriteReference = null;
    [SerializeField] private Image image = null;

    public IEnumerator Start()
    {
      yield return LoginToPlayFab();
      yield return InitializeAddressables();
      TestRemoteAddressableAsset();
    }

    private IEnumerator InitializeAddressables()
    {
      Addressables.ResourceManager.ResourceProviders.Add(new AssetBundleProvider());
      Addressables.ResourceManager.ResourceProviders.Add(new PlayFabStorageHashProvider());
      Addressables.ResourceManager.ResourceProviders.Add(new PlayFabStorageAssetBundleProvider());
      Addressables.ResourceManager.ResourceProviders.Add(new PlayFabStorageJsonAssetProvider());

      return Addressables.InitializeAsync();
    }

    private IEnumerator LoginToPlayFab()
    {
      var loginSuccessful = false;
      var request = new LoginWithCustomIDRequest {CustomId = "MyPlayerId", CreateAccount = true};
      PlayFabClientAPI.LoginWithCustomID(request, result => loginSuccessful = true,
        error => error.GenerateErrorReport());
      return new WaitUntil(() => loginSuccessful);
    }

    private void TestRemoteAddressableAsset()
    {
      var asyncOperation = spriteReference.LoadAssetAsync<Sprite>();
      asyncOperation.Completed += result => image.sprite = asyncOperation.Result;
    }
  }
}