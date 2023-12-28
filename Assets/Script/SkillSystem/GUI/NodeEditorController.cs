using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
public class NodeEditorController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private GameObject contextMenuPrefab;
    public static NodeEditorController instance;
    public GameEntity targetEntity;
    private SkillSystem.SkillManager skillManager;
    public List<SkillNode> skillNodes = new List<SkillNode>();
    public List<AlwaysScheduleLineNode> alwaysScheduleLineNodes = new List<AlwaysScheduleLineNode>();
    [SerializeField]
    private float zoomSensitivity = 1.0f;
    [SerializeField]
    private float minZoom = 0.5f;
    [SerializeField]
    private float maxZoom = 2.0f;

    private Vector2 lastMousePosition;
    #region node
    public List<NodeBase> nodes = new List<NodeBase>();
    public NodeBase selectedNode;
    public void BuildSkill()
    {
        skillManager.ClearAll();
        SkillSystem.Skill[] skills = new SkillSystem.Skill[skillNodes.Count];
        for(int i=0;i<skillNodes.Count;i++)
        {
            skills[i] = skillNodes[i].BuildSkill();
            skillManager.Add(skills[i]);
            
        }
        
    }
    public void ConnectLine()
    {
        foreach (var item in alwaysScheduleLineNodes)
        {
            skillManager.ConnectAlways(item.GetSkill(),item.GetTrigger(),item.IsResetTargetReady);
        }
        foreach (var item in skillNodes)
        {
            item.ConnectScheduleLine();
        }
    }
    public NodeBase AddNode(NodeBase node)
    {
        if(!nodes.Contains(node))
        {
            nodes.Add(node);
            node.nodeEditorController = this;
            if(node is SkillNode)
            {
                skillNodes.Add(node as SkillNode);
            }
            if(node is AlwaysScheduleLineNode)
            {
                alwaysScheduleLineNodes.Add(node as AlwaysScheduleLineNode);
            }
        }
        return node;
    }
    public void DeleteSelectNode()
    {
        if (selectedNode != null)
        {
            if(selectedNode is Node)
            {
                (selectedNode as Node).OnDelete();
                Destroy(selectedNode.gameObject);
            }
            else
            {
                RemoveNode(selectedNode);
                Destroy(selectedNode.gameObject);
            }
        }
    }
    public void RemoveNode(NodeBase node)
    {
        if (nodes.Contains(node))
        {
            nodes.Remove(node);
            if (node is SkillNode)
            {
                skillNodes.Remove(node as SkillNode);
            }
            if (node is AlwaysScheduleLineNode)
            {
                alwaysScheduleLineNodes.Remove(node as AlwaysScheduleLineNode);
            }
        }
    }
    #endregion
    void Awake()
    {
        instance = this;
    }
    void OnEnable()
    {
        
    }
    void OnDisable()
    {
        if (skillManager != null)
        skillManager.IsEditorMode = false;
    }
    void Start()
    {
        skillManager = targetEntity.GetComponent<SkillSystem.SkillManager>();
    }
    private void Update()
    {
        if(skillManager!=null)
        skillManager.IsEditorMode = true;
        //维护选中状态
        foreach (var item in nodes)
        {
            if (item != selectedNode)
            {
                item.isSelected = false;
            }
        }
        //
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePosition = Input.mousePosition;
            OnRightClick(mousePosition);
            OnAnyPress(mousePosition);
        }
         if (Input.GetMouseButtonDown(2))
        {
            Vector2 mousePosition = Input.mousePosition;
            OnMiddleMouseDown(mousePosition);
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            OnMouseScroll(Input.mouseScrollDelta.y);
        }
        
        if (Input.GetMouseButton(0)|| Input.GetMouseButton(1)|| Input.GetMouseButton(2))
        {
            Vector2 mousePosition = Input.mousePosition;
            OnAnyPress(mousePosition);
        }
    }
    private void OnMiddleMouseDown(Vector2 position)
    {
        lastMousePosition = position;
    }

    private void OnMouseScroll(float delta)
    {
        float newZoom = Mathf.Clamp(transform.localScale.x + delta * zoomSensitivity * Time.deltaTime, minZoom, maxZoom);
        transform.localScale = new Vector3(newZoom, newZoom, 1);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            selectedNode = null;
        }
    }
    private void OnAnyPress(Vector2 position)
    {
        if (Input.GetMouseButton(2))
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastMousePosition;
            transform.position += (Vector3)delta;
            lastMousePosition = Input.mousePosition;
        }
    }

    private void OnRightClick(Vector2 position)
    {
        // PointerEventData pointerData = new PointerEventData(EventSystem.current)
        // {
        //     position = position
        // };

        // List<RaycastResult> results = new List<RaycastResult>();
        // EventSystem.current.RaycastAll(pointerData, results);
        //DOTO:对任意实现了IContextMenu接口的对象都可以创建上下文菜单
        
        CreateContextMenu(position);
    }
        private void CreateContextMenu(Vector2 position)
    {
        // 示例菜单项列表
        List<string> menuItems = new List<string> {"AlwaysScheduleLineNode","EntityFloatGetterParameterNode","EntityVector2GetterParameterNode","FloatParameterNode","ProgressTriggerNode","InputTriggerNode","ReadyLineNode","ScheduleLineNode","ShootSkillEffectNode","SkillNode","Vector2ParameterNode","ProjectileLogicSubNode"};

        // 创建上下文菜单实例
        GameObject contextMenuInstance = Instantiate(contextMenuPrefab, transform.root);
        contextMenuInstance.transform.position = position;

        // 初始化上下文菜单
        contextMenuInstance.GetComponent<ContextMenuController>().Initialize(menuItems, OnMenuItemClick);
        contextMenuInstance.transform.SetAsLastSibling();
    }

    private void OnMenuItemClick(string menuItem)
    {
        // 根据选择的菜单项创建不同类型的节点
        GameObject newNode = Instantiate(nodePrefab);
        newNode.transform.SetParent(transform, false);
        newNode.transform.position = Input.mousePosition;
        // 在这里添加自定义节点类型的脚本，例如：
            if (menuItem == "AlwaysScheduleLineNode") newNode.AddComponent<AlwaysScheduleLineNode>();
        else if (menuItem == "EntityFloatGetterParameterNode") newNode.AddComponent<EntityFloatGetterParameterNode>();
        else if (menuItem == "EntityVector2GetterParameterNode") newNode.AddComponent<EntityVector2GetterParameterNode>();
        else if (menuItem == "FloatParameterNode") newNode.AddComponent<FloatParameterNode>();
        else if (menuItem == "InputTriggerNode") newNode.AddComponent<InputTriggerNode>();
        else if (menuItem == "ProgressTriggerNode") newNode.AddComponent<ProgressTriggerNode>();
        else if (menuItem == "ReadyLineNode") newNode.AddComponent<ReadyLineNode>();
        else if (menuItem == "ScheduleLineNode") newNode.AddComponent<ScheduleLineNode>();
        else if (menuItem == "ShootSkillEffectNode") newNode.AddComponent<ShootSkillEffectNode>();
        else if (menuItem == "SkillNode") newNode.AddComponent<SkillNode>();
        else if (menuItem == "Vector2ParameterNode") newNode.AddComponent<Vector2ParameterNode>();
        else if (menuItem == "ProjectileLogicSubNode") newNode.AddComponent<ProjectileLogicSubNode>();
            }
}
