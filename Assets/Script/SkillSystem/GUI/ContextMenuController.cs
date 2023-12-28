using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ContextMenuController : MonoBehaviour
{
    [SerializeField] private Button menuItemPrefab;

    public void Initialize(List<string> menuItems, System.Action<string> onItemClick)
    {
        foreach (string menuItem in menuItems)
        {
            Button menuItemInstance = Instantiate(menuItemPrefab, transform);
            menuItemInstance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = menuItem;
            menuItemInstance.onClick.AddListener(() =>
            {
                onItemClick?.Invoke(menuItem);
                Destroy(gameObject);
            });
        }
    }
        bool _firstButtonUp = false;
    void Update()
    {
        if(Input.GetMouseButtonUp(0)||Input.GetMouseButtonUp(1)||Input.GetMouseButtonUp(2))
        {
            if(!_firstButtonUp)
            {
                _firstButtonUp = true;
                return;
            }
            Check();
        }
    }

    void Check()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        if(results.Count>0)
        {
            foreach (var item in results)
            {
                if(item.gameObject==gameObject)
                {
                    return;
                }
            }
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
