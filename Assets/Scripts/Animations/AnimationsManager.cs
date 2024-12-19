using UnityEngine;

public class AnimationsManager
{
    private readonly Animator animator;

    public AnimationsManager(Animator animator)
    {
        this.animator = animator;
    }

    public PlayerAnimations GetPlayerAnimations() => new(animator);
}
