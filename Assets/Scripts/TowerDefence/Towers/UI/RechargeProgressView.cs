using System;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence.Towers.UI
{
    public sealed class RechargeProgressView : MonoBehaviour
    {
        [SerializeField]
        private WeaponPlatform m_platform;

        [SerializeField]
        private Slider m_slider;

        // Start is called before the first frame update
        private void Start()
        {
            Setup(m_platform);
        }

        //TODO - make public.
        private void Setup(IRechargeable rechargeable)
        {
            //TODO - cache rechargeable.
            rechargeable.Recharged += OnRecharged;

            void OnRecharged(float current, float total)
            {
                m_slider.maxValue = total;
                m_slider.minValue = 0;
                m_slider.value = current;

                m_slider.gameObject.SetActive(current < total);
            }
        }
    }
}