using UnityEngine;

public class playerCameraLook : MonoBehaviour
{

    public Camera cam;
    private float xRotation = 0f;

    //mouse sensitivity 
    public float xSense = 30f;
    public float ySense = 30f;

    public void lookProcess(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // For looking up and down using mouse 
        xRotation -= (mouseY * Time.deltaTime) * ySense;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        //camera transform 
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //For looking to the left and right using mouse 
        transform.Rotate(Vector3.up * (mouseX *  Time.deltaTime) * xSense);
    }

}
