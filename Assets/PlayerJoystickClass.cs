using UnityEngine;
using System.Collections;
using System;
 
 
/// <summary>
/// 测试游戏手柄键值
/// </summary>
public class PlayerJoystickClass : MonoBehaviour
{
    private string currentButton;//当前按下的按键
    private string currentAxis;//当前移动的轴向
    private float axisInput;//当前轴向的值
 
    void Update()
    {
        getAxis();
        getButtons();
    }
 
    /// <summary>
    /// Get Button data of the joysick
    /// </summary>
    void getButtons()
    {
        var values = Enum.GetValues(typeof(KeyCode));//存储所有的按键
        for (int x = 0; x < values.Length; x++)
        {
            if (Input.GetKeyDown((KeyCode)values.GetValue(x)))
            {
                currentButton = values.GetValue(x).ToString();//遍历并获取当前按下的按键
            }
        }
    }
 
    /// <summary>
    /// Get Axis data of the joysick
    /// </summary>
    void getAxis()
    {
        if (Input.GetAxisRaw("X axis") > 0.3 || Input.GetAxisRaw("X axis") < -0.3)
        {
            currentAxis = "X axis";
            axisInput = Input.GetAxisRaw("X axis");
        }
 
        if (Input.GetAxisRaw("Y axis") > 0.3 || Input.GetAxisRaw("Y axis") < -0.3)
        {
            currentAxis = "Y axis";
            axisInput = Input.GetAxisRaw("Y axis");
        }
 
        if (Input.GetAxisRaw("3rd axis") > 0.3 || Input.GetAxisRaw("3rd axis") < -0.3)
        {
            currentAxis = "3rd axis";
            axisInput = Input.GetAxisRaw("3rd axis");
        }
 
        if (Input.GetAxisRaw("4th axis") > 0.3 || Input.GetAxisRaw("4th axis") < -0.3)
        {
            currentAxis = "4th axis";
            axisInput = Input.GetAxisRaw("4th axis");
        }
 
        if (Input.GetAxisRaw("5th axis") > 0.3 || Input.GetAxisRaw("5th axis") < -0.3)
        {
            currentAxis = "5th axis";
            axisInput = Input.GetAxisRaw("5th axis");
        }
 
        if (Input.GetAxisRaw("6th axis") > 0.3 || Input.GetAxisRaw("6th axis") < -0.3)
        {
            currentAxis = "6th axis";
            axisInput = Input.GetAxisRaw("6th axis");
        }
 
        if (Input.GetAxisRaw("7th axis") > 0.3 || Input.GetAxisRaw("7th axis") < -0.3)
        {
            currentAxis = "7th axis";
            axisInput = Input.GetAxisRaw("7th axis");
        }
 
        if (Input.GetAxisRaw("8th axis") > 0.3 || Input.GetAxisRaw("8th axis") < -0.3)
        {
            currentAxis = "8th axis";
            axisInput = Input.GetAxisRaw("8th axis");
        }
 
        if (Input.GetAxisRaw("9th axis") > 0.3 || Input.GetAxisRaw("9th axis") < -0.3)
        {
            currentAxis = "9th axis";
            axisInput = Input.GetAxisRaw("9th axis");
        }
 
        if (Input.GetAxisRaw("10th axis") > 0.3 || Input.GetAxisRaw("10th axis") < -0.3)
        {
            currentAxis = "10th axis";
            axisInput = Input.GetAxisRaw("10th axis");
        }
 
        if (Input.GetAxisRaw("11th axis") > 0.3 || Input.GetAxisRaw("11th axis") < -0.3)
        {
            currentAxis = "11th axis";
            axisInput = Input.GetAxisRaw("11th axis");
        }
 
        if (Input.GetAxisRaw("12th axis") > 0.3 || Input.GetAxisRaw("12th axis") < -0.3)
        {
            currentAxis = "12th axis";
            axisInput = Input.GetAxisRaw("12th axis");
        }
 
        if (Input.GetAxisRaw("13th axis") > 0.3 || Input.GetAxisRaw("13th axis") < -0.3)
        {
            currentAxis = "13th axis";
            axisInput = Input.GetAxisRaw("13th axis");
        }
 
        if (Input.GetAxisRaw("14th axis") > 0.3 || Input.GetAxisRaw("14th axis") < -0.3)
        {
            currentAxis = "14th axis";
            axisInput = Input.GetAxisRaw("14th axis");
        }
    }
 
    /// <summary>
    /// show the data onGUI
    /// </summary>
    void OnGUI()
    {
        GUI.TextArea(new Rect(0, 0, 250, 50), "Current Button : " + currentButton);//使用GUI在屏幕上面实时打印当前按下的按键
        GUI.TextArea(new Rect(0, 100, 250, 50), "Current Axis : " + currentAxis);//使用GUI在屏幕上面实时打印当前按下的轴
        GUI.TextArea(new Rect(0, 200, 250, 50), "Axis Value : " + axisInput);//使用GUI在屏幕上面实时打印当前按下的轴的量
    }
}