namespace GravityEater.Lib.AI
{
    /// <summary>
    ///     Define o comportamento da criatura em relação a um personagem
    /// </summary>
    public enum BehaviorType
    {
        Passive, // Não reage ao personagem
        Scared, // Foge do personagem, mas ataca caso possível
        Agressive, // Ataca e persegue o personagem
        Coward // Foge do personagem sem atacar
    }
}