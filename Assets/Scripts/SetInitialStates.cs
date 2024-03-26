using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.UI;

public class SetInitialStates : MonoBehaviour
{
    private string apiUrl = "http://10.19.4.148:8123/api";
    private string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiI4OTA4OTI5YzZhYTg0NjNhYmJhMGMxOWM4OGJhZmU0NSIsImlhdCI6MTcxMTQ2MDg5OSwiZXhwIjoyMDI2ODIwODk5fQ.FX1Q6EiTu6iHN8NkBwJqqk-2hTJZAiEz665BdcrRAME";
    private string responseBody;
    private RootObject root;
    public static bool isLightOn = false;
    public Interactable toggleButton;
    public PinchSlider pinchSlider;
    async void Start()
    {
        await GetResponseBody();
        setInitialToggleState();
        setInitialSliderValue();
        setInitialBackPlate();
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

            responseBody = await response.Content.ReadAsStringAsync();
            root = JsonUtility.FromJson<RootObject>(responseBody);

            if(root.state == "on")
            {
                isLightOn = true;
            }
            else
            {
                isLightOn = false;
            }

            Debug.Log($"Initial: light is {(isLightOn ? "on" : "off")}");
            Debug.Log(responseBody);
        }
    }

    public void setInitialToggleState()
    {
        if (isLightOn)
        {
            toggleButton.IsToggled = true;
        }
    }

    public void setInitialSliderValue()
    {
        if (isLightOn)
        {
            pinchSlider.enabled = true;
            pinchSlider.SliderValue = root.attributes.brightness / 255f;
        }
        else
        {
            pinchSlider.SliderValue = 0f;
            pinchSlider.enabled = false;
        }

    }

    public void setInitialBackPlate()
    {
        if (isLightOn)
        {
            Debug.Log("[" + root.attributes.rgb_color[0] + " " + root.attributes.rgb_color[1] + " " + root.attributes.rgb_color[2] + "]");
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