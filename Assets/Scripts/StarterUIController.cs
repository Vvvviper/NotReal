using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarterUIController : MonoBehaviour
{
    public void StartPressed()
    {
        SceneManager.LoadScene(1);
    }

}
