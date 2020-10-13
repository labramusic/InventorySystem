using UnityEngine;

public class BuffTimer : MonoBehaviour
{
    public float Duration
    {
        set
        {
            if (!_running)
            {
                _totalSeconds = value;
            }
        }
    }

    public float EventTickSeconds;

    public TimedAttributeModifier TimedModifier;

    public bool Finished => _started && !_running;
    public bool Running => _running;
    public float TimeRemaining => _totalSeconds - _elapsedSeconds;

    public delegate void OnTimerTick(TimedAttributeModifier modifier);
    public event OnTimerTick TimerTick;

    public delegate void OnTimerFinished(TimedAttributeModifier modifier, BuffTimer timer);
    public event OnTimerFinished TimerFinished;

    private float _totalSeconds;
    private float _elapsedSeconds;
    private float _eventTickAcc;
    private bool _running;
    private bool _started;

    void Update()
    {
        if (_running)
        {
            _elapsedSeconds += Time.deltaTime;

            if (EventTickSeconds > 0f)
            {
                _eventTickAcc += Time.deltaTime;
                if (_eventTickAcc >= EventTickSeconds)
                {
                    _eventTickAcc -= EventTickSeconds;
                    TimerTick?.Invoke(TimedModifier);
                }
            }

            if (_elapsedSeconds >= _totalSeconds)
            {
                _running = false;
                TimerFinished?.Invoke(TimedModifier, this);
            }
        }
    }

    public void Run()
    {
        if (_totalSeconds > 0)
        {
            _started = true;
            _running = true;
            _elapsedSeconds = 0;
            _eventTickAcc = 0;
        }
    }
}
