using System;
using System.IO;
using System.Text.Json;

unsafe class Program 
{
    static ListaUsuarios listaUsuarios = new ListaUsuarios();
    static ListaVehiculos listaVehiculos = new ListaVehiculos();
    static ArbolAVLRepuestos arbolRepuestos = new ArbolAVLRepuestos();
    static ArbolBinarioServicios arbolServicios = new ArbolBinarioServicios(
        id => arbolRepuestos.Buscar(id) != null,
        id => listaVehiculos.BuscarPorId(id) != null
    );
    static ArbolB5Facturas arbolFacturas = new ArbolB5Facturas();

    static void Main() 
    {
        // Inicializar el árbol de servicios con las funciones de validación
        arbolServicios = new ArbolBinarioServicios(
            id => arbolRepuestos.Buscar(id) != null,
            id => listaVehiculos.BuscarPorId(id) != null
        );

        Console.WriteLine("=== Sistema de Administración ===");
        Login();
    }

    static void Login() 
    {
        string emailAdmin = "admin@usac.com";
        string passwordAdmin = "admin123";

        while (true) 
        {
            Console.Write("Ingrese su correo: ");
            string email = Console.ReadLine() ?? "";
            Console.Write("Ingrese su contraseña: ");
            string password = Console.ReadLine() ?? "";

            if (email == emailAdmin && password == passwordAdmin) 
            {
                Console.WriteLine("\n¡Acceso concedido!\n");
                MostrarMenuAdministrador();
                break;
            } 
            else 
            {
                Console.WriteLine("\nCredenciales incorrectas. Intente de nuevo.\n");
            }
        }
    }

    static void MostrarMenuAdministrador() 
    {
        while (true) 
        {
            Console.WriteLine("\n=== Menú Administrador ===");
            Console.WriteLine("1. Carga masiva de usuarios");
            Console.WriteLine("2. Carga masiva de vehículos");
            Console.WriteLine("3. Carga masiva de repuestos");
            Console.WriteLine("4. Buscar Usuario por ID");
            Console.WriteLine("5. Eliminar Usuario por ID");
            Console.WriteLine("6. Buscar Vehiculo por ID");
            Console.WriteLine("7. Eliminar Vehiculo por ID");
            Console.WriteLine("8. Ver Repuestos");
            Console.WriteLine("9. Editar Repuesto por ID"); 
            Console.WriteLine("10. Visualización de Repuestos");
            Console.WriteLine("11. Generar Servicio");
            Console.WriteLine("12. Ver Factura por ID");
            Console.WriteLine("13. Salir"); 
            Console.Write("Seleccione una opción: ");

            string opcion = Console.ReadLine() ?? "";
            switch (opcion) 
            {
                case "1":
                    CargarUsuarios();
                    break;
                case "2":
                    CargarVehiculos();
                    break;
                case "3":
                    CargarRepuestos();
                    break;
                case "4":
                    BuscarUsuarioPorId();
                    break;
                case "5":
                    EliminarUsuarioPorId();
                    break;
                case "6":
                    BuscarVehiculoPorId();
                    break;
                case "7":
                    EliminarVehiculoPorId();
                    break;
                case "8":
                    arbolRepuestos.MostrarRepuestos();
                    break;
                case "9":
                    EditarRepuesto();
                    break;
                case "10":
                    MostrarRepuestosOrden();
                    break;
                case "11":
                    GenerarServicio();
                    break;
                case "12":
                    VerFacturaPorId();
                    break;
                case "13":
                    Console.WriteLine("Saliendo del sistema...");
                    return;
                default:
                    Console.WriteLine("Opción no válida. Intente nuevamente.");
                    break;
            }
        }
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