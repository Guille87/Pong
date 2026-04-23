public static class GameSettings
{
    public enum ControlType { Humano, IA, Desactivado }

    public static ControlType P1 = ControlType.Humano;
    public static ControlType P2 = ControlType.IA;
    public static ControlType P3 = ControlType.Desactivado;
    public static ControlType P4 = ControlType.Desactivado;
}
