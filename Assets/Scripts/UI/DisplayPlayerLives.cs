using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DisplayPlayerLives : MonoBehaviour
{
    private Image m_image;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private StatReference playerLives;

    private void Awake()
    {
        m_image = GetComponent<Image>();
    }

    private void Start()
    {
        if (sprites == null || sprites.Length < 1)
        {
            gameObject.SetActive(false);
            return;
        }

        UpdatePlayerLivesDisplay();
    }

    public void UpdatePlayerLivesDisplay()
    {
        int livesToDisplay = (int) playerLives.Value;

        if (livesToDisplay >= sprites.Length)
            livesToDisplay = sprites.Length - 1;

        m_image.sprite = sprites[livesToDisplay];
    }
}
