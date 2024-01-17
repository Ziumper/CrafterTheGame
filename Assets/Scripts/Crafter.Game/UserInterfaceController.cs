using System;
using UnityEngine;

namespace Crafter.Game
{
    public class UserInterfaceController : MonoSingleton<UserInterfaceController>
    {
        [SerializeField] private GameObject _starPanel;

        public void ShowStartPanel()
        {
            _starPanel.SetActive(true);
        }

        public void OnStartOrContinueGameClick()
        {
            GameManager.Instance.StartGameOrContinue();
        }

        public void OnExitGameClick()
        {
            GameManager.Instance.ExitGame();
        }

    }

}