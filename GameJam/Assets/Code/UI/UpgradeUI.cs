using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
    public TMP_Text descriptionText;
    private BuffManager buffManager;
    private EnemySpawner spawner;
    private WeaponController weapon;
    public Image[] buffIconSlots;    
    public Image[] debuffIconSlots;
    private List<BuffData> buffOptions;
    private List<DebuffData> debuffOptions;
    private int buffPoints = 0;
    private bool buffselected = false;
    private bool debuffselected = false;

    void Awake()
    {
        buffManager = FindObjectOfType<BuffManager>();
        spawner = FindObjectOfType<EnemySpawner>();
        weapon = FindObjectOfType<WeaponController>();
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
        weapon.ClearHeat();
        // 更新顶部已选列表
        PopulateSelectedLists();

        // 更新Buff点数显示
        buffPointText.text = $"Buff Points: {buffPoints}";

        // 设置下方随机按钮
        SetupOptionButtons(buffOptionButtons, buffOptions, OnBuffOptionClicked);
        SetupOptionButtons(debuffOptionButtons, debuffOptions, OnDebuffOptionClicked);
        descriptionText.text = "";
        panel.SetActive(true);
    }

    void PopulateSelectedLists()
    {
        for (int i = 0; i < selectedBuffButtons.Count; i++)
        {
            var btn = selectedBuffButtons[i];
            if (i < buffManager.activeBuffs.Count)
            {
                var data = buffManager.activeBuffs[i];

                // 给Image换图
                var img = btn.GetComponent<Image>();
                img.sprite = data.icon;
                img.type = Image.Type.Simple;
                img.preserveAspect = true;

                // 隐藏文字（如果有Text）
                var txt = btn.GetComponentInChildren<TMP_Text>();
                if (txt != null) txt.enabled = false;

                // 交互：只有有点数且没到满级时才能升级
                btn.interactable = (buffPoints > 0 && data.level < 3);

                // 点击升级
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    if (buffPoints <= 0 || data.level >= 3) return;
                    buffManager.ApplyBuff(data.id);
                    buffPoints--;
                    buffPointText.text = $"Buff Points: {buffPoints}";
                    UpdateSelectedIcons();
                    PopulateSelectedLists();
                });

                btn.gameObject.SetActive(true);
            }
            else
            {
                btn.gameObject.SetActive(false);
            }
        }

        // 已选 Debuff
        for (int i = 0; i < selectedDebuffButtons.Count; i++)
        {
            var btn = selectedDebuffButtons[i];
            if (i < buffManager.activeDebuffs.Count)
            {
                var data = buffManager.activeDebuffs[i];

                // 给Image换图
                var img = btn.GetComponent<Image>();
                img.sprite = data.icon;
                img.type = Image.Type.Simple;
                img.preserveAspect = true;

                // 隐藏文字
                var txt = btn.GetComponentInChildren<TMP_Text>();
                if (txt != null) txt.enabled = false;

                // 点击升级Debuff，升级时 +1 buffPoint
                btn.interactable = (data.level < 3);
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    if (data.level >= 3) return;
                    buffManager.ApplyDebuff(data.id);
                    buffPoints++;
                    buffPointText.text = $"Buff Points: {buffPoints}";
                    UpdateSelectedIcons();
                    PopulateSelectedLists();
                });

                btn.gameObject.SetActive(true);
            }
            else
            {
                btn.gameObject.SetActive(false);
            }
        }
    }

    void OnBuffOptionClicked(BuffData data, Button btn)
    {
        buffManager.ApplyBuff(data.id);
        buffPointText.text = $"Buff Points: {buffPoints}";
        // 立即刷新顶部
        buffselected = true;
        PopulateSelectedLists();
        UpdateSelectedIcons();
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
        UpdateSelectedIcons();
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
            btn.interactable = true;

 
            var txt = btn.GetComponentInChildren<TMP_Text>();
                    if (txt != null) txt.enabled = false;

            var img = btn.GetComponent<Image>();
            img.sprite = (data is BuffData bd) ? bd.icon : ((DebuffData)(object)data).icon;
            img.type = Image.Type.Sliced;     
            img.preserveAspect = true;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => callback(data, btn));

            var trigger = btn.GetComponent<EventTrigger>();
            if (trigger == null) trigger = btn.gameObject.AddComponent<EventTrigger>();
            trigger.triggers.Clear();
            string desc = (data is BuffData b2) ? b2.description : ((DebuffData)(object)data).description;

            var e1 = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            e1.callback.AddListener(_ => descriptionText.text = desc);
            trigger.triggers.Add(e1);

            var e2 = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            e2.callback.AddListener(_ => descriptionText.text = "");
            trigger.triggers.Add(e2);
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

    void UpdateSelectedIcons()
    {
        // Buff 
        for (int i = 0; i < buffIconSlots.Length; i++)
        {
            if (i < buffManager.activeBuffs.Count)
            {
                var data = buffManager.activeBuffs[i];
                buffIconSlots[i].sprite = data.icon;
                buffIconSlots[i].gameObject.SetActive(true);
            }
            else
            {
                buffIconSlots[i].gameObject.SetActive(false);
            }
        }

        // Debuff
        for (int i = 0; i < debuffIconSlots.Length; i++)
        {
            if (i < buffManager.activeDebuffs.Count)
            {
                var data = buffManager.activeDebuffs[i];
                debuffIconSlots[i].sprite = data.icon;
                debuffIconSlots[i].gameObject.SetActive(true);
            }
            else
            {
                debuffIconSlots[i].gameObject.SetActive(false);
            }
        }
    }
}

