using System;
using System.Collections.Generic;

public class UsuarioJson {
    public int ID { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string Correo { get; set; }
    public int Edad { get; set; }
    public string Contrasenia { get; set; }
}

public class VehiculoJson {
    public int ID { get; set; }
    public int ID_Usuario { get; set; }
    public string Marca { get; set; }
    public int Modelo { get; set; }
    public string Placa { get; set; }
}

public class RepuestoJson {
    public int ID { get; set; }
    public string Repuesto { get; set; }
    public string Detalles { get; set; }
    public double Costo { get; set; }
}

public class DatosJson {
    public List<UsuarioJson> Usuarios { get; set; }
    public List<VehiculoJson> Vehiculos { get; set; }
    public List<RepuestoJson> Repuestos { get; set; }
}
