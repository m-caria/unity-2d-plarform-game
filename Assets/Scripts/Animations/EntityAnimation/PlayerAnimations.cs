using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : IEntityAnimation
{
    private readonly Animator animator;

    public PlayerAnimations(Animator animator)
    {
        this.animator = animator;
    }

    public Dictionary<PlayerAnimationParams, string> animationParams = new()
    {
        { PlayerAnimationParams.RUN, "isRunning" },
        { PlayerAnimationParams.GROUNDED, "isGrounded" },
        { PlayerAnimationParams.DOUBLE_JUMP, "doubleJump" },
        { PlayerAnimationParams.WALL_SLIDE, "isWallSliding" },
        { PlayerAnimationParams.Y_VELOCITY, "yVelocity" },
        { PlayerAnimationParams.HIT, "hit" }
    };

    public void DoubleJump() 
        => animator.SetTrigger(animationParams[PlayerAnimationParams.DOUBLE_JUMP]);

    public void Grounded(bool enabled)
        => animator.SetBool(animationParams[PlayerAnimationParams.GROUNDED], enabled);

    public void Run(bool enabled)
        => animator.SetBool(animationParams[PlayerAnimationParams.RUN], enabled);

    public void WallSlide(bool enabled)
        => animator.SetBool(animationParams[PlayerAnimationParams.WALL_SLIDE], enabled);

    public void SetYVelocity(float yVelocity)
        => animator.SetFloat(animationParams[PlayerAnimationParams.Y_VELOCITY], yVelocity);

    public void Hit()
        => animator.SetTrigger(animationParams[PlayerAnimationParams.HIT]);

    public void UpdateAnimations(float yVelocity, bool isGrounded, bool isSlidingWall)
    {
        SetYVelocity(yVelocity);
        Grounded(isGrounded);
        WallSlide(isSlidingWall);
    }
}
