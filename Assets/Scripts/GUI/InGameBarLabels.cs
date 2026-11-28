using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameBarLabels : MonoBehaviour
{
    [Serializable]
    private class LabelPair
    {
        public RectTransform anchor;
        public TextMeshProUGUI label;
    }

    [SerializeField] private List<LabelPair> shortcuts = new();

    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        RefreshPositions();
    }

    private void RefreshPositions()
    {
        foreach (LabelPair shortcut in shortcuts)
            shortcut.label.transform.position = shortcut.anchor.position;
    }
}