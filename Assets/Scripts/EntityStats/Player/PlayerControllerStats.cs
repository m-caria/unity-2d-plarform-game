[System.Serializable]
public struct PlayerControllerStats
{
    public float Movement { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsOnWall { get; set; }
    public bool IsFacingRight { get; set; }
    public bool IsWallSliding { get; set; }
    public bool CanMove { get; set; }
    public bool CanDoubleJump { get; set; }
    public bool CanSlideOnWall { get; set; }
}
