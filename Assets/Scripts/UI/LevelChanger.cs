/*
 * Fades out to another scene.
 * 
 * Author: Cristion Dominguez
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The animator responsible for fading in and out of scenes")]
    private Animator fadeAnimator;

    public static LevelChanger singleton;  // for accessing this script without instantiating a reference
    private string sceneToLoad;  // the next scene to load

    /// <summary>
    /// Convert this script into a singleton.
    /// </summary>
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Prevent the scene from fading in.
    /// </summary>
    public void PreventFadeIn()
    {
        fadeAnimator.SetBool("FadeIn", false);
    }

    /// <summary>
    /// Sets the next scene to load and fades out the current scene.
    /// </summary>
    /// <param name="levelName"> the level to load after fading out </param>
    public void FadeOutToLevel(string levelName)
    {
        sceneToLoad = levelName;
        fadeAnimator.SetTrigger("FadeOut");
    }

    /// <summary>
    /// Loads the next scene to load once the fade out animation completes.
    /// </summary>
    private void OnFadeOutComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
