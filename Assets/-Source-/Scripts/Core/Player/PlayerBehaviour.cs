namespace Core.Player
{
    using GenericAbilitySystem;

    /// <summary> Inherit from this if you want a MonoBehaviour with access to your CharacterCore. </summary>
    public abstract partial class PlayerBehaviour : AbilitySystemBehaviour<PlayerCore>
    {
        //No code needed thus far, it's all done in the parent.
    }
}