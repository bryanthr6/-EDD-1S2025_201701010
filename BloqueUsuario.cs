using System;
using System.Security.Cryptography;
using System.Text;

public class BloqueUsuario
{
    public int Index { get; set; }
    public DateTime Timestamp { get; set; }
    public UsuarioData Data { get; set; }
    public string PreviousHash { get; set; }
    public string Hash { get; set; }
    public int Nonce { get; set; }

    public BloqueUsuario(int index, DateTime timestamp, UsuarioData data, string previousHash)
    {
        Index = index;
        Timestamp = timestamp;
        Data = data;
        PreviousHash = previousHash;
        Nonce = 0;
        Hash = CalcularHash();
    }

    public string CalcularHash()
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            string rawData = $"{Index}{Timestamp:dd-MM-yy::HH:mm:ss}{Data.Serializar()}{PreviousHash}{Nonce}";
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    public void MinarBloque(int dificultad)
    {
        string prefijo = new string('0', dificultad);
        while (Hash.Substring(0, dificultad) != prefijo)
        {
            Nonce++;
            Hash = CalcularHash();
        }
    }
}

public class UsuarioData
{
    public int ID { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty; 
    public int Edad { get; set; } 
    public string Contrasenia { get; set; } = string.Empty;

    public string Serializar()
    {
        return $"{ID}|{Nombres}|{Apellidos}|{Correo}|{Edad}|{Contrasenia}";
    }

    public static UsuarioData Deserializar(string data)
    {
        var partes = data.Split('|');
        return new UsuarioData
        {
            ID = int.Parse(partes[0]),
            Nombres = partes[1],
            Apellidos = partes[2],
            Correo = partes[3],
            Edad = int.Parse(partes[4]),
            Contrasenia = partes[5]
        };
    }
}