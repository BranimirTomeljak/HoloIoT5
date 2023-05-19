using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class oldExample : MonoBehaviour
{
    public string apiUrl = "http://10.19.128.173:8123/api";
    public string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiI4MWU3MDQ5MzIyYTM0YWY0YTMxM2U2NmZiNDY2MWE1ZiIsImlhdCI6MTY4NDUwMjExNiwiZXhwIjoxOTk5ODYyMTE2fQ.ioN1hFsnLVj2wye_JDaymqdJ2KPisBDZpBAXCwYt04U";

    void Start()
    {
        //StartCoroutine(TurnOnDevice("light.hue_floor_shade_1"));
    }

    IEnumerator TurnOnDevice(string entityId)
    {
        string url = apiUrl + "/services/light/toggle";
        WWWForm form = new WWWForm();
        form.AddField("entity_id", entityId);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + authToken);
            //webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("sve: " + webRequest.downloadHandler.text);
                Debug.Log("Success. Turned on device: " + entityId);
            }
            else
            {
                Debug.Log("Error: " + webRequest.error);
            }
        }
    }

    public void OnClick() {
        StartCoroutine(TurnOnDevice("light.hue_floor_shade_1"));
    }
}