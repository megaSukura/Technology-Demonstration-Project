using System.Collections.Generic;
using System;
/// <summary>
/// 用于提供一个可复杂组合的的面板计算
/// </summary>
public class PanleLayer
{
    public string LayerName;
    public float FixedPanle { get; set; }
    public float PercentagePanle { get; set; }
    public bool IncludeBaseValue { get; set; }
    public bool IncludeOwnFixedPanle { get; set; }
    public List<PanleLayer> IncludedLayers { 
        get{return _includedLayers;} 
        set{ 
            _includedLayers = value;
            CheckForCircularReference(new HashSet<PanleLayer>());
     } }
    private List<PanleLayer> _includedLayers;
    public PanleLayer(string layerName,float fixedPanle, float percentagePanle, bool includeBaseValue, bool includeOwnFixedPanle, List<PanleLayer> includedLayers)
    {
        LayerName=layerName;
        FixedPanle = fixedPanle;
        PercentagePanle = percentagePanle;
        IncludeBaseValue = includeBaseValue;
        IncludeOwnFixedPanle = includeOwnFixedPanle;
        IncludedLayers = includedLayers;
    }

    public float CalculatePanle(float baseValue)
    {
        float result = 0;
        if (IncludeBaseValue)
        {
            result += baseValue;
        }
        if (IncludeOwnFixedPanle)
        {
            result += FixedPanle;
        }
        if (IncludedLayers != null)
        {
            foreach (var layer in IncludedLayers)
            {
                result += layer.CalculatePanle(baseValue);
            }
        }
        result = result * ( PercentagePanle) + FixedPanle;
        return result;
    }

    public void CheckForCircularReference(HashSet<PanleLayer> visitedLayers)
    {
        if (visitedLayers.Contains(this))
        {
            throw new Exception("Circular reference detected in PanleLayer");
        }
        visitedLayers.Add(this);
        if (IncludedLayers != null)
        {
            foreach (var layer in IncludedLayers)
            {
                layer.CheckForCircularReference(visitedLayers);
            }
        }
    }
}


/// <summary>
/// 用于将多个PanleLayer组合成一个复杂的面板计算
/// </summary>
public class PanleFramework
{
    public List<PanleLayer> _layers;
    private List<PanleLayer> _layersToInclude;

    public PanleFramework(List<PanleLayer> layers)
    {
        _layers = layers;
    }

    public List<PanleLayer> LayersToInclude
    {
        get { return _layersToInclude; }
        set
        {
            CheckForCircularReference(value);
            _layersToInclude = value;
        }
    }

    public float CalculatePanle(float baseValue)
    {
        float result = baseValue;
        if(_layers!=null&&_layersToInclude!=null)
            foreach (var layer in _layersToInclude)
            {
                result += layer.CalculatePanle(baseValue);
            }
        return result;
    }

    public PanleLayer this[string name]
    {
        get { return _layers.Find( (e)=>e.LayerName==name ); }
        set {  }
    }

    private void CheckForCircularReference(List<PanleLayer> layers)
    {
        foreach (var layer in layers)
        {
            // if (visitedLayers.Contains(layer))
            // {
            //     throw new Exception("Circular reference detected in PanleLayer");
            // }
            // visitedLayers.Add(layer);

            layer.CheckForCircularReference(new HashSet<PanleLayer>());
        }
    }
}
