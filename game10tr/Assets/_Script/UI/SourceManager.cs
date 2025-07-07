using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private List<AudioSource> listAudio = new List<AudioSource>();
    private float currentVolume = 1f;

    private AudioSource bgmSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Tự cập nhật khi đổi scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        RefreshAudioList();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshAudioList();
    }

    public void RefreshAudioList()
    {
        listAudio.Clear();

        // Quét toàn bộ AudioSource hiện có
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();
        foreach (var audio in allAudio)
        {
            // Không thêm bgmSource nếu đã có
            if (audio != bgmSource)
            {
                listAudio.Add(audio);
                audio.volume = currentVolume;
            }
        }

        // Nếu chưa có bgmSource, tạo mới
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.volume = currentVolume;
        }
    }

    public void ChangeVolume(float volume)
    {
        currentVolume = volume;

        // Áp dụng cho toàn bộ âm thanh (gồm cả bgmSource)
        foreach (var audio in listAudio)
        {
            if (audio != null)
                audio.volume = volume;
        }

        if (bgmSource != null)
            bgmSource.volume = volume;
    }

    public void PlayBGM(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>("Sounds/" + name);

        if (clip != null && bgmSource != null && bgmSource.clip != clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }
}
