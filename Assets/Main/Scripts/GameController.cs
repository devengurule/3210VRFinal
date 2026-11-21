using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    public TransitionController transitionController { get; private set; }

    private void Awake()
    {
        // Creates singleton
        
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this);
            }
        }

        instance = this;

        transitionController = GetComponent<TransitionController>();
    }
}
