using UnityEngine;

public class CharacterCustomizer : MonoBehaviour
{
    public SpriteRenderer backHairRenderer;
    public SpriteRenderer skinRenderer;
    public SpriteRenderer eyesRenderer;
    public SpriteRenderer shirtRenderer;
    public SpriteRenderer frontHairRenderer;
    public SpriteRenderer shoesRenderer;
    public SpriteRenderer pantsRenderer;

    public void PreviewBackHair(string name)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Characters/Parts/Layer0_Back Hair/{name}");
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError($"[BackHair] Failed to load sprites from: {name}");
            return;
        }
        backHairRenderer.sprite = sprites[0];
        backHairRenderer.transform.localScale = new Vector3(5f, 5f, 5f);
        PlayerPrefs.SetString("SelectedBackHair", name);
    }

    public void PreviewSkin(string name)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Characters/Parts/Layer1_Skin/{name}");
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError($"[Skin] Failed to load sprites from: {name}");
            return;
        }
        skinRenderer.sprite = sprites[0];
        skinRenderer.transform.localScale = new Vector3(5f, 5f, 5f);
        PlayerPrefs.SetString("SelectedSkin", name);
    }

    public void PreviewEyes(string name)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Characters/Parts/Layer2_Eyes/{name}");
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError($"[Eyes] Failed to load sprites from: {name}");
            return;
        }
        eyesRenderer.sprite = sprites[0];
        eyesRenderer.transform.localScale = new Vector3(5f, 5f, 5f);
        PlayerPrefs.SetString("SelectedEyes", name);
    }

    public void PreviewShirt(string name)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Characters/Parts/Layer3_Shirt/{name}");
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError($"[Shirt] Failed to load sprites from: {name}");
            return;
        }
        shirtRenderer.sprite = sprites[0];
        shirtRenderer.transform.localScale = new Vector3(5f, 5f, 5f);
        PlayerPrefs.SetString("SelectedShirt", name);
    }

    public void PreviewFrontHair(string name)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Characters/Parts/Layer4_Front Hair/{name}");
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError($"[FrontHair] Failed to load sprites from: {name}");
            return;
        }
        frontHairRenderer.sprite = sprites[0];
        frontHairRenderer.transform.localScale = new Vector3(5f, 5f, 5f);
        PlayerPrefs.SetString("SelectedFrontHair", name);
    }

    public void PreviewShoes(string name)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Characters/Parts/Layer5_Shoes/{name}");
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError($"[Shoes] Failed to load sprites from: {name}");
            return;
        }
        shoesRenderer.sprite = sprites[0];
        shoesRenderer.transform.localScale = new Vector3(5f, 5f, 5f);
        PlayerPrefs.SetString("SelectedShoes", name);
    }

    public void PreviewPants(string name)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>($"Characters/Parts/Layer6_Pants/{name}");
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError($"[Pants] Failed to load sprites from: {name}");
            return;
        }
        pantsRenderer.sprite = sprites[0];
        pantsRenderer.transform.localScale = new Vector3(5f, 5f, 5f);
        PlayerPrefs.SetString("SelectedPants", name);
    }
}
