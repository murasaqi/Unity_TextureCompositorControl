using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class TextureCompositorControlBehaviour : PlayableBehaviour
{
    public bool wiggle;
    [HideInInspector][SerializeField] public Camera camera;
    [SerializeField] public Vector2 noiseSeed;
    [SerializeField] public Vector2 noiseScale;
    [SerializeField] public float roughness;
    [SerializeField] public Vector2 wiggleRange;
    [SerializeField] public Vector2 offsetPosition;
   
    public override void OnPlayableCreate (Playable playable)
    {
       
    }
}
