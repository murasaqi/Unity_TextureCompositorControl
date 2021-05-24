using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]

public class TextureCompositorControlClip : PlayableAsset, ITimelineClipAsset
{
    [SerializeField] TextureCompositorControlBehaviour template = new TextureCompositorControlBehaviour ();
    [SerializeField] ExposedReference<Camera> camera;
    private TextureCompositorControlBehaviour clone;
    // public ExposedReference<Camera> camera02;
    // public ExposedReference<ReflectionProbe> reflectionProbe;    
    // public AnimationCurve curve;

    // public bool wiggle = false;
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        // Debug.Log(owner.name);
        
        var playable = ScriptPlayable<TextureCompositorControlBehaviour>.Create (graph, template);
        
        clone = playable.GetBehaviour ();
        clone.camera= camera.Resolve (graph.GetResolver ());
        
        // clone.name = name;
        // Debug.Log(playable.di)
        // clone.camera02 = camera02.Resolve(graph.GetResolver());
        // clone.curve = curve;
        // clone.reflectionProbe = reflectionProbe.Resolve(graph.GetResolver());
        return playable;
        
    }
    
    
    private void OnDestroy()
    {
        
    }
}
