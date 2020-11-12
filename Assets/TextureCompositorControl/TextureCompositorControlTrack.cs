using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.8700427f, 0.4803044f, 0.790566f)]
[TrackClipType(typeof(TextureCompositorControlClip))]
[TrackBindingType(typeof(TextureCompositorManager))]

public class TextureCompositorControlTrack : TrackAsset
{
    public bool performanceMode = false;
    public RenderTexture referenceRenderTextureSetting;

    // private List<RenderTexture> m_texturePool;

    public List<RenderTexture> texturePool;

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var playableDirector = go.GetComponent<PlayableDirector>();
        var playable = ScriptPlayable<TextureCompositorControlMixerBehaviour>.Create (graph, inputCount);
        var playableBehaviour = playable.GetBehaviour();

       

       InitTexturePools();
       
        
        if (playableDirector != null)
        {
            playableBehaviour.director = playableDirector;
            playableBehaviour.clips = GetClips();
            playableBehaviour.track = this;
        }

        return playable;
        // return ScriptPlayable<TextureCompositorControlMixerBehaviour>.Create (graph, inputCount);
    }

    public void InitTexturePools()
    {
       KillRenderTexturePool();

        if(texturePool == null) texturePool = new List<RenderTexture>();
        if (referenceRenderTextureSetting != null)
        {
            while(texturePool.Count()< 2)
            {
                texturePool.Add(new RenderTexture(referenceRenderTextureSetting));
            }

            int i = 0;
            foreach (var t in texturePool)
            {
                t.name = i.ToString();
                i++;
            }
        }
        else
        {
            Debug.LogWarning("referenceRenderTextureSetting is null.");
        }
    }

    public void KillRenderTexturePool()
    {
        // if (m_texturePool != null)
        // {
        //     while (m_texturePool.Count() > 0)
        //     {
        //         var last = m_texturePool.Last();
        //         m_texturePool.Remove(last);
        //         DestroyImmediate(last);
        //         
        //     }
        //     
        //     m_texturePool.Clear();
        // }
    }

    private void OnDestroy()
    {
        KillRenderTexturePool();
    }
}
