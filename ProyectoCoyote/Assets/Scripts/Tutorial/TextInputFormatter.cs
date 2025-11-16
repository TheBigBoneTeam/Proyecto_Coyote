using UnityEngine;

public static class InputTextFormatter
{
    public static string Cambiar(string texto, GameInput.DeviceType device)
    {
        switch (device)
        {
            case GameInput.DeviceType.Gamepad:
                return FormatearGamepad(texto);

            case GameInput.DeviceType.Mobile:
                return FormatearMobile(texto);

            default:
                return FormatearKeyboard(texto);
        }
    }

    private static string FormatearKeyboard(string t)
    {
        return t.Replace("/movimiento/", "<b>WASD</b>")
                .Replace("/camara/", "<b>el ratón</b>")
                .Replace("/esquivar/", "<b>Espacio</b>")
                .Replace("/dashear/", "<b>Shift</b>")
                .Replace("/pegar/", "<b>Clic Izquierdo</b>")
                .Replace("/lockeo/", "<b>Q</b>");
    }

    private static string FormatearGamepad(string t)
    {
        return t.Replace("/movimiento/", "<b>el Joystick Izquierdo</b>")
                .Replace("/camara/", "<b>el Joystick Derecho</b>")
                .Replace("/esquivar/", "<b>B / O</b>")
                .Replace("/dashear/", "<b>L3</b>")
                .Replace("/pegar/", "<b>RT / R2</b>")
                .Replace("/lockeo/", "<b>Click Derecho Stick</b>");
    }

    private static string FormatearMobile(string t)
    {
        return t.Replace("/movimiento/", "<b>el joystick móvil</b>")
                .Replace("/camara/", "<b>arrastrar con el dedo</b>")
                .Replace("/esquivar/", "<b>Botón Esquivar</b>")
                .Replace("/dashear/", "<b>Botón Dash</b>")
                .Replace("/pegar/", "<b>Botón Ataque</b>")
                .Replace("/lockeo/", "<b>Botón Lock</b>");
    }
}
