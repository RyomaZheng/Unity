using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class MouseManager : Singleton<MouseManager>
{
    public Texture2D point, doorway, attack, target, arrow;

    RaycastHit raycastHit;

    public event UnityAction<Vector3> OnMouseClicked;
    public event UnityAction<GameObject> OnEnemyClicked;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        SetCursorTexture();
        if (Input.GetMouseButtonDown(0) && InteractWithUI())
        {
            return;
        }
        MouseControl();
    }

    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 判断光线是否有碰撞
        if (Physics.Raycast(ray, out raycastHit))
        {
            // 切换鼠标贴图
            switch (raycastHit.collider.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Portal":
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Item":
                    Cursor.SetCursor(point, new Vector2(16, 16), CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseControl()
    {
        // 点击鼠标左键 且 raycastHit的碰撞器是否不为空
        if (Input.GetMouseButtonDown(0) && raycastHit.collider != null)
        {
            // 判断raycastHit是否地面标签
            if (raycastHit.collider.CompareTag("Ground"))
            {
                // 使用raycastHit的point，触发OnMouseClicked事件
                OnMouseClicked?.Invoke(raycastHit.point);
            }

            // 判断raycastHit是否敌人标签
            if (raycastHit.collider.CompareTag("Enemy"))
            {
                // 使用raycastHit.collider的gameObject，触发OnEnemyClicked事件
                OnEnemyClicked?.Invoke(raycastHit.collider.gameObject);
            }

            // 判断raycastHit是否可攻击标签
            if (raycastHit.collider.CompareTag("Attackable"))
            {
                // 使用raycastHit.collider的gameObject，触发OnEnemyClicked事件
                OnEnemyClicked?.Invoke(raycastHit.collider.gameObject);
            }

            // 判断raycastHit是否传送门标签
            if (raycastHit.collider.CompareTag("Portal"))
            {
                // 使用raycastHit的point，触发OnMouseClicked事件
                OnMouseClicked?.Invoke(raycastHit.point);
            }

            // 判断raycastHit是否传送门标签
            if (raycastHit.collider.CompareTag("Item"))
            {
                // 使用raycastHit的point，触发OnMouseClicked事件
                OnMouseClicked?.Invoke(raycastHit.point);
            }

        }
    }

    bool InteractWithUI()
    {
        Debug.Log("EventSystem.current.IsPointerOverGameObject():" + EventSystem.current.IsPointerOverGameObject());
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        else
        {
            return false;

        }
    }
}
