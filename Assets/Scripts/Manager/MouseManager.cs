using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class MouseManager : Singleton<MouseManager>
{
    //�������ָ��
    public Texture2D point, doorway, attack, target, arrow;
    
    //����������ײ����������Ϣ
    RaycastHit hitInfo; 

    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClick;

    protected override void Awake()
    {
        base.Awake();
        //��֤�ڼ���ʱ��Ȼ���ڣ�ʹ�����ܹ�˳������
        DontDestroyOnLoad(this);
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
                case "Attackable":
                    //vector��ʾָ����Чλ�������Ͻǵ�ƫ����
                    Cursor.SetCursor(attack, new Vector2(16, 16), cursorMode: CursorMode.Auto);
                    break;
                case "Portal":
                    //vector��ʾָ����Чλ�������Ͻǵ�ƫ����
                    Cursor.SetCursor(doorway, new Vector2(16, 16), cursorMode: CursorMode.Auto);
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
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClick?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
            {
                OnEnemyClick?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Portal"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
        }
    }
}
