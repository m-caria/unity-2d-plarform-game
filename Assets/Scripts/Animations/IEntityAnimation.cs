public interface IEntityAnimation
{
    void Run(bool enabled);
    void Grounded(bool enabled);
    void DoubleJump();
    void WallSlide(bool enabled);
    void Hit();
}
