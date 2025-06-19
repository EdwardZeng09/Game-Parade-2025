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
        Instance = this;
    }

    void InitBuffList()
    {
        allBuffs = new List<BuffData>
    {
        new BuffData {
            id = "RapidFire",
            displayName = "���䵼��",
            description = "�������ټӿ죨10%/20%/30%��",
            level = 0
        },
        new BuffData {
            id = "ExplosiveAmmo",
            displayName = "�����ӵ�",
            description = "�ӵ������з�Χ�˺���С/��/�� �뾶��",
            level = 0
        },
        new BuffData {
            id = "MultiShot",
            displayName = "ɢ��",
            description = "ÿ��������ⷢ���ӵ���ÿ���˺�Ϊ10%��",
            level = 0
        },
        new BuffData {
            id = "HeatCapacity",
            displayName = "��ʱ�洢",
            description = "�¶����䳤��15%/30%/50%��",
            level = 0
        },
        new BuffData {
            id = "Overclock",
            displayName = "����",
            description = "�����ӵ��˺����ӣ�10%/20%/30%��",
            level = 0
        },
        new BuffData {
            id = "Overdrive",
            displayName = "��Ƶ",
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
            displayName = "����",
            description = "�ƶ��ٶȱ�����10%/20%/30%��",
            level = 0
        },
        new DebuffData {
            id = "Sacrifice",
            displayName = "����",
            description = "Ѫ�����޽��ͣ�5%/10%/15%��",
            level = 0
        },
        new DebuffData {
            id = "Fragile",
            displayName = "����",
            description = "�ܵ��˺���ߣ�10%/20%/30%��",
            level = 0
        },
        new DebuffData {
            id = "Wither",
            displayName = "��ή",
            description = "��ѪЧ�����٣�10%/20%/30%��",
            level = 0
        },
        new DebuffData {
            id = "Blindness",
            displayName = "��ä",
            description = "��Ұ���٣�С/��/��",
            level = 0
        },
        new DebuffData {
            id = "Berserk",
            displayName = "��",
            description = "ʧ��ʱ��䳤��1s/2s/3s��",
            level = 0
        }
    };
    }

    public float GetBuffMultiplier(string id)
    {
        var buff = activeBuffs.Find(b => b.id == id);
        if (buff == null) return 1f;

        switch (id)
        {
            case "RapidFire":
                return 1f + 0.1f * buff.level;
            default:
                return 1f;
        }
    }
}
