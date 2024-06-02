using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCamera : MonoBehaviour
{
    public float panSpeed = 30f;
    public float panBorderThickness = 10f;
    public float scrollSpeed = 5f;
    public float minZoomY = 2f;
    public float maxZoomY = 30f;
    private bool isMouseCameraPanEnabled = true;
    private bool isMovementEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        isMouseCameraPanEnabled = GameController.instance.isMouseCameraPanEnabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovementEnabled){
            //Mouse and WASD Input
            if (Input.GetKey("w") || (Input.mousePosition.y >= Screen.height - panBorderThickness) && isMouseCameraPanEnabled) {
                transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("d") || (Input.mousePosition.x >= Screen.width - panBorderThickness) && isMouseCameraPanEnabled) {
                transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("a") || (Input.mousePosition.x <= panBorderThickness) && isMouseCameraPanEnabled) {
                transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey("s") || (Input.mousePosition.y <= panBorderThickness) && isMouseCameraPanEnabled) {
                transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel"); 
            Vector3 currentPosition = transform.position;
            currentPosition.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
            currentPosition.y = Mathf.Clamp(currentPosition.y, minZoomY, maxZoomY);
            transform.position = currentPosition;
        }
    }
}
