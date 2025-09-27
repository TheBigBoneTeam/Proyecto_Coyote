
//Clase para los cambios de estado
//Inclute el estado anterior y el actual
public class stateData
{
    public GameState oldState;
    public GameState currentState;
    public stateData(GameState _old, GameState _new)
    {
        oldState = _old;
        currentState = _new;
    }
}