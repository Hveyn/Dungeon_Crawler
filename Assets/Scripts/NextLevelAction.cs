using UnityEngine;


public class NextLevelAction : MonoBehaviour
{
    [SerializeField] 
    private GameObject UiText;

    private DungeonData _dungeonData;
    
    private bool _playerIsClose = false;

    private int _countSceneInBuild;
    private int _indexActiveScene;
    private void Awake()
    {
        _countSceneInBuild = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        _indexActiveScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        _dungeonData = FindObjectOfType<DungeonData>();
    }

    private void Update()
    {
        if (!_playerIsClose || _dungeonData.countEnemy > 0)
        {
            UiText.SetActive(false);
            return;
        }
        
        UiText.SetActive(true);
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_indexActiveScene + 1 <= _countSceneInBuild - 1)
            {
                SceneManager.Instance.LoadNextScene(_indexActiveScene+1);
            }
            else
            {
                UiInGame.Instance.FinalGame();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerIsClose = false;
        }
    }
}
