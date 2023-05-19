using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowColorPicker : MonoBehaviour
{
    public void enableColorPicker() {
        GameObject.Find("ColorPicker").SetActive(true);
    }

}
