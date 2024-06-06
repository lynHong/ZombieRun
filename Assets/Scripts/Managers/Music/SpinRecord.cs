using UnityEngine;

public class SpinRecord : MonoBehaviour
{
    public float rotationSpeed = 100f; // 旋转速度
    public RectTransform needleTransform; // 撞针的 RectTransform
    private bool isPlaying = false; // 标记是否正在播放
    private Quaternion initialRotation; // 撞针的初始旋转
    private Quaternion targetRotation; // 撞针的目标旋转
    private float rotationDuration = 5f; // 旋转持续时间
    private float rotationTime = 0f; // 旋转计时

    // 公共属性用于访问 isPlaying 变量
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
            needleTransform.pivot = new Vector2(0, 1); // 设置 pivot 为左上角
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            // 按照指定速度旋转
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }

        if (needleTransform != null && rotationTime < rotationDuration)
        {
            rotationTime += Time.deltaTime;
            float t = rotationTime / rotationDuration;
            needleTransform.rotation = Quaternion.Lerp(needleTransform.rotation, targetRotation, t);
        }
    }

    // 开始播放
    public void StartPlaying()
    {
        isPlaying = true;
        RotateNeedle(-45f); // 撞针顺时针旋转45度
    }

    // 停止播放
    public void StopPlaying()
    {
        isPlaying = false;
        RotateNeedle(45f); // 撞针逆时针旋转45度
    }

    // 旋转撞针的方法
    private void RotateNeedle(float angle)
    {
        if (needleTransform != null)
        {
            rotationTime = 0f; // 重置旋转计时
            targetRotation = Quaternion.Euler(needleTransform.eulerAngles + new Vector3(0, 0, angle));
        }
        else
        {
            Debug.LogWarning("Needle Transform is not assigned.");
        }
    }
}
