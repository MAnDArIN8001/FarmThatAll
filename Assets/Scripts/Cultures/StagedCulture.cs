using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Cultures
{
    public class StagedCulture : Culture
    {
        private int _stepIndex;
        
        [SerializeField] private List<GameObject> _targets;

        private void Start()
        {
            foreach (var target in _targets)
            {
                target.transform.localScale = Vector3.zero;
                
                target.transform.DOScale(Vector3.one*1.5f, CultureSetup.GrowingTime);
            }
        }
    }
}