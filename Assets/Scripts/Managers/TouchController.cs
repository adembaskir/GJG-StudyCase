using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchController : MonoBehaviour
{
    public static TouchController Instance { get; private set; }
    [HideInInspector]public Touch touch;
    [HideInInspector]public float pressTime;// { get; private set; }
    [HideInInspector]public bool isTouchedRightSide;
    public bool isTouch;
    [Header("Events")]
    public UnityEvent onBeganTouchEvent;
    public UnityEvent<float> onHoldEvent;
    public UnityEvent onEndTouchEvent;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        } 
        Instance = this;
    }
    public bool canMove
    {
        get
        {
            return isTouch && pressTime > Time.deltaTime;
        }
    }
    void Update()
    {
        isTouch = Input.touchCount > 0;
        if (isTouch) // Eger ekrana dokunuluyorsa. (toplamda 5 parmak algılanabiliyor.)
        {

            touch = Input.GetTouch(0); // ilk temas eden parmagi cagiriyoruz.
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    InvokeBeganTouch();
                    break;
                case TouchPhase.Moved:
                    HoldTouch();
                    break;
                case TouchPhase.Stationary:
                    HoldTouch();
                    break;
                case TouchPhase.Ended:
                    InvokeEndTouch();
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }

        }

    }
    /// <summary>
    /// Ekrana basili tutuldugu surece calisir.
    /// </summary>
    private void HoldTouch()
    {
        pressTime += Time.deltaTime; //ekrana basılı tutuldugu surece pressTime artar.
        onHoldEvent?.Invoke(pressTime);
    }
    private void InvokeBeganTouch()
    {
        onBeganTouchEvent?.Invoke(); // Ekrana ilk dokuldugunda tetiklenecek event islemleri.
        //GameManager.Instance?.InvokeStartGame();
        // isTouchedRightSide = firstTouch.x > (Screen.width / 2); // ekranın sag tarafini tikladik mi diye kontrol ediyor.

        pressTime = 0;
    }
    public void InvokeEndTouch()
    {
        onEndTouchEvent?.Invoke();
    }
}
