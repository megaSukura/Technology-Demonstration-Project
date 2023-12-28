using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TRefParameterMultiInputPort<T> : MonoBehaviour, IUIportRefIn<T> where T : class
{
    [SerializeField]
    private List<IUIportRefOut<T>> outPorts=new List<IUIportRefOut<T>>();
    public T defaultValue;
    public NodeBase Owner{get{ return mOwner;} set{mOwner = value;}}
    private NodeBase mOwner;
    public void OnConnect(IUIportRefOut<T> OutPort)
    {
        Debug.Log("OnConnect in MultiInputPort");
        if(!outPorts.Contains(OutPort))
            outPorts.Add(OutPort);
    }
    public void OnDesconnect(IUIportRefOut<T> OutPort)
    {
        Debug.Log("OnDesconnect in MultiInputPort");
        if(outPorts.Contains(OutPort))
        outPorts.Remove(OutPort);
    }
    public List<RefParameter<T>> Build()
    {
        if(outPorts == null)
        {
            return null;
        }
        return outPorts.Select(x=>x.Build()).ToList();
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
        connectionHandler.connectType = NodeConnectionHandler.UIConnectionType.MultiIn;
        connectionHandler.OnPointerUpFunc = (PointerEventData eventData) =>
        {
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results );
            if (results.Count > 0)
            {
                GameObject go = results.Where(x => x.gameObject.GetComponent<IUIportRefOut<T>>() != null).FirstOrDefault().gameObject;
                IUIportRefOut<T> outPort = go?.GetComponent<IUIportRefOut<T>>();
                if (outPort != null)
                {
                    if(outPort.Owner == Owner)
                        return null;
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
                IUIportRefOut<T> outPort = handler.GetComponent<IUIportRefOut<T>>();
                if(outPort != null )
                {
                    this.OnDesconnect(outPort);
                }
            }
        };
    }
}
