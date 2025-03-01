# Manual Técnico

## Introducción
Este documento describe la estructura interna del sistema de gestión de usuarios, vehículos, repuestos, facturas y servicios. Se detallan las clases, listas utilizadas, el uso de `unsafe` para punteros en C#, y las librerías necesarias para su ejecución.

---

## Librerías utilizadas

El programa utiliza las siguientes librerías:

```csharp
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
```

- `System`: Permite el uso de clases básicas de C#.
- `System.Collections.Generic`: Para el manejo de listas genéricas.
- `System.Runtime.InteropServices`: Necesaria para el uso de `unsafe` y punteros.

---

## Estructuras de datos utilizadas

El sistema maneja múltiples estructuras de datos para almacenar y gestionar información de manera eficiente.

### **Lista de Usuarios (Lista Simplemente Enlazada)**

**Clase `NodoUsuario`**:
```csharp
public class NodoUsuario {
    public string ID;
    public string Nombre;
    public NodoUsuario siguiente;
}
```

**Clase `ListaUsuarios`**:
```csharp
public class ListaUsuarios {
    private NodoUsuario cabeza;
    
    public void Agregar(string ID, string nombre) {
        NodoUsuario nuevo = new NodoUsuario { ID = ID, Nombre = nombre, siguiente = null };
        if (cabeza == null) {
            cabeza = nuevo;
        } else {
            NodoUsuario actual = cabeza;
            while (actual.siguiente != null) {
                actual = actual.siguiente;
            }
            actual.siguiente = nuevo;
        }
    }
}
```

**Descripción**:
- Es una **lista simplemente enlazada**, donde cada nodo apunta al siguiente.
- Se usa para gestionar los usuarios del sistema.

---

### **Lista de Vehículos (Lista Doblemente Enlazada)**

**Clase `NodoVehiculo`**:
```csharp
public class NodoVehiculo {
    public string Placa;
    public string Modelo;
    public NodoVehiculo siguiente;
    public NodoVehiculo anterior;
}
```

**Clase `ListaVehiculos`**:
```csharp
public class ListaVehiculos {
    private NodoVehiculo cabeza;
    
    public void Agregar(string placa, string modelo) {
        NodoVehiculo nuevo = new NodoVehiculo { Placa = placa, Modelo = modelo };
        if (cabeza == null) {
            cabeza = nuevo;
        } else {
            NodoVehiculo actual = cabeza;
            while (actual.siguiente != null) {
                actual = actual.siguiente;
            }
            actual.siguiente = nuevo;
            nuevo.anterior = actual;
        }
    }
}
```

**Descripción**:
- Es una **lista doblemente enlazada**, permitiendo recorrer en ambas direcciones.
- Se usa para gestionar vehículos.

---

### **Lista de Repuestos (Lista Circular)**

**Clase `NodoRepuesto`**:
```csharp
public class NodoRepuesto {
    public string Codigo;
    public string Nombre;
    public NodoRepuesto siguiente;
}
```

**Clase `ListaRepuestos`**:
```csharp
public class ListaRepuestos {
    private NodoRepuesto cabeza;
    
    public void Agregar(string codigo, string nombre) {
        NodoRepuesto nuevo = new NodoRepuesto { Codigo = codigo, Nombre = nombre };
        if (cabeza == null) {
            cabeza = nuevo;
            cabeza.siguiente = cabeza;
        } else {
            NodoRepuesto actual = cabeza;
            while (actual.siguiente != cabeza) {
                actual = actual.siguiente;
            }
            actual.siguiente = nuevo;
            nuevo.siguiente = cabeza;
        }
    }
}
```

**Descripción**:
- Es una **lista circular**: el último nodo apunta al primero.
- Se usa para gestionar repuestos.

---

### **Lista de Facturas (Pila - LIFO)**

**Clase `NodoFactura`**:
```csharp
public class NodoFactura {
    public string ID;
    public double Monto;
    public NodoFactura siguiente;
}
```

**Clase `PilaFacturas`**:
```csharp
public class PilaFacturas {
    private NodoFactura tope;
    
    public void Push(string id, double monto) {
        NodoFactura nuevo = new NodoFactura { ID = id, Monto = monto, siguiente = tope };
        tope = nuevo;
    }
}
```

**Descripción**:
- Es una **pila (LIFO)**, donde el último en entrar es el primero en salir.
- Se usa para almacenar facturas.

---

### **Lista de Servicios (Cola - FIFO)**

**Clase `NodoServicio`**:
```csharp
public class NodoServicio {
    public string ID;
    public string Descripcion;
    public NodoServicio siguiente;
}
```

**Clase `ColaServicios`**:
```csharp
public class ColaServicios {
    private NodoServicio frente;
    private NodoServicio final;
    
    public void Enqueue(string id, string descripcion) {
        NodoServicio nuevo = new NodoServicio { ID = id, Descripcion = descripcion };
        if (final == null) {
            frente = final = nuevo;
        } else {
            final.siguiente = nuevo;
            final = nuevo;
        }
    }
}
```

**Descripción**:
- Es una **cola (FIFO)**, donde el primero en entrar es el primero en salir.
- Se usa para gestionar servicios.

---

### **Matriz Bitácora**

Se utiliza una matriz dispersa para almacenar eventos en la bitácora de uso del sistema.

```csharp
public class MatrizBitacora {
    private Dictionary<int, Dictionary<int, string>> datos;
    
    public MatrizBitacora() {
        datos = new Dictionary<int, Dictionary<int, string>>();
    }
}
```

---

## Uso de `unsafe` y punteros

El sistema usa `unsafe` para manejar punteros en C#, lo que mejora el rendimiento al manipular estructuras de datos grandes.

Ejemplo de uso de punteros:
```csharp
unsafe class EjemploUnsafe {
    public void Metodo() {
        int num = 10;
        int* p = &num;
        Console.WriteLine($"Valor de num: {*p}");
    }
}
```

- Se usa en listas para mejorar acceso y rendimiento.
- Requiere habilitar `unsafe` en el compilador.

---

## Conclusión

Este documento cubre la arquitectura del sistema, incluyendo las listas utilizadas, su estructura, y el uso de `unsafe` en C#. Para más detalles, revisar los archivos fuente del proyecto.

Cualquier consulta ingresar al repositorio: https://github.com/bryanthr6/-EDD-1S2025_201701010