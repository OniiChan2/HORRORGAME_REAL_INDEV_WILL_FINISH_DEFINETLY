using UnityEngine;

public class playerHeadBobSystem : MonoBehaviour {

    [SerializeField] private CharacterController characterController;
    [SerializeField] private float BobFreq = 10f;
    [SerializeField] private float BobAmount = 0.002f;
    [SerializeField] private float Smooth = 10f;
    [SerializeField] private Vector3 StartPOS; 

    void Start()
    {
        StartPOS = transform.localPosition;
    }

 
    void Update()
    {
        CheckForHeadBob();
        StopHeadBob();
    }

    private void CheckForHeadBob()
    {
        float inputMag = new Vector3(characterController.velocity.x, 0f, characterController.velocity.y).magnitude;

        if (inputMag > 0)
        {
            startHeadBob();
        }
    }

    private Vector3 startHeadBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y = Mathf.Lerp(pos.y, Mathf.Sin(Time.time * BobFreq) * BobAmount * 1.4f, Smooth * Time.deltaTime);
        pos.x = Mathf.Lerp(pos.y, Mathf.Cos(Time.time * BobFreq / 2f) * BobAmount * 1.6f, Smooth * Time.deltaTime);
        transform.position += pos;

        return pos;
    
    }

    private void StopHeadBob()
    {
        if (transform.localPosition == StartPOS) return;
        transform.localPosition = Vector3.Lerp(transform.localPosition, StartPOS, 1 * Time.deltaTime);
    }



}
