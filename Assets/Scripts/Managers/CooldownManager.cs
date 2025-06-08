using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CooldownManager
{
    // Start is called before the first frame update
    public List<Skill> List_Skill = new List<Skill>();
    private Dictionary<string, Skill> Dictionary_Skill = new Dictionary<string, Skill>();

    public void RegisterSkill(string _name, float _cooldown)
    {
        if (!Dictionary_Skill.ContainsKey(_name))
        {
            Skill newskill = new Skill(_name, _cooldown);
            Dictionary_Skill.Add(_name, newskill);
        }
    }

    public bool CheckCooldownSkill(string name) // 쿨다운 체크후 사용
    {

        if (Dictionary_Skill.TryGetValue(name, out Skill skill))
        {
            if (skill.TryUse())
            {
                //Debug.Log($"{name} 사용됨");
                return true;
            }
            else
            {
                //Debug.Log($"{name} 쿨타임: {skill.GetRemainingCooldown():F1}s 남음");
            }
        }
        else
        {
            Debug.Log($"{name} 을 찾을 수 없음!");

        }

        return false;
    }

    public float GetCooldown(string name)
    {
        return Dictionary_Skill.ContainsKey(name) ? Dictionary_Skill[name].GetRemainingCooldown() : 0f;
    }

    public void ResetCooldownTime(string name, float _time)
    {
        if (Dictionary_Skill.TryGetValue(name, out Skill skill))
            skill.SetCoolDownTime(_time);
        else
            Debug.Log($"{name} 찾을 수 없음!");
    }
}
