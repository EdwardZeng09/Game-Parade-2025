using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;

    public List<BuffData> allBuffs;      // 所有可选 Buff
    public List<DebuffData> allDebuffs;  // 所有可选 Debuff

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
            displayName = "速射导轨",
            description = "弹道射速加快（10%/20%/30%）",
            level = 0
        },
        new BuffData {
            id = "ExplosiveAmmo",
            displayName = "爆裂子弹",
            description = "子弹命中有范围伤害（小/中/大 半径）",
            level = 0
        },
        new BuffData {
            id = "MultiShot",
            displayName = "散射",
            description = "每次射击额外发出子弹（每发伤害为10%）",
            level = 0
        },
        new BuffData {
            id = "HeatCapacity",
            displayName = "临时存储",
            description = "温度条变长（15%/30%/50%）",
            level = 0
        },
        new BuffData {
            id = "Overclock",
            displayName = "聚能",
            description = "单发子弹伤害增加（10%/20%/30%）",
            level = 0
        },
        new BuffData {
            id = "Overdrive",
            displayName = "超频",
            description = "武器散热更快（20%/40%/60%）",
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
            displayName = "虚弱",
            description = "移动速度变慢（10%/20%/30%）",
            level = 0
        },
        new DebuffData {
            id = "Sacrifice",
            displayName = "牺牲",
            description = "血量上限降低（5%/10%/15%）",
            level = 0
        },
        new DebuffData {
            id = "Fragile",
            displayName = "易伤",
            description = "受到伤害提高（10%/20%/30%）",
            level = 0
        },
        new DebuffData {
            id = "Wither",
            displayName = "枯萎",
            description = "回血效果减少（10%/20%/30%）",
            level = 0
        },
        new DebuffData {
            id = "Blindness",
            displayName = "致盲",
            description = "视野减少（小/中/大）",
            level = 0
        },
        new DebuffData {
            id = "Berserk",
            displayName = "癫狂",
            description = "失控时间变长（1s/2s/3s）",
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
