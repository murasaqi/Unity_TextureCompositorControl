using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]

public class TextureCompositorControlClip : PlayableAsset, ITimelineClipAsset
{
    public TextureCompositorControlBehaviour template = new TextureCompositorControlBehaviour ();
    public ExposedReference<Camera> camera;

    private TextureCompositorControlBehaviour clone;
    // public ExposedReference<Camera> camera02;
    // public ExposedReference<ReflectionProbe> reflectionProbe;    
    // public AnimationCurve curve;
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TextureCompositorControlBehaviour>.Create (graph, template);
        clone = playable.GetBehaviour ();
        clone.camera= camera.Resolve (graph.GetResolver ());
        
        // clone.camera02 = camera02.Resolve(graph.GetResolver());
        // clone.curve = curve;
        // clone.reflectionProbe = reflectionProbe.Resolve(graph.GetResolver());
        return playable;
    }

    private void OnDestroy()
    {
        if (clone != null)
        {
            if (clone.camera != null)
            {
                clone.camera.targetTexture = null;
            }
        }
    }
}
