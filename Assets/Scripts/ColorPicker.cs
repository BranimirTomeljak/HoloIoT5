using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text;
using Microsoft.MixedReality.Toolkit.UI;

public class ColorPicker : MonoBehaviour
{
    private string apiUrl = "http://10.19.128.173:8123/api";
    private string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiI4MWU3MDQ5MzIyYTM0YWY0YTMxM2U2NmZiNDY2MWE1ZiIsImlhdCI6MTY4NDUwMjExNiwiZXhwIjoxOTk5ODYyMTE2fQ.ioN1hFsnLVj2wye_JDaymqdJ2KPisBDZpBAXCwYt04U";
    public int identifier;

    public GameObject thisBackPlate;

    public async void OnClick() // dodat da se backplate ne minja ako je ugaseno svitlo -> disableat backplate kad se ugasi svitlo
    {
        //if (SetInitialStates.isLightOn)
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

            thisBackPlate.SetActive(true);
            await ChangeColor();
        }

    }

    public async Task ChangeColor() // mozda prominit u hue/saturation (hs_color)
    {
        string url = apiUrl + "/services/light/turn_on";
        string entryId = "light.hue_floor_shade_1";

        using (var httpClient = new HttpClient())
        {
            RGBValues rgb = RGBValues.GetRGBValues(identifier); // iz nekog razloga uvik shifta za random rijednost r g b
            var json = "{\"entity_id\": \"" + entryId + "\", \"rgb_color\": [" + rgb.r + "," + rgb.g + "," + rgb.b + "]}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.BaseAddress = new Uri(url);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + authToken);

            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            Debug.Log("Changed color: " + responseBody);
        }
    }

    public struct RGBValues
    {
        public int r, g, b;

        public RGBValues(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public static RGBValues GetRGBValues(int identifier)
        {
            switch (identifier)
            {
                case 1: return new RGBValues(255, 0, 0);        // hue = 0, saturation = 100
                case 2: return new RGBValues(255, 165, 0);      // hue = 30, saturation = 100
                case 3: return new RGBValues(255, 255, 0);      // hue = 60, saturation = 100
                case 4: return new RGBValues(0, 255, 0);        // hue = 120, saturation = 100
                case 5: return new RGBValues(0, 128, 0);        // hue = 120, saturation = 50
                case 6: return new RGBValues(0, 255, 255);      // hue = 180, saturation = 100
                case 7: return new RGBValues(0, 0, 255);        // hue = 240, saturation = 100
                case 8: return new RGBValues(135, 206, 250);    // hue = 210, saturation = 92
                case 9: return new RGBValues(75, 0, 130);       // hue = 271, saturation = 100
                case 10: return new RGBValues(128, 0, 128);     // hue = 300, saturation = 50
                case 11: return new RGBValues(255, 0, 255);     // hue = 300, saturation = 100
                case 12: return new RGBValues(255, 192, 203);   // hue = 350, saturation = 100
                case 13: return new RGBValues(255, 255, 255);   // hue = 0, saturation = 0
                default: return new RGBValues(0, 0, 0);
            }
        }

        public static int GetIdentifierForRGBValues(int r, int g, int b)
        {
            RGBValues[] rgbValues = new RGBValues[]
            {
                new RGBValues(255, 0, 0),
                new RGBValues(255, 165, 0),
                new RGBValues(255, 255, 0),
                new RGBValues(0, 255, 0),
                new RGBValues(0, 128, 0),
                new RGBValues(0, 255, 255),
                new RGBValues(0, 0, 255),
                new RGBValues(135, 206, 250),
                new RGBValues(75, 0, 130),
                new RGBValues(128, 0, 128),
                new RGBValues(255, 0, 255),
                new RGBValues(255, 192, 203),
                new RGBValues(255, 255, 255)
            };

            for (int i = 0; i < rgbValues.Length; i++)
            {
                if (rgbValues[i].r == r && rgbValues[i].g == g && rgbValues[i].b == b)
                {
                    return i + 1;
                }
            }

            return 0;
        }

    }

}
