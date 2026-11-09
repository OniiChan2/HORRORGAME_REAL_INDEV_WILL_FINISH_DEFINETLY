using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading;

public class playerCameraLook : MonoBehaviour
{
    public Camera cam;
    [Header("Camera & Look Settings")]
    [SerializeField] private Transform camHolder;
    [SerializeField] private float LookUpMax = -75f;
    [SerializeField] private float LookDownMax = 55f;
    private float xRotation = 0f;

    //mouse sensitivity 
    [Header("Mouse Sensitivity")]
    public float xSense = 30f;
    public float ySense = 30f;

    //Head-Bobbing Settings 
    [Header("Camera / Head Bobbing Settings")]
    [SerializeField] private float walkHeadBobFreq = 10f;
    [SerializeField] private float SprintHeadBobFreq = 20f;
    [SerializeField] private float HeadBobAmount = 0.05f;
    [SerializeField] private float HeadBobSmoothing = 10f;
    private float HeadBobFreq;
    private Vector3 StartPos;
    private playerMovement movement;

    private void Start()
    {
        StartPos = camHolder.transform.localPosition;
        movement = GetComponent<playerMovement>();
    }

    private void Update()
    {
        StopHeadBob();
    }

    //look process for the camera 
    public void lookProcess(Vector2 input, Vector2 Kb_Input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        float inputMagnude = Kb_Input.magnitude;

        //Head Bobbing Trigger 
        if (Kb_Input.magnitude > 0)
        {
            StartHeadBob();
        }

        // For looking up and down using mouse 
        xRotation -= (mouseY * Time.deltaTime) * ySense;
        xRotation = Mathf.Clamp(xRotation, LookUpMax, LookDownMax);

        //camera transform 
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //For looking to the left and right using mouse 
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSense);
     
    }



    //Head Bobbing Logic
    private Vector3 StartHeadBob()
    {
        HeadBobFreq = movement.isSprinting ? SprintHeadBobFreq : walkHeadBobFreq;
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * HeadBobFreq) * HeadBobAmount * 1.4f, HeadBobSmoothing * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * HeadBobFreq / 2f) * HeadBobAmount * 1.6f, HeadBobSmoothing * Time.deltaTime);
        camHolder.transform.localPosition += pos;

        return pos;
    }

    private void StopHeadBob()
    {
        if (camHolder.transform.localPosition == StartPos) return;
        camHolder.transform.localPosition = Vector3.Lerp(camHolder.transform.localPosition, StartPos, 1 * Time.deltaTime);
    }



}
