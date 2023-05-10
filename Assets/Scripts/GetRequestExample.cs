using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GetRequestExample : MonoBehaviour
{
    [SerializeField] private string apiUrl = "https://jsonplaceholder.typicode.com/posts/1";

    void Start()
    {
        StartCoroutine(GetRequest(apiUrl));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Parse the JSON response into an object
                ResponseData responseData = JsonUtility.FromJson<ResponseData>(webRequest.downloadHandler.text);

                // Access the userId property of the object
                int userId = responseData.userId;

                Debug.Log("Success. User ID: " + userId);
            }
            else
            {
                Debug.Log("Error: " + webRequest.error);
            }
        }
    }

    // Define a class to represent the JSON response
    [System.Serializable]
    private class ResponseData
    {
        public int userId;
        public int id;
        public string title;
        public string body;
    }
}
