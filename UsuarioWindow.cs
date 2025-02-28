using System;
using Gtk;

public unsafe class UsuarioWindow : Window
{
    private NodoUsuario* usuario;

    public UsuarioWindow(NodoUsuario* usuario) : base("Panel de Usuario")
    {
        this.usuario = usuario;  // Guardamos el usuario actual

        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);

        Box vbox = new Box(Orientation.Vertical, 5);
        Label nameLabel = new Label($"Bienvenido, {GetString(usuario->Nombres)} {GetString(usuario->Apellidos)}");
        Label emailLabel = new Label($"Correo: {GetString(usuario->Correo)}");

        // Lista de vehículos
        Label vehiculosLabel = new Label("\n Tus Vehículos:");
        Box vehiculosBox = new Box(Orientation.Vertical, 5);
        CargarVehiculos(vehiculosBox);

        // Lista de servicios
        Label serviciosLabel = new Label("\n historial de Servicios:");
        Box serviciosBox = new Box(Orientation.Vertical, 5);
        CargarServicios(serviciosBox);

        Button logoutButton = new Button("Cerrar Sesión");
        logoutButton.Clicked += (sender, e) =>
        {
            new LoginWindow().Show();
            Destroy();
        };

        vbox.PackStart(nameLabel, false, false, 10);
        vbox.PackStart(emailLabel, false, false, 10);
        vbox.PackStart(vehiculosLabel, false, false, 5);
        vbox.PackStart(vehiculosBox, false, false, 5);
        vbox.PackStart(serviciosLabel, false, false, 5);
        vbox.PackStart(serviciosBox, false, false, 5);
        vbox.PackStart(logoutButton, false, false, 10);

        Add(vbox);
        ShowAll();
    }

    // 🔹 Función para cargar los vehículos del usuario
    private void CargarVehiculos(Box contenedor)
    {
        NodoVehiculo* vehiculo = Program.vehiculos.cabeza;
        bool tieneVehiculos = false;

        while (vehiculo != null)
        {
            if (vehiculo->ID_Usuario == usuario->ID)
            {
                Label vehiculoLabel = new Label($"{GetString(vehiculo->Marca)} {GetString(vehiculo->Modelo)} - {GetString(vehiculo->Placa)}");
                contenedor.PackStart(vehiculoLabel, false, false, 5);
                tieneVehiculos = true;
            }
            vehiculo = vehiculo->siguiente;
        }

        if (!tieneVehiculos)
        {
            contenedor.PackStart(new Label("No tienes vehículos registrados."), false, false, 5);
        }
    }

    // 🔹 Función para cargar los servicios de los vehículos del usuario
    private void CargarServicios(Box contenedor)
    {
        NodoServicio* servicio = Program.servicios.frente;
        bool tieneServicios = false;

        while (servicio != null)
        {
            // Verificar si el servicio está relacionado con algún vehículo del usuario
            NodoVehiculo* vehiculo = Program.vehiculos.cabeza;
            while (vehiculo != null)
            {
                if (vehiculo->ID_Vehiculo == servicio->ID_Vehiculo && vehiculo->ID_Usuario == usuario->ID)
                {
                    Label servicioLabel = new Label($"Servicio en {GetString(vehiculo->Marca)} {GetString(vehiculo->Modelo)} - {GetString(vehiculo->Placa)}\n" +
                                                    $"Detalle: {GetString(servicio->Detalles)} | Costo: Q{servicio->Costo}");
                    contenedor.PackStart(servicioLabel, false, false, 5);
                    tieneServicios = true;
                }
                vehiculo = vehiculo->siguiente;
            }
            servicio = servicio->siguiente;
        }

        if (!tieneServicios)
        {
            contenedor.PackStart(new Label("No tienes servicios registrados."), false, false, 5);
        }
    }

    // 🔹 Función para convertir `char*` en `string`
    private unsafe string GetString(char* charArray)
    {
        return new string(charArray).TrimEnd('\0');
    }
}
