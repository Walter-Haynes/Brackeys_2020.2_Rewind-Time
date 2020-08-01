namespace Core.Player.GenericAbilitySystem.Example.RAB
{
    using Player.GenericAbilitySystem;

    /// <summary> Inherit from this if you want a MonoBehaviour with access to your CharacterCore. </summary>
    public abstract class RABBehaviour : AbilitySystemBehaviour<RABCore>
    {
        //No code needed thus far, it's all done in the parent.
    }
}