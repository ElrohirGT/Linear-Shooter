using UnityEngine;
using UnityEngine.UI;
using Utilities.Constants;
using Utilities;

public class SliderKeyObserver : MonoBehaviour
{
    //Delay between changes support.
    bool _canChange = false;
    Timer _cooldownResponseTimer;
    float _cooldownResponseDuration = 0.2f;

    public Button RightButton { get; set; }
    public Button LeftButton { get; set; }

    void Start()
    {
        _cooldownResponseTimer = gameObject.AddComponent<Timer>();
        _cooldownResponseTimer.Finished += HandleCooldownResponseTimerFinished;
        _canChange = true;
    }

    void Update()
    {
        if (Input.GetAxis(InputAxisConstants.HORIZONTAL) < 0 && _canChange)
            ClickButton(LeftButton);
        if (Input.GetAxis(InputAxisConstants.HORIZONTAL) > 0 && _canChange)
            ClickButton(RightButton);
    }

    void ClickButton(Button buttonToClick)
    {
        buttonToClick?.onClick.Invoke();
        _cooldownResponseTimer.StartTimer(_cooldownResponseDuration);
        _canChange = false;
    }

    void HandleCooldownResponseTimerFinished() => _canChange = true;
}
