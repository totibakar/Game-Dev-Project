using UnityEngine;

public class FPS_Move : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float minimumY = -60f;
    float maximumY = 60f;
    float speedPutar = 5f;
    float rotationY = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //putar horizontal
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * speedPutar;
        this.transform.localEulerAngles = new Vector3(0, rotationX, 0);
        //putar vertikal
        rotationY += Input.GetAxis("Mouse Y") * speedPutar;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        Camera.main.transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        //gerakan karakter
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        this.transform.Translate(new Vector3(h, 0, v) * Time.deltaTime * 5f);
    }
}
