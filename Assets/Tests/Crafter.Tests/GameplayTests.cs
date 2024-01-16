using System.Collections;
using Crafter.Game;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;


namespace Crafter.Tests
{
    public class GameplayTests
    {
        [UnityTest]
        public IEnumerator ValidateGameplayScene()
        {
            var gameplaySceneIndex = 0;

            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(gameplaySceneIndex);

            yield return new WaitUntil(() => loadingOperation.isDone);

            var player = Object.FindObjectOfType<PlayerBehaviour>(true);
            Assert.That(!player.gameObject.activeInHierarchy, "Player is active,Please make sure the player game object is not active");
            var ui = Object.FindObjectOfType<UserInterfaceController>(true);
            Assert.That(ui.gameObject.activeInHierarchy, "Base UI is not active. Please, make sure it it's active in scene!");
        }
    }

}
