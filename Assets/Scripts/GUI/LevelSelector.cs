using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public string LevelName;

    public virtual void GoToLevel()
    {
        LevelManager.Instance.GotoLevel(LevelName);
    }

}
