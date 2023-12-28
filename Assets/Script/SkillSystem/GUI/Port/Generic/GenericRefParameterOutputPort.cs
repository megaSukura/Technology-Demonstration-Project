using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
[RequireComponent(typeof(NodeConnectionHandler))]
public class TRefParameterOutputPort<T> : MonoBehaviour, IUIportRefOut<T> where T : class
{
    public RefParameter<T> instance;
    public Func<RefParameter<T>> ParameterGetter;
    public NodeBase Owner{get{ return mOwner;} set{mOwner = value;}}
    private NodeBase mOwner;
    public TRefParameterOutputPort(Func<RefParameter<T>> parameterGetter)
    {
        ParameterGetter = parameterGetter;
    }

    public virtual RefParameter<T> Build()
    {
        //if (instance == null)//lazy init 会导致instance只能被初始化一次
            instance = ParameterGetter();
        return instance;
    }
    public void Delete()
    {
        connectionHandler.Delete();
    }
    //
    public NodeConnectionHandler connectionHandler;
    void Start()
    {
        connectionHandler = GetComponent<NodeConnectionHandler>();
        connectionHandler.connectType = NodeConnectionHandler.UIConnectionType.Out;
        connectionHandler.OnPointerUpFunc = (PointerEventData eventData) =>
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results );
            if (results.Count > 0)
            {
                GameObject go = results.Where(x => x.gameObject.GetComponent<IUIportRefIn<T>>() != null).FirstOrDefault().gameObject;
                IUIportRefIn<T> inPort = go?.GetComponent<IUIportRefIn<T>>();
                if (inPort != null)
                {
                    if(inPort.Owner == Owner)
                        return null;
                    inPort.OnConnect(this);
                    return go.GetComponent<NodeConnectionHandler>();
                }
            }
            return null;
        };
        connectionHandler.OnDisconnectAction = (NodeConnectionHandler handler) =>
        {
            IUIportRefIn<T> inPort = handler.GetComponent<IUIportRefIn<T>>();
            if (inPort != null)
            {
                inPort.OnDesconnect(this);
            }
        };
    }
    
}

