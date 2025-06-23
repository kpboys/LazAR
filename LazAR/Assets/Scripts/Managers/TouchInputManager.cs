using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Events;

/// <summary>
/// TouchInputManager, manages InputSystem.EnhancedTouch touch events,
/// as well as initializing input / touch settings.
/// Use to handle continous sampling of touches. Button touches are done by Unity.
/// </summary>
public class TouchInputManager : Singleton<TouchInputManager>
{
    [SerializeField, Tooltip("The minimum distance the finger needs to have moved across the screen from start to end, for it to count as a swipe in stead of a press.")]
    float swipeMinDistance = 50;
    /// <summary>
    /// SwipeMinDistance        (Write sumary later)
    /// </summary>
    public float SwipeMinDistance { get => swipeMinDistance; }

    [SerializeField, Header("Primary Events"), Space(10)]
    UnityEvent onPress;
    /// <summary>
    /// OnPress     (Write sumary later)
    /// </summary>
    public UnityEvent OnPress { get => onPress; }

    [SerializeField]
    UnityEvent onSwipe;
    /// <summary>
    /// OnSwipe     (Write sumary later)
    /// </summary>
    public UnityEvent OnSwipe { get => onSwipe; }

    [SerializeField, Header("Secondary Events"), Space(10)]
    UnityEvent onTouchBegan;
    /// <summary>
    /// OnTouchBegan        (Write sumary later)
    /// </summary>
    public UnityEvent OnTouchBegan { get => onTouchBegan; }

    [SerializeField]
    UnityEvent onTouchMoved;
    /// <summary>
    /// OnTouchMoved        (Write sumary later)
    /// </summary>
    public UnityEvent OnTouchMoved { get => onTouchMoved; }

    [SerializeField]
    UnityEvent onTouchEnded;
    /// <summary>
    /// OnTouchEnded        (Write sumary later)
    /// </summary>
    public UnityEvent OnTouchEnded { get => onTouchEnded; }

    [SerializeField]
    UnityEvent onTouchCanceled;
    /// <summary>
    /// OnTouchCanceled     (Write sumary later)
    /// </summary>
    public UnityEvent OnTouchCanceled { get => onTouchCanceled; }

    [SerializeField]
    UnityEvent onTouchStationary;
    /// <summary>
    /// OnTouchStationary       (Write sumary later)
    /// </summary>
    public UnityEvent OnTouchStationary { get => onTouchStationary; }

    [HideInInspector]
    private ReadOnlyArray<UnityEngine.InputSystem.EnhancedTouch.Touch> currentTouches;
    /// <summary>
    /// CurrentTouches      (Write sumary later)
    /// </summary>
    public static ReadOnlyArray<UnityEngine.InputSystem.EnhancedTouch.Touch> CurrentTouches
    {
        get => Instance.currentTouches;
        private set => Instance.currentTouches = value;
    }

    [HideInInspector]
    /// <summary>
    /// PrimaryTouch        (Write sumary later)
    /// </summary>
    public static UnityEngine.InputSystem.EnhancedTouch.Touch PrimaryTouch
    {
        get => Instance.currentTouches[0];
    }

    [HideInInspector]
    /// <summary>
    /// PrimaryTouchPosition        (Write sumary later)
    /// </summary>
    public static Vector2 PrimaryTouchPosition
    {
        get => Instance.currentTouches[0].screenPosition;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
    }

    private void Update()
    {
        ReadOnlyArray<UnityEngine.InputSystem.EnhancedTouch.Touch> touches =
            UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches;

        MyDebug.Watch("touches.Count", touches.Count);

        if (touches.Count > 0)
        {
            UnityEngine.InputSystem.EnhancedTouch.Touch touch = touches[0];
            CurrentTouches = touches;

            switch (touch.phase)
            {
                case UnityEngine.InputSystem.TouchPhase.None:
                    break;



                case UnityEngine.InputSystem.TouchPhase.Began:
                    OnTouchBegan.Invoke();
                    break;



                case UnityEngine.InputSystem.TouchPhase.Moved:
                    OnTouchMoved.Invoke();
                    break;



                case UnityEngine.InputSystem.TouchPhase.Ended:
                    OnTouchEnded.Invoke();
                    if (Vector2.Distance(touch.screenPosition, touch.startScreenPosition) < swipeMinDistance)
                    {
                        OnPress.Invoke();
                    }
                    else
                    {
                        OnSwipe.Invoke();
                    }
                    break;



                case UnityEngine.InputSystem.TouchPhase.Canceled:
                    OnTouchCanceled.Invoke();
                    break;



                case UnityEngine.InputSystem.TouchPhase.Stationary:
                    OnTouchStationary.Invoke();
                    break;



                default:
                    break;
            }
        }
    }
}
