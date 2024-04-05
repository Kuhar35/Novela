using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] public int _health;
    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onDie;
    [SerializeField] private HealthChangeEvent _onChange;
    [SerializeField] public int _PrecentageChangeDamage=0;
    // Start is called before the first frame update
    public int damage;

    public void ApplyDamage(int damageValue,int _PrecentageChangeDamage)

    {
        this._PrecentageChangeDamage = _PrecentageChangeDamage;
      System.Random random = new System.Random();
        if (_health <= 0) return;
        if (damageValue < 0)
        {
            damage = damageValue + random.Next(damageValue * _PrecentageChangeDamage / 100, 0);
            _health += damage;


            _onChange?.Invoke(_health);
        }
        else if (_health > 0 && _health < damageValue)
        {
            _health += damageValue;
            _health = _health >= damageValue ? damageValue : _health;
        }


       // Debug.Log("Health " + _health);

        if (damageValue <= -1) { _onDamage?.Invoke(); }//else if(damageValue>0)    
        if (_health <= 0)
        {
            _onDie?.Invoke();
        }
    }


    internal void SetHealth(int health)
    {
        _health = health;
    }


    [Serializable]
    public class HealthChangeEvent : UnityEvent<int>
    {

    }
}
