using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class Skill
{
    public string SkillName;
    public float fCoolDownTime = 0;
    private float lastUsedTime = -Mathf.Infinity;

    public bool IsReady => Time.time >= lastUsedTime + fCoolDownTime;

    public Skill(string _name, float _cooldown)
    {
        if (string.IsNullOrWhiteSpace(_name))
            Debug.Log("스킬 이름은 비어 있을 수 없습니다.");
            

        if (_cooldown <= 0f)
            Debug.Log("쿨타임은 0보다 커야 합니다.");

        SkillName = _name;
        fCoolDownTime = _cooldown;
    }


    public bool TryUse()
    {
        if (IsReady)
        {
            lastUsedTime = Time.time;
            return true;
        }
        return false;
    }

    public float GetRemainingCooldown()
    {
        float remaining = (lastUsedTime + fCoolDownTime) - Time.time;
        return Mathf.Max(0f, remaining);
    }

    public void SetCoolDownTime(float _time)
    {
        fCoolDownTime = _time;
    }
}