using UnityEngine;

public class Timer
{
    private float _time;
    private float _cooldown;

    private bool _active;

    public Timer(float cooldown)
    {
        _cooldown = cooldown;
        _time = 0;
    }

    public void Start()
    {
        _active = true;
    }

    public void Stop()
    {
        _active = false;
    }

    public void Update()
    {
        if (!_active) return;
        _time += Time.deltaTime;
    }

    public bool Ready()
    {
        if (!_active) return false;
        if (_time >= _cooldown)
        {
            _time = 0;
            return true;
        }
        return false;
    }
}