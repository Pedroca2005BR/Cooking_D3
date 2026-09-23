using System;

public static class EventBusCorte
{
    public static Action<IngredienteDataSO> OnIniciarMiniGame; //evento para quando iniciar o mini game
    public static Action OnFimMiniGame; //evento para quando termina o mini game ao atingir a quantidade maxima de cortes

    public static Action OnTerminarMiniGame; //evento para quando o mini game eh terminado bruscamente

    public static Action OnCortar; //evento lancado quando detectado input do jogador

    public static Action OnCorteAcertado; //evento lancado quando acerta o corte
    public static Action OnCorteErrado; //quando erra o corte

    public static Action<float, float> OnAtualizarZA; // PosX, Largura da Zona de Acerto
    public static Action<float> OnCursorMove;         // PosX do Cursor
}