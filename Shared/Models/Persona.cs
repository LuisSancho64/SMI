﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMI.Shared.Models;

public partial class Persona
{
    public int id { get; set; }

    public int? id_Genero { get; set; }

    public string nombre { get; set; }

    public string apellido { get; set; }
    public DateTime? FechaNacimiento { get; set; }

    public string Correo { get; set; }

    //Relacion con Usuarios
    public ICollection<User> Usuarios { get; set; }

    public ICollection<PersonaDocumento> Documentos { get; set; } = new List<PersonaDocumento>();
    
    public ICollection<PersonaLugarResidencia> LugaresResidencia { get; set; }

    public ICollection<PersonaEstadoCivil> EstadosCiviles { get; set; } = new List<PersonaEstadoCivil>();

    public  virtual PersonaDireccion Direccion { get; set; }
}