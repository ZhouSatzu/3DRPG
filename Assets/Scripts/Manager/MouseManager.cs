using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class MouseManager : MonoBehaviour
{
    //����ģʽ
    public static MouseManager instance;

    //�������ָ��
    public Texture2D point, doorway, attack, target, arrow;
    
    //����������ײ����������Ϣ
    RaycastHit hitInfo; 

    public event Action<Vector3> OnMouseClicked;

    private void Awake()
    {
        if (instance != null) 
            Destroy(gameObject);

        instance = this;
    }

    private void Update()
    {
        SetCursorTexture();
        MouseControl();
    }

    void SetCursorTexture()
    {
        //��������������ָ�봦������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //�л������ͼ
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    //vector��ʾָ����Чλ�������Ͻǵ�ƫ����
                    Cursor.SetCursor(target, new Vector2(16, 16), cursorMode: CursorMode.Auto);
                    break;
                case "Enemy":
                    //vector��ʾָ����Чλ�������Ͻǵ�ƫ����
                    Cursor.SetCursor(attack, new Vector2(16, 16), cursorMode: CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
        }
    }
}
