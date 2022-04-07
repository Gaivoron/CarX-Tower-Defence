using UnityEngine;
using TMPro;

namespace TowerDefence.UI
{
    public sealed class LivesCounterView : MonoBehaviour
    {
        [SerializeField]
        private GameplayInitializer m_initializer;
        [SerializeField]
        private TextMeshProUGUI m_text;

        private IGameplayData m_data;

        private void Start()
        {
            Setup(m_initializer);
        }

        //TODO - should be public.
        private void Setup(IGameplayData data)
        {
            //TODO - unsubscribe previous m_data?

            m_data = data;
            m_data.LiveForceChanged += OnLiveForceChanged;

            OnLiveForceChanged(m_data.LiveForce);
        }

        private void OnLiveForceChanged(int value)
        {
            m_text.text = $"Citizens : {value}";
        }
    }
}