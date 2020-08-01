using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Popcron
{
    [ExecuteInEditMode]
    [AddComponentMenu("")]
    public class GizmosInstance : MonoBehaviour
    {
        private const int _DEFAULT_QUEUE_SIZE = 512;

        private static GizmosInstance _instance;
        private static bool _hotReloaded = true;
        private static Material _defaultMaterial;
        internal static Camera CurrentRenderingCamera;

        private Material _overrideMaterial;
        private int _queueIndex = 0;
        private Element[] _queue = new Element[_DEFAULT_QUEUE_SIZE];

        /// <summary>
        /// The material being used to render
        /// </summary>
        public static Material Material
        {
            get
            {
                GizmosInstance __inst = GetOrCreate();
                if (__inst._overrideMaterial)
                {
                    return __inst._overrideMaterial;
                }

                return DefaultMaterial;
            }
            set
            {
                GizmosInstance __inst = GetOrCreate();
                __inst._overrideMaterial = value;
            }
        }

        /// <summary>
        /// The default line renderer material
        /// </summary>
        public static Material DefaultMaterial
        {
            get
            {
                if (!_defaultMaterial)
                {
                    // Unity has a built-in shader that is useful for drawing
                    // simple colored things.
                    Shader __shader = Shader.Find("Hidden/Internal-Colored");
                    _defaultMaterial = new Material(__shader)
                    {
                        hideFlags = HideFlags.HideAndDontSave
                    };

                    // Turn on alpha blending
                    _defaultMaterial.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
                    _defaultMaterial.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
                    _defaultMaterial.SetInt("_Cull", (int)CullMode.Off);
                    _defaultMaterial.SetInt("_ZWrite", 0);
                }

                return _defaultMaterial;
            }
        }

        internal static GizmosInstance GetOrCreate()
        {
            if (!_hotReloaded && _instance) return _instance;
            
            bool __markDirty = false;
            GizmosInstance[] __gizmosInstances = FindObjectsOfType<GizmosInstance>();
            for (int __i = 0; __i < __gizmosInstances.Length; __i++)
            {
                _instance = __gizmosInstances[__i];

                //destroy any extra gizmo instances
                if (__i <= 0) continue;
                
                if (Application.isPlaying)
                {
                    Destroy(__gizmosInstances[__i]);
                }
                else
                {
                    DestroyImmediate(__gizmosInstances[__i]);
                    __markDirty = true;
                }
            }

            //none were found, create a new one
            if (!_instance)
            {
                _instance = new GameObject("Popcron.Gizmos.GizmosInstance").AddComponent<GizmosInstance>();
                _instance.gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;

                __markDirty = true;
            }

            #if UNITY_EDITOR
            //mark scene as dirty
            if (__markDirty && !Application.isPlaying)
            {
                Scene __scene = SceneManager.GetActiveScene();
                EditorSceneManager.MarkSceneDirty(__scene);
            }
            #endif

            _hotReloaded = false;

            return _instance;
        }

        internal static void Add(Vector3[] points, Color? color, bool dashed)
        {
            GizmosInstance __inst = GetOrCreate();

            //excedeed the length, so loopback
            if (__inst._queueIndex >= _DEFAULT_QUEUE_SIZE)
            {
                __inst._queueIndex = 0;
            }

            __inst._queue[__inst._queueIndex].active = true;
            __inst._queue[__inst._queueIndex].color = color ?? Color.white;
            __inst._queue[__inst._queueIndex].points = points;
            __inst._queue[__inst._queueIndex].dashed = dashed;

            __inst._queueIndex++;
        }

        private void OnEnable()
        {
            //populate queue with empty elements
            _queue = new Element[_DEFAULT_QUEUE_SIZE];
            for (int __i = 0; __i < _DEFAULT_QUEUE_SIZE; __i++)
            {
                _queue[__i] = new Element();
            }

            if (GraphicsSettings.renderPipelineAsset == null)
            {
                Camera.onPostRender += OnRendered;
            }
            else
            {
                RenderPipelineManager.endCameraRendering += OnRendered;
            }
        }

        private void OnDisable()
        {
            if (GraphicsSettings.renderPipelineAsset == null)
            {
                Camera.onPostRender -= OnRendered;
            }
            else
            {
                RenderPipelineManager.endCameraRendering -= OnRendered;
            }
        }

        private void OnRendered(ScriptableRenderContext context, Camera camera)
        {
            OnRendered(camera);
        }

        private void OnRendered(Camera camera)
        {
            //dont render if this camera isnt the main camera
            bool __allow = false;
            if (camera.name == "SceneCamera")
            {
                __allow = true;
            }
            else if (camera == Gizmos.Camera)
            {
                __allow = true;
            }

            if (!__allow) return;

            CurrentRenderingCamera = camera;
            Vector3 __offset = Gizmos.Offset;
            Material.SetPass(0);

            GL.PushMatrix();
            GL.Begin(GL.LINES);

            //draw elements
            float __time = 0f;
            if (Application.isPlaying)
            {
                __time = Time.time;
            }
            else
            {
#if UNITY_EDITOR
                __time = (float)UnityEditor.EditorApplication.timeSinceStartup;
#endif
            }

            bool __alt = __time % 1 > 0.5f;
            float __dashGap = Mathf.Clamp(Gizmos.DashGap, 0.01f, 32f);
            for (int __e = 0; __e < _queue.Length; __e++)
            {
                if (!_queue[__e].active) continue;

                //set to inactive
                _queue[__e].active = false;

                List<Vector3> __points = new List<Vector3>();
                if (_queue[__e].dashed)
                {
                    //subdivide
                    for (int __i = 0; __i < _queue[__e].points.Length - 1; __i++)
                    {
                        Vector3 __pointA = _queue[__e].points[__i];
                        Vector3 __pointB = _queue[__e].points[__i + 1];
                        Vector3 __direction = __pointB - __pointA;
                        float __magnitude = __direction.magnitude;
                        if (__magnitude > __dashGap * 2f)
                        {
                            int __amount = Mathf.RoundToInt(__magnitude / __dashGap);
                            __direction /= __magnitude;

                            for (int __p = 0; __p < __amount - 1; __p++)
                            {
                                if (__p % 2 == (__alt ? 1 : 0))
                                {
                                    float __startLerp = __p / ((float)__amount - 1);
                                    float __endLerp = (__p + 1) / ((float)__amount - 1);
                                    Vector3 __start = Vector3.Lerp(__pointA, __pointB, __startLerp);
                                    Vector3 __end = Vector3.Lerp(__pointA, __pointB, __endLerp);
                                    __points.Add(__start);
                                    __points.Add(__end);
                                }
                            }
                        }
                        else
                        {
                            __points.Add(__pointA);
                            __points.Add(__pointB);
                        }
                    }
                }
                else
                {
                    __points.AddRange(_queue[__e].points);
                }

                GL.Color(_queue[__e].color);
                for (int __i = 0; __i < __points.Count; __i++)
                {
                    GL.Vertex(__points[__i] + __offset);
                }
            }

            GL.End();
            GL.PopMatrix();
        }
    }
}
