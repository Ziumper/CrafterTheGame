using UnityEngine;
using UnityEngine.Events;


namespace Crafter.Game
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public UnityEvent OnBackToMenu;

        public bool StartGameOrContinue()
        {
            PlayerBehaviour playerBehaviour = Object.FindObjectOfType<PlayerBehaviour>(true);
            if (playerBehaviour == null)
            {
                Debug.LogError("No player has been found can't start the game, please add it!");
                return false;
            }

            HideCursor();

            playerBehaviour.StartPlaying();
            return true;
        }

        public void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void BackToMenu()
        {
            ShowCursor();
            PlayerBehaviour playerBehaviour = Object.FindObjectOfType<PlayerBehaviour>(true);
            playerBehaviour.gameObject.SetActive(false);
            OnBackToMenu.Invoke();
        }
    }

}