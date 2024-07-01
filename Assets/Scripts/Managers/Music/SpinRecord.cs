using UnityEngine;

public class SpinRecord : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public RectTransform needleTransform;
    private bool isPlaying = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private float rotationDuration = 5f;
    private float rotationTime = 0f; 

    public bool IsPlaying
    {
        get { return isPlaying; }
    }

    void Start()
    {
        if (needleTransform != null)
        {
            initialRotation = needleTransform.rotation;
            targetRotation = Quaternion.Euler(needleTransform.eulerAngles + new Vector3(0, 0, 45f));
            needleTransform.pivot = new Vector2(0, 1);
        }
    }

    void Update()
    {
        if (isPlaying)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }

        if (needleTransform != null && rotationTime < rotationDuration)
        {
            rotationTime += Time.deltaTime;
            float t = rotationTime / rotationDuration;
            needleTransform.rotation = Quaternion.Lerp(needleTransform.rotation, targetRotation, t);
        }
    }

    public void StartPlaying()
    {
        isPlaying = true;
        RotateNeedle(-45f);
    }

    public void StopPlaying()
    {
        isPlaying = false;
        RotateNeedle(45f);
    }

    private void RotateNeedle(float angle)
    {
        if (needleTransform != null)
        {
            rotationTime = 0f;
            targetRotation = Quaternion.Euler(needleTransform.eulerAngles + new Vector3(0, 0, angle));
        }
        else
        {
            Debug.LogWarning("Needle Transform is not assigned.");
        }
    }
}
