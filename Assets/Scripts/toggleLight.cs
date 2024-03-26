using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text;
using Microsoft.MixedReality.Toolkit.UI;

public class toggleLight : MonoBehaviour
{
    private string apiUrl = "http://10.19.4.148:8123/api";
    private string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiI4OTA4OTI5YzZhYTg0NjNhYmJhMGMxOWM4OGJhZmU0NSIsImlhdCI6MTcxMTQ2MDg5OSwiZXhwIjoyMDI2ODIwODk5fQ.FX1Q6EiTu6iHN8NkBwJqqk-2hTJZAiEz665BdcrRAME";
    private RootObject root;
    public Interactable toggleButton;
    public PinchSlider pinchSlider;

    public async void OnClick()
    {
        await SendPostRequest();
        await Task.Delay(500);
        await GetResponseBody();
        SetToggleState();
        SetSliderValue();
        SetBackPlate();
    }
    public async Task SendPostRequest()
    {
        string url = apiUrl + "/services/light/toggle";
        string entityId = "light.hue_go_1";

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

    public async Task GetResponseBody()
    {
        string entityId = "light.hue_go_1";
        string url = apiUrl + "/states/" + entityId;

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            String responseBody = await response.Content.ReadAsStringAsync();
            root = JsonUtility.FromJson<RootObject>(responseBody);

            Debug.Log(responseBody);

            if (root.state == "on")
            {
                SetInitialStates.isLightOn = true;

            }
            else
            {
                SetInitialStates.isLightOn = false;
            }
        }
    }

    public void SetToggleState()
    {
        if (root.state == "on")
        {
            toggleButton.IsToggled = true;
        }
        else
        {
            toggleButton.IsToggled = false;
        }
    }

    public void SetSliderValue()
    {
        if (root.state == "on")
        {
            pinchSlider.enabled = true;
            pinchSlider.SliderValue = root.attributes.brightness / 255f;
        }
        else
        {
            pinchSlider.enabled = false;
            pinchSlider.SliderValue = 0;
        }
    }

    public void SetBackPlate()
    {
        if (root.state == "on")
        {
            int identifier = ColorPicker.RGBValues.GetIdentifierForRGBValues(root.attributes.rgb_color[0], root.attributes.rgb_color[1], root.attributes.rgb_color[2]);
            if (identifier != 0)
            {
                GameObject parentObject = GameObject.Find("Color" + identifier);
                for (int i = 0; i < parentObject.transform.childCount; i++)
                {
                    Transform child = parentObject.transform.GetChild(i);
                    child.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 1; i <= 13; i++)
            {
                GameObject iBackPlate = GameObject.Find("BackPlate (" + i + ")");
                if (iBackPlate != null)
                {
                    iBackPlate.SetActive(false);
                    break;
                }
            }
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