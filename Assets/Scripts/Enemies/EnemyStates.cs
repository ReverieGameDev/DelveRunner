
public enum EnemyMode 
{ 
    Formation, 
    Decide, 
    Environment, 
    Solo 
}
public enum EnemyState
{
    Attack,
    Death,
    Reposition,
    Retreat,
    Idle
}

public enum EnemySoloState
{
    None,
    soloIsMoving,
    soloIsIdle,
    soloIsSearching
}
