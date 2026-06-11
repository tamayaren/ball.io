using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance { get; private set; }

    private void Awake() => instance = this;

    [SerializeField] private TextMeshProUGUI eventText;
    [SerializeField] private TextMeshProUGUI currentMinions;
    [SerializeField] private TextMeshProUGUI winConditionCount;

    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
    
    [SerializeField] private LeaderAgent playerAgent;

    private Coroutine lastCoroutine;
    private IEnumerator HideEventString()
    {
        yield return new WaitForSeconds(5f);
        this.eventText.text = "";
    }
    
    public void SetEventUI(string text)
    {
        if (this.lastCoroutine != null) StopCoroutine(this.lastCoroutine);
        this.eventText.text = text;
        this.lastCoroutine = StartCoroutine(HideEventString());
    }

    private void Start()
    {
        Time.timeScale = 1;
        this.playerAgent.MinionChanged.AddListener((Transform minion) =>
        {
            this.currentMinions.text = $"CURRENT MINIONS: {this.playerAgent.minions.Count}";
        });
    }

    private void LateUpdate() => this.winConditionCount.text =
        $"KILL ENEMIES: {WinCondition.instance.currentKills}/{WinCondition.instance.requiredKills}";

    private void Hide()
    {
        this.currentMinions.gameObject.SetActive(false);
        this.winConditionCount.gameObject.SetActive(false);
    }
    
    private void DressDescription(Transform ui)
    {
        Image uiImage = ui.GetComponent<Image>();
        if (uiImage)
            uiImage.DOFade(.8f, .5f).SetEase(Ease.OutQuad);
        
        Transform description = ui.Find("Description");

        if (description != null)
        {
            TextMeshProUGUI text = description.GetComponent<TextMeshProUGUI>();

            if (text == null) return;
            text.text = $"MINIONS OBTAINED: {this.playerAgent.minions.Count} | ENEMIES KILLED: {WinCondition.instance.currentKills}/{WinCondition.instance.requiredKills}";
        }
    }
    
    public void Lose()
    {
        Hide();
        this.loseUI.SetActive(true);
        DressDescription(this.loseUI.transform);
    }

    public void Win()
    {
        Destroy(this.loseUI);
        Hide();
        this.winUI.SetActive(true);
        DressDescription(this.loseUI.transform);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
