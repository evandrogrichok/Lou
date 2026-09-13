using UnityEngine;

public class EyesBlinkController : MonoBehaviour
{

    [SerializeField] private float minBlinkInterval = 5f;
    [SerializeField] private float maxBlinkInterval = 15f;

    private Animator animator;
    private float blinkTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        ResetBlinkTimer();
        
    }

    // Update is called once per frame
    void Update()
    {
        blinkTimer -= Time.deltaTime;

        if (blinkTimer <= 0)
        {
            animator.SetTrigger("Blink");
            ResetBlinkTimer();

        }

    }

    private void ResetBlinkTimer()
    {
        blinkTimer = Random.Range(minBlinkInterval, maxBlinkInterval);
    }
}
