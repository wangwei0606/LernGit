// The MIT License (MIT)

// Copyright 2015 Siney/Pangweiwei siney@yeah.net
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SLua
{
    using System.Collections.Generic;
    using System;

    public class CustomExport
    {
        public static void OnGetAssemblyToGenerateExtensionMethod(out List<string> list) {
            list = new List<string> {
                "Assembly-CSharp",
            };
        }

        public static void OnAddCustomClass(LuaCodeGen.ExportGenericDelegate add)
        {
			// below lines only used for demostrate how to add custom class to export, can be delete on your app

            add(typeof(System.Func<int>), null);
            add(typeof(System.Action<int, string>), null);
            add(typeof(System.Action<int, Dictionary<int, object>>), null);
            add(typeof(List<int>), "ListInt");
            // .net 4.6 export class not match used class on runtime, so skip it
            //add(typeof(Dictionary<int, string>), "DictIntStr");
            add(typeof(string), "String");
            
            // add your custom class here
            // add( type, typename)
            // type is what you want to export
            // typename used for simplify generic type name or rename, like List<int> named to "ListInt", if not a generic type keep typename as null or rename as new type name
            add(typeof(AppCoreExtend), "AppCoreExtend");
            add(typeof(AudioCSToLua), "AudioCSToLua");
            add(typeof(UICSToLua), "UICSToLua");
            add(typeof(LuaCallMgr), "LuaCallMgr");
            add(typeof(UICanvasScaler), "UICanvasScaler");
            add(typeof(UICanvas), "UICanvas");
            add(typeof(EventListener), "EventListener");
            add(typeof(WText), "WText");
            add(typeof(LoopScrollRect), "LoopScrollRect");
            add(typeof(LoopHScrollRect), "LoopHScrollRect");
            add(typeof(LoopVScrollRect), "LoopVScrollRect");
            add(typeof(WImage), "WImage");

            add(typeof(Dropdownscript), "Dropdownscript");
            add(typeof(WBoxCollider2D), "WBoxCollider2D");
            add(typeof(WButton), "WButton");
            add(typeof(WInputText), "WInputText");
            add(typeof(WSlider), "WSlider");
            add(typeof(WRangeSlider), "WRangeSlider");
            add(typeof(WToggle), "WToggle");
            add(typeof(GuideManagers), "GuideManagers");
            add(typeof(WWLineChart), "WWLineChart");

        }

        public static void OnAddCustomAssembly(ref List<string> list)
        {
            // add your custom assembly here
            // you can build a dll for 3rd library like ngui titled assembly name "NGUI", put it in Assets folder
            // add its name into list, slua will generate all exported interface automatically for you

            //list.Add("NGUI");
        }

        public static HashSet<string> OnAddCustomNamespace()
        {
            return new HashSet<string>
            {
                //"NLuaTest.Mock"
            };
        }

        // if uselist return a white list, don't check noUseList(black list) again
        public static void OnGetUseList(out List<string> list)
        {
            list = new List<string>
            {
                // "UnityEngine.Font",
            };
        }

        public static List<string> FunctionFilterList = new List<string>()
        {
            "UIWidget.showHandles",
            "UIWidget.showHandlesWithMoveTool",
            "UnityEngine.QualitySettings.get_streamingMipmapsRenderersPerFrame",
            "UnityEngine.QualitySettings.set_streamingMipmapsRenderersPerFrame",
            "UnityEngine.QualitySettings.streamingMipmapsRenderersPerFrame",
            "UnityEngine.Texture.get_imageContentsHash",
            "UnityEngine.Texture.set_imageContentsHash",
            "UnityEngine.Texture.imageContentsHash",
        };
        // black list if white list not given
        public static void OnGetNoUseList(out List<string> list)
        {
            list = new List<string>
            {
                "VRDevice","VR","Windows","SceneManagement","JetBrains","Analytics","Rendering","Profiling","Experimental","Assertions",
                "Audio","CharacterController","Collections","ContactFilter2D","CullingGroup","Debug","DynamicGI","Effector","UIBehaviour","Hash","Human","JsonUtility","LineAlignment","Matrix","Motion","OperatingSystemFamily",
                "Physics","PropertyName","Random","RangeInt","ReflectionProbe","RemoteSettings","RenderTargetSetup","RuntimeInitializeLoadType","ShadowQuality","SortingLayer","StateMachineBehaviour","StaticBatchingUtility","StereoTargetEyeMask",
                "SurfaceEffector2D","AspectRatioFitter","Clipp","ContentSizeFitter","DefaultControls","Layout","Navigation","Scrollbar","ScrollRect","Selectable","Shadow","Video","WindZone","BatteryStatus","BoundingSphere","Bounds","CapsuleDirection2D",
                "CustomYieldInstruction","DefaultExecutionOrder","Diagnostics","ExposedPropertyResolver","ImageEffectAllowedInSceneView","Mathf","PreferBinarySerialization","QueryTriggerInteraction","RectTransformUtility","ColorBlock","Dropdown","PositionAsUV1",
                "RawImage","WaitUntil","WaitWhile","VertexHelper",
                "*.OnRebuildResquested",
                "Application.ExternalEval",
                "Resources.LoadAssetAtPath",
                "Input.IsJoystickPreconfigured",
                "NetworkView",
            "MonoBehaviour",
            "LightmappingMode",
            "BillboardAsset",
            "NativeLeakDetection",
            "HideInInspector",
            "ExecuteInEditMode",
            "AddComponentMenu",
            "ContextMenu",
            "RequireComponent",
            "DisallowMultipleComponent",
            "SerializeField",
            "AssemblyIsEditorAssembly",
            "Attribute",
            "Types",
            "UnitySurrogateSelector",
            "TrackedReference",
            "TypeInferenceRules",
            "FFTWindow",
            "RPC",
            "Network",
            "MasterServer",
            "BitStream",
            "HostData",
            "ConnectionTesterStatus",
            "GUI",
            "EventType",
            "EventModifiers",
            "FontStyle",
            "TextAlignment",
            "TextEditor",
            "TextEditorDblClickSnapping",
            "TextGenerator",
            "TextClipping",
            "Gizmos",
            "ADBannerView",
            "ADInterstitialAd",
            "Android",
            "Tizen",
            "jvalue",
            "iPhone",
            "iOS",
            "CalendarIdentifier",
            "CalendarUnit",
            "CalendarUnit",
            "ClusterInput",
            "FullScreenMovieControlMode",
            "FullScreenMovieScalingMode",
            "Handheld",
            "LocalNotification",
            "NotificationServices",
            "RemoteNotificationType",
            "RemoteNotification",
            "SamsungTV",
            "TextureCompressionQuality",
            "TouchScreenKeyboardType",
            "TouchScreenKeyboard",
            "MovieTexture",
            "UnityEngineInternal",
            "Terrain",
            "Tree",
            "SplatPrototype",
            "DetailPrototype",
            "DetailRenderMode",
            "MeshSubsetCombineUtility",
            "AOT",
            "Social",
            "Enumerator",
            "SendMouseEvents",
            "Cursor",
            "Flash",
            "ActionScript",
            "OnRequestRebuild",
            "Ping",
            "ShaderVariantCollection",
            "SimpleJson.Reflection",
            "CoroutineTween",
            "GraphicRebuildTracker",
            "Advertisements",
            "UnityEditor",
            "WSA",
            "EventProvider",
            "Apple",
            "ClusterInput",

            "FilterMode",
            "TextureWrapMode",
            "NPOTSupport",
            "TextureFormat",
            "CubemapFace",
            "RenderTextureFormat",
            "RenderTextureReadWrite",
            "BlendMode",
            "BlendOp",
            "CompareFunction",
            "CullMode",
            "ColorWriteMask",
            "StencilOp",
            "Security",
            "StackTraceUtility",
            "UnityException",
            "MissingComponentException",
            "UnassignedReferenceException",
            "MissingReferenceException",
            "TextGenerationSettings",
            "PersistentListenerMode",
            "UnityEventCallState",
            "UnityEventBase",
            "UnityEvent",
            "WWW",
            "AsyncOperation",
            "AssetBundleCreateRequest",
            "AssetBundleRequest",
            //"Object",
            "AssetBundle",
            "HideFlags",
            "SendMessageOptions",
            "PrimitiveType",
            "Space",
            //"LayerMask",
            "RuntimePlatform",
            "SystemLanguage",
            "LogType",
            "DeviceType",
            "SystemInfo",
            "WaitForSeconds",
            "WaitForFixedUpdate",
            "WaitForEndOfFrame",
            "ScriptableObject",
            "ResourceRequest",
            "Resources",
            "ThreadPriority",
            "Profiler",
            "CrashReport",
            "LightType",
            "LightRenderMode",
            "LightShadows",
            //"Component",
            "OcclusionArea",
            "OcclusionPortal",
            "FogMode",
            "RenderSettings",
            "ShadowProjection",
            "QualitySettings",
            "CameraClearFlags",
            "DepthTextureMode",
            "TexGenMode",
            "AnisotropicFiltering",
            "BlendWeights",
            "MeshFilter",
            "CombineInstance",
            "MeshTopology",
            "Mesh",
            "BoneWeight",
            "SkinQuality",
            "Renderer",
            "SkinnedMeshRenderer",
            "Flare",
            //"Behaviour",
            "LensFlare",
            "Projector",
            "Skybox",
            "TextMesh",
            "Particle",
            "ParticleEmitter",
            "ParticleAnimator",
            "TrailRenderer",
            "ParticleRenderMode",
            "ParticleRenderer",
            "LineRenderer",
            "MaterialPropertyBlock",
            "RenderBuffer",
            "Graphics",
            "Resolution",
            "LightmapData",
            "LightmapsMode",
            "ColorSpace",
            "LightProbes",
            "LightmapSettings",
            "GeometryUtility",
            "ScreenOrientation",
            //"Screen",
            "SleepTimeout",
            "GL",
            "MeshRenderer",
            //"StaticBatchingUtility",
            "ImageEffectTransformsToLDR",
            "ImageEffectOpaque",
            "Texture",
            "Texture2D",
            "Cubemap",
            "Texture3D",
            "SparseTexture",
            "RenderTexture",
            "TextAnchor",
            "HorizontalWrapMode",
            "VerticalWrapMode",
            "CharacterInfo",
            "Font",
            "UICharInfo",
            "UILineInfo",
            "LOD",
            "LODGroup",
            "GradientColorKey",
            "GradientAlphaKey",
            "Gradient",
            "ScaleMode",
            "FocusType",
            "RectOffset",
            "ImagePosition",
            // "Event",
            "KeyCode",
            "LightProbeGroup",
            // "Vector2",
            // "Vector3",
            // "Color",
            // "Color32",
            // "Quaternion",
            // "Rect",
            // "Matrix4x4",
            // "Bounds",
            // "Vector4",
            //"Ray",
            //"Ray2D",
            "Plane",
            //"Mathf",


            "ParticleSystemRenderMode",
            "ParticleSystemSimulationSpace",
            "ParticleSystem",
            "ParticleSystem_Particle",
            "ParticleSystem_CollisionEvent",
            "ParticleSystemRenderer",
            "TextAsset",
            "Shader",
            "Material",
            "ProceduralProcessorUsage",
            "ProceduralCacheSize",
            "ProceduralLoadingBehavior",
            "ProceduralPropertyType",
            "ProceduralOutputType",
            "ProceduralPropertyDescription",
            "ProceduralMaterial",
            "ProceduralTexture",
            "SpriteAlignment",
            "SpritePackingMode",
            "SpritePackingRotation",
            "SpriteMeshType",
            "Sprite",
            "SpriteRenderer",
            "Sprites_DataUtility",
            "WWWForm",
            "Caching",
            "Application",
            "UserAuthorization",
            "RenderingPath",
            "TransparencySortMode",
            //"Camera",
            "ComputeShader",
            "ComputeBufferType",
            "ComputeBuffer",
            //"Debug",
            "Display",
            //"MonoBehaviour",
            "TouchPhase",
            "IMECompositionMode",
            "Touch",
            "DeviceOrientation",
            "AccelerationEvent",
            "Gyroscope",
            "LocationInfo",
            "LocationServiceStatus",
            "LocationService",
            "Compass",
            "Input",
            "Light",
            //"GameObject",
            //"Transform",
            //"Time",
            //"Random",
            "PlayerPrefsException",
            //"PlayerPrefs",
            //"Motion",
            "ForceMode",
            //"Physics",




            "RigidbodyConstraints",
            "Rigidbody",
            "RigidbodyInterpolation",
            "JointMotor",
            "JointSpring",
            "JointLimits",
            "Joint",
            "HingeJoint",
            "SpringJoint",
            "FixedJoint",
            "SoftJointLimit",
            "JointDriveMode",
            "JointProjectionMode",
            "JointDrive",
            "CharacterJoint",
            "ConfigurableJointMotion",
            "RotationDriveMode",
            "ConfigurableJoint",
            "ConstantForce",
            "CollisionDetectionMode",
            "Collider",
            "BoxCollider",
            "SphereCollider",
            //"MeshCollider",
            "CapsuleCollider",
            "WheelFrictionCurve",
            "WheelHit",
            "WheelCollider",
            //"RaycastHit",
            "PhysicMaterialCombine",
            "PhysicMaterial",
            "ContactPoint",
            "Collision",
            "CollisionFlags",
            "ControllerColliderHit",
            //"CharacterController",
            "Cloth",
            "InteractiveCloth",
            "ClothSkinningCoefficient",
            "SkinnedCloth",
            "ClothRenderer",
            "Physics2D",
            "RaycastHit2D",
            "RigidbodyInterpolation2D",
            "RigidbodySleepMode2D",
            "CollisionDetectionMode2D",
            "ForceMode2D",
            "Rigidbody2D",
            "Collider2D",
            "CircleCollider2D",
            "BoxCollider2D",
            "EdgeCollider2D",
            "PolygonCollider2D",
            "ContactPoint2D",
            "Collision2D",
            "JointLimitState2D",
            "JointAngleLimits2D",
            "JointTranslationLimits2D",
            "JointMotor2D",
            "JointSuspension2D",
            "Joint2D",
            "AnchoredJoint2D",
            "SpringJoint2D",
            "DistanceJoint2D",
            "HingeJoint2D",
            "SliderJoint2D",
            "WheelJoint2D",
            "PhysicsMaterial2D",
            "ObstacleAvoidanceType",
            "NavMeshAgent",
            "OffMeshLinkType",
            "OffMeshLinkData",
            "NavMeshHit",
            "NavMeshTriangulation",
            "NavMesh",
            "OffMeshLink",
            "NavMeshPathStatus",
            "NavMeshPath",
            "NavMeshObstacle",
            "AudioSpeakerMode",
            "AudioSettings",
            "AudioType",
            "AudioClip",
            "AudioVelocityUpdateMode",
            "AudioListener",
            "AudioRolloffMode",
            "AudioSource",
            "AudioReverbPreset",
            "AudioReverbZone",
            "AudioLowPassFilter",
            "AudioHighPassFilter",
            "AudioDistortionFilter",
            "AudioEchoFilter",
            "AudioChorusFilter",
            "AudioReverbFilter",
            "Microphone",
            "WebCamFlags",
            "WebCamDevice",
            "WebCamTexture",
            "AnimationClipPair",
            "RuntimeAnimatorController",
            "AnimatorOverrideController",
            "WrapMode",
            "AnimationEvent",
            "AnimationClip",
            "Keyframe",
            "AnimationCurve",
            "PlayMode",
            "QueueMode",
            "AnimationBlendMode",
            "AnimationPlayMode",
            "AnimationCullingType",
            "Animation",
            "AnimationState",
            "AvatarTarget",
            "AvatarIKGoal",
            "AnimationInfo",
            "AnimatorCullingMode",
            "AnimatorUpdateMode",
            "AnimatorStateInfo",
            "MatchTargetWeightMask",
            "Animator",
            "AnimatorUtility",
            "SkeletonBone",
            "HumanLimit",
            "HumanBone",
            "HumanDescription",
            "AvatarBuilder",
            "HumanBodyBones",
            "Avatar",
            "HumanTrait",
            "DrivenTransformProperties",
            "DrivenRectTransformTracker",
            //"RectTransform",
            "Edge",
            "Axis",
            //"RectTransformUtility",
            "RenderMode",
            "Canvas",
            "CanvasGroup",
            "UIVertex",
            "CanvasRenderer",
            "ReadCommand",
            "PlayableGraph",
            "Device",
            "Remote",
            };
        }
    }
}
