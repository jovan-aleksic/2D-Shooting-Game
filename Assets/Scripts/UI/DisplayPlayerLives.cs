using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Image))]
public class DisplayPlayerLives : MonoBehaviour
{
    private Image m_image;

    [SerializeField] private Sprite[] sprites;
    [SerializeField] private StatReference playerLives;

    private void Awake()
    {
        m_image = GetComponent<Image>();

        if (sprites == null || sprites.Length < 1 || playerLives == null)
        {
            gameObject.SetActive(false);
        }

        Debug.Assert(playerLives != null, nameof(playerLives) + " != null");
        if (playerLives.useConstant || playerLives.variable != null) return;
        gameObject.SetActive(false);
    }

    private void Start()
    {
        UpdatePlayerLivesDisplay();
    }

    public void UpdatePlayerLivesDisplay()
    {
        Debug.Assert(playerLives != null, nameof(playerLives) + " != null");
        int livesToDisplay = (int) playerLives.Value;

        Debug.Assert(sprites != null, nameof(sprites) + " != null");
        if (livesToDisplay >= sprites.Length)
            livesToDisplay = sprites.Length - 1;

        Debug.Assert(m_image != null, nameof(m_image) + " != null");
        m_image.sprite = sprites[livesToDisplay];
    }
}
