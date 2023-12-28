using UnityEngine;
using UnityEngine.EventSystems;

public class NodeBase : MonoBehaviour, IPointerClickHandler
{
    public bool isSelected { get { return _isSelected; } set { _isSelected = value; OnNodeSelected(_isSelected); } }
    private bool _isSelected = false;
    public NodeEditorController nodeEditorController;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isSelected = true;
            transform.SetAsLastSibling();
            nodeEditorController.selectedNode = this;
            OnNodeSelected(isSelected);
        }
    }
    private bool _isInit = false;
    protected virtual void OnNodeSelected(bool selected)
    {
        //debug
        //Debug.Log("OnNodeSelected"+selected);
        GetComponent<UnityEngine.UI.Image>().color = selected ? Color.gray : Color.gray*0.7f;
        if(!_isInit)
        {
            _isInit = true;
            if(!nodeEditorController.nodes.Contains(this))
            {
                nodeEditorController.nodes.Add(this);
                _isInit = true;
            }
        }
    }
   
}