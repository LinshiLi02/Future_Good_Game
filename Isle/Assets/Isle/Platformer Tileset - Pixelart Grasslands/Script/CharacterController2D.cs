using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Header("Animation Parts")]
    public Animator backHairAnimator;
    public Animator skinAnimator;
    public Animator eyesAnimator;
    public Animator shirtAnimator;
    public Animator frontHairAnimator;
    public Animator shoesAnimator;
    public Animator pantsAnimator;

    [Header("Movement Settings")]
    public float moveSpeed = 4f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator baseAnimator;
    private float dirX = 0f;

    private bool isMale = false; // 性别标志

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        baseAnimator = GetComponent<Animator>();

        DetectGenderFromPrefs();

        LoadCharacterFromPrefs();

        // 设置为“running”状态并暂停在第一帧
        if (baseAnimator != null && baseAnimator.runtimeAnimatorController != null)
        {
            baseAnimator.SetBool("running", true);
        }
        SetAllAnimatorsSpeed(0f);
    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        if (dirX > 0f)
        {
            baseAnimator.SetBool("running", true);
            sprite.flipX = false;
            SetAllAnimatorsSpeed(1f);  // 播放动画
        }
        else if (dirX < 0f)
        {
            baseAnimator.SetBool("running", true);
            sprite.flipX = true;
            SetAllAnimatorsSpeed(1f);  // 播放动画
        }
        else
        {
            baseAnimator.SetBool("running", true);  // 保持在 running 状态
            SetAllAnimatorsSpeed(0f);               // 停在第一帧
        }
    }

    // === 同步动画速度 ===
    private void SetAllAnimatorsSpeed(float speed)
    {
        if (baseAnimator != null) baseAnimator.speed = speed;
        if (backHairAnimator != null) backHairAnimator.speed = speed;
        if (frontHairAnimator != null) frontHairAnimator.speed = speed;
        if (skinAnimator != null) skinAnimator.speed = speed;
        if (eyesAnimator != null) eyesAnimator.speed = speed;
        if (shirtAnimator != null) shirtAnimator.speed = speed;
        if (shoesAnimator != null) shoesAnimator.speed = speed;
        if (pantsAnimator != null) pantsAnimator.speed = speed;
    }

    // === 根据PlayerPrefs检测是否为男性 ===
    private void DetectGenderFromPrefs()
    {
        string backHairName = PlayerPrefs.GetString("SelectedBackHair", "");
        string frontHairName = PlayerPrefs.GetString("SelectedFrontHair", "");

        // 判定是否包含 "Male" 字符串（区分大小写）
        if (backHairName.Contains("Male") || frontHairName.Contains("Male"))
        {
            isMale = true;
            Debug.Log("Detected Male character.");

            // 男性角色不加载后发，清除相关保存的后发Key
            PlayerPrefs.DeleteKey("SelectedBackHair");
            PlayerPrefs.Save();
        }
        else
        {
            isMale = false;
            Debug.Log("Detected Female character.");
        }
    }

    // === 换装加载 ===
    private void LoadCharacterFromPrefs()
    {
        Debug.Log("🧍‍♀️ Loading character from PlayerPrefs...");

        // 男性角色不加载后发动画
        if (!isMale)
        {
            LoadAnim(backHairAnimator, "SelectedBackHair", "BackHair");
        }
        else
        {
            if (backHairAnimator != null)
            {
                backHairAnimator.runtimeAnimatorController = null;
                backHairAnimator.gameObject.SetActive(false);
            }
        }

        LoadAnim(frontHairAnimator, "SelectedFrontHair", "FrontHair");
        LoadAnim(skinAnimator, "SelectedSkin", "Skin");
        LoadAnim(eyesAnimator, "SelectedEyes", "Eyes");
        LoadAnim(shirtAnimator, "SelectedShirt", "Shirt");
        LoadAnim(shoesAnimator, "SelectedShoes", "Shoes");
        LoadAnim(pantsAnimator, "SelectedPants", "Pants");
    }

    private void LoadAnim(Animator animator, string key, string label)
    {
        string animName = PlayerPrefs.GetString(key, "");
        Debug.Log($"[{label}] Loading animation. PlayerPrefs key = {key}, value = {animName}");

        if (animator == null || string.IsNullOrEmpty(animName))
        {
            Debug.LogWarning($"[{label}] Missing animator or animation name. Disabling.");
            if (animator != null) animator.gameObject.SetActive(false);
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
