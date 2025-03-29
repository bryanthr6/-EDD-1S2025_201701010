using System;
using System.IO;
using System.Text.Json;
using Gtk;

unsafe class Program
{
    public static ListaUsuarios listaUsuarios = new ListaUsuarios();
    public static ListaVehiculos listaVehiculos = new ListaVehiculos();
    public static ArbolAVLRepuestos arbolRepuestos = new ArbolAVLRepuestos();
    public static ArbolBinarioServicios arbolServicios = new ArbolBinarioServicios(
        id => arbolRepuestos.Buscar(id) != null,
        id => listaVehiculos.BuscarPorId(id) != null
    );
    public static ArbolB5Facturas arbolFacturas = new ArbolB5Facturas();

    public static void Main()
    {
        Application.Init();

        // Abre la ventana de login
        WindowLogin loginWindow = new WindowLogin();
        loginWindow.Show();

        Application.Run();
    }

    static void GenerarServicio()
    {
        Console.WriteLine("\n--- Generar Nuevo Servicio ---");
        
        Console.Write("ID del servicio: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        Console.Write("ID del repuesto: ");
        if (!int.TryParse(Console.ReadLine(), out int idRepuesto))
        {
            Console.WriteLine("ID de repuesto inválido.");
            return;
        }

        Console.Write("ID del vehículo: ");
        if (!int.TryParse(Console.ReadLine(), out int idVehiculo))
        {
            Console.WriteLine("ID de vehículo inválido.");
            return;
        }

        Console.Write("Detalles del servicio: ");
        string detalles = Console.ReadLine() ?? "";

        Console.Write("Costo del servicio: ");
        if (!double.TryParse(Console.ReadLine(), out double costo))
        {
            Console.WriteLine("Costo inválido.");
            return;
        }

        // Generar servicio con factura automática
        if (arbolServicios.GenerarServicioConFactura(arbolFacturas, id, idRepuesto, idVehiculo, detalles, (int)costo))
        {
            Console.WriteLine("Operación completada exitosamente.");
        }
    }

    static void VerFacturaPorId()
    {
        Console.Write("\nIngrese el ID de la factura a buscar: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        arbolFacturas.MostrarFacturaPorId(id);
    }

    static void EditarRepuesto()
    {
        Console.Write("Ingrese el ID del repuesto a editar: ");
        if (int.TryParse(Console.ReadLine(), out int idRepuesto))
        {
            arbolRepuestos.EditarRepuestoPorId(idRepuesto);
        }
        else
        {
            Console.WriteLine("ID inválido. Intente nuevamente.");
        }
    }

    static void MostrarRepuestosOrden()
    {
        Console.WriteLine("Seleccione tipo de recorrido:");
        Console.WriteLine("1. PreOrden");
        Console.WriteLine("2. InOrden");
        Console.WriteLine("3. PostOrden");
        Console.Write("Opción: ");
        string recorrido = Console.ReadLine() ?? "";

        switch (recorrido)
        {
            case "1":
                arbolRepuestos.MostrarRepuestosPreOrden();
                break;
            case "2":
                arbolRepuestos.MostrarRepuestos();
                break;
            case "3":
                arbolRepuestos.MostrarRepuestosPostOrden();
                break;
            default:
                Console.WriteLine("Opción de recorrido no válida.");
                break;
        }
    }

    static void CargarUsuarios() 
    {
        DatosJson datos = LeerJson();
        if (datos.Usuarios.Count == 0) return;

        foreach (var usuario in datos.Usuarios) 
        {
            listaUsuarios.AgregarUsuario(usuario.ID, usuario.Nombres, usuario.Apellidos, usuario.Correo, usuario.Edad, usuario.Contrasenia);
        }
        Console.WriteLine($"Se han cargado {datos.Usuarios.Count} usuarios.");
    }

    static void CargarVehiculos() 
    {
        DatosJson datos = LeerJson();
        if (datos.Vehiculos.Count == 0) return;

        foreach (var vehiculo in datos.Vehiculos) 
        {
            listaVehiculos.AgregarVehiculo(vehiculo.ID, vehiculo.ID_Usuario, vehiculo.Marca, vehiculo.Modelo, vehiculo.Placa);
        }
        Console.WriteLine($"Se han cargado {datos.Vehiculos.Count} vehículos.");
    }

    static void CargarRepuestos() 
    {
        DatosJson datos = LeerJson();
        if (datos.Repuestos.Count == 0) return;

        foreach (var repuesto in datos.Repuestos) 
        {
            arbolRepuestos.Insertar(repuesto.ID, repuesto.Repuesto, repuesto.Detalles, (int)repuesto.Costo);
        }
        Console.WriteLine($"Se han cargado {datos.Repuestos.Count} repuestos.");
    }

    static DatosJson LeerJson() 
    {
        Console.Write("Ingrese la ruta del archivo JSON: ");
        string rutaArchivo = Console.ReadLine() ?? "";

        Console.WriteLine($"Directorio actual: {Directory.GetCurrentDirectory()}");

        string rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), rutaArchivo);

        if (!File.Exists(rutaArchivo) && !File.Exists(rutaCompleta)) 
        {
            Console.WriteLine("El archivo JSON no se encontró. Verifique la ruta e intente nuevamente.");
            Console.WriteLine($"Se buscó en: {rutaArchivo}");
            Console.WriteLine($"Y también en: {rutaCompleta}");
            return new DatosJson { Usuarios = new List<UsuarioJson>(), Vehiculos = new List<VehiculoJson>(), Repuestos = new List<RepuestoJson>() };
        }

        string rutaFinal = File.Exists(rutaArchivo) ? rutaArchivo : rutaCompleta;

        try 
        {
            string json = File.ReadAllText(rutaFinal);
            return JsonSerializer.Deserialize<DatosJson>(json) ?? new DatosJson();
        } 
        catch (Exception ex) 
        {
            Console.WriteLine($"Error al leer el archivo JSON: {ex.Message}");
            return new DatosJson();
        }
    }

