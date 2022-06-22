using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Final());
    }

    private IEnumerator Final()
    {
        yield return new WaitForSeconds(7);

        SceneManager.LoadScene(0);
    }
}
