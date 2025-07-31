using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public Animator backHairAnimator;
    public Animator skinAnimator;
    public Animator eyesAnimator;
    public Animator shirtAnimator;
    public Animator frontHairAnimator;
    public Animator shoesAnimator;
    public Animator pantsAnimator;

    private void Start()
    {
        // 默认加载一个角色，比如女性角色
        LoadFemaleCharacter();
    }

    /// <summary>
    /// 加载女性角色：保留所有部件
    /// </summary>
    public void LoadFemaleCharacter()
    {
        Debug.Log("👩‍🦰 Loading Female Character...");

        ResetAllAnimators();

        LoadAnim(backHairAnimator, "SelectedBackHair", "BackHair");
        LoadAnim(frontHairAnimator, "SelectedFrontHair", "FrontHair");
        LoadAnim(skinAnimator, "SelectedSkin", "Skin");
        LoadAnim(eyesAnimator, "SelectedEyes", "Eyes");
        LoadAnim(shirtAnimator, "SelectedShirt", "Shirt");
        LoadAnim(shoesAnimator, "SelectedShoes", "Shoes");
        LoadAnim(pantsAnimator, "SelectedPants", "Pants");
    }

    /// <summary>
    /// 加载男性角色：禁用前/后发部件，避免加载女性动画残留
    /// </summary>
    public void LoadMaleCharacter()
    {
        Debug.Log("👨‍🦱 Loading Male Character...");

        // 清除无效部件保存记录
        PlayerPrefs.DeleteKey("SelectedBackHair");
        PlayerPrefs.Save();

        ResetAllAnimators();

        LoadAnim(skinAnimator, "SelectedSkin", "Skin");
        LoadAnim(eyesAnimator, "SelectedEyes", "Eyes");
        LoadAnim(skinAnimator, "SelectedSkin", "FrontHair");
        LoadAnim(shirtAnimator, "SelectedShirt", "Shirt");
        LoadAnim(shoesAnimator, "SelectedShoes", "Shoes");
        LoadAnim(pantsAnimator, "SelectedPants", "Pants");

        // 不加载 backHair / frontHair，确保它们保持隐藏状态
    }

    /// <summary>
    /// 重置所有动画控制器并隐藏部件
    /// </summary>
    public void ResetAllAnimators()
    {
        Debug.Log("🔄 Resetting all animators...");

        ResetAnimator(backHairAnimator, "BackHair");
        ResetAnimator(frontHairAnimator, "FrontHair");
        ResetAnimator(skinAnimator, "Skin");
        ResetAnimator(eyesAnimator, "Eyes");
        ResetAnimator(shirtAnimator, "Shirt");
        ResetAnimator(shoesAnimator, "Shoes");
        ResetAnimator(pantsAnimator, "Pants");

        Debug.Log("✅ All animators reset.");
    }

    void ResetAnimator(Animator animator, string label)
    {
        if (animator == null)
        {
            Debug.LogWarning($"[{label}] Animator is null.");
            return;
        }

        animator.runtimeAnimatorController = null;
        animator.gameObject.SetActive(false);
        Debug.Log($"[{label}] ❎ Cleared controller & deactivated.");
    }

    void LoadAnim(Animator animator, string key, string label)
    {
        string animName = PlayerPrefs.GetString(key, "");
        Debug.Log($"[{label}] Loading animation. PlayerPrefs key = {key}, value = {animName}");

        if (animator == null)
        {
            Debug.LogWarning($"[{label}] Animator is null.");
            return;
        }

        if (string.IsNullOrEmpty(animName))
        {
            Debug.LogWarning($"[{label}] No animation set. Disabling.");
            animator.gameObject.SetActive(false);
            return;
        }

        string resourcePath = $"Animations/{label}/{animName}";
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(resourcePath);

        if (controller == null)
        {
            Debug.LogError($"[{label}] ❌ Failed to load from Resources/{resourcePath}");
            animator.gameObject.SetActive(false);
            return;
        }

        animator.runtimeAnimatorController = controller;
        animator.gameObject.SetActive(true);
        Debug.Log($"[{label}] ✅ Loaded: {controller.name}");
    }
}
