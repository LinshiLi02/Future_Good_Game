using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    public Animator backHairAnimator;
    public Animator skinAnimator;
    public Animator eyesAnimator;
    public Animator shirtAnimator;
    public Animator frontHairAnimator;
    //public Animator shoesAnimator;
    public Animator pantsAnimator;

    private void Start()
    {
        LoadAnim(backHairAnimator, "SelectedBackHair", "BackHair");
        LoadAnim(skinAnimator, "SelectedSkin", "Skin");
        LoadAnim(eyesAnimator, "SelectedEyes", "Eyes");
        LoadAnim(shirtAnimator, "SelectedShirt", "Shirt");
        LoadAnim(frontHairAnimator, "SelectedFrontHair", "FrontHair");
        //LoadAnim(shoesAnimator, "SelectedShoes", "Shoes");
        LoadAnim(pantsAnimator, "SelectedPants", "Pants");
    }

    void LoadAnim(Animator animator, string key, string layerName)
    {
        string animName = PlayerPrefs.GetString(key, "");
        Debug.Log($"[{layerName}] Trying to load animation. PlayerPrefs key = {key}, value = {animName}");

        if (string.IsNullOrEmpty(animName))
        {
            Debug.LogWarning($"[{layerName}] No animation name saved in PlayerPrefs.");
            return;
        }

        string resourcePath = $"Animations/{layerName}/{animName}";
        Debug.Log($"[{layerName}] Resource path: {resourcePath}");

        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(resourcePath);

        if (controller == null)
        {
            Debug.LogError($"[{layerName}] ❌ Failed to load AnimatorController at: Resources/{resourcePath}.controller");
            return;
        }

        animator.runtimeAnimatorController = controller;
        Debug.Log($"[{layerName}] ✅ Successfully loaded and applied AnimatorController: {controller.name}");

        // ✅ 额外调试信息
        Debug.Log($"Animator: {animator.name}, IsActive: {animator.isActiveAndEnabled}, Controller: {animator.runtimeAnimatorController?.name}");

        if (animator == null)
        {
            Debug.LogWarning($"[{layerName}] Animator reference is null. Did you forget to assign it in the Inspector?");
            return;
        }
    }
}
