using GorillaTag;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Utilla.Tools;

namespace Utilla.Behaviours;

internal class ConductBoardManager : MonoBehaviour
{
    private GameObject stumpRootObject;

    private TextMeshPro baseHeaderText, footerText;

    public void Start()
    {
        stumpRootObject = Array.Find(ZoneManagement.instance.allObjects, gameObject => gameObject.name == "TreeRoom");
        baseHeaderText = stumpRootObject.transform.FindChildRecursive("CodeOfConductHeadingText")?.GetComponent<TextMeshPro>();

        GameObject footerTextObject = Instantiate(baseHeaderText.gameObject);
        footerTextObject.transform.position = baseHeaderText.transform.position;
        footerTextObject.transform.rotation = baseHeaderText.transform.rotation;
        footerTextObject.transform.localScale = baseHeaderText.transform.localScale;
        SanitizeTextObject(footerTextObject);

        footerText = footerTextObject.GetComponent<TextMeshPro>();
        footerText.text = $"{PluginInfo.Name} {PluginInfo.Version}".ToUpper();
        footerText.enableAutoSizing = false;
        footerText.fontSize = 45;
        footerText.margin = new Vector4(0f, 110f, 0f, 0f);
        footerText.characterSpacing = -11.5f;
        footerText.enabled = true;
        footerText.renderer.enabled = true;

        CheckVersion();
    }

    public async void CheckVersion()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(string.Join('/', PluginInfo.UtillaRepoURL, "Version.txt"));
        UnityWebRequestAsyncOperation asyncOperation = webRequest.SendWebRequest();
        await asyncOperation;

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Logging.Fatal($"Version could not be accessed from {webRequest.url}");
            Logging.Info(webRequest.downloadHandler.error);
            return;
        }

        if (Version.TryParse(PluginInfo.Version, out Version installedVersion) && Version.TryParse(webRequest.downloadHandler.text, out Version latestVersion) && latestVersion > installedVersion)
        {
            footerText.color = Color.red;
            footerText.fontSize *= 0.85f;
            footerText.text = $"{PluginInfo.Name} {PluginInfo.UtillaRepoURL} - please update to {latestVersion}".ToUpper();
        }
    }

    private void SanitizeTextObject(GameObject gameObject)
    {
        Type[] typesToRemove = [typeof(PlayFabTitleDataTextDisplay), typeof(LocalizedText), typeof(StaticLodGroup)];
        Component[] components = gameObject.GetComponents<Component>();

        for (int i = 0; i < components.Length; i++)
        {
            Type type = components[i].GetType();
            if (typesToRemove.Contains(type)) Destroy(components[i]);
        }
    }
}
