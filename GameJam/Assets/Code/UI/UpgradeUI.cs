using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [Header("整体面板")]
    public GameObject panel;

    [Header("随机选项区域")]
    public List<Button> buffOptionButtons;    // 下方随机 Buff
    public List<Button> debuffOptionButtons;  // 下方随机 Debuff

    [Header("已选列表区域")]
    public List<Button> selectedBuffButtons;   // 顶部已选 Buff
    public List<Button> selectedDebuffButtons; // 顶部已选 Debuff

    [Header("UI 显示")]
    public TMP_Text buffPointText;             // 显示剩余 Buff 点数
    public Button continueButton;
    private BuffManager buffManager;
    private EnemySpawner spawner;

    private List<BuffData> buffOptions;
    private List<DebuffData> debuffOptions;
    private int buffPoints = 0;
    private bool buffselected = false;
    private bool debuffselected = false;

    void Awake()
    {
        buffManager = FindObjectOfType<BuffManager>();
        spawner = FindObjectOfType<EnemySpawner>();
        continueButton.onClick.AddListener(OnContinueClicked);
        continueButton.interactable = false;
    }

    public void ShowUpgradePanel()
    {
        // 计算随机池：排除已经激活的
        var availableBuffs = buffManager.allBuffs.FindAll(b =>
            buffManager.activeBuffs.Find(x => x.id == b.id) == null);
        var availableDebuffs = buffManager.allDebuffs.FindAll(d =>
            buffManager.activeDebuffs.Find(x => x.id == d.id) == null);

        // 随机取三
        buffOptions = GetRandom(availableBuffs, buffOptionButtons.Count);
        debuffOptions = GetRandom(availableDebuffs, debuffOptionButtons.Count);

        // 更新顶部已选列表
        PopulateSelectedLists();

        // 更新Buff点数显示
        buffPointText.text = $"Buff Points: {buffPoints}";

        // 设置下方随机按钮
        SetupOptionButtons(buffOptionButtons, buffOptions, OnBuffOptionClicked);
        SetupOptionButtons(debuffOptionButtons, debuffOptions, OnDebuffOptionClicked);

        panel.SetActive(true);
    }

    void PopulateSelectedLists()
    {
        // 已选 Buff
        for (int i = 0; i < selectedBuffButtons.Count; i++)
        {
            var btn = selectedBuffButtons[i];
            if (i < buffManager.activeBuffs.Count)
            {
                var data = buffManager.activeBuffs[i];
                SetButton(btn, $"{data.displayName} Lv{data.level}",
                    () =>
                    {
                        if (buffPoints <= 0 || data.level >= 3) return;
                        buffManager.ApplyBuff(data.id);
                        buffPoints--;
                        buffPointText.text = $"Buff Points: {buffPoints}";
                        // 更新按钮文本为新等级
                        btn.GetComponentInChildren<TMP_Text>().text = $"{data.displayName} Lv{data.level}";
                    });
                btn.gameObject.SetActive(true);
            }
            else btn.gameObject.SetActive(false);
        }

        // 已选 Debuff
        for (int i = 0; i < selectedDebuffButtons.Count; i++)
        {
            var btn = selectedDebuffButtons[i];
            if (i < buffManager.activeDebuffs.Count)
            {
                var data = buffManager.activeDebuffs[i];
                SetButton(btn, $"{data.displayName} Lv{data.level}",
                    () =>
                    {
                        if (data.level >= 3) return;
                        buffManager.ApplyDebuff(data.id);
                        buffPoints++;
                        buffPointText.text = $"Buff Points: {buffPoints}";
                        btn.GetComponentInChildren<TMP_Text>().text = $"{data.displayName} Lv{data.level}";
                    });
                btn.gameObject.SetActive(true);
            }
            else btn.gameObject.SetActive(false);
        }
    }

    void OnBuffOptionClicked(BuffData data, Button btn)
    {
        buffManager.ApplyBuff(data.id);
        buffPointText.text = $"Buff Points: {buffPoints}";
        // 立即刷新顶部
        buffselected = true;
        PopulateSelectedLists();
        buffOptionButtons.ForEach(b => b.interactable = false);
        TryEnableContinue();
    }

    void OnDebuffOptionClicked(DebuffData data, Button btn)
    {
        buffManager.ApplyDebuff(data.id);
        buffPointText.text = $"Buff Points: {buffPoints}";
        // 立即刷新顶部
        debuffselected = true;
        PopulateSelectedLists();
        debuffOptionButtons.ForEach(b => b.interactable = false);
        TryEnableContinue();
    }

    void SetupOptionButtons<T>(List<Button> buttons, List<T> datas, System.Action<T, Button> callback)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var btn = buttons[i];
            if (i >= datas.Count)
            {
                btn.gameObject.SetActive(false);
                continue;
            }

            var data = datas[i];
            btn.gameObject.SetActive(true);

            // extract the displayName statically
            string label;
            if (data is BuffData bd) label = bd.displayName;
            else if (data is DebuffData dd) label = dd.displayName;
            else label = data.ToString();

            SetButton(btn, label, () => callback(data, btn));
        }
    }

    void SetButton(Button btn, string label, UnityEngine.Events.UnityAction onClick)
    {
        btn.interactable = true;
        var text = btn.GetComponentInChildren<TMP_Text>();
        if (text != null) text.text = label;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(onClick);
    }

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

    void TryEnableContinue()
    {
        if (buffselected && debuffselected)
            continueButton.interactable = true;
    }

    void OnContinueClicked()
    {
        panel.SetActive(false);
        spawner.OnUpgradeSelected();// 或者 OnUpgradeSelected()
    }
}

