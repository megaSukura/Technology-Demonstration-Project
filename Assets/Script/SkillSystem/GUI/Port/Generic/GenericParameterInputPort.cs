using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
public class TParameterInputPort<T> : MonoBehaviour, IUIportIn<T> where T : struct
{
    [SerializeField]
    private IUIportOut<T> outPort;
    public T defaultValue;
  public NodeBase Owner{get{ return mOwner;} set{mOwner = value;}}
    private NodeBase mOwner;
    public void OnConnect(IUIportOut<T> OutPort)
    {
        outPort = OutPort;
    }
    public void OnDesconnect(IUIportOut<T> OutPort)
    {
        outPort = null;
    }
    public Parameter<T> Build()
    {
        if(outPort == null)
        {
            return defaultValue;
        }
        return outPort.Build();
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
        connectionHandler.connectType = NodeConnectionHandler.UIConnectionType.In;
        connectionHandler.OnPointerUpFunc = (PointerEventData eventData) =>
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results );
            if (results.Count > 0)
            {
                GameObject go = results. Where(x => x.gameObject.GetComponent<IUIportOut<T>>() != null).FirstOrDefault().gameObject;
                IUIportOut<T> outPort = go?.GetComponent<IUIportOut<T>>();
                if (outPort != null)
                {
                    if(outPort.Owner == Owner)
                    {    
                        
                        return null;
                    }
                    this.OnConnect(outPort);
                    return go.GetComponent<NodeConnectionHandler>();
                }
            }
            return null;
        };
        connectionHandler.OnDisconnectAction = (NodeConnectionHandler handler) =>
        {
            if(handler.connectType == NodeConnectionHandler.UIConnectionType.Out)
            {
                IUIportOut<T> outPort = handler.GetComponent<IUIportOut<T>>();
                if(outPort != null && outPort == this.outPort)
                {
                    this.OnDesconnect(outPort);
                }
            }
        };
    }
}
