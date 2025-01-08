using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBottle : MonoBehaviour
{
    private Stack<Color> colorInBottle = new Stack<Color>();

    [SerializeField] private SpriteRenderer renderMask;

    [SerializeField] private Color[] colors = new[]
    {
        Color.yellow,
        Color.blue,
        Color.red,
        Color.green,
    };

    private void Start()
    {
        SetColor();
    }

    private void SetColor()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            renderMask.material.SetColor($"_Color0{i + 1}", colors[i]);
        }
    }
}