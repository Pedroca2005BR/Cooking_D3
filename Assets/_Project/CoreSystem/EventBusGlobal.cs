using UnityEngine;
using System;

public static class EventBusGlobal{
    /*SISTEMA DE TEMPO
    [-------------------------------------------------------]*/


    //evento que vai passar o tempo restante
    public static event Action<int> OnTempoAtual;
    //evento que vai avisar que acabou o tempo
    public static event Action OnTempoEsgotado;

    //funcao que o gerenciador de tempo vai usar para avisar a UI e outros sistemas
    public static void DispararTempoAtual(int tempo) {
        OnTempoAtual?.Invoke(tempo);
    }

    //funcao que o gerenciador de tempo vai usar para avisar q o tempo esgotou
    public static void DispararTempoEsgotado() {
        OnTempoEsgotado?.Invoke();
    }
}
