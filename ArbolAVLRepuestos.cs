using System;
using System.Runtime.InteropServices;
using System.Text;

unsafe struct NodoRepuesto {
    public int Id;
    public fixed char Nombre[50];
    public fixed char Detalles[100];
    public int Precio;
    public int Altura;
    public NodoRepuesto* Izquierdo;
    public NodoRepuesto* Derecho;

    public NodoRepuesto(int id, string nombre, string detalles, int precio) {
        Id = id;
        Precio = precio;
        Altura = 1;
        Izquierdo = null;
        Derecho = null;

        fixed (char* ptrNombre = Nombre) {
            CopyToFixedArray(ptrNombre, nombre, 50);
        }

        fixed (char* ptrDetalles = Detalles) {
            CopyToFixedArray(ptrDetalles, detalles, 100);
        }
    }

    private static void CopyToFixedArray(char* destination, string source, int maxLength) {
        for (int i = 0; i < maxLength; i++) {
            destination[i] = i < source.Length ? source[i] : '\0';
        }
    }
}

unsafe class ArbolAVLRepuestos {
    private NodoRepuesto* raiz;

    public ArbolAVLRepuestos() {
        raiz = null;
    }

    private int ObtenerAltura(NodoRepuesto* nodo) {
        return nodo == null ? 0 : nodo->Altura;
    }

    private int ObtenerBalance(NodoRepuesto* nodo) {
        return nodo == null ? 0 : ObtenerAltura(nodo->Izquierdo) - ObtenerAltura(nodo->Derecho);
    }

    private NodoRepuesto* RotarDerecha(NodoRepuesto* y) {
        NodoRepuesto* x = y->Izquierdo;
        NodoRepuesto* T2 = x->Derecho;

        x->Derecho = y;
        y->Izquierdo = T2;

        y->Altura = Math.Max(ObtenerAltura(y->Izquierdo), ObtenerAltura(y->Derecho)) + 1;
        x->Altura = Math.Max(ObtenerAltura(x->Izquierdo), ObtenerAltura(x->Derecho)) + 1;

        return x;
    }

    private NodoRepuesto* RotarIzquierda(NodoRepuesto* x) {
        NodoRepuesto* y = x->Derecho;
        NodoRepuesto* T2 = y->Izquierdo;

        y->Izquierdo = x;
        x->Derecho = T2;

        x->Altura = Math.Max(ObtenerAltura(x->Izquierdo), ObtenerAltura(x->Derecho)) + 1;
        y->Altura = Math.Max(ObtenerAltura(y->Izquierdo), ObtenerAltura(y->Derecho)) + 1;

        return y;
    }

    public void Insertar(int id, string nombre, string detalles, int precio) {
        raiz = InsertarRec(raiz, id, nombre, detalles, precio);
    }

    private NodoRepuesto* InsertarRec(NodoRepuesto* nodo, int id, string nombre, string detalles, int precio) {
        if (nodo == null) {
            NodoRepuesto* nuevo = (NodoRepuesto*)Marshal.AllocHGlobal(sizeof(NodoRepuesto));
            *nuevo = new NodoRepuesto(id, nombre, detalles, precio);
            return nuevo;
        }

        if (id < nodo->Id)
            nodo->Izquierdo = InsertarRec(nodo->Izquierdo, id, nombre, detalles, precio);
        else if (id > nodo->Id)
            nodo->Derecho = InsertarRec(nodo->Derecho, id, nombre, detalles, precio);
        else
            return nodo;

        nodo->Altura = 1 + Math.Max(ObtenerAltura(nodo->Izquierdo), ObtenerAltura(nodo->Derecho));
        int balance = ObtenerBalance(nodo);

        if (balance > 1 && id < nodo->Izquierdo->Id)
            return RotarDerecha(nodo);

        if (balance < -1 && id > nodo->Derecho->Id)
            return RotarIzquierda(nodo);

        if (balance > 1 && id > nodo->Izquierdo->Id) {
            nodo->Izquierdo = RotarIzquierda(nodo->Izquierdo);
            return RotarDerecha(nodo);
        }

        if (balance < -1 && id < nodo->Derecho->Id) {
            nodo->Derecho = RotarDerecha(nodo->Derecho);
            return RotarIzquierda(nodo);
        }

        return nodo;
    }

    public NodoRepuesto* Buscar(int id) {
        return BuscarRec(raiz, id);
    }

    private NodoRepuesto* BuscarRec(NodoRepuesto* nodo, int id) {
        if (nodo == null || nodo->Id == id)
            return nodo;

        if (id < nodo->Id)
            return BuscarRec(nodo->Izquierdo, id);
        else
            return BuscarRec(nodo->Derecho, id);
    }

