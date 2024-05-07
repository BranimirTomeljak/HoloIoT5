using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Text;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;

public class hue1rgb : MonoBehaviour
{
    [SerializeField] private GameObject colorPicker;

    public void Start() 
    {
        colorPicker.SetActive(false);
    }

    public async void OnClick()
    {
        if (colorPicker.activeSelf)
        {
            colorPicker.SetActive(false);
        }
        else
        {
            colorPicker.SetActive(true);
        }
    }
}
