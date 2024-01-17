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

            playerBehaviour.gameObject.SetActive(true);
            return true;
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void BackToMenu()
        {
            PlayerBehaviour playerBehaviour = Object.FindObjectOfType<PlayerBehaviour>(true);
            playerBehaviour.gameObject.SetActive(false);
            UserInterfaceController.Instance.ShowStartPanel();
        }
    }

}