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
    public static bool isLightOn;
    public Interactable toggleButton;
    public PinchSlider pinchSlider;
    async void Start()
    {
        await getResponseBody();
        setInitialToggleState();
    }

    public async Task getResponseBody()
    {
        string entryId = "light.hue_floor_shade_1";
        string url = apiUrl + "/states/" + entryId;

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            responseBody = await response.Content.ReadAsStringAsync();
            Debug.Log(responseBody);
        }
    }

    public void setInitialToggleState()
    {
        isLightOn = responseBody.Contains("\"state\":\"on\"");

        Debug.Log($"Initial: light is {(isLightOn ? "on" : "off")}");

        if (isLightOn)
        {
            toggleButton.IsToggled = true;
            setInitialSliderValue();
            setInitialColorValue();
        }
        else
        {
            pinchSlider.enabled = false;
        }
    }

    public void setInitialSliderValue()
    {
        Debug.Log(responseBody);
        // Parse the JSON string into a RootObject using JsonUtility
        RootObject root = JsonUtility.FromJson<RootObject>(responseBody);

        pinchSlider.SliderValue = root.attributes.brightness / 255f;
    }

    public void setInitialColorValue()
    {
        // enableat BackPlate (1) ako je odabrana ta boja -> sad to napravit kad se odabere boja u ColorPickeru
        RootObject root = JsonUtility.FromJson<RootObject>(responseBody);
        Debug.Log("[" + root.attributes.rgb_color[0] + " " + root.attributes.rgb_color[1] + " " +root.attributes.rgb_color[2] + "]");
        int identifier = ColorPicker.RGBValues.GetIdentifierForRGBValues(root.attributes.rgb_color[0], root.attributes.rgb_color[1], root.attributes.rgb_color[2]);
        GameObject activeButton = GameObject.Find("BackPlate " + identifier);
        activeButton.SetActive(true);
        
        // takoder, namistit da se ne moze minjat boja dok nije upaljena svica
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
    }

}