using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;

    public List<BuffData> allBuffs;      // ���п�ѡ Buff
    public List<DebuffData> allDebuffs;  // ���п�ѡ Debuff

    public List<BuffData> activeBuffs = new List<BuffData>();
    public List<DebuffData> activeDebuffs = new List<DebuffData>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        //InitBuffList();
        //InitDebuffList();
    }

    public void ApplyBuff(string id)
    {
        // �� allBuffs �в��һ�������
        var baseBuff = allBuffs.Find(b => b.id == id);
        if (baseBuff == null) return;

        // �鿴�Ƿ�����Ч
        var existing = activeBuffs.Find(b => b.id == id);
        if (existing != null)
        {
            // �Ѵ�������������������� 3 ��
            existing.level = Mathf.Min(existing.level + 1, 3);
        }
        else
        {
            // ����һ�� 1 �� Buff
            activeBuffs.Add(new BuffData
            {
                id = baseBuff.id,
                displayName = baseBuff.displayName,
                description = baseBuff.description,
                icon = baseBuff.icon,
                level = 1
            });
        }
        if (id == "RapidFire")
        {
            var wc = FindObjectOfType<WeaponController>();
            if (wc != null)
                wc.ApplyRapidFireBuff(activeBuffs.Find(b => b.id == id).level);
        }
        else if (id == "MultiShot")
        {
            var wc = FindObjectOfType<WeaponController>();
            if (wc != null)
                wc.ApplyMultiShotBuff(activeBuffs.Find(b => b.id == id).level);
        }
        else if (id == "ExplosiveAmmo")
        {
            var wc = FindObjectOfType<WeaponController>();
            if (wc != null)
                wc.ApplyExplosiveAmmoBuff(activeBuffs.Find(b => b.id == id).level);
        }
        else if (id == "HeatCapacity")
        {
            var wc = FindObjectOfType<WeaponController>();
            if (wc != null) wc.ApplyHeatCapacityBuff(activeBuffs.Find(b => b.id == id).level);
        }
        else if (id == "Overclock")
        {
            var wc = FindObjectOfType<WeaponController>();
            if (wc != null) wc.ApplyOverclockBuff(activeBuffs.Find(b => b.id == id).level);
        }
        else if (id == "Overdrive")
        {
            var wc = FindObjectOfType<WeaponController>();
            if (wc != null) wc.ApplyOverdriveBuff(activeBuffs.Find(b => b.id == id).level);
        }
    }
    public void ApplyDebuff(string id)
    {
        // �� allDebuffs �в��һ�������
        var baseDebuff = allDebuffs.Find(d => d.id == id);
        if (baseDebuff == null) return;

        // �鿴�Ƿ�����Ч
        var existing = activeDebuffs.Find(d => d.id == id);
        if (existing != null)
        {
            // �Ѵ�������������������� 3 ��
            existing.level = Mathf.Min(existing.level + 1, 3);
        }
        else
        {
            // ����һ�� 1 �� Debuff
            activeDebuffs.Add(new DebuffData
            {
                id = baseDebuff.id,
                displayName = baseDebuff.displayName,
                description = baseDebuff.description,
                icon = baseDebuff.icon,
                level = 1
            });
        }

        if (id == "Weakened")
        {
            var player = FindObjectOfType<PlayerCharacter>();
            if (player != null)
                player.ApplyWeakenedDebuff(activeDebuffs.Find(b => b.id == id).level);
        }
        else if (id == "Sacrifice")
        {
            var player = FindObjectOfType<PlayerCharacter>();
            if (player != null)
                player.ApplySacrificeDebuff(activeDebuffs.Find(b => b.id == id).level);
        }
        else if (id == "Fragile")
        {
            var player = FindObjectOfType<PlayerCharacter>();
            if (player != null)
                player.ApplyFragileDebuff(activeDebuffs.Find(b => b.id == id).level);
        }
        else if (id == "Blindness")
        {
            var player = FindObjectOfType<PlayerCharacter>();
            if (player != null)
                player.ApplyBlindnessDebuff(activeDebuffs.Find(b => b.id == id).level);
        }
        else if (id == "Berserk")
        {
            var wc = FindObjectOfType<WeaponController>();
            if (wc != null)
                wc.ApplyBerserkBuff(activeDebuffs.Find(b => b.id == id).level);
        }
    }
}
