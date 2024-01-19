using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Bir sonraki seviyeye geçiş fonksiyonu
    public void LoadNextLevel()
    {
        // Şuanki sahnenin index'ini al
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Bir sonraki sahnenin index'ini belirle
        int nextSceneIndex = currentSceneIndex + 1;

        // Eğer bir sonraki sahne varsa, o sahneyi yükle
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Eğer bir sonraki sahne yoksa, oyunu yeniden başlatabilir veya başka bir işlem yapabilirsiniz.
            Debug.Log("Oyun bitti. Bir sonraki seviye yok.");
        }
    }

    // Seviyenin yeniden başlatılması fonksiyonu
    public void RestartLevel()
    {
        // Şuanki sahnenin index'ini al
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Şuanki sahneyi yeniden yükle
        SceneManager.LoadScene(currentSceneIndex);
    }
}