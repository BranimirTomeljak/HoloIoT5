using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class hue1rgb : MonoBehaviour
{
    [SerializeField] private GameObject colorPicker;
    private float moveDistance = 0.1f;

    public void Start()
    {
        colorPicker.SetActive(false);
    }

    public void OnClick()
    {
        colorPicker.SetActive(!colorPicker.activeSelf);
    }

    public void GoUp()
    {
        MoveColorPicker(Vector3.up);
    }

    public void GoDown()
    {
        MoveColorPicker(Vector3.down);
    }

    public void GoLeft()
    {
        MoveColorPicker(Vector3.left);
    }

    public void GoRight()
    {
        MoveColorPicker(Vector3.right);
    }

    public void GoForward()
    {
        MoveColorPicker(Vector3.forward);
    }

    public void GoBack()
    {
        MoveColorPicker(Vector3.back);
    }

    private void MoveColorPicker(Vector3 direction)
    {
        colorPicker.transform.position += direction * moveDistance;
    }
}
