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

        // Cargar datos iniciales (admin y datos de prueba)
        CargarDatosIniciales();

        // Abre la ventana de login
        WindowLogin loginWindow = new WindowLogin();
        loginWindow.Show();

        Application.Run();
    }

    private static void CargarDatosIniciales()
    {
        try
        {
            // Cargar usuario admin (requerido por el enunciado)
            listaUsuarios.AgregarUsuario(0, "Admin", "Sistema", "admin@usac.com", 30, "admin123");

            // Datos de prueba opcionales
            CargarDatosPrueba();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar datos iniciales: {ex.Message}");
        }
    }

    private static void CargarDatosPrueba()
    {
        // Solo cargar datos de prueba si no hay datos existentes
        if (listaUsuarios.GetCabeza() == null || listaUsuarios.GetCabeza()->Siguiente == null)
        {
            // Usuarios de prueba
            listaUsuarios.AgregarUsuario(1, "Juan", "Pérez", "juan@usac.com", 25, "clave123");
            listaUsuarios.AgregarUsuario(2, "María", "López", "maria@usac.com", 28, "clave456");
            
            // Vehículos de prueba
            listaVehiculos.AgregarVehiculo(1, 1, "Toyota", 2015, "P123ABC");
            listaVehiculos.AgregarVehiculo(2, 2, "Honda", 2018, "P456XYZ");
            
            // Repuestos de prueba
            arbolRepuestos.Insertar(1, "Filtro de aceite", "Filtra impurezas del aceite del motor", 150);
            arbolRepuestos.Insertar(2, "Bujías", "Para encendido del motor", 200);
            arbolRepuestos.Insertar(3, "Pastillas de freno", "Para sistema de frenado", 300);
            
            Console.WriteLine("Datos de prueba cargados exitosamente");
        }
    }

    static void GenerarServicio()
    {
        Console.WriteLine("\n--- Generar Nuevo Servicio ---");
        
        try
        {
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
            string detalles = Console.ReadLine() ?? string.Empty;

            Console.Write("Costo del servicio: ");
            if (!double.TryParse(Console.ReadLine(), out double costo) || costo <= 0)
            {
                Console.WriteLine("Costo inválido. Debe ser un número positivo.");
                return;
            }

            // Generar servicio con factura automática
            if (arbolServicios.GenerarServicioConFactura(arbolFacturas, id, idRepuesto, idVehiculo, detalles, (int)costo))
            {
                Console.WriteLine("Operación completada exitosamente.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar servicio: {ex.Message}");
        }
    }

    static void VerFacturaPorId()
    {
        try
        {
            Console.Write("\nIngrese el ID de la factura a buscar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            arbolFacturas.MostrarFacturaPorId(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al buscar factura: {ex.Message}");
        }
    }

    static void EditarRepuesto()
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error al editar repuesto: {ex.Message}");
        }
    }

    static void MostrarRepuestosOrden()
    {
        try
        {
            Console.WriteLine("Seleccione tipo de recorrido:");
            Console.WriteLine("1. PreOrden");
            Console.WriteLine("2. InOrden");
            Console.WriteLine("3. PostOrden");
            Console.Write("Opción: ");
            string recorrido = Console.ReadLine() ?? string.Empty;

            string resultado = "";
            switch (recorrido)
            {
                case "1":
                    resultado = Program.arbolRepuestos.ObtenerRepuestosPreOrden();
                    break;
                case "2":
                    resultado = Program.arbolRepuestos.ObtenerRepuestosInOrden();
                    break;
                case "3":
                    resultado = Program.arbolRepuestos.ObtenerRepuestosPostOrden();
                    break;
                default:
                    Console.WriteLine("Opción de recorrido no válida.");
                    return;
            }
            
            Console.WriteLine("\n=== Resultado del Recorrido ===");
            Console.WriteLine(resultado);
            Console.WriteLine("===============================");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al mostrar repuestos: {ex.Message}");
        }
    }

    static void CargarUsuarios() 
    {
        try
        {
            DatosJson datos = LeerJson();
            if (datos.Usuarios.Count == 0) 
            {
                Console.WriteLine("No se encontraron usuarios en el archivo.");
                return;
            }

            foreach (var usuario in datos.Usuarios) 
            {
                // Validar que el usuario admin no sea modificado
                if (usuario.ID != 0)
                {
                    listaUsuarios.AgregarUsuario(usuario.ID, usuario.Nombres ?? string.Empty, 
                                               usuario.Apellidos ?? string.Empty, 
                                               usuario.Correo ?? string.Empty, 
                                               usuario.Edad, usuario.Contrasenia ?? string.Empty);
                }
            }
            Console.WriteLine($"Se han cargado {datos.Usuarios.Count} usuarios.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar usuarios: {ex.Message}");
        }
    }

    static void CargarVehiculos() 
    {
        try
        {
            DatosJson datos = LeerJson();
            if (datos.Vehiculos.Count == 0) 
            {
                Console.WriteLine("No se encontraron vehículos en el archivo.");
                return;
            }

            foreach (var vehiculo in datos.Vehiculos) 
            {
                listaVehiculos.AgregarVehiculo(vehiculo.ID, vehiculo.ID_Usuario, 
                                              vehiculo.Marca ?? string.Empty, 
                                              vehiculo.Modelo, 
                                              vehiculo.Placa ?? string.Empty);
            }
            Console.WriteLine($"Se han cargado {datos.Vehiculos.Count} vehículos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar vehículos: {ex.Message}");
        }
    }

    static void CargarRepuestos() 
    {
        try
        {
            DatosJson datos = LeerJson();
            if (datos.Repuestos.Count == 0) 
            {
                Console.WriteLine("No se encontraron repuestos en el archivo.");
                return;
            }

            foreach (var repuesto in datos.Repuestos) 
            {
                arbolRepuestos.Insertar(repuesto.ID, 
                                      repuesto.Repuesto ?? string.Empty, 
                                      repuesto.Detalles ?? string.Empty, 
                                      (int)repuesto.Costo);
            }
            Console.WriteLine($"Se han cargado {datos.Repuestos.Count} repuestos.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar repuestos: {ex.Message}");
        }
    }

    static DatosJson LeerJson() 
    {
        try
        {
            Console.Write("Ingrese la ruta del archivo JSON: ");
            string? rutaArchivo = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(rutaArchivo))
            {
                Console.WriteLine("Ruta no proporcionada.");
                return new DatosJson();
            }

            string rutaCompleta = System.IO.Path.Combine(Directory.GetCurrentDirectory(), rutaArchivo);

            string? rutaFinal = File.Exists(rutaArchivo) ? rutaArchivo : 
                              (File.Exists(rutaCompleta) ? rutaCompleta : null);

            if (rutaFinal == null)
            {
                Console.WriteLine("El archivo JSON no se encontró en las rutas:");
                Console.WriteLine($"1. {rutaArchivo}");
                Console.WriteLine($"2. {rutaCompleta}");
                return new DatosJson();
            }

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
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error al buscar usuario: {ex.Message}");
        }
    }

    static void EliminarUsuarioPorId() 
    {
        try
        {
            Console.Write("Ingrese el ID del usuario a eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int id)) 
            {
                // Prevenir eliminación del admin
                if (id == 0)
                {
                    Console.WriteLine("No se puede eliminar al usuario administrador.");
                    return;
                }
                listaUsuarios.EliminarPorId(id);
            } 
            else 
            {
                Console.WriteLine("ID inválido.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar usuario: {ex.Message}");
        }
    }

    static unsafe void BuscarVehiculoPorId() 
    {
        try
        {
            Console.Write("Ingrese el ID del vehículo a buscar: ");
            if (int.TryParse(Console.ReadLine(), out int id)) 
            {
                NodoVehiculo* vehiculo = listaVehiculos.BuscarPorId(id);
                if (vehiculo != null)
                {
                    Console.WriteLine(listaVehiculos.ObtenerInfoVehiculo(vehiculo));
                }
                else
                {
                    Console.WriteLine("Vehículo no encontrado.");
                }
            } 
            else 
            {
                Console.WriteLine("ID inválido.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al buscar vehículo: {ex.Message}");
        }
    }

    static void EliminarVehiculoPorId() 
    {
        try
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar vehículo: {ex.Message}");
        }
    }
}