
public enum EnemyMode 
{ 
    Formation, 
    Decide, 
    Environment, 
    Solo,
    Free
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
