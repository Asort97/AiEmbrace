using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutObject : MonoBehaviour
{
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        rectTransform.GetLocalCorners(corners);

        // Позиция нижней грани изображения
        Vector3 bottomPosition = corners[0]; // Левый нижний угол
        // Или используйте corners[2]; // Правый нижний угол

        Debug.Log("Позиция нижней грани: " + bottomPosition);
    }
}
