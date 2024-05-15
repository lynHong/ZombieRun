using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveAnimation : MonoBehaviour
{
    private float width = 110.0f;
    private float height = 100.0f;
    public Text headerText;
    private string myText;
    private GameObject textObject;
    private RectTransform rTransform, rTransform2;
    public Transform target1, target2;
    private float speed = 200.0f;
    private Vector3 initial;

    //public Vector3 startP, endP;
    // Use this for initialization
    void Start()
    {
        textObject = GameObject.Find("textObj");
        rTransform = textObject.transform as RectTransform;
        rTransform2 = target2 as RectTransform;

        rTransform.sizeDelta = new Vector2(width, height);
        rTransform2.sizeDelta = new Vector2(width, height);
    }


    Transform getLoaderFromCanvas(Transform canvas, string nameOfImage)
    {
        Transform[] trans = canvas.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trans)
        {
            if (t.gameObject.name.Equals(nameOfImage))
            {
                return t;
            }
        }
        return null;
    }
    IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(5);
    }
    // Update is called once per frame
    void Update()
    {
        myText = headerText.text;

        // 更新 RectTransform 的尺寸以适应文本长度
        rTransform.sizeDelta = new Vector2(myText.Length * 11, height);
        setSizeofTarget2();

        // 仅在 x 轴上移动对象
        Vector3 targetPosition = new Vector3(target1.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 当到达目标位置时，切换到 target2 的位置，同时更新 pivot
        if (transform.position == targetPosition)
        {
            transform.position = new Vector3(target2.position.x, transform.position.y, transform.position.z);
            rTransform2.pivot = new Vector2(0, 0);
        }
    }


    private void setSizeofTarget2()
    {
        rTransform2.pivot = new Vector2(1, 0);
        rTransform2.sizeDelta = new Vector2(myText.Length * 11, height);
    }
}