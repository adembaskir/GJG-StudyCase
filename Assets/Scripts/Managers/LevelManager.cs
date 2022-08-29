using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour
{
    [Header("Variables:")]
    [SerializeField] private EventManager eventManager;
    [SerializeField] private Animator armAnim;
    [SerializeField] private int currentLevel;
    [Space]
    [Header("UI Elements:")]
    [SerializeField] private GameObject canvasWinMenu;
   
    private void Start()
    {
        
        currentLevel = PlayerPrefs.GetInt("level", 0);
        eventManager.CallLevelStartedEvent();

    }
   
    private void LevelCompleted()
    {
        currentLevel++;
        PlayerPrefs.SetInt("level", currentLevel);
        armAnim.SetTrigger("Pain");
        StartCoroutine(WaitForAnim());
        
    }

    public IEnumerator WaitForAnim()
    {
        yield return new WaitForSeconds(1f);
        armAnim.SetTrigger("Win");
        yield return new WaitForSeconds(2f);
        canvasWinMenu.SetActive(true);

    }
    public void RestartScene() 
    {
        SceneManager.LoadScene(0);
        canvasWinMenu.SetActive(false);
    }
    void OnEnable()
    {
        EventManager.onLevelCompleted += LevelCompleted;
    }
    void OnDisable()
    {
        EventManager.onLevelCompleted -= LevelCompleted;
    }
}