    static unsafe void BuscarUsuarioPorId() 
    {
        Console.Write("Ingrese el ID del usuario a buscar: ");
        if (int.TryParse(Console.ReadLine(), out int id)) 
        {
            NodoUsuario* usuario = listaUsuarios.BuscarPorId(id);
            if (usuario != null) 
            {
                Console.WriteLine($"ID: {usuario->Id}");
                Console.WriteLine($"Nombre: {listaUsuarios.PtrToString(usuario->Nombres)} {listaUsuarios.PtrToString(usuario->Apellidos)}");
                Console.WriteLine($"Correo: {listaUsuarios.PtrToString(usuario->Correo)}");
                Console.WriteLine($"Edad: {usuario->Edad}");
            }
            else 
            {
                Console.WriteLine("Usuario no encontrado.");
            }
        } 
        else 
        {
            Console.WriteLine("ID inválido.");
        }
    }

    static void EliminarUsuarioPorId() 
    {
        Console.Write("Ingrese el ID del usuario a eliminar: ");
        if (int.TryParse(Console.ReadLine(), out int id)) 
        {
            listaUsuarios.EliminarPorId(id);
        } 
        else 
        {
            Console.WriteLine("ID inválido.");
        }
    }

    static unsafe void BuscarVehiculoPorId() 
    {
        Console.Write("Ingrese el ID del vehículo a buscar: ");
        if (int.TryParse(Console.ReadLine(), out int id)) 
        {
            NodoVehiculo* vehiculo = listaVehiculos.BuscarPorId(id);
            Console.WriteLine(listaVehiculos.ObtenerInfoVehiculo(vehiculo));
        } 
        else 
        {
            Console.WriteLine("ID inválido.");
        }
    }

    static void EliminarVehiculoPorId() 
    {
        Console.Write("Ingrese el ID del vehículo a eliminar: ");
        if (int.TryParse(Console.ReadLine(), out int id)) 
        {
            listaVehiculos.EliminarPorId(id);
        } 
        else 
        {
            Console.WriteLine("ID inválido.");
        }
    }
}