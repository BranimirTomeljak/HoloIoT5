using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text;

public class newExample : MonoBehaviour
{
    private string apiUrl = "http://10.19.128.173:8123/api";
    private string authToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJmYjJjMjlhZGJhNmI0ZWVkYTIwYWEzMWQ4M2JlNjg2YSIsImlhdCI6MTY4MzEwOTE3NSwiZXhwIjoxOTk4NDY5MTc1fQ.-MCfkM1S06SAdP_NJBqXJspu6vyQZ9StJVO3cDPXbz4";

    public async Task SendPostRequest()
    {
        string url = apiUrl + "/services/light/toggle";
        string entryId = "light.hue_floor_shade_1";

        //string url2 = apiUrl + "/states/weather.forecast_mobility_lab";

        using (var httpClient = new HttpClient())
        {
            var json = "{\"entity_id\": \"" + entryId + "\"}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.BaseAddress = new Uri(url);
            //httpClient.BaseAddress = new Uri(url2);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + authToken);

            var result = await httpClient.PostAsync(url, content);
            //var result = await httpClient.GetAsync(url2);
            var resultContent = await result.Content.ReadAsStringAsync();

            Debug.Log(resultContent);
        }
    }

    public void Start()
    {
        //await SendPostRequest();
    }

    public async void OnClick() {
        await SendPostRequest();
    }

    public void znj() {
        Debug.Log("znj");
    }
}