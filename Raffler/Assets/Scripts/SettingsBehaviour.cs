using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class SettingsBehaviour : MonoBehaviour
    {
        public ControlBehaviour Controller;


        [Header("UI Elements")]
        //UI Elements
        public CanvasGroup Panel;

        public TMP_InputField Input;

        private bool ShowSettingsPanel;
        
        private void Start()
        {
            Panel.alpha = 0f;
            ShowSettingsPanel = false;
            Input.text = Controller.maxNumber.ToString();
        }

        public void ToggleSettingsPanel()
        {
            ShowSettingsPanel = !ShowSettingsPanel;

            if (!ShowSettingsPanel)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

        private void Update()
        {
            if (ShowSettingsPanel && Panel.alpha < 0.99f)
            {
                Panel.alpha = 1f;
            }

            if (!ShowSettingsPanel && Panel.alpha > 0.01f)
            {
                Panel.alpha = 0f;
            }
        }

        public void Save()
        {
            try
            {
                Controller.maxNumber = int.Parse(Input.text);
                Controller.UsedNumbers.Clear();

                ToggleSettingsPanel();
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Wrong Input for maxNumber!");
                Input.text = Controller.maxNumber.ToString();
            }
        }
    }
}