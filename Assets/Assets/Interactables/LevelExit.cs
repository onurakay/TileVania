using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] int loadLevelDelay = 1;
    PlayerMovement playerMovement;

    void Start() {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && playerMovement != null && playerMovement.IsAlive)
        {
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(loadLevelDelay);
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        // Check if the next scene index is within bounds
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            // Handle the case where there is no next scene
            Debug.LogWarning("No next scene to load.");
            yield break;
        }

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }
}
