using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void GotoDeepOcean ()
    {
        // Entrance에서 녹음 종료 후 호출
        SceneManager.LoadScene("DeepOcean");
    }
}
