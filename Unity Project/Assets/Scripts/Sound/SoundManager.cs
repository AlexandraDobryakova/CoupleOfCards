using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private bool defaultState = true;
    [SerializeField] private Transform soundContainer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public bool IsSoundsEnabled
    {
        get => PlayerPrefs.GetInt("soundState", defaultState ? 1 : 0) == 1;
        private set => PlayerPrefs.SetInt("soundState", value ? 1 : 0);
    }

    public void PlaySound(GameObject sound)
    {
        if (IsSoundsEnabled == false || sound == null) return;

        Instantiate(sound, soundContainer);
    }

    public void Mute()
    {
        IsSoundsEnabled = false;
    }

    public void UnMute()
    {
        IsSoundsEnabled = true;
    }
}