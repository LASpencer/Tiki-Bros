using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonManager : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("S1_Tutorial v2");
    }

}
