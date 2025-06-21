using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [Header("�������")]
    public GameObject panel;

    [Header("���ѡ������")]
    public List<Button> buffOptionButtons;    // �·���� Buff
    public List<Button> debuffOptionButtons;  // �·���� Debuff

    [Header("��ѡ�б�����")]
    public List<Button> selectedBuffButtons;   // ������ѡ Buff
    public List<Button> selectedDebuffButtons; // ������ѡ Debuff

    [Header("UI ��ʾ")]
    public TMP_Text buffPointText;             // ��ʾʣ�� Buff ����
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
        // ��������أ��ų��Ѿ������
        var availableBuffs = buffManager.allBuffs.FindAll(b =>
            buffManager.activeBuffs.Find(x => x.id == b.id) == null);
        var availableDebuffs = buffManager.allDebuffs.FindAll(d =>
            buffManager.activeDebuffs.Find(x => x.id == d.id) == null);

        // ���ȡ��
        buffOptions = GetRandom(availableBuffs, buffOptionButtons.Count);
        debuffOptions = GetRandom(availableDebuffs, debuffOptionButtons.Count);

        // ���¶�����ѡ�б�
        PopulateSelectedLists();

        // ����Buff������ʾ
        buffPointText.text = $"Buff Points: {buffPoints}";

        // �����·������ť
        SetupOptionButtons(buffOptionButtons, buffOptions, OnBuffOptionClicked);
        SetupOptionButtons(debuffOptionButtons, debuffOptions, OnDebuffOptionClicked);

        panel.SetActive(true);
    }

    void PopulateSelectedLists()
    {
        // ��ѡ Buff
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
                        // ���°�ť�ı�Ϊ�µȼ�
                        btn.GetComponentInChildren<TMP_Text>().text = $"{data.displayName} Lv{data.level}";
                    });
                btn.gameObject.SetActive(true);
            }
            else btn.gameObject.SetActive(false);
        }

        // ��ѡ Debuff
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
        // ����ˢ�¶���
        buffselected = true;
        PopulateSelectedLists();
        buffOptionButtons.ForEach(b => b.interactable = false);
        TryEnableContinue();
    }

    void OnDebuffOptionClicked(DebuffData data, Button btn)
    {
        buffManager.ApplyDebuff(data.id);
        buffPointText.text = $"Buff Points: {buffPoints}";
        // ����ˢ�¶���
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
        spawner.OnUpgradeSelected();// ���� OnUpgradeSelected()
    }
}

