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
   
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TextureCompositorControlBehaviour>.Create (graph, template);
        clone = playable.GetBehaviour ();
        clone.camera= camera.Resolve (graph.GetResolver ());
        return playable;
        
    }
    
    
    private void OnDestroy()
    {
        
    }
}
