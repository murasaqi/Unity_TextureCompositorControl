using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TextureCompositorControlBehaviour : PlayableBehaviour
{
    [SerializeField] public Camera camera;
    // public Camera camera01;
    // public string name;
    // public ReflectionProbe reflectionProbe;
    // public AnimationCurve curve;
    public bool wiggle;
    public override void OnPlayableCreate (Playable playable)
    {
       
    }
}
