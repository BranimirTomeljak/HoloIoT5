using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;

public class ikea1 : MonoBehaviour
{
    //[SerializeField] private GameObject objectPrefab;
    private string apiUrl = "http://10.19.4.148:8123/api";
    private string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiI4OTA4OTI5YzZhYTg0NjNhYmJhMGMxOWM4OGJhZmU0NSIsImlhdCI6MTcxMTQ2MDg5OSwiZXhwIjoyMDI2ODIwODk5fQ.FX1Q6EiTu6iHN8NkBwJqqk-2hTJZAiEz665BdcrRAME";
    private RootObject root;

    public async void OnClick()
    {
        //InstantiateObject(eventData.Pointer.Result.Details.Point, eventData.Pointer.Result.Details.Normal);
        await SendPostRequest();
        //await Task.Delay(500);
        //await GetResponseBody();
        //SetToggleState();
        //SetSliderValue();
        //  SetBackPlate();
        Debug.Log("AIR TAP");   
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData) { }

    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }

    public void OnPointerUp(MixedRealityPointerEventData eventData) { }

    /*private void InstantiateObject(Vector3 position, Vector3 normal)
    {
        // Adjust position and rotation based on the user's gaze
        Vector3 objectPosition = position + normal * 0.1f; // Adjust distance from the user
        Quaternion objectRotation = Quaternion.LookRotation(normal, Vector3.up);

        // Instantiate the object
        Instantiate(objectPrefab, objectPosition, objectRotation);
    }*/

    public async Task SendPostRequest()
    {
        string url = apiUrl + "/services/light/toggle";
        string entityId = "light.lightbulb";

        using (var httpClient = new HttpClient())
        {
            var json = "{\"entity_id\": \"" + entityId + "\"}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + authToken);

            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            String responseBody = await response.Content.ReadAsStringAsync();

            Debug.Log("1:" + responseBody);
        }
    }

    [System.Serializable]
    public class Attributes
    {
        public int brightness;
        public int[] rgb_color;
    }

    [System.Serializable]
    public class RootObject
    {
        public Attributes attributes;
        public string state;
    }
}
