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

    void Start()
    {
        //button.SetQuadIconByName("IconPin");
    }

    public async Task ChangeColor()
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
    public async void OnClick()
    {
        await ChangeColor();
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
                case 1: return new RGBValues(250, 229, 0);
                case 2: return new RGBValues(259, 198, 11);
                case 3: return new RGBValues(241, 142, 28);
                case 4: return new RGBValues(234, 98, 31);
                case 5: return new RGBValues(227, 35, 34);
                case 6: return new RGBValues(196, 3, 125);
                case 7: return new RGBValues(109, 57, 139);
                case 8: return new RGBValues(68, 78, 159);
                case 9: return new RGBValues(42, 113, 179);
                case 10: return new RGBValues(6, 150, 187);
                case 11: return new RGBValues(0, 142, 91);
                case 12: return new RGBValues(140, 187, 38);
                default: return new RGBValues(0, 0, 0);
            }
        }

        public static int GetIdentifierForRGBValues(int r, int g, int b)
        {
            RGBValues[] rgbValues = new RGBValues[]
            {
                new RGBValues(250, 229, 0),
                new RGBValues(259, 198, 11),
                new RGBValues(241, 142, 28),
                new RGBValues(234, 98, 31),
                new RGBValues(227, 35, 34),
                new RGBValues(196, 3, 125),
                new RGBValues(109, 57, 139),
                new RGBValues(68, 78, 159),
                new RGBValues(42, 113, 179),
                new RGBValues(6, 150, 187),
                new RGBValues(0, 142, 91),
                new RGBValues(140, 187, 38)
            };

            for (int i = 0; i < rgbValues.Length; i++)
                if (rgbValues[i].r == r && rgbValues[i].g == g && rgbValues[i].b == b)
                    return i + 1; // Adding 1 to match the identifier (1-based indexing)

            return 0; // Return 0 if no match is found
        }

    }

}
