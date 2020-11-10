
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Timeline;
public class TextureCompositorControlMixerBehaviour : PlayableBehaviour
{
    
    private IEnumerable<TimelineClip> m_Clips;
    private PlayableDirector m_Director;

    // private TimelineClip m_preClip = null;
    
    internal PlayableDirector director
    {
        get { return m_Director; }
        set { m_Director = value; }
    }

    internal IEnumerable<TimelineClip> clips
    {
        get { return m_Clips; }
        set { m_Clips = value; }
    }

    private List<RenderTexture> m_texturePool =new List<RenderTexture>();
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        // var currentClipIndex = 0;
        TextureCompositorManager trackBinding = playerData as TextureCompositorManager;

        if (!trackBinding)
            return;

        int inputCount = playable.GetInputCount ();
        
        int inputPort = 0;

        
        
        if (m_Clips.Last().start+m_Clips.Last().duration < m_Director.time)
        {
            for(int i = 0; i < m_Clips.Count(); i++)
            {
                var scriptPlayable =  (ScriptPlayable<TextureCompositorControlBehaviour>)playable.GetInput(i);
                var playableBehaviour = scriptPlayable.GetBehaviour();
                playableBehaviour.camera.gameObject.SetActive(false);

            }
            return;

        }
        
        
        m_texturePool.Clear();
        foreach (TimelineClip clip in m_Clips)
        {
            var inputWeight = playable.GetInputWeight(inputPort);
           
            var scriptPlayable =  (ScriptPlayable<TextureCompositorControlBehaviour>)playable.GetInput(inputPort);
            var playableBehaviour = scriptPlayable.GetBehaviour();

            if (clip.start <= m_Director.time && m_Director.time < clip.start + clip.duration)
            {
                m_texturePool.Add(playableBehaviour.camera.targetTexture);
            }
            inputPort++;
        
        }
        
        
        
        // foreach (var VARIABLE in m_texturePool)
        // {
        //     Debug.Log(VARIABLE.name);
        // }
        
        if (trackBinding != null && m_texturePool.Count != 0)
        {
            
            trackBinding.SetRenderTexture02(m_texturePool.First());
            trackBinding.SetRenderTexture01(m_texturePool.Last());   
        }
        
     

        
        inputPort = 0;

        
        foreach (TimelineClip clip in m_Clips)
        {
            
            var inputWeight = playable.GetInputWeight(inputPort);
           
            var scriptPlayable =  (ScriptPlayable<TextureCompositorControlBehaviour>)playable.GetInput(inputPort);
            var playableBehaviour = scriptPlayable.GetBehaviour();

            

           
            if (clip.start <= m_Director.time && m_Director.time <= clip.start + clip.duration )
            {
                
                for(int i = 0; i < m_Clips.Count(); i++)
                {
                    var _scriptPlayable =  (ScriptPlayable<TextureCompositorControlBehaviour>)playable.GetInput(i);
                    var _playableBehaviour = _scriptPlayable.GetBehaviour();
                    _playableBehaviour.camera.gameObject.SetActive(false);

                }

                if (inputPort - 1 > 0)
                {
                    var _scriptPlayable =  (ScriptPlayable<TextureCompositorControlBehaviour>)playable.GetInput(inputPort-1);
                    var _playableBehaviour = _scriptPlayable.GetBehaviour();
                    _playableBehaviour.camera.gameObject.SetActive(true);
                }
                
                if (inputPort + 1 < m_Clips.Count())
                {
                    var _scriptPlayable =  (ScriptPlayable<TextureCompositorControlBehaviour>)playable.GetInput(inputPort+1);
                    var _playableBehaviour = _scriptPlayable.GetBehaviour();
                    _playableBehaviour.camera.gameObject.SetActive(true);
                }
                
                
                playableBehaviour.camera.gameObject.SetActive(true);
                var initializedTime = (m_Director.time - clip.start) / clip.duration;
                trackBinding.fader = inputWeight;//Mathf.Clamp(playableBehaviour.curve.Evaluate((float) initializedTime),0,1);
                if (trackBinding.probeController != null)
                {
                    if (trackBinding.fader < 0.5)
                    {
                    
                        if (trackBinding.probeController.cam.name != playableBehaviour.camera.name)
                        {
                            trackBinding.probeController.cam = playableBehaviour.camera;
                        }
                    }
                    else
                    {
                        if (trackBinding.probeController.cam.name != playableBehaviour.camera.name)
                        {
                            trackBinding.probeController.cam = playableBehaviour.camera;
                        }
                    }  
                }


                // currentClipIndex = inputPort;
                break;
                
            }


            // m_preClip = clip;
            inputPort++;
        }

       

        // for (int i = 0; i < inputCount; i++)
        // {
        //     float inputWeight = playable.GetInputWeight(i);
        //     ScriptPlayable<TextureCompositorControlBehaviour> inputPlayable = (ScriptPlayable<TextureCompositorControlBehaviour>)playable.GetInput(i);
        //     TextureCompositorControlBehaviour input = inputPlayable.GetBehaviour ();
        //     
        //     
        //     // Use the above variables to process each frame of this playable.
        //     
        // }
    }

    // private void AllCameraDisable()
    // {
    //     
    // }
}
