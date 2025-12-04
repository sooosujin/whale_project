using UnityEngine;
using UnityEngine.Playables;

public class SequenceStarter : MonoBehaviour
{
    private PlayableDirector director;

    void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }

    void Start()
    {
        if (director != null)
        {
            director.time = 0;
            director.Play();
        }
    }
}
