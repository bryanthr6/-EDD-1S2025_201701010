using System;
using System.Runtime.InteropServices;
using System.Text;

public unsafe struct NodoServicio 
{
    public int Id;
    public int IdRepuesto;
    public int IdVehiculo;
    public fixed char Detalles[100];
    public int Costo;
    public NodoServicio* Izquierdo;
    public NodoServicio* Derecho;

    public NodoServicio(int id, int idRepuesto, int idVehiculo, string detalles, int costo) 
    {
        Id = id;
        IdRepuesto = idRepuesto;
        IdVehiculo = idVehiculo;
        Costo = costo;
        Izquierdo = null;
        Derecho = null;
        
        fixed (char* ptr = Detalles) 
        {
            for (int i = 0; i < detalles.Length && i < 100; i++) 
                ptr[i] = detalles[i];
            if (detalles.Length < 100) ptr[detalles.Length] = '\0';
        }
    }
}

public unsafe class ArbolBinarioServicios : IDisposable 
{
    private NodoServicio* raiz;
    private Func<int, bool> validarRepuesto;
    private Func<int, bool> validarVehiculo;

    public ArbolBinarioServicios(Func<int, bool> validarRepuesto, Func<int, bool> validarVehiculo) 
    {
        raiz = null;
        this.validarRepuesto = validarRepuesto;
        this.validarVehiculo = validarVehiculo;
    }

    ~ArbolBinarioServicios() 
    {
        Dispose(false);
    }

    public void Dispose() 
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) 
    {
        if (raiz != null) 
        {
            LiberarMemoria(raiz);
            raiz = null;
        }
    }

    private void LiberarMemoria(NodoServicio* nodo) 
    {
        if (nodo == null) return;

        LiberarMemoria(nodo->Izquierdo);
        LiberarMemoria(nodo->Derecho);
        Marshal.FreeHGlobal((IntPtr)nodo);
    }

    public bool Insertar(int id, int idRepuesto, int idVehiculo, string detalles, int costo) 
    {
        if (!validarRepuesto(idRepuesto)) 
        {
            Console.WriteLine($"Error: No existe repuesto con ID {idRepuesto}");
            return false;
        }
        if (!validarVehiculo(idVehiculo)) 
        {
            Console.WriteLine($"Error: No existe vehículo con ID {idVehiculo}");
            return false;
        }
        if (string.IsNullOrEmpty(detalles)) 
        {
            Console.WriteLine("Error: Los detalles del servicio no pueden estar vacíos");
            return false;
        }

        raiz = InsertarRec(raiz, id, idRepuesto, idVehiculo, detalles, costo);
        return true;
    }

    public bool GenerarServicioConFactura(ArbolB5Facturas arbolFacturas, int id, int idRepuesto, int idVehiculo, string detalles, int costo)
    {
        if (!Insertar(id, idRepuesto, idVehiculo, detalles, costo))
            return false;
        
        // Generar factura automática
        int idFactura = (int)(DateTime.Now.Ticks % 1000000);
        Factura nuevaFactura = new Factura(idFactura, id, costo);
        arbolFacturas.Insertar(nuevaFactura);

        Console.WriteLine("\n¡Servicio y factura generados exitosamente!");
        Console.WriteLine($"ID Servicio: {id}");
        Console.WriteLine($"ID Factura: {idFactura}");
        Console.WriteLine($"Total: Q{costo:F2}");
        
        return true;
    }

    private NodoServicio* InsertarRec(NodoServicio* nodo, int id, int idRepuesto, int idVehiculo, string detalles, int costo) 
    {
        if (nodo == null) 
        {
            NodoServicio* nuevo = (NodoServicio*)Marshal.AllocHGlobal(sizeof(NodoServicio));
            *nuevo = new NodoServicio(id, idRepuesto, idVehiculo, detalles, costo);
            Console.WriteLine($"Servicio {id} creado exitosamente");
            return nuevo;
        }

        if (id < nodo->Id)
            nodo->Izquierdo = InsertarRec(nodo->Izquierdo, id, idRepuesto, idVehiculo, detalles, costo);
        else if (id > nodo->Id)
            nodo->Derecho = InsertarRec(nodo->Derecho, id, idRepuesto, idVehiculo, detalles, costo);
        else
            Console.WriteLine($"Error: Ya existe un servicio con ID {id}");

        return nodo;
    }

    public NodoServicio* Buscar(int id) 
    {
        return BuscarRec(raiz, id);
    }

    private NodoServicio* BuscarRec(NodoServicio* nodo, int id) 
    {
        if (nodo == null || nodo->Id == id)
            return nodo;

        return id < nodo->Id ? BuscarRec(nodo->Izquierdo, id) : BuscarRec(nodo->Derecho, id);
    }

    public void MostrarInOrden() 
    {
        Console.WriteLine("\n=== SERVICIOS (IN ORDEN) ===");
        MostrarInOrdenRec(raiz);
        Console.WriteLine("============================");
    }

    public void MostrarPreOrden() 
    {
        Console.WriteLine("\n=== SERVICIOS (PRE ORDEN) ===");
        MostrarPreOrdenRec(raiz);
        Console.WriteLine("=============================");
    }

    public void MostrarPostOrden() 
    {
        Console.WriteLine("\n=== SERVICIOS (POST ORDEN) ===");
        MostrarPostOrdenRec(raiz);
        Console.WriteLine("==============================");
    }

    private void MostrarInOrdenRec(NodoServicio* nodo) 
    {
        if (nodo == null) return;
        MostrarInOrdenRec(nodo->Izquierdo);
        MostrarServicio(nodo);
        MostrarInOrdenRec(nodo->Derecho);
    }

    private void MostrarPreOrdenRec(NodoServicio* nodo) 
    {
        if (nodo == null) return;
        MostrarServicio(nodo);
        MostrarPreOrdenRec(nodo->Izquierdo);
        MostrarPreOrdenRec(nodo->Derecho);
    }

    private void MostrarPostOrdenRec(NodoServicio* nodo) 
    {
        if (nodo == null) return;
        MostrarPostOrdenRec(nodo->Izquierdo);
        MostrarPostOrdenRec(nodo->Derecho);
        MostrarServicio(nodo);
    }

    private void MostrarServicio(NodoServicio* servicio) 
    {
        if (servicio == null) return;
        
        Console.WriteLine($"ID: {servicio->Id}");
        Console.WriteLine($"Repuesto ID: {servicio->IdRepuesto}");
        Console.WriteLine($"Vehículo ID: {servicio->IdVehiculo}");
        Console.WriteLine($"Detalles: {PtrToString(servicio->Detalles)}");
        Console.WriteLine($"Costo: Q{servicio->Costo}");
        Console.WriteLine("----------------------------");
    }

    private static string PtrToString(char* ptr) 
    {
        var sb = new StringBuilder();
        for (int i = 0; i < 100 && ptr[i] != '\0'; i++)
            sb.Append(ptr[i]);
        return sb.ToString();
    }
}