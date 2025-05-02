using System;
using System.Collections.Generic;

public class UsuarioJson {
    public int ID { get; set; } = 0;
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public int Edad { get; set; } = 0;
    public string Contrasenia { get; set; } = string.Empty;
}

public class VehiculoJson {
    public int ID { get; set; } = 0;
    public int ID_Usuario { get; set; } = 0;
    public string Marca { get; set; } = string.Empty;
    public int Modelo { get; set; } = 0;
    public string Placa { get; set; } = string.Empty;
}

public class RepuestoJson {
    public int ID { get; set; }
    public string Repuesto { get; set; } = string.Empty;
    public string Detalles { get; set; } = string.Empty;
    public double Costo { get; set; } = 0.0;
}

public class DatosJson {
    public List<UsuarioJson> Usuarios { get; set; } = new List<UsuarioJson>();
    public List<VehiculoJson> Vehiculos { get; set; } = new List<VehiculoJson>();
    public List<RepuestoJson> Repuestos { get; set; } = new List<RepuestoJson>();
}
