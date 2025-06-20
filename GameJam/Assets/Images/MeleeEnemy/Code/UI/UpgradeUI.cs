using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [Header("UI 面板")]
    public GameObject panel;

    [Header("Buff 按钮 (3 个)")]
    public List<Button> buffButtons;

    [Header("Debuff 按钮 (3 个)")]
    public List<Button> debuffButtons;

    private BuffManager buffManager;
    private EnemySpawner spawner;

    private List<BuffData> currentBuffOptions;
    private List<DebuffData> currentDebuffOptions;

    private bool buffSelected;
    private bool debuffSelected;

    void Awake()
    {
        buffManager = FindObjectOfType<BuffManager>();
        spawner = FindObjectOfType<EnemySpawner>();
    }

    public void ShowUpgradePanel()
    {
        panel.SetActive(true);
        buffSelected = false;
        debuffSelected = false;

        // 随机取 3 个选项
        //var rapid = buffManager.allBuffs.Find(b => b.id == "RapidFire");
        //var mult = buffManager.allBuffs.Find(b => b.id == "MultiShot");
        //var exp = buffManager.allBuffs.Find(b => b.id == "ExplosiveAmmo");
        //var heat = buffManager.allBuffs.Find(b => b.id == "HeatCapacity");
        //var overclock = buffManager.allBuffs.Find(b => b.id == "Overclock");
        //var overdrive = buffManager.allBuffs.Find(b => b.id == "Overdrive");
        currentBuffOptions = GetRandom(buffManager.allBuffs, 3);
        currentDebuffOptions = GetRandom(buffManager.allDebuffs, 3);

        SetupBuffButtons();
        SetupDebuffButtons();
    }

    void SetupBuffButtons()
    {
        for (int i = 0; i < buffButtons.Count; i++)
        {
            var btn = buffButtons[i];
            var data = currentBuffOptions[i];

            btn.interactable = true;
            btn.GetComponentInChildren<TMP_Text>().text = data.displayName;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                if (buffSelected) return;
                buffManager.ApplyBuff(data.id);
                btn.interactable = false;
                buffSelected = true;
                var applied = buffManager.activeBuffs.Find(b => b.id == data.id);
                Debug.Log($"[BuffTest] Applied {data.displayName} → Level {applied.level}, ");
                TryFinish();
            });
        }
    }

    void SetupDebuffButtons()
    {
        for (int i = 0; i < debuffButtons.Count; i++)
        {
            var btn = debuffButtons[i];
            var data = currentDebuffOptions[i];

            btn.interactable = true;
            btn.GetComponentInChildren<TMP_Text>().text = data.displayName;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() =>
            {
                if (debuffSelected) return;
                buffManager.ApplyDebuff(data.id);
                btn.interactable = false;
                debuffSelected = true;
                TryFinish();
            });
        }
    }

    void TryFinish()
    {
        // 当 Buff 和 Debuff 都选完
        if (buffSelected && debuffSelected)
        {
            panel.SetActive(false);
            spawner.OnUpgradeSelected();
        }
    }

    // 从 list 中随机挑 count 个元素
    List<T> GetRandom<T>(List<T> list, int count)
    {
        var copy = new List<T>(list);
        for (int i = copy.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            var tmp = copy[i];
            copy[i] = copy[r];
            copy[r] = tmp;
        }
        return copy.GetRange(0, Mathf.Min(count, copy.Count));
    }
}

