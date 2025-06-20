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

        InitBuffList();
        InitDebuffList();
    }

    void InitBuffList()
    {
        allBuffs = new List<BuffData>
    {
        new BuffData {
            id = "RapidFire",
            displayName = "RapidFire",
            description = "�������ټӿ죨10%/20%/30%��",
            level = 0
        },
        new BuffData {
            id = "ExplosiveAmmo",
            displayName = "ExplosiveAmmo",
            description = "�ӵ������з�Χ�˺���С/��/�� �뾶��",
            level = 0
        },
        new BuffData {
            id = "MultiShot",
            displayName = "MultiShot",
            description = "ÿ��������ⷢ���ӵ���ÿ���˺�Ϊ10%��",
            level = 0
        },
        new BuffData {
            id = "HeatCapacity",
            displayName = "HeatCapacity",
            description = "�¶����䳤��15%/30%/50%��",
            level = 0
        },
        new BuffData {
            id = "Overclock",
            displayName = "Overclock",
            description = "�����ӵ��˺����ӣ�10%/20%/30%��",
            level = 0
        },
        new BuffData {
            id = "Overdrive",
            displayName = "Overdrive",
            description = "����ɢ�ȸ��죨20%/40%/60%��",
            level = 0
        }
    };
    }

    void InitDebuffList()
    {
        allDebuffs = new List<DebuffData>
    {
        new DebuffData {
            id = "Weakened",
            displayName = "Weakened",
            description = "�ƶ��ٶȱ�����10%/20%/30%��",
            level = 0
        },
        new DebuffData {
            id = "Sacrifice",
            displayName = "Sacrifice",
            description = "Ѫ�����޽��ͣ�5%/10%/15%��",
            level = 0
        },
        new DebuffData {
            id = "Fragile",
            displayName = "Fragile",
            description = "�ܵ��˺���ߣ�10%/20%/30%��",
            level = 0
        },
        //new DebuffData {
        //   id = "Wither",
        //    displayName = "Wither",
        //   description = "��ѪЧ�����٣�10%/20%/30%��",
        //    level = 0
        //},
        new DebuffData {
            id = "Blindness",
            displayName = "Blindness",
            description = "��Ұ���٣�С/��/��",
            level = 0
        },
        new DebuffData {
            id = "Berserk",
            displayName = "Berserk",
            description = "ʧ��ʱ��䳤��1s/2s/3s��",
            level = 0
        }
    };
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
