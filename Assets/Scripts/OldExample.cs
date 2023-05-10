using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class oldExample : MonoBehaviour
{
    public string apiUrl = "http://10.19.128.173:8123/api";
    public string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJmYjJjMjlhZGJhNmI0ZWVkYTIwYWEzMWQ4M2JlNjg2YSIsImlhdCI6MTY4MzEwOTE3NSwiZXhwIjoxOTk4NDY5MTc1fQ.-MCfkM1S06SAdP_NJBqXJspu6vyQZ9StJVO3cDPXbz4";

    void Start()
    {
        StartCoroutine(TurnOnDevice("light.hue_floor_shade_1"));
    }

    IEnumerator TurnOnDevice(string entityId)
    {
        string url = apiUrl + "/services/light/turn_on";
        WWWForm form = new WWWForm();
        form.AddField("entity_id", entityId);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            webRequest.SetRequestHeader("Authorization", "Bearer " + authToken);
            webRequest.SetRequestHeader("Content-Type", "application/json");

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
}