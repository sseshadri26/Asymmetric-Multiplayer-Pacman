using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManage : MonoBehaviour
{
    // Start is called before the first frame update
    public void onClick() {
        if(SceneManager.GetActiveScene().buildIndex==0)
            SceneManager.LoadScene(1);
        if (SceneManager.GetActiveScene().buildIndex == 1)
            SceneManager.LoadScene(0);
    }
}
