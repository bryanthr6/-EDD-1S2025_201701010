class Usuario {
    public int ID { get; set; }
    public string Nombres { get; set; } = "";
    public string Apellidos { get; set; } = "";
    public string Correo { get; set; } = "";
    public string Contrasenia { get; set; } = "";
}

class Vehiculo {
    public int ID { get; set; }
    public int ID_Usuario { get; set; }
    public string Marca { get; set; } = "";
    public string Modelo { get; set; } = "";
    public string Placa { get; set; } = "";
}

class Repuesto {
    public int ID { get; set; }
    public string RepuestoNombre { get; set; } = "";
    public float Costo { get; set; }
}
