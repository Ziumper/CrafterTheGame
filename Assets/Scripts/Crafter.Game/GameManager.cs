using UnityEngine;


namespace Crafter.Game
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public bool StartGameOrContinue()
        {
            PlayerBehaviour playerBehaviour = Object.FindObjectOfType<PlayerBehaviour>(true);
            if (playerBehaviour == null)
            {
                Debug.LogError("No player has been found can't start the game, please add it!");
                return false;
            }

            HideCursor();

            playerBehaviour.gameObject.SetActive(true);
            return true;
        }

        public void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        public void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
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
            UserInterfaceController.Instance.ShowStartPanel();
        }
    }

}