    public void EditarRepuestoPorId(int id) {
        NodoRepuesto* encontrado = Buscar(id);
        if (encontrado == null) {
            Console.WriteLine($"No se encontró ningún repuesto con ID: {id}");
            return;
        }

        Console.WriteLine($"\nRepuesto encontrado:");
        Console.WriteLine($"ID: {encontrado->Id}");
        Console.WriteLine($"Nombre: {PtrToString(encontrado->Nombre)}");
        Console.WriteLine($"Detalles: {PtrToString(encontrado->Detalles)}");
        Console.WriteLine($"Precio: Q{encontrado->Precio}");

        Console.Write("¿Desea editar este repuesto? (1 = Sí, 2 = No): ");
        string opcion = Console.ReadLine() ?? "";

        if (opcion != "1") {
            Console.WriteLine("Edición cancelada.");
            return;
        }

        Console.Write("Nuevo nombre: ");
        string nuevoNombre = Console.ReadLine() ?? "";

        Console.Write("Nuevos detalles: ");
        string nuevosDetalles = Console.ReadLine() ?? "";

        Console.Write("Nuevo precio: ");
        if (!int.TryParse(Console.ReadLine(), out int nuevoPrecio)) {
            Console.WriteLine("Precio inválido.");
            return;
        }

        for (int i = 0; i < 50; i++) {
            encontrado->Nombre[i] = i < nuevoNombre.Length ? nuevoNombre[i] : '\0';
        }

        for (int i = 0; i < 100; i++) {
            encontrado->Detalles[i] = i < nuevosDetalles.Length ? nuevosDetalles[i] : '\0';
        }

        encontrado->Precio = nuevoPrecio;

        Console.WriteLine("Repuesto actualizado correctamente.");
    }

    // Métodos para mostrar repuestos (nuevas versiones que devuelven strings)
    public string ObtenerRepuestosInOrden() {
        var sb = new StringBuilder();
        if (raiz == null) {
            sb.AppendLine("No hay repuestos cargados.");
            return sb.ToString();
        }

        sb.AppendLine("ID\tRepuesto\tDetalles\tCosto");
        sb.AppendLine("--------------------------------------------------");
        ObtenerInOrden(raiz, sb);
        return sb.ToString();
    }

    public string ObtenerRepuestosPreOrden() {
        var sb = new StringBuilder();
        if (raiz == null) {
            sb.AppendLine("No hay repuestos cargados.");
            return sb.ToString();
        }

        sb.AppendLine("ID\tRepuesto\tDetalles\tCosto");
        sb.AppendLine("--------------------------------------------------");
        ObtenerPreOrden(raiz, sb);
        return sb.ToString();
    }

    public string ObtenerRepuestosPostOrden() {
        var sb = new StringBuilder();
        if (raiz == null) {
            sb.AppendLine("No hay repuestos cargados.");
            return sb.ToString();
        }

        sb.AppendLine("ID\tRepuesto\tDetalles\tCosto");
        sb.AppendLine("--------------------------------------------------");
        ObtenerPostOrden(raiz, sb);
        return sb.ToString();
    }

    private void ObtenerInOrden(NodoRepuesto* nodo, StringBuilder sb) {
        if (nodo == null) return;

        ObtenerInOrden(nodo->Izquierdo, sb);
        sb.AppendLine($"{nodo->Id}\t{PtrToString(nodo->Nombre)}\t{PtrToString(nodo->Detalles)}\tQ{nodo->Precio}");
        ObtenerInOrden(nodo->Derecho, sb);
    }

    private void ObtenerPreOrden(NodoRepuesto* nodo, StringBuilder sb) {
        if (nodo == null) return;
        
        sb.AppendLine($"{nodo->Id}\t{PtrToString(nodo->Nombre)}\t{PtrToString(nodo->Detalles)}\tQ{nodo->Precio}");
        ObtenerPreOrden(nodo->Izquierdo, sb);
        ObtenerPreOrden(nodo->Derecho, sb);
    }

    private void ObtenerPostOrden(NodoRepuesto* nodo, StringBuilder sb) {
        if (nodo == null) return;
        
        ObtenerPostOrden(nodo->Izquierdo, sb);
        ObtenerPostOrden(nodo->Derecho, sb);
        sb.AppendLine($"{nodo->Id}\t{PtrToString(nodo->Nombre)}\t{PtrToString(nodo->Detalles)}\tQ{nodo->Precio}");
    }

    // Métodos originales que escriben en consola (los mantengo por compatibilidad)
    public void MostrarRepuestos() {
        Console.WriteLine(ObtenerRepuestosInOrden());
    }

    public void MostrarRepuestosPreOrden() {
        Console.WriteLine(ObtenerRepuestosPreOrden());
    }

    public void MostrarRepuestosPostOrden() {
        Console.WriteLine(ObtenerRepuestosPostOrden());
    }

    public string PtrToString(char* ptr) {
        var resultado = new StringBuilder();
        for (int i = 0; i < 100 && ptr[i] != '\0'; i++) {
            resultado.Append(ptr[i]);
        }
        return resultado.ToString();
    }

    public void LiberarMemoria(NodoRepuesto* nodo) {
        if (nodo == null) return;

        LiberarMemoria(nodo->Izquierdo);
        LiberarMemoria(nodo->Derecho);

        Marshal.FreeHGlobal((IntPtr)nodo);
    }

    ~ArbolAVLRepuestos() {
        LiberarMemoria(raiz);
    }
}