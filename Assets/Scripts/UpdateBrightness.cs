using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text;
using Microsoft.MixedReality.Toolkit.UI;

public class UpdateBrightness : MonoBehaviour
{
    private string apiUrl = "http://10.19.4.148:8123/api";
    private string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiI4OTA4OTI5YzZhYTg0NjNhYmJhMGMxOWM4OGJhZmU0NSIsImlhdCI6MTcxMTQ2MDg5OSwiZXhwIjoyMDI2ODIwODk5fQ.FX1Q6EiTu6iHN8NkBwJqqk-2hTJZAiEz665BdcrRAME";

    public PinchSlider pinchSlider;
    void Start()
    {
        pinchSlider.OnInteractionEnded.AddListener(OnSliderInteractionEnded);
    }

    public async void OnSliderInteractionEnded(SliderEventData eventData)
    {
        Debug.Log("New brightness percentage: " + pinchSlider.SliderValue);
        await SendPostRequest((int)Math.Round(pinchSlider.SliderValue * 100));
    }

    public async Task SendPostRequest(int SliderPercentage)
    {
        string url = apiUrl + "/services/light/turn_on";
        string entityId = "light.hue_go_1";

        using (var httpClient = new HttpClient())
        {
            var json = "{\"entity_id\": \"" + entityId + "\", \"brightness_pct\": " + SliderPercentage + "}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + authToken);

            var result = await httpClient.PostAsync(url, content);
            var resultContent = await result.Content.ReadAsStringAsync();

            Debug.Log(resultContent);
        }
    }


}
