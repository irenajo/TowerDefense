using UnityEngine;

public class Timer
{
    private float _time;
    private float _cooldown;

    public float Read(){
        return _time;
    }

    public Timer(float cooldown)
    {
        _cooldown = cooldown;
        _time = 0;
    }

    public void setCooldownAndRestart(float newCooldown){
        _cooldown = newCooldown;
        _time = 0;
    }

    public void Update()
    {
        _time += Time.deltaTime;
    }

    public bool Ready()
    {
        if (_time >= _cooldown)
        {
            _time = 0;
            return true;
        }
        return false;
    }
}