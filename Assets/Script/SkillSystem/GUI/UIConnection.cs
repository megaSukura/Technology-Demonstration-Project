using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIConnection : MonoBehaviour,IPointerClickHandler
{
    [SerializeField]
    private Image lineImage;
    private Vector2 startPos;
    private Vector2 endPos;
    private NodeConnectionHandler startConnectionHandler;
    private NodeConnectionHandler endConnectionHandler;
    float lastClickTime = 0;
    float doubleClickTimeThreshold = 0.2f; // 双击事件的时间阈值

    public void OnPointerClick(PointerEventData eventData)
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        if (timeSinceLastClick <= doubleClickTimeThreshold)
        {
            // 如果在阈值时间内双击，则删除UI元素
            Destroy(gameObject);
        }
        lastClickTime = Time.time;
    }
    public void SetPositions(Vector2 startPos, Vector2 endPos)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        Vector2 difference = endPos - startPos;
        float distance = difference.magnitude;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        lineImage.rectTransform.sizeDelta = new Vector2(distance/transform.parent.localScale.x, lineImage.rectTransform.sizeDelta.y);
        lineImage.rectTransform.pivot = new Vector2(0, 0.5f);
        lineImage.rectTransform.position = startPos;
        lineImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
    public void SetStart(Vector2 startPos,NodeConnectionHandler startConnectionHandler)
    {
        this.startConnectionHandler = startConnectionHandler;
        this.startPos = startPos;
        Vector2 difference = endPos - startPos;
        float distance = difference.magnitude;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        lineImage.rectTransform.sizeDelta = new Vector2(distance/transform.parent.localScale.x, lineImage.rectTransform.sizeDelta.y);
        lineImage.rectTransform.pivot = new Vector2(0, 0.5f);
        lineImage.rectTransform.position = startPos;
        lineImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
    public void SetEnd(Vector2 endPos, NodeConnectionHandler endConnectionHandler)
    {
        this.endConnectionHandler = endConnectionHandler;
        this.endPos = endPos;
        Vector2 difference = endPos - startPos;
        float distance = difference.magnitude;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        lineImage.rectTransform.sizeDelta = new Vector2(distance/transform.parent.localScale.x, lineImage.rectTransform.sizeDelta.y);
        lineImage.rectTransform.pivot = new Vector2(0, 0.5f);
        lineImage.rectTransform.position = startPos;
        lineImage.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetColor(Color color)
    {
        lineImage.color = color;
    }
    
    void OnDestroy()
    {
        
        if (startConnectionHandler != null)
        {
            startConnectionHandler.OnDisconnect(endConnectionHandler,this);
        }
        if (endConnectionHandler != null)
        {
            endConnectionHandler.OnDisconnect(startConnectionHandler,this);
        }
        
    }

}
