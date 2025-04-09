using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{

    #region Private ����
    private BaseState currentState;
    #endregion 

    public StateMachine(BaseState startState)
    {
        currentState = startState;
    }

    public void ChangeState(BaseState changeState)
    {

        if (changeState == null)
        {
            return;
        }

        if (currentState != changeState)
        {
            currentState.OnStateExit();
            currentState = changeState;
            currentState.OnStateEnter();
        }
    }

    public void UpdateState()
    {
        if (currentState != null)
        {
            currentState.OnStateUpdate();
        }
    }
    
}
