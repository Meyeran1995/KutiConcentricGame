﻿using Meyham.DataObjects;
using Meyham.EditorHelpers;
using UnityEngine;
using UnityEngine.UI;

namespace Meyham.UI
{
    public class TimerUi : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image timerUiRight;
        [SerializeField] private Image timerUiLeft;

        [Header("Properties")]
        [SerializeField] private FloatParameter fillPerUnit;
        [SerializeField] private int startingCircleOffset = 1;
        
        [Header("Debug")]
        [ReadOnly, SerializeField] private float startAmount;

        public const int NumberOfDots = 37;

        public void DepleteTime()
        {
            timerUiLeft.fillAmount -= fillPerUnit;
            timerUiRight.fillAmount -= fillPerUnit;
        }

        private void OnValidate()
        {
            startAmount = 1f - startingCircleOffset * fillPerUnit;
            timerUiLeft.fillAmount = startAmount;
            timerUiRight.fillAmount = startAmount;
        }
    }
}