using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
[RequireComponent(typeof(NodeConnectionHandler))]
public class TParameterOutputPort<T> : MonoBehaviour, IUIportOut<T> where T : struct
{
    public Parameter<T> instance;
    public Func<Parameter<T>> ParameterGetter;
    public NodeBase Owner{get{ return mOwner;} set{mOwner = value;}}
    private NodeBase mOwner;
    public TParameterOutputPort(Func<Parameter<T>> parameterGetter)
    {
        ParameterGetter = parameterGetter;
    }

    public Parameter<T> Build()
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
                GameObject go = results.Where(x => x.gameObject.GetComponent<IUIportIn<T>>() != null).FirstOrDefault().gameObject;
                IUIportIn<T> inPort = go?.GetComponent<IUIportIn<T>>();
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
            IUIportIn<T> inPort = handler.GetComponent<IUIportIn<T>>();
            if (inPort != null)
            {
                inPort.OnDesconnect(this);
            }
        };
    }
    
}

