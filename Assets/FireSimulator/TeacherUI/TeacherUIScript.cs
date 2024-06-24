using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class TeacherUIScript : MonoBehaviour{
    #region Components
    [Header("Required Components")]
    [SerializeField] Button launchButton;
    [SerializeField] TextMeshProUGUI title, stats, status;

    Animator animator;
    #endregion
    void Awake(){
        animator = GetComponent<Animator>();
    }

    public void OnSimulationStart(){
        title.text = "<b>В одной из комнат случилось внезапное возгарание!</b>";

        animator.Play("simulationStarted");
    }

    public void SetStatus(string content) => status.text = content;

    public void OnSimulationEnd(){
        title.text = "<b>Симуляция окончена</b>";

        stats.text = $"Время симуляции\t-\t{FireManager.Instance.seconds.ToString("0")} сек.\n\n\nПроцент повреждений\t-\t{FireManager.Instance.damagePrecent.ToString("0")}%";

        launchButton.onClick = OnSimulationEnded;
        launchButton.GetComponentInChildren<TextMeshProUGUI>().text = "Повторить симуляцию";

        animator.Play("simulationEnded");
    }

    public void ReloadLevel(){
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }

    public Button.ButtonClickedEvent OnSimulationEnded;
}