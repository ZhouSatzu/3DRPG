using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class MouseManager : Singleton<MouseManager>
{
    //五种鼠标指针
    public Texture2D point, doorway, attack, target, arrow;
    
    //保存射线碰撞物体的相关信息
    RaycastHit hitInfo; 

    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClick;

    protected override void Awake()
    {
        base.Awake();
        //保证在加载时依然存在，使加载能够顺利进行
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        SetCursorTexture();
        MouseControl();
    }

    void SetCursorTexture()
    {
        //从主摄像机往鼠标指针处的射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //切换鼠标贴图
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    //vector表示指针有效位置自左上角的偏移量
                    Cursor.SetCursor(target, new Vector2(16, 16), cursorMode: CursorMode.Auto);
                    break;
                case "Enemy":
                    //vector表示指针有效位置自左上角的偏移量
                    Cursor.SetCursor(attack, new Vector2(16, 16), cursorMode: CursorMode.Auto);
                    break;
                case "Attackable":
                    //vector表示指针有效位置自左上角的偏移量
                    Cursor.SetCursor(attack, new Vector2(16, 16), cursorMode: CursorMode.Auto);
                    break;
                case "Portal":
                    //vector表示指针有效位置自左上角的偏移量
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
