using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text;
using Microsoft.MixedReality.Toolkit.UI;

public class toggleLight : MonoBehaviour
{
    private string apiUrl = "http://10.19.128.173:8123/api";
    private string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiI4MWU3MDQ5MzIyYTM0YWY0YTMxM2U2NmZiNDY2MWE1ZiIsImlhdCI6MTY4NDUwMjExNiwiZXhwIjoxOTk5ODYyMTE2fQ.ioN1hFsnLVj2wye_JDaymqdJ2KPisBDZpBAXCwYt04U";
    public PinchSlider pinchSlider;
    public async Task SendPostRequest()
    {
        string url = apiUrl + "/services/light/toggle";
        //string url = apiUrl + "/services/light/turn_on";
        string entryId = "light.hue_floor_shade_1";

        //string url2 = apiUrl + "/states/weather.forecast_mobility_lab";

        using (var httpClient = new HttpClient())
        {
            var json = "{\"entity_id\": \"" + entryId + "\"}";
            //var json = "{\"entity_id\": \"" + entryId + "\", \"rgb_color\": [" + r + "," + g + "," + b + "]}";
            //rgb_color, color_temp_kelvin, brightness_pct -> https://www.home-assistant.io/integrations/light/
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.BaseAddress = new Uri(url);
            //httpClient.BaseAddress = new Uri(url2);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + authToken);

            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            //var response = await httpClient.GetAsync(url2);
            string responseBody = await response.Content.ReadAsStringAsync();

            Debug.Log("1:" + responseBody);
        }
    }

    public async Task SetInitialSliderValue()
    {
        string entryId = "light.hue_floor_shade_1";
        string url = apiUrl + "/states/" + entryId;

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            Debug.Log("2: " + responseBody);

            RootObject root = JsonUtility.FromJson<RootObject>(responseBody);

            if (root.state == "on"){
                pinchSlider.enabled = true;
                pinchSlider.SliderValue = root.attributes.brightness / 255f;}
            else{
                pinchSlider.enabled = false;
                pinchSlider.SliderValue = 0;
            }
        }
    }

    public async void OnClick()
    {
        await SendPostRequest();
        await Task.Delay(500);
        await SetInitialSliderValue();
    }

    [System.Serializable]
    public class Attributes
    {
        public int brightness;
    }

    [System.Serializable]
    public class RootObject
    {
        public Attributes attributes;
        public string state;
    }

}