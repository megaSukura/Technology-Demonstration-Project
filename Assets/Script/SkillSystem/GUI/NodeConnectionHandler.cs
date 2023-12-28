using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
public class NodeConnectionHandler : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private UIConnection connectionPrefab;

    private UIConnection currentConnection;
    private bool isConnecting = false;
    public UIConnectionType connectType ;
    public List<UIConnection> connectedConnections=new List<UIConnection>();
    public void Delete()
    {
        foreach (var item in connectedConnections)
        {
            
            Debug.Log("Delete");
            Destroy(item.gameObject);
        }
        connectedConnections.Clear();
    }

    public Func<PointerEventData,NodeConnectionHandler> OnPointerUpFunc;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isConnecting = true;
            currentConnection = Instantiate(connectionPrefab, NodeEditorController.instance.transform); 
            currentConnection.SetColor(UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1));
        }
    }

    public void mOnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        if (isConnecting&&eventData.button == PointerEventData.InputButton.Left)
        {

            isConnecting = false;

            if (OnPointerUpFunc!=null)
            {   
                NodeConnectionHandler otherConnectionHandler;
                
                if(otherConnectionHandler =OnPointerUpFunc(eventData))
                {
                    if(connectType==UIConnectionType.In&&otherConnectionHandler.connectType==UIConnectionType.Out)
                    {
                        if(connectedConnections.Count>0)
                        {
                            Destroy(connectedConnections[0].gameObject);
                            connectedConnections.Clear();
                            connectedConnections.Add(currentConnection);
                            otherConnectionHandler.OnBeConnected(this);
                        }
                        else
                        {
                            
                            connectedConnections.Add(currentConnection);
                            otherConnectionHandler.OnBeConnected(this);
                        }
                    }
                    else if(connectType==UIConnectionType.Out&&otherConnectionHandler.connectType==UIConnectionType.In)
                    {
                        connectedConnections.Add(currentConnection);
                        otherConnectionHandler.OnBeConnected(this);
                    }
                    else if(connectType==UIConnectionType.MultiIn&&otherConnectionHandler.connectType==UIConnectionType.Out)
                    {
                        connectedConnections.Add(currentConnection);
                        otherConnectionHandler.OnBeConnected(this);
                    }
                    else if(connectType==UIConnectionType.Out&&otherConnectionHandler.connectType==UIConnectionType.MultiIn)
                    {
                        connectedConnections.Add(currentConnection);
                        otherConnectionHandler.OnBeConnected(this);
                    }
                    else
                    {
                        Destroy(currentConnection.gameObject);
                    }
                    currentConnection=null;
                    return;
                }
                else
                {
                    
                    Destroy(currentConnection.gameObject);
                    return;
                }
            }
            Destroy(currentConnection.gameObject);
        }
    }
    public void OnBeConnected(NodeConnectionHandler otherConnectionHandler)
    {
        if (otherConnectionHandler.connectType == connectType)
        return;
        
        connectedConnections.Add(otherConnectionHandler.currentConnection);
    }
    public Action<NodeConnectionHandler> OnDisconnectAction;
    public void OnDisconnect(NodeConnectionHandler otherConnectionHandler,UIConnection connection)
    {
        if(otherConnectionHandler)
        {
        connectedConnections.Remove(connection);
        
        OnDisconnectAction?.Invoke(otherConnectionHandler);
        }
    }

    private void Update()
    {
        if (isConnecting)
        {
            Vector3 mousePosition = Input.mousePosition;
            if(connectType==UIConnectionType.In||connectType==UIConnectionType.MultiIn)
            {
                currentConnection.SetPositions( transform.position,mousePosition);
            }
            else
            {
                currentConnection.SetPositions(mousePosition,transform.position);
            }
        //
            if(Input.GetMouseButtonUp(0))
            {
                var t=new PointerEventData(EventSystem.current);
                t.position=Input.mousePosition;
                mOnPointerUp(t);
            }
        
        }
        
        if(connectType==UIConnectionType.In&&connectedConnections.Count>1)
        {
            
            Destroy(connectedConnections[0].gameObject);
            connectedConnections.RemoveAt(0);
        }

        foreach (var item in connectedConnections)
        {
            if (! item )
            {
                
                connectedConnections.Remove(item);
                break;
            }
            if(connectType==UIConnectionType.In||connectType==UIConnectionType.MultiIn)
            {
                item.SetStart(transform.position,this);
            }
            else
            {
                item.SetEnd(transform.position,this);
            }
        }
        
    }
public enum UIConnectionType
{
    In,//只能有一个连接
    Out,//可以有多个连接
    MultiIn,//可以有多个连接
}
    
}