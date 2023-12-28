using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : NodeBase
{
    /// <summary>
    /// Prefab需要在运行时赋值
    /// </summary>
    public static GameObject NodePrefab;
    public GameObject header;
    public GameObject body_input;
    public GameObject body_output;
    private void Start() {
        Init();
    }
    public void Init()
    {
        nodeEditorController = transform.parent.GetComponent<NodeEditorController>();
        nodeEditorController.AddNode(this);
        header = transform.Find("header").gameObject;
        body_input = transform.Find("body").Find("inputArea").gameObject;
        body_output = transform.Find("body").Find("outputArea").gameObject;
        SetHeader(header);
        SetBody(body_input, body_output);
    }
    protected Action deleteAction;
    public virtual void OnDelete()
    {
        nodeEditorController.RemoveNode(this);
        deleteAction?.Invoke();
    }
    public abstract void SetHeader(GameObject header);
    public abstract void SetBody(GameObject body_input, GameObject body_output);

}
