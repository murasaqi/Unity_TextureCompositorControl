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
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var playableDirector = go.GetComponent<PlayableDirector>();
        var playable = ScriptPlayable<TextureCompositorControlMixerBehaviour>.Create (graph, inputCount);
        var playableBehaviour = playable.GetBehaviour();

        if (playableDirector != null)
        {
            playableBehaviour.director = playableDirector;
            playableBehaviour.clips = GetClips();
            playableBehaviour.track = this;
        }

        return playable;
        // return ScriptPlayable<TextureCompositorControlMixerBehaviour>.Create (graph, inputCount);
    }
}
