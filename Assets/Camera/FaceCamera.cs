using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}