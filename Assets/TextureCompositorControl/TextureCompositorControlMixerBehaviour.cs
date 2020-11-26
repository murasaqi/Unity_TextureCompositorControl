
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Timeline;
public class TextureCompositorControlMixerBehaviour : PlayableBehaviour
{
    
    private IEnumerable<TimelineClip> m_Clips;
    private PlayableDirector m_Director;

    private TextureCompositorControlTrack m_track;
    // private TimelineClip m_preClip = null;
    
    internal PlayableDirector director
    {
        get { return m_Director; }
        set { m_Director = value; }
    }
    
    internal TextureCompositorControlTrack track
    {
        get { return m_track; }
        set { m_track = value; }
    }

    internal IEnumerable<TimelineClip> clips
    {
        get { return m_Clips; }
        set { m_Clips = value; }
    }

    
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        // var currentClipIndex = 0;
        
        TextureCompositorManager trackBinding = playerData as TextureCompositorManager;

        if (!trackBinding)
            return;
        
        
        
        trackBinding.SetRenderTexture01(track.textureA);
        trackBinding.SetRenderTexture02(track.textureB);
        var updateClipCount = 0;


        int i = 0;
        foreach (TimelineClip clip in m_Clips)
        {
            var scriptPlayable =  (ScriptPlayable<TextureCompositorControlBehaviour>)playable.GetInput(i);
            var playableBehaviour = scriptPlayable.GetBehaviour();
            if(playableBehaviour.camera != null)playableBehaviour.camera.enabled = false;
            i++;
        }

        i = 0;

#if UNITY_EDITOR
        // var missingCameraCount = 0;
        if (m_track.findMissingCameraInHierarchy)
        {
           
            var timelineAsset = director.playableAsset as TimelineAsset;
            var fps = timelineAsset != null ? timelineAsset.editorSettings.fps : 60;
            foreach (var clip in m_Clips)
            {
                var scriptPlayable =  (ScriptPlayable<TextureCompositorControlBehaviour>)playable.GetInput(i);
                var playableBehaviour = scriptPlayable.GetBehaviour();

                var missing = false;
                if (playableBehaviour.camera == null)
                {
                    Debug.Log($"<color=#DA70D6>[frame: {Mathf.CeilToInt((float)clip.start*fps )} clip: {clip.displayName}] Camera reference is null.</color>");
                    missing = true;
                }
                else
                {
                      if (clip.displayName != playableBehaviour.camera.name)
                                    {
                                        Debug.Log($"<color=#9370DB>[frame: {Mathf.CeilToInt((float)clip.start*fps )} clip: {clip.displayName}] Name does not match the camera</color>");
                                        missing = true;
                                    }
                }



                if (missing && m_track.fixMissingPrefabByCameraName)
                {
                    
                    var cam = GameObject.Find(clip.displayName);
                    if (cam!=null)
                    {
                        var serializedObject = new SerializedObject(clip.asset, director);
                        var serializedProperty = serializedObject.FindProperty("camera");
                        // if(cam.GetComponent<Camera>())Debug.Log(cam);
                        Debug.Log(cam.name);
                        // playableBehaviour.camera = cam.GetComponent<Camera>();
                        if(cam.GetComponent<Camera>())serializedProperty.exposedReferenceValue = cam.GetComponent<Camera>();
                        serializedObject.ApplyModifiedProperties();
                        Debug.Log($"<color=#00BFFF>Fix {clip.displayName}</color>");
                    }
                }

                i++;
            }
           
        }

#endif
        int inputPort = 0;
        
        foreach (TimelineClip clip in m_Clips)
        {
            
            var inputWeight = playable.GetInputWeight(inputPort);
           
            var scriptPlayable =  (ScriptPlayable<TextureCompositorControlBehaviour>)playable.GetInput(inputPort);
            var playableBehaviour = scriptPlayable.GetBehaviour();

            
           
            if (clip.start <= m_Director.time && m_Director.time <= clip.start + clip.duration )
            {
                    
                if (playableBehaviour.camera != null)
                {
                    // Debug.Log(playableBehaviour.camera.name);
                    playableBehaviour.camera.enabled = true;
                    playableBehaviour.camera.targetTexture = track.texturePool.First();
                }

                if (inputPort + 1 < m_Clips.Count())
                {
                    var _scriptPlayable =  (ScriptPlayable<TextureCompositorControlBehaviour>)playable.GetInput(inputPort+1);
                    var _playableBehaviour = _scriptPlayable.GetBehaviour();
                    if (_playableBehaviour.camera != null)
                    {
                        _playableBehaviour.camera.enabled = true;
                        _playableBehaviour.camera.targetTexture = track.texturePool.Last();
                    }
                }
                
                
                playableBehaviour.camera.enabled = true;
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
