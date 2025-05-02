using System;
using System.Collections.Generic;

public class BlockchainUsuarios
{
    public List<BloqueUsuario> Cadena { get; private set; }
    private int Dificultad { get; set; } = 4; // 4 ceros al inicio del hash

    public BlockchainUsuarios()
    {
        Cadena = new List<BloqueUsuario>();
        CrearBloqueGenesis();
    }

    private void CrearBloqueGenesis()
    {
        var genesisData = new UsuarioData
        {
            ID = 0,
            Nombres = "Admin",
            Apellidos = "Sistema",
            Correo = "admin@usac.com",
            Edad = 30,
            Contrasenia = "admin123"
        };

        var genesisBlock = new BloqueUsuario(0, DateTime.Now, genesisData, "0000");
        genesisBlock.MinarBloque(Dificultad);
        Cadena.Add(genesisBlock);
    }

    public BloqueUsuario AgregarUsuario(UsuarioData usuario)
    {
        var ultimoBloque = Cadena[Cadena.Count - 1];
        var nuevoBloque = new BloqueUsuario(ultimoBloque.Index + 1, DateTime.Now, usuario, ultimoBloque.Hash);
        nuevoBloque.MinarBloque(Dificultad);
        Cadena.Add(nuevoBloque);
        return nuevoBloque;
    }

    public bool EsValida()
    {
        for (int i = 1; i < Cadena.Count; i++)
        {
            BloqueUsuario bloqueActual = Cadena[i];
            BloqueUsuario bloqueAnterior = Cadena[i - 1];

            // Verificar hash del bloque actual
            if (bloqueActual.Hash != bloqueActual.CalcularHash())
                return false;

            // Verificar que apunte al hash correcto del bloque anterior
            if (bloqueActual.PreviousHash != bloqueAnterior.Hash)
                return false;

            // Verificar que cumple con la dificultad
            if (!bloqueActual.Hash.StartsWith(new string('0', Dificultad)))
                return false;
        }
        return true;
    }

    public UsuarioData? BuscarUsuarioPorId(int id)
    {
        foreach (var bloque in Cadena)
        {
            if (bloque.Data.ID == id)
                return bloque.Data;
        }
        return null;
    }

    public UsuarioData? BuscarUsuarioPorCorreo(string correo)
    {
        foreach (var bloque in Cadena)
        {
            if (bloque.Data.Correo == correo)
                return bloque.Data;
        }
        return null;
    }
}