using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text;
using Microsoft.MixedReality.Toolkit.UI;

public class SetInitialStates : MonoBehaviour
{
    private string apiUrl = "http://10.19.128.173:8123/api";
    private string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiI4MWU3MDQ5MzIyYTM0YWY0YTMxM2U2NmZiNDY2MWE1ZiIsImlhdCI6MTY4NDUwMjExNiwiZXhwIjoxOTk5ODYyMTE2fQ.ioN1hFsnLVj2wye_JDaymqdJ2KPisBDZpBAXCwYt04U";
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
        string entityId = "light.hue_floor_shade_1";
